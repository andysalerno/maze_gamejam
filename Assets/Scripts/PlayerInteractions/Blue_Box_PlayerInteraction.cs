using UnityEngine;

public class Blue_Box_PlayerInteraction : Box_PlayerInteraction
{
    /// <summary>
    /// returning null indicates we do not have a new dialog tree for you yet
    /// and you should continue with your existing one
    /// </summary>
    protected override Saying DialogTree
    {
        get
        {
            if (!SceneLevelVars.BlueBoxEnabled)
            {
                return SystemCorruptBranch();
            }

            return null;
        }
    }

    private Saying SystemCorruptBranch()
    {
        Saying root = new Saying(string.Empty, Amatic, new CorruptCallback()).Loop();

        return root;
    }

    private class CorruptCallback : Saying.ISayingCallback
    {
        // show a bunch of corruption
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
            var existingBabble = playerInteract.GetComponent<BabbleCorruptionScript>();

            if (existingBabble == null)
            {
                playerInteract.gameObject.AddComponent<BabbleCorruptionScript>();
            }
        }
    }
}
