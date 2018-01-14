public class BoxInfo2_PlayerInteractionAction : Box_PlayerInteraction
{
    protected override Saying RootSaying
    {
        get
        {
            var rootSaying = new Saying("PACE yourself", Unipix);
            rootSaying.SetNextSaying(rootSaying);

            return rootSaying;
        }
    }
}
