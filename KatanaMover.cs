using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaMover : MonoBehaviour
{
    public GameObject hand;

    private void Start()
    {
        transform.SetParent(hand.transform, false);
        transform.localScale = new Vector3(0.002f, 0.002f, 0.002f);

        Quaternion ro = hand.transform.rotation;
        transform.rotation = ro;
        transform.Rotate(45, 90, 0);


        // small offset to match hand properly
        transform.localPosition  = new Vector3(-0.0021f,0.0022f,-0.0021f);
    }

    void Update()
    {
        // if (!once)
        // {
        //     transform.SetParent(hand.transform, false);
        //     transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //     once = true;
        // }
        
        
        
        //move katana to girl's hand and rotate to match grip
        // Vector3 handPos = hand.transform.position;
        // // handPos.x += 0.08f;
        // handPos.z -= 0.02f;
        // handPos.y -= 0.06f;
        //

        // if (pos != null)
        // {
            // transform.position = pos;
        // }
        
        // transform.position = new Vector3(handPos.x - 0.0021f,handPos.y + 0.0022f,handPos.z - 0.0021f);
        
        // transform.position = new Vector3(-0.0021f,0.0022f,-0.0021f);
        
        
        
        // Quaternion handRotation = hand.transform.rotation;
        // Vector3 eulerRotation = handRotation.eulerAngles;
        //
        // eulerRotation.z = 100f;
        // eulerRotation.y -= 180f;
        // eulerRotation.x += 120f;
        // transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z);
    }
}
