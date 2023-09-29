using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolMover : MonoBehaviour
{
    public GameObject hand;
    public bool aiming = false;

    void Start()
    {
        aiming = false;
    }

    void Update()
    {
        // move pistol to girl's hand and rotate to match grip
        Vector3 handPos = hand.transform.position;
        Quaternion handRotation = hand.transform.rotation;
        Vector3 eulerRotation = handRotation.eulerAngles;
        if (aiming)
        {
            eulerRotation.y += 60f;
            eulerRotation.z += 180f;

            handPos.y += 0.02f;
            handPos.z += 0.07f;
            handPos.x -= 0.03f;

        }
        else
        {
            handPos.y -= 0.1f;
            handPos.x -= 0.02f;
            handPos.z -= 0.06f;
            
            eulerRotation.y += 90f;
            eulerRotation.z += 60f;
            eulerRotation.x += +90f;
        }
        transform.position = handPos;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z);
    }
}
