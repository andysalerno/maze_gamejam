using UnityEngine;
using UnityEngine.UI;

public class Redwall_PlayerInteractionAction : Box_PlayerInteraction
{
    protected override Saying DialogTree
    {
        get
        {
            if (!SceneLevelVars.MetSadRedwall && !SceneLevelVars.RedwallDanceComplete)
            {
                return this.FirstContact();
            }
            else if (SceneLevelVars.RedwallDanceComplete)
            {
                return this.DanceCompleteDialog();
            }
            else
            {
                return null;
            }
        }
    }

    private Saying FirstContact()
    {
        var rootSaying = new Saying("Sigh...", Amatic, new MetSadRedwall()).EndBranch();
        rootSaying
            .SetNextSaying(new Saying(":(", Amatic).EndBranch())
            .SetNextSaying(new Saying("You can't help me...", Amatic).EndBranch())
            .SetNextSaying(new Saying("Just go...", Amatic).EndBranch())
            .SetNextSaying(new Saying("There's no point...", Amatic).EndBranch());

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

        var loopedSaying = new Saying("Get down!!", Amatic);
        loopedSaying
            .SetNextSaying(new Saying("Feel the beat!", Amatic))
            .SetNextSaying(new Saying("Awww yeah!", Amatic))
            .SetNextSaying(new Saying("Don't stop the music!", Amatic))
            .SetNextSaying(loopedSaying);

        rootSaying.GetLast().SetNextSaying(loopedSaying);

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

        this.BeginDancing();
        SceneLevelVars.RedwallDanceComplete = true;
    }

    public void BeginDancing()
    {
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
