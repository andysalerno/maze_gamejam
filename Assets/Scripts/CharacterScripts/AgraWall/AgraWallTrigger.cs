using UnityEngine;

public class AgraWallTrigger : MonoBehaviour
{
    private Transform player;
    private const string PLAYER_TAG = "Player";

    public bool IsPlayerColliding { get; private set; }

    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform == this.player.transform)
        {
            this.IsPlayerColliding = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform == this.player.transform)
        {
            this.IsPlayerColliding = false;
        }
    }

}
