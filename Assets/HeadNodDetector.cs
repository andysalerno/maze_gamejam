using UnityEngine;

public class HeadNodDetector : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector2? headPositionLastFrame = null;

    // capture how much up/down motion there is in the polling period
    private float upDownAmount = 0f;

    // capture how much left/right motion there is in the polling period
    private float leftRightAmount = 0f;

    // "no" travels less than "yes", so we need a multiplier
    private const int NoMultiplier = 7;

    // how much headmotion distance should we collect
    // before we determine our nod/shake conclusion
    public float motionThreshold = 2000;

    public interface IHeadNodCallback
    {
        void HeadNodCallback(bool wasHeadYes);
    }

    private IHeadNodCallback callBack = null;

    private void Reset()
    {
        this.callBack = null;
        this.headPositionLastFrame = null;
        this.upDownAmount = 0f;
        this.leftRightAmount = 0f;
    }

    /// <summary>
    /// Register a HeadNodCallBack.
    /// Only one allowed at a time.
    /// Does not register, and returns false, if
    /// there is an existing callback.
    /// </summary>
    /// <param name="callBack"></param>
    public bool TryRegisterCallBack(IHeadNodCallback callBack)
    {
        if (this.callBack == null)
        {
            this.callBack = callBack;

            return true;
        }

        return false;
    }

    private void Start()
    {
        this.cameraTransform = this.GetComponentInChildren<Camera>().transform;

        this.Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.callBack == null)
        {
            return;
        }

        if (this.headPositionLastFrame == null)
        {
            this.headPositionLastFrame = new Vector2(
                this.cameraTransform.rotation.eulerAngles.y,
                this.cameraTransform.rotation.eulerAngles.x);

            return;
        }

        // yes, X is Y and Y is X
        float currentX = this.cameraTransform.rotation.eulerAngles.y;
        float currentY = this.cameraTransform.rotation.eulerAngles.x;

        this.upDownAmount += Mathf.Abs(currentY - this.headPositionLastFrame.Value.y);

        // scale by some amount, because up/down covers much more distance
        this.leftRightAmount += (NoMultiplier * Mathf.Abs(currentX - this.headPositionLastFrame.Value.x)); 

        if (this.upDownAmount > this.motionThreshold)
        {
            this.callBack.HeadNodCallback(true);
            Debug.Log("He said yes!!");
            this.Reset();
        }
        else if (this.leftRightAmount > this.motionThreshold)
        {
            this.callBack.HeadNodCallback(false);
            Debug.Log("He said no :(");
            this.Reset();
        }

        this.headPositionLastFrame = new Vector2(currentX, currentY);
    }
}
