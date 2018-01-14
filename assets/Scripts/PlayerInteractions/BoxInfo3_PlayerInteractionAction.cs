public class BoxInfo3_PlayerInteractionAction : Box_PlayerInteraction
{
    protected override Saying DialogTree
    {
        get
        {
            var rootSaying = new Saying("good luck", Amatic);

            Saying watchClock = new Saying("Watch the CLOCK", Unipix);
            watchClock.SetNextSaying(watchClock);

            rootSaying.SetNextSaying(watchClock);

            return rootSaying;
        }
    }
}
