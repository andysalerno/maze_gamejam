using UnityEngine.UI;

public class Redwall_PlayerInteractionAction : Box_PlayerInteraction
{
    protected override Saying DialogTree
    {
        get
        {
            var rootSaying = new Saying("Sigh...", Amatic);
            rootSaying
                .SetNextSaying(new Saying(":(", Amatic))
                .SetNextSaying(new Saying("You can't help me...", Amatic))
                .SetNextSaying(new Saying("Just go...", Amatic))
                .SetNextSaying(new Saying("There's no point...", Amatic));

            rootSaying.GetLast().SetNextSaying(rootSaying);

            return rootSaying;
        }
    }

    // TODO: animate this, like a typing animation
    public void SetFaceSurprise()
    {
        var text = this.GetComponentInChildren<Text>();

        text.text = ":O !!!";
    }

    public void SetFaceHappy()
    {
        var text = this.GetComponentInChildren<Text>();

        text.text = ":)";
    }
}
