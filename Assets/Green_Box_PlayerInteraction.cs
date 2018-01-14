public class Green_Box_PlayerInteraction : Box_PlayerInteraction
{

    protected override Saying RootSaying
    {
        get
        {
            var rootSaying = new Saying("Go away :'(", Unipix);
            rootSaying.SetNextSaying(new Saying("You don't understand.", Unipix))
                .SetNextSaying(new Saying("You can't understand.", Unipix))
                .SetNextSaying(new Saying("You don't understand.", Unipix))
                .SetNextSaying(new Saying("You don't understand.", Unipix))
                .SetNextSaying(new Saying("You don't understand.", Unipix));

            return rootSaying;
        }
    }
}
