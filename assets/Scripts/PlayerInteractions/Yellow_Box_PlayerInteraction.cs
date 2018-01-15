using UnityEngine;

public class Yellow_Box_PlayerInteraction : Box_PlayerInteraction
{

    /// <summary>
    /// returning null indicates we do not have a new dialog tree for you yet
    /// and you should continue with your existing one
    /// </summary>
    protected override Saying DialogTree
    {
        get
        {
            // first stage: always get the first dialog first
            if (!SceneLevelVars.YellowFirstDialogComplete)
            {
                return FirstEpochBranch();
            }
            
            // if we haven't met blue, say this next
            if (!SceneLevelVars.MetBlue && !SceneLevelVars.YellowSeenBlueDialogComplete)
            {
                return NotMetBlueBranch();
            }

            if (SceneLevelVars.MetBlue && !SceneLevelVars.YellowHaveYouMetBlueDialogComplete && !SceneLevelVars.MetSadRedwall)
            {
                return HaveYouMetBlueBranch();
            }

            if(SceneLevelVars.MetSadRedwall && !SceneLevelVars.YellowRedwallDialogComplete)
            {
                return RedwallBranch();
            }

            return null;
        }
    }

    private Saying FirstEpochBranch()
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
        .SetNextSaying(new Saying("I just love giving hints.", Amatic))
        .SetNextSaying(new Saying("Life is about helping others. That's my philosophy!", Amatic))
        .SetNextSaying(new Saying("now I just need to establish some plot here!!", Amatic))
        .SetNextSaying(new Saying("Anyway, you should get going!", Amatic))
        .SetNextSaying(new Saying("Good luck out there!", Amatic).Loop().EndBranch());

        introEnd.YesSaying.NextSaying.SetNextSaying(nextGreatHint);
        introEnd.NoSaying.NextSaying.SetNextSaying(nextGreatHint);

        rootSaying.GiveAllEndingsCallback(new FirstBranchComplete());

        return rootSaying;
    }

    // when you talk to Yellow before meeting Blue
    private Saying NotMetBlueBranch()
    {
        var root = new Saying("And let me know if you run into my friend Blue out there!", Amatic);
        root.SetNextSaying(new Saying("Later!", Amatic, new NotYetSeenBlueDialogCallback()).Loop().EndBranch());

        return root;
    }

    // if you have seen CorruptBlue and now you are talking to Yellow
    private Saying HaveYouMetBlueBranch()
    {
        var dotdotdot = new Saying("...", Amatic, new HaveYouMetBlueDialogCallback()).Loop().EndBranch();
        var betterLeave = new Saying("You should go now.", Amatic, new HaveYouMetBlueDialogCallback()).Loop().EndBranch();

        // is Blue out there??
        var blueSaidNothing = new Saying(":(", Amatic);
        blueSaidNothing.SetNextSaying(new Saying("I didn't think so...", Amatic)
            .SetNextSaying_Parent(new Saying("...well, come find me if you ever need help.", Amatic)
            .SetNextSaying_Parent(betterLeave)));

        var blueSaidSomething = new Saying("...that's a lie, isn't it.", Amatic, new LieAboutBlueTalking());
        blueSaidSomething
            .SetNextSaying(new Saying("I wish you wouldn't lie.", Amatic))
            .SetNextSaying(new Saying("I'm trying to help you.", Amatic))
            .SetNextSaying(new Saying("And you are lying to me.", Amatic))
            .SetNextSaying(new Saying("I am required by protocol to say:", Amatic))
            .SetNextSaying(new Saying("Come find me if you ever need help.", Amatic))
            .SetNextSaying(betterLeave);

        var isBlueOutThere = new Saying("Hey, I have a question for you...", Amatic);
        isBlueOutThere
            .SetNextSaying(new Saying("have you seen Blue out there?", Amatic))
                .SetYesSaying(new Saying("Really? Did he mention me?", Amatic)
                    .SetYesSaying(blueSaidSomething)
                    .SetNoSaying(blueSaidNothing))
                .SetNoSaying(new Saying("...", Amatic)
                    .SetNextSaying_Parent(new Saying("That's too bad....", Amatic)
                    .SetNextSaying_Parent(new Saying("I haven't heard from her in forever.", Amatic)
                    .SetNextSaying_Parent(new Saying("...anyway, better get going.", Amatic)
                    .SetNextSaying_Parent(new Saying("Come find me if you ever need help!", Amatic)
                    .SetNextSaying_Parent(dotdotdot))))));

        return isBlueOutThere;
    }

    private Saying RedwallBranch()
    {
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
                .SetYesSaying(new Saying("You don't have to lie to impress me!", Amatic))
                .SetNoSaying(new Saying("That's fine, I'll tell you :)", Amatic));

        // dance instructions

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
                .SetYesSaying(new Saying("Now get out there and show that wall some moves!", Amatic, new AddDanceDetectorCallback()).Loop())
                .SetNoSaying(danceInstructions);

        metRedwall.GetYesNo().YesSaying
            .SetNextSaying(new Saying("I can tell just by looking at you that you couldn't dance to save your life!", Amatic))
            .SetNextSaying(danceInstructions);

        metRedwall.GetYesNo().NoSaying.SetNextSaying(danceInstructions);

        var needToHearAgain = new Saying("Need to hear those dance instructions again?", Amatic);
        needToHearAgain.SetYesSaying(danceInstructions);
        needToHearAgain.SetNoSaying(new Saying("Then go cheer that big wall up :)", Amatic)
            .SetNextSaying(needToHearAgain));

        var bustAMove = new Saying("Go bust a move!", Amatic);
        bustAMove.SetNextSaying(needToHearAgain);

        danceInstructions.GetYesNo().YesSaying.SetNextSaying(bustAMove);

        return metRedwall;
    }

    private class CutTimeCallback : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
            playerInteract.gameObject.GetComponent<CountdownDisplay>().timeOnclockMs /= 2;
        }
    }

    private class FirstBranchComplete : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
            Debug.Log("dialog event: yellow first branch complete");
            SceneLevelVars.YellowFirstDialogComplete = true;
        }
    }

    private class NotYetSeenBlueDialogCallback : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
            Debug.Log("dialog event: seen blue dialog complete");
            SceneLevelVars.YellowSeenBlueDialogComplete = true;
        }
    }

    private class HaveYouMetBlueDialogCallback : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
            Debug.Log("dialog event: have you met blue dialog complete");
            SceneLevelVars.YellowHaveYouMetBlueDialogComplete = true;
        }
    }

    private class LieAboutBlueTalking : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
            Debug.Log("dialog event: lied about blue talking");
            SceneLevelVars.LiedAboutBlueTalking = true;
        }
    }

    // used in the RedWall dance section
    private class AddDanceDetectorCallback : Saying.ISayingCallback
    {
        public void callBackMethod(PlayerInteract playerInteract, Box_PlayerInteraction interactee)
        {
            Debug.Log("dialog event: added dance detector");
            SceneLevelVars.YellowRedwallDialogComplete = true;
            playerInteract.gameObject.AddComponent<DanceDetector>();
        }
    }
}
