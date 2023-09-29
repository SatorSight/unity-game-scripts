using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// basically an item handler
public class FollowItem : MonoBehaviour
{
    // public because we set it from ItemController
    public GameObject target;
    public float distanceFromObject = 1f;
    public float distanceFromPlayer = 5f;
    private Camera camera;
    private GameObject player;
    private DialogueManager dialog;

    void Start()
    {
        camera = Camera.main;
        player = GameObject.FindWithTag("Player");
        dialog = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
        if (!target)
        {
            return;
        }
        // text should be above the object its attached to
        Vector3 targetPos = target.transform.position;
        targetPos.y += distanceFromObject;
        if (target.tag == "Door")
        {
            // if player is in front or behind door, move to respective side
            Vector3 directionVector = (player.transform.position - targetPos).normalized;
            if (directionVector.z < 0)
            {
                targetPos.z -= 0.4f;
            }
            else
            {
                targetPos.z += 0.4f;
            }
        }
        transform.position = targetPos;

        // face towards camera but reverse rotation, otherwise text is displayed backwards
        transform.rotation = Quaternion.LookRotation((transform.position - camera.transform.position).normalized);


        handleVisibility();
        processItemInteraction();
    }

    private void handleVisibility()
    {
        if (isVisibleToPlayer())
        {
            this.GetComponent<Renderer>().enabled = true;
        }
        else
        {
            this.GetComponent<Renderer>().enabled = false;
        }
    }

    private void processItemInteraction()
    {
        if (isVisibleToPlayer())
        {
            if (Input.GetKeyDown("e") == true)
            {
                if (target.tag == "Door")
                {
                    openDoor();
                }
                else
                {
                    addToPlayerInventory();
                    itemTaken();
                }
                
            }
        }
    }

    private bool isVisibleToPlayer()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        return distance < distanceFromPlayer && isInView();

    }

    private void openDoor()
    {
        var other = player.GetComponent<GirlMover>();
        if (target.name == "metal_gates_of_the_USSR")
        {
            if (!other.canOpenGate())
            {

                

                var dialogTrigger = target.GetComponent<DialogueTrigger>();
                dialog.StartDialogue(dialogTrigger.dialogue);
                
                
                return;
            }
        }
        
        
        
        Animator targetAnimator = target.GetComponent<Animator>();
        if (targetAnimator.GetBool("Open"))
        {
            targetAnimator.SetBool("Open", false);
        }
        else
        {
            targetAnimator.SetBool("Open", true);
        }
    }

    private bool isInView()
    {
        Vector3 cameraForwardVec = camera.transform.forward;
        Vector3 toObjVec = transform.position - camera.transform.position;

        cameraForwardVec.Normalize();
        toObjVec.Normalize();

        float cameraAngle = Vector3.Dot(cameraForwardVec, toObjVec);

        // check if angle between camera forward vector and object->camera vector are facing almost same direction
        // feels around 20-30 degrees around center
        if (cameraAngle > 0.99f)
        {
            return true;
        }
        return false;
    }

    private void itemTaken()
    {
        Destroy(target);
        Object.Destroy(this.gameObject);
    }

    private void addToPlayerInventory()
    {
        GirlMover other = (GirlMover)player.GetComponent(typeof(GirlMover));
        other.handlePickingAnimation();

        if (target.tag == "Weapon")
        {
            if (target.name == "PistolItem")
            {
                other.hasPistol = true;
                var dialogTrigger = target.GetComponent<DialogueTrigger>();
                dialog.StartDialogue(dialogTrigger.dialogue);
            }
            if (target.name == "KatanaItem")
            {
                other.hasKatana = true;
                var dialogTrigger = target.GetComponent<DialogueTrigger>();
                dialog.StartDialogue(dialogTrigger.dialogue);
            }
        }
        else if(target.tag == "Ammo")
        {
            other.addAmmo();
        }
        else
        {
            other.addToInventory();
        }
    }
}
