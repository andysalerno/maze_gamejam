using UnityEngine;

public class AgraWall : MonoBehaviour
{
    // TDOO: have these guys spawn in on a trigger instead of always available

    /// <summary>
    /// True iff this AgraWall faces +/- X,
    /// instead of +/- Z
    /// </summary>
    public bool facesX;

    private float speed = 4f;
    private float resetSpeed = 0.75f;

    private Rigidbody rigidbody;

    private Vector3 initialPosition;

    private const string PLAYER_TAG = "Player";
    private Transform player;

    private AgraWallEye eyeball;
    private AgraWallTrigger trigger;

    public void Awake()
    {
        this.initialPosition = this.transform.position;
    }

    public void Start()
    {
        this.eyeball = this.GetComponentInChildren<AgraWallEye>();
        this.trigger = this.GetComponentInChildren<AgraWallTrigger>();

        this.player = GameObject.FindWithTag("Player").transform;
        this.rigidbody = this.GetComponent<Rigidbody>();

        this.rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

        if (this.facesX)
        {
            this.rigidbody.constraints |= RigidbodyConstraints.FreezePositionZ;
        }
        else
        {
            this.rigidbody.constraints |= RigidbodyConstraints.FreezePositionX;
        }
    }

    public void Update()
    {
        if (this.trigger.IsPlayerColliding && this.eyeball.IsPlayerVisible)
        {
            this.MoveTowardsPlayer();
        }
        else
        {
            this.MoveTowardResetPosition();
        }
    }

    private Vector3 GetIdealVelocity()
    {
        if (this.facesX)
        {
            var normalized = (this.player.position.x < this.transform.position.x)
                ? Vector3.left : Vector3.right;

            return normalized * speed;
        }
        else
        {
            var normalized = (this.player.position.z < this.transform.position.z)
                ? Vector3.back : Vector3.forward;

            return normalized * speed;
        }
    }

    private void MoveTowardsPlayer()
    {
        this.rigidbody.velocity = this.GetIdealVelocity();
    }

    private void MoveTowardResetPosition()
    {
        var vector = (this.initialPosition - this.transform.position).normalized;
        this.rigidbody.velocity = vector * this.resetSpeed;
    }
}
