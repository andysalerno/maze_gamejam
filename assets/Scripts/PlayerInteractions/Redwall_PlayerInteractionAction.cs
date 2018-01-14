using UnityEngine;
using UnityEngine.UI;

public class Redwall_PlayerInteractionAction : Box_PlayerInteraction
{
    protected override Saying DialogTree
    {
        get
        {
            if (!SceneLevelVars.RedwallDanceComplete)
            {
                return this.FirstContact();
            }
            else if (!SceneLevelVars.RedwallHappyDialogComplete)
            {
                return this.DanceCompleteDialog();
            }
            else
            {
                return this.HappyLoopedDialog();
            }
        }
    }

    private Saying FirstContact()
    {
        var rootSaying = new Saying("Sigh...", Amatic, new MetSadRedwall());
        rootSaying
            .SetNextSaying(new Saying(":(", Amatic))
            .SetNextSaying(new Saying("You can't help me...", Amatic))
            .SetNextSaying(new Saying("Just go...", Amatic))
            .SetNextSaying(new Saying("There's no point...", Amatic));

        rootSaying.GetLast().SetNextSaying(rootSaying);

        return rootSaying;
    }

    private Saying DanceCompleteDialog()
    {
        var rootSaying = new Saying("Wow!", Amatic);
        rootSaying
            .SetNextSaying(new Saying("Sweet moves!!", Amatic))
            .SetNextSaying(new Saying("Let me give it a shot!", Amatic))
            .SetNextSaying(new Saying("Stand back...", Amatic))
            .SetNextSaying(new Saying("And check this out!!", Amatic, new BeginDancingTrigger()));

        return rootSaying;
    }

    private Saying HappyLoopedDialog()
    {
        var rootSaying = new Saying("Sigh...", Amatic, new MetSadRedwall());
        rootSaying
            .SetNextSaying(new Saying(":(", Amatic))
            .SetNextSaying(new Saying("You can't help me...", Amatic))
            .SetNextSaying(new Saying("Just go...", Amatic))
            .SetNextSaying(new Saying("There's no point...", Amatic));

        rootSaying.GetLast().SetNextSaying(rootSaying);

        return rootSaying;
    }


    private class MetSadRedwall : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
            Debug.Log("dialog event: met sad redwall");
            SceneLevelVars.MetSadRedwall = true;
        }
    }

    public void DanceHalfwayTrigger()
    {
        var text = this.GetComponentInChildren<Text>();
        text.text = ":/";
    }

    public void DanceCompleteTrigger()
    {
        var text = this.GetComponentInChildren<Text>();
        text.text = @"XD";

        SceneLevelVars.RedwallDanceComplete = true;
    }


    // implement to make the wall happier
    public void BeginDancing()
    {
        Debug.Log("Begin dancing!!!");
        this.gameObject.AddComponent<SpinScript>();
    }

    private class BeginDancingTrigger : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
            var redwallAction = (Redwall_PlayerInteractionAction)interactee;

            redwallAction.BeginDancing();
        }
    }
}
