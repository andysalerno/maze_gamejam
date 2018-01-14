public class Yellow_Box_PlayerInteraction : Box_PlayerInteraction
{

    protected override Saying RootSaying
    {
        get
        {
            var rootSaying = new Saying("*zzzzzzzzz*...", Amatic);

            var introEnd =
                rootSaying
                .SetNextSaying(new Saying("WAH!!", Amatic))
                .SetNextSaying(new Saying("Uh...!", Amatic))
                .SetNextSaying(new Saying("Press 'TAB' to leave a breadcrumb", Unipix))
                .SetNextSaying(new Saying("Wait, don't leave!", Amatic))
                .SetNextSaying(new Saying("Do you think my advice was useful?", Amatic))
                .SetNextSaying(new Saying("WAIT, don't answer yet!", Amatic))
                .SetNextSaying(new Saying("Think about how useful that is!", Amatic))
                .SetNextSaying(new Saying("It will help you avoid going in circles!", Amatic))
                .SetNextSaying(new Saying("So what do you say? Was it useful?", Amatic))
                .SetNextSaying(new Saying("Haven't had speech training yet, huh?", Amatic))
                .SetNextSaying(new Saying("How about, nod for yes and shake for no!", Amatic))
                    .SetYesSaying(new Saying("Great!! I love giving helpful hints :)", Amatic))
                    .SetNoSaying(new Saying("Aww :(", Amatic));

            introEnd.YesSaying.SetNextSaying(new Saying("Here's another great hint!", Amatic));
            introEnd.NoSaying.SetNextSaying(new Saying("Well, maybe you'll like this one more!", Amatic));

            // was my hint useful?
            var nextGreatHint = new Saying("Press '5' to walk through the walls", Unipix);
            var nextGreatHintEnd = nextGreatHint
            .SetNextSaying(new Saying("Give it a try!", Amatic))
            .SetNextSaying(new Saying("Did you try it yet?", Amatic))
            .SetNextSaying(new Saying("It doesn't actually work, but it's a great hint, isn't it!", Amatic))
            .SetNextSaying(new Saying("Sorry, I'm still not very good at this...", Amatic))
            .SetNextSaying(new Saying("No one ever finds my little corner here...", Amatic))
            .SetNextSaying(new Saying("...and the last time someone did, I *really* screwed it up.", Amatic));

            introEnd.YesSaying.NextSaying.SetNextSaying(nextGreatHint);
            introEnd.NoSaying.NextSaying.SetNextSaying(nextGreatHint);

            // say '...' forever.
            var dotdotdot = new Saying("...", Amatic);
            dotdotdot.SetNextSaying(dotdotdot);

            var betterLeave = new Saying("You should go now.", Amatic);
            betterLeave.SetNextSaying(betterLeave);

            // is Blue out there??
            var blueSaidNothing = new Saying(":(", Amatic);
            blueSaidNothing.SetNextSaying(new Saying("I didn't think so...", Amatic)
                .SetNextSaying_Parent(new Saying("See you around, then.", Amatic)));

            var blueSaidSomething = new Saying("You're lying.", Amatic);
            blueSaidSomething
                .SetNextSaying(new Saying("I know you are lying.", Amatic))
                .SetNextSaying(new Saying("Lying is bad behavior.", Amatic))
                .SetNextSaying(new Saying("I've cut your time in half.", Amatic, new CutTimeCallback()))
                .SetNextSaying(new Saying("Now I wish I hadn't given you that hint.", Amatic))
                .SetNextSaying(betterLeave);

            var isBlueOutThere = new Saying("Hey, I have a question for you...", Amatic);
            isBlueOutThere
                .SetNextSaying(new Saying("is Blue still out there?", Amatic))
                    .SetYesSaying(new Saying("Really? Did he mention me?", Amatic)
                        .SetYesSaying(blueSaidSomething)
                        .SetNoSaying(blueSaidNothing))
                    .SetNoSaying(new Saying("...", Amatic)
                        .SetNextSaying_Parent(new Saying("That's too bad....", Amatic)
                            .SetNextSaying_Parent(new Saying("...better get going. Time's running out.", Amatic)
                                .SetNextSaying_Parent(dotdotdot))));

            nextGreatHintEnd.SetNextSaying(isBlueOutThere);


            var metRedwall = new Saying("Say, have you met the red wall?", Amatic);
            metRedwall.SetNextSaying(new Saying("Yeah, he's a bit of a downer.", Amatic))
                .SetNextSaying(new Saying("He probably isn't letting you pass, huh.", Amatic))
                .SetNextSaying(new Saying("I think I can help you with that! :)", Amatic))
                .SetNextSaying(new Saying("He may look imposing, but he has a soft spot...", Amatic))
                .SetNextSaying(new Saying("For dancing!!!", Amatic))
                .SetNextSaying(new Saying(@"\:)\", Amatic))
                .SetNextSaying(new Saying(@"    /:)/", Amatic))
                .SetNextSaying(new Saying(@"\:)\", Amatic))
                .SetNextSaying(new Saying(@"    /:)/", Amatic))
                .SetNextSaying(new Saying("He can't dance himself,", Amatic))
                .SetNextSaying(new Saying("But if you dance for him, it'll make him really happy!", Amatic))
                .SetNextSaying(new Saying("Do you know how to dance?", Amatic))
                    .SetYesSaying(new Saying("Awesome!", Amatic))
                    .SetNoSaying(new Saying("That's fine, I'll tell you :)", Amatic));

            // dance instructions
            var bustAMove = new Saying("Go bust a move!", Amatic).Loop();

            var danceInstructions = new Saying("The secret is to get all the steps just right.", Amatic);
            danceInstructions
                .SetNextSaying(new Saying("Remember what I'm about to tell you!", Amatic))
                .SetNextSaying(new Saying("First, go left two times.", Amatic))
                .SetNextSaying(new Saying("The, go right once.", Amatic))
                .SetNextSaying(new Saying("Then, go back once...", Amatic))
                .SetNextSaying(new Saying("Then forward once...", Amatic))
                .SetNextSaying(new Saying("Then right two times!", Amatic))
                .SetNextSaying(new Saying("Then go left once...", Amatic))
                .SetNextSaying(new Saying("Then, for the big finale,", Amatic))
                .SetNextSaying(new Saying("Spin around while staying put!", Amatic))
                .SetNextSaying(new Saying("Get all that?", Amatic))
                    .SetYesSaying(new Saying("Now get out there and show that wall some moves!", Amatic))
                    .SetNoSaying(danceInstructions);

            danceInstructions.GetYesNo().YesSaying.SetNextSaying(bustAMove);

            metRedwall.GetYesNo().YesSaying.SetNextSaying(new Saying("Then get out there and show him some moves!", Amatic));
            metRedwall.GetYesNo().NoSaying.SetNextSaying(danceInstructions);

            return rootSaying;
        }
    }

    private class CutTimeCallback : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract)
        {
            playerInteract.gameObject.GetComponent<CountdownDisplay>().timeOnclockMs /= 2;
        }
    }
}
