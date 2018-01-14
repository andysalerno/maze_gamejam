public class Green_Box_PlayerInteraction : Box_PlayerInteraction
{

    protected override Saying DialogTree
    {
        get
        {
            var rootSaying = new Saying("Go away :'(", Unipix);
            rootSaying.SetNextSaying(new Saying("You don't understand.", Unipix))
                .SetNextSaying(new Saying("You can't understand.", Unipix))
                .SetNextSaying(new Saying("You don't understand.", Unipix))
                .SetNextSaying(new Saying("You don't understand.", Unipix))
                .SetNextSaying(new Saying("You don't understand.", Unipix));


            // I'm not like Yellow!! I won't just *tell* you the answers!!

            return rootSaying;
        }
    }
}
