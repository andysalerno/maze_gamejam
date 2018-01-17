using UnityEngine;

public class AgraWallEye : MonoBehaviour
{
    private Transform player;

    public bool IsPlayerVisible { get; private set; }

    // Use this for initialization
    void Start()
    {
        this.player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        this.IsPlayerVisible = this.CanSeePlayer();
    }

    private bool CanSeePlayer()
    {
        // cast a Ray toward the player
        var playerDirection = this.player.position - this.transform.position;

        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, playerDirection);
        //if (Physics.Raycast(this.transform.position, playerDirection, out hit, QueryTriggerInteraction.Ignore))
        if (Physics.Raycast(ray, out hit, float.PositiveInfinity, -1, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform == this.player)
            {
                return true;
            }

        }
        return false;
    }
}
