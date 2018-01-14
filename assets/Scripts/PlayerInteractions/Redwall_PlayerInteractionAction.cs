using UnityEngine;
using UnityEngine.UI;

public class Redwall_PlayerInteractionAction : Box_PlayerInteraction
{
    protected override Saying DialogTree
    {
        get
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
    }

    private class MetSadRedwall : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract)
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

    }
}
