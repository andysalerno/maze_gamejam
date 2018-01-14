public class BoxInfo1_PlayerInteractionAction : Box_PlayerInteraction
{
    protected override Saying DialogTree
    {
        get
        {
            var rootSaying = new Saying("Avoid DEAD ENDS", Unipix);
            rootSaying.SetNextSaying(rootSaying);
            return rootSaying;
        }
    }
}
