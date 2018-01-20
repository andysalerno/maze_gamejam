public class BoxInfo2_PlayerInteractionAction : ABox_PlayerInteraction
{
    protected override Saying DialogTree
    {
        get
        {
            var rootSaying = new Saying("PACE yourself", Unipix);
            rootSaying.SetNextSaying(rootSaying);

            return rootSaying;
        }
    }
}
