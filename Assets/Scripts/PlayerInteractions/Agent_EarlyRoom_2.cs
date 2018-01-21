using UnityEngine;

public class Agent_EarlyRoom_2 : ABox_PlayerInteraction
{
    protected override Saying DialogTree
    {
        get
        {
            var root = new Saying("ROOM 1", Unipix);
            root.SetNextSaying(new Saying("REACH THE BLUE FIELD", Unipix))
            .SetNextSaying(new Saying("CAUTION: RED WALLS ARE AGGRESSIVE", Unipix)).Loop();

            return root;
        }
    }
}
