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

    private bool isOnDanceFloor = false;

    // when we are testing for the dance moves
    private bool isTestingForDance = false;

    // when we are testing for the big spin at end
    private bool isTestingForSpin = false;

    void Start()
    {
        // for debugging only, delete this call soon
        this.StartTestingForDance();
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

        //if (this.movements.Count >= CompleteDance.Count)
        //{
        //    string danceSoFar = string.Join(", ", this.movements.GetRange(movements.Count - CompleteDance.Count, CompleteDance.Count).ToArray());
        //    Debug.Log($"Dance so far: {danceSoFar}");
        //    Debug.Log($"Need to do:   {string.Join(", ", CompleteDance.ToArray())}");
        //}

        if (this.IsDanceHalfway())
        {
            this.redwallInteraction.SetFaceSurprise();
        }
        if (this.IsDanceComplete())
        {
            Debug.Log("Dance complete!!!!");
            this.isTestingForDance = false;
            this.isTestingForSpin = true;
            this.redwallInteraction.SetFaceHappy();
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
