using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    private CompositeCollider2D flagCollider;
    private LevelCompleteMenu levelCompleteMenu;

    public bool levelCompleted;

    // Start is called before the first frame update
    void Start()
    {
        flagCollider = GetComponent <CompositeCollider2D>();
        levelCompleteMenu = GameObject.FindGameObjectWithTag("PopupMenu").GetComponent<LevelCompleteMenu>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(levelCompleted == false)
            {
                Debug.Log("You win nerd");
                levelCompleted = true;
                levelCompleteMenu.ToggleLevelComplete();
            }  
        }
    }


}
