public class BoxInfo1_PlayerInteractionAction : Box_PlayerInteraction
{
    protected override Saying RootSaying
    {
        get
        {
            var rootSaying = new Saying("Avoid DEAD ENDS", Unipix);
            rootSaying.SetNextSaying(rootSaying);
            return rootSaying;
        }
    }
}
