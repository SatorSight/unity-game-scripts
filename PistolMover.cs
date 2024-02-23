using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolMover : MonoBehaviour
{
    public GameObject hand;
    public bool aiming = false;
    private Quaternion originalRotation;

    void Start()
    {
        aiming = false;
        
        // Vector3 handPos = hand.transform.position;
        // transform.position = handPos;
        // // Vector3 handPos = hand.transform.position;
        // transform.SetParent(hand.transform, true);
        // Vector3 pos = transform.position;
        // Quaternion ro = transform.rotation;
        // pos.x += 0.05f;
        // transform.position = pos;
        // transform.rotation = ro;
        
        
        
        transform.SetParent(hand.transform, false);
        transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);

        Quaternion ro = hand.transform.rotation;
        transform.rotation = ro;
        transform.Rotate(-90, -90, 0);


        // small offset to match hand properly
        transform.localPosition  = new Vector3(0,0.01113f,-0.00191f);
        originalRotation = transform.localRotation;


    }

    void Update()
    {

        if (aiming)
        {
            transform.localPosition = new Vector3(-0.0003f, 0.0135f, -0.0022f);
            transform.localRotation = new Quaternion(-0.356173664f, -0.613138139f, -0.568636835f, 0.416958123f);
        }
        else
        {
            transform.localPosition  = new Vector3(0,0.01113f,-0.00191f);
            transform.localRotation = originalRotation;
        }
        
        // Quaternion(-0.356173664f,-0.613138139f,-0.568636835f,0.416958123f)
        // Vector3(-0.0003f,0.0135f,-0.0022f)
        
        
        
        
        // move pistol to girl's hand and rotate to match grip
        // Vector3 handPos = hand.transform.position;
        // Quaternion handRotation = hand.transform.rotation;
        // Vector3 eulerRotation = handRotation.eulerAngles;
        // if (aiming)
        // {
            // eulerRotation.y += 60f;
            // eulerRotation.z += 180f;
            //
            // handPos.y += 0.02f;
            // handPos.z += 0.07f;
            // handPos.x -= 0.03f;
        
        // }
        // else
        // {
        //     handPos.y -= 0.05f;
            // handPos.x += 0.02f;
            // handPos.z -= 1.06f;
            
            // eulerRotation.y += 90f;
            // eulerRotation.z += 60f;
            // eulerRotation.x += +90f;
        // }
        // transform.position = handPos;
        // transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z);
    }
}
