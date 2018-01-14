using UnityEngine;

public class SceneLevelVars : MonoBehaviour
{
    // starting state of all global vars
    public static bool YellowFirstDialogComplete = false;
    public static bool YellowRedwallDialogComplete = false;
    public static bool YellowDoRedDoorDialog = false;

    // in the first dialog with yellow, true if you lie
    public static bool LiedAboutBlueTalking = false;

    // when the dance for Redwall is done
    public static bool MetSadRedwall = false;
    public static bool RedwallDanceComplete = false;

    // TODO: implement to dump a "save" file
    // of the current game state values
    public static void DumpToText()
    {

    }
}
