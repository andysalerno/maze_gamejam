using UnityEngine;

public class SceneLevelVars : MonoBehaviour
{
    // starting state of all global vars
    public static bool YellowFirstDialogComplete = false;
    public static bool YellowRedwallDialogComplete = false;
    public static bool YellowDoRedDoorDialog = false;
    public static bool YellowSeenBlueDialogComplete = false;
    public static bool YellowHaveYouMetBlueDialogComplete = false;

    // in the first dialog with yellow, true if you lie
    public static bool LiedAboutBlueTalking = false;

    // when the dance for Redwall is done
    public static bool MetSadRedwall = false;
    public static bool RedwallDanceComplete = false;
    public static bool RedwallHappyDialogComplete = false;

    // was Blue switched on yet?
    public static bool MetBlue = false;
    public static bool BlueBoxEnabled = false;

    // Green
    public static bool MetGreen = false;

    // big door at beginning
    public static bool MysteryDoorSlideUp = false;

    // TODO: implement to dump a "save" file
    // of the current game state values
    public static void DumpToText()
    {

    }
}
