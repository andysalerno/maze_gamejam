using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DanceDetector : MonoBehaviour
{
    private enum Movements
    {
        Up,
        Down,
        Left,
        Right
    }

    private const string DanceFloorTag = "dancefloor";

    private List<Movements> movements = new List<Movements>();

    private static readonly List<Movements> CompleteDance = new List<Movements>
    {
        Movements.Left,
        Movements.Left,
        Movements.Right,
        Movements.Down,
        Movements.Up,
        Movements.Right,
        Movements.Right,
        Movements.Left,

        // last move is a spin around, checked with other logic
    };

    private Redwall_PlayerInteractionAction redwallInteraction;

    // used for spin move
    private Transform cameraTransform;

    private bool isOnDanceFloor = false;

    // when we are testing for the dance moves
    private bool isTestingForDance = false;

    // when we are testing for the big spin at end
    private bool isTestingForSpin = false;
    // the starting angle of the spin, recording when the dance section is complete
    // after 360 degrees, the spin is complete
    private float spinDistance = 0;
    private const float SpinDistanceRequired = 600;
    private float? lastFrameAngle = null;

    void Start()
    {
        this.cameraTransform = this.GetComponentInChildren<Camera>().transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == DanceFloorTag)
        {
            this.isOnDanceFloor = true;
            this.redwallInteraction = other.GetComponentInParent<Redwall_PlayerInteractionAction>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == DanceFloorTag)
        {
            this.isOnDanceFloor = false;
        }
    }

    void Update()
    {
        if (this.isTestingForDance && this.isOnDanceFloor)
        {
            this.RecordDanceMoves();
        }
        else if (this.isTestingForSpin && this.isOnDanceFloor)
        {
            this.RecordSpin();
        }
    }

    private void RecordSpin()
    {
        if (this.lastFrameAngle == null)
        {
            this.lastFrameAngle = this.cameraTransform.rotation.eulerAngles.y;
            return;
        }

        // yes, it is y for some reason
        var currentX = this.cameraTransform.rotation.eulerAngles.y;

        this.spinDistance += Mathf.Abs(currentX - this.lastFrameAngle.Value);

        // update tracking status
        this.lastFrameAngle = currentX;

        if (this.spinDistance >= SpinDistanceRequired)
        {
            // you've finished it
            this.redwallInteraction.DanceCompleteTrigger();
        }

        Debug.Log($"spin distance: {this.spinDistance}");
    }

    private void RecordDanceMoves()
    {
        int movesCount = this.movements.Count;

        if (Input.GetKeyUp(KeyCode.W))
        {
            movements.Add(Movements.Up);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            movements.Add(Movements.Down);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            movements.Add(Movements.Left);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            movements.Add(Movements.Right);
        }

        if (this.IsDanceHalfway())
        {
            this.redwallInteraction.DanceHalfwayTrigger();
        }
        if (this.IsDanceComplete())
        {
            Debug.Log("Dance section complete!!!!");
            this.isTestingForDance = false;
            this.isTestingForSpin = true;
        }
    }

    public void StartTestingForDance()
    {
        this.isTestingForDance = true;
    }

    private bool IsDanceHalfway()
    {
        if (movements.Count < CompleteDance.Count / 2)
        {
            return false;
        }
        else if (movements.GetRange(movements.Count - CompleteDance.Count / 2, CompleteDance.Count / 2).SequenceEqual(CompleteDance.Take(CompleteDance.Count / 2)))
        {
            return true;
        }

        return false;
    }

    private bool IsDanceComplete()
    {
        if (movements.Count < CompleteDance.Count)
        {
            return false;
        }
        else if (movements.GetRange(movements.Count - CompleteDance.Count, CompleteDance.Count).SequenceEqual(CompleteDance))
        {
            return true;
        }

        return false;
    }
}
