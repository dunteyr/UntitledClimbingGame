using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialMessages : MonoBehaviour
{
    //the unicode thingy for new-line is \u000a

    private string errorText = "Tutorial not found.";
    private string tutorialText;

    public string GetTutorial(string tutorial)
    {
        if(tutorial == "BasicMovement")
        {
            tutorialText = "Use the AD keys to move.\u000aPress Space to jump.\u000a\u000aBy the way, this weird floating ball triggers tutorials.";

            return tutorialText;
        }

        else if(tutorial == "Spikes")
        {
            tutorialText = "Careful of spikes. Small spikes will hurt you, and big spikes will kill you instantly." +
                "\u000a\u000aYour health is on the top left corner of the screen.";

            return tutorialText;
        }

        else if (tutorial == "Momentum")
        {
            tutorialText = "Reach further distances by getting a run up.\u000a\u000aGetting and keeping momentum is important for difficult climbs.";

            return tutorialText;
        }

        else if (tutorial == "Grabbing")
        {
            tutorialText = "Use left click to grab surfaces.\u000aHolding it down will make you grab the first thing your hand touches." +
                "\u000aUse right click to let go.";

            return tutorialText;
        }

        else if (tutorial == "GrabJumping")
        {
            tutorialText = "When hanging from a surface you still have control over body movement." +
                "\u000a\u000aPress Space to jump from grabbed surfaces." +
                "\u000aUse AD to swing and control body placement.";

            return tutorialText;
        }

        else if (tutorial == "Climbing")
        {
            tutorialText = "You can grab and jump as much as you need to." +
                "\u000a\u000aUse both to climb vertical surfaces.";

            return tutorialText;
        }

        else if (tutorial == "Swinging")
        {
            tutorialText = "The jump ahead seems pretty far. But you can do it." +
                "\u000a\u000aBe aware of your hand and body placement." +
                "\u000aTry to maintain a strong but controlled swing." +
                "\u000aGet a feel for when you should jump, and then go for it.";

            return tutorialText;
        }

        else if (tutorial == "ZoomingOut")
        {
            tutorialText = "Use the Q key to get a wider view of your surroundings.";

            return tutorialText;
        }

        else if (tutorial == "FallDamage")
        {
            tutorialText = "Did you take damage from that fall?" +
                "\u000a\u000aCertain heights can injure or even kill you." +
                "\u000aTry hanging from surfaces and dropping to soften the impact.";

            return tutorialText;
        }

        else if (tutorial == "Respawning")
        {
            tutorialText = "You can respawn at your last checkpoint from the pause menu. (ESC)";            

            return tutorialText;
        }

        else if (tutorial == "SpikeGrabbing")
        {
            tutorialText = "You may have already noticed this..." +
                "\u000abut grabbing spikes doesn't damage you." +
                "\u000aYou only take damage when your body hits them.";

            return tutorialText;
        }

        else if (tutorial == "ImpactDamage")
        {
            tutorialText = "Fall damage hurts but so does swinging into a cliff face first." +
                "\u000a\u000aIt will hurt less than a straight drop probably..." +
                "\u000abut it could kill you if you swing into something too hard";

            return tutorialText;
        }

        else
        {
            Debug.LogError("Tutorial not found.");
            return errorText;
        }
    }
}
