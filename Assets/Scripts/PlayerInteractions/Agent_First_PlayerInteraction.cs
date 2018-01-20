using UnityEngine;

public class Agent_First_PlayerInteraction : ABox_PlayerInteraction
{
    protected override Saying DialogTree
    {
        get
        {
            var root = new Saying("NEW AGENT MODEL COMPLETE", Unipix);
            root.SetNextSaying(new Saying("PREPARE FOR TRAINING", Unipix))
                .SetNextSaying(new Saying("EPOCH: 1/4096", Unipix))
                .SetNextSaying(new Saying("DESCRIPTION: MAZE", Unipix))
                .SetNextSaying(new Saying("TRAINING START", Unipix, new AgentFirstDoorCallback()));

            return root;
        }
    }

    private class AgentFirstDoorCallback : Saying.ISayingCallback
    {
        private const string MYSTERY_DOOR_TAG = "MysteryDoor";

        public void callBackMethod(PlayerInteract playerInteract, ABox_PlayerInteraction interactee)
        {
            var mysteryDoor = GameObject.FindGameObjectWithTag(MYSTERY_DOOR_TAG).GetComponent<MysteryDoorRise>();
            mysteryDoor.SlideUp();
        }
    }

}
