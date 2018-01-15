using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private Camera camera;

    private PlayerTextDisplay textDisplay;
    private HeadNodDetector headNodder;
    private CountdownDisplay countdownDisplay;

    public Transform playerSpawnPosition;
    public Transform breadcrumb;

    void Start()
    {
        this.RespawnAtSpawnpoint();
        this.camera = this.GetComponentInChildren<Camera>();
        this.textDisplay = this.GetComponentInChildren<PlayerTextDisplay>();
        this.headNodder = this.GetComponentInChildren<HeadNodDetector>();
        //this.countdownDisplay = GetComponentInChildren<CountdownDisplay>();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            var breadcrumbObj = Instantiate(this.breadcrumb, null, true);
            var crumbPos = this.camera.transform.position + this.camera.transform.forward * 2;
            // crumbPos -= this.camera.transform.up * 1;
            crumbPos.y = this.camera.transform.position.y;
            breadcrumbObj.SetPositionAndRotation(crumbPos, Quaternion.Euler(90, 0, 0));
        }
        if (Input.GetMouseButtonDown(0))
        {
            // if there is any text showing, clear it and exit
            if (this.textDisplay.IsShowingText())
            {
                this.textDisplay.ClearText();

                // return if you don't want to allow displaying multiple
                // text displays at once
                //return;
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                var thingHit = hit.transform;
                var colliderHit = hit.collider;

                Debug.Log("Thing hit: " + colliderHit.name);

                // trigger the Actioned event of the thing hit, if it has one
                var interaction = hit.collider.GetComponentInParent<PlayerInteractionAction>();

                if (interaction == null)
                {
                    Debug.Log("No interaction found.");
                    return;
                }

                if (DistanceTo(interaction) <= interaction.DistanceActionable)
                {
                    Debug.Log("Executing interaction.");
                    interaction.Action(this);
                }
            }
        }
    }

    public bool TryRegisterHeadNodCallback(HeadNodDetector.IHeadNodCallback callback)
    {
        return this.headNodder.TryRegisterCallBack(callback);
    }

    public void ShowText(string text, Font font, int fontSize)
    {
        this.textDisplay.ShowText(text, font, fontSize);
    }

    // evict whatever text is currently showing and show the given text
    public void ForceShowText(string text, Font font, int fontSize)
    {
        this.textDisplay.ForceShowText(text, font, fontSize);
    }

    private float DistanceTo(MonoBehaviour monoObject)
    {
        return Vector3.Distance(monoObject.transform.position, this.transform.position);
    }

    public void TimerUpBehavior()
    {
        this.RespawnAtSpawnpoint();
    }

    public void RespawnAtSpawnpoint()
    {
        this.transform.SetPositionAndRotation(playerSpawnPosition.position, Quaternion.Euler(0, 0, 0));

        // note: tried setting this in Start(), it wouldn't work
        // I think because they refer to one another
        var countdownDisplay = this.GetComponentInChildren<CountdownDisplay>();
        countdownDisplay.ResetTimer();
    }
}
