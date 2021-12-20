using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningMessages : MonoBehaviour
{
    /*
     This class is for holding warning messages that can be used as arguments in
     WarningMenu.cs function ShowWarning() which turns on the warning menu and
     sets its text   
    */


    public string RestartLevelWarning()
    {
        string restartLevelWarning = "Restarting will erase all of this level's progress and checkpoints.";

        return restartLevelWarning;
    }

    public string QuitLevelWarning()
    {
        string quitLevelWarning = "You will lose all progress after the last checkpoint.";

        return quitLevelWarning;
    }

    public string EraseSaveWarning()
    {
        string eraseSaveWarning = "Erasing your save will make you lose all game progress";

        return eraseSaveWarning;
    }
}
