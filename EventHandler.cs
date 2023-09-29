using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    private GameObject uiCanvas;
    private DialogueTrigger initDialogue;
    
    
    // Start is called before the first frame update
    void Start()
    {
        uiCanvas = GameObject.Find("UIBox");
        var initDialogueObject = GameObject.Find("InitialDialog");
        initDialogue = initDialogueObject.GetComponent<DialogueTrigger>();
        moveCanvasSomewhere(false);
        // uiCanvas.SetActive(false);
    }

    public void enableUI()
    {
        initDialogue.TriggerDialogue();
        moveCanvasSomewhere(true);
        // uiCanvas.SetActive(true);
    }

    private void moveCanvasSomewhere(bool back)
    {
        var vec = uiCanvas.transform.position;

        if (back)
        {
            vec.x += 1000f;    
            vec.y += 1000f;    
        }
        else
        {
            vec.x -= 1000f;
            vec.y -= 1000f;
        }
        
        uiCanvas.transform.position = vec;
    }
}
