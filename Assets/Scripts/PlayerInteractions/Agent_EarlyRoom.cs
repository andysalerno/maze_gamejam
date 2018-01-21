using UnityEngine;

public class Agent_EarlyRoom : ABox_PlayerInteraction
{
    protected override Saying DialogTree
    {
        get
        {
            var root = new Saying("ROOM 0", Unipix);
            root.SetNextSaying(new Saying("REACH THE BLUE FIELD", Unipix)).Loop();

            return root;
        }
    }
}
