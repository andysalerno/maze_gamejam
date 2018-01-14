public class Red_Wall_PlayerInteraction : Box_PlayerInteraction
{

    protected override Saying RootSaying
    {
        get
        {
            var rootSaying = new Saying("[SLEEP MODE DEACTIVATED]", Amatic);
            return rootSaying;
        }
    }
}
