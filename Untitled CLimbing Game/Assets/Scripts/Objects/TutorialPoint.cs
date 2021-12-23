using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPoint : MonoBehaviour
{
    private TutorialMessages tutorialMessages;

    //the specific tutorial is set in the editor for each tutorial point
    public string pointTutorial;
    private GameObject canvasToCopy;
    public GameObject tutorialCanvas;
    public bool tutorialCanvasActive;
    private TextMeshProUGUI tutorialTextbox;

    private float canvasVertOffset = 4;

    private void Start()
    {
        tutorialMessages = GetComponentInParent<TutorialMessages>();
        canvasToCopy = GameObject.FindGameObjectWithTag("TutorialCanvas");

        tutorialCanvas = Instantiate(canvasToCopy);
        tutorialCanvas.SetActive(false);
        tutorialCanvasActive = false;

        tutorialTextbox = tutorialCanvas.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            tutorialCanvas.transform.position = transform.position;
            tutorialCanvas.transform.position += new Vector3(0, canvasVertOffset, 0);
            //usual layer is 8 which makes it invisible in scene. copy is set to 0 so it will render
            tutorialCanvas.layer = 0;
            tutorialCanvas.SetActive(true);
            tutorialCanvasActive = true;

            ShowTutorial(pointTutorial);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            tutorialCanvas.SetActive(false);
            tutorialCanvasActive = false;
        }
    }

    private void ShowTutorial(string tutorial)
    {
        tutorialTextbox.text = tutorialMessages.GetTutorial(tutorial);
    }
}
