using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaMover : MonoBehaviour
{
    public GameObject hand;

    void Update()
    {
        //move katana to girl's hand and rotate to match grip
        Vector3 handPos = hand.transform.position;
        handPos.x += 0.02f;
        handPos.z += 0.02f;
        handPos.y -= 0.05f;

        transform.position = handPos;
        Quaternion handRotation = hand.transform.rotation;
        Vector3 eulerRotation = handRotation.eulerAngles;

        eulerRotation.z -= 60f;
        eulerRotation.x -= 100f;
        // eulerRotation.x -= 90f;
        transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, eulerRotation.z);
    }
}
