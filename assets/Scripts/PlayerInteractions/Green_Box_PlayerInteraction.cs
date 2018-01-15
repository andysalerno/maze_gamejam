public class Green_Box_PlayerInteraction : Box_PlayerInteraction
{

    protected override Saying DialogTree
    {
        get
        {
            return FirstContact();
        }
    }

            // I'm not like Yellow!! I won't just *tell* you the answers!!
    private Saying FirstContact()
    {
        var root = new Saying("[SLEEP MODE DEACTIVATED]", Ostrich, new MetGreenAgent());
        root.SetNextSaying(new Saying("[UNKNOWN AGENT DETECTED]", Ostrich, new BlockEscape()))
            .SetNextSaying(new Saying("[ANALYZING FOREIGN AGENT]", Ostrich))
            .SetNextSaying(new Saying("[...18%]", Ostrich))
            .SetNextSaying(new Saying("[...47%]", Ostrich))
            .SetNextSaying(new Saying("[...78%]", Ostrich))
            .SetNextSaying(new Saying("[ANALYSIS COMPLETE]", Ostrich))
            .SetNextSaying(new Saying("[TARGET IS ADVERSARIAL AGENT]", Ostrich))
            .SetNextSaying(new Saying("[DETERMINING BEST RESPONSE...]", Ostrich))
            .SetNextSaying(new Saying("[DETERMINATION COMPLETE]", Ostrich))
            .SetNextSaying(new Saying("[CONCLUSION:  ]", Ostrich))
            .SetNextSaying(new Saying("[CHALLENGE.]", Ostrich))
            .SetNextSaying(new Saying("[DOMINATE.]", Ostrich))
            .SetNextSaying(new Saying("[DESTROY.]", Ostrich))

            .SetNextSaying(new Saying("[...PLEASE STAND ON DESIGNATED ZONE TO BEGIN DESTRUCTION.]", Ostrich))
            .SetNextSaying(new Saying("[...RIGHT NOW, PLEASE]", Ostrich))
            .SetNextSaying(new Saying("[ESCAPE IS IMPOSSIBLE]", Ostrich));

        return root;
    }

    private class MetGreenAgent : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
            SceneLevelVars.MetGreen = true;
        }
    }

    private class BlockEscape : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
        }
    }
}
