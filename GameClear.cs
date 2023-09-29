using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClear : MonoBehaviour
{
    private bool triggered = false;
    void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            triggered = true;
            var dialogTrigger = this.gameObject.GetComponent<DialogueTrigger>();
            dialogTrigger.TriggerDialogue();    
        }
    }
}
