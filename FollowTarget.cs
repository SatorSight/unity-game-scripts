using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public const float MIN_CAMERA_TURN_SPEED = 0.2f;
    // will be modified from settings, so use with adding to MIN_CAMERA_TURN_SPEED
    private float turnSpeed;
    public Camera camera;

    void Start()
    {
        // do it here because its heavy call
        camera = Camera.main;
        turnSpeed = 0.5f;
    }

    void Update()
    {
        // Debug.Log(turnSpeed);
        float turn = Input.GetAxis("Mouse X");
        float turnV = Input.GetAxis("Mouse Y");

        Vector3 vecH = turn < 0 ? Vector3.left : Vector3.right;
        Vector3 vecV = turnV < 0 ? Vector3.down : Vector3.up;

        Quaternion horizontalRotation = Quaternion.LookRotation(vecH);
        Quaternion verticalRotation = Quaternion.LookRotation(vecV);

        horizontalRotation.eulerAngles = new Vector3(horizontalRotation.eulerAngles.x, horizontalRotation.eulerAngles.y, 0f);
        verticalRotation.eulerAngles = new Vector3(verticalRotation.eulerAngles.x, verticalRotation.eulerAngles.y, 0f);

        // rotate horizontally
        transform.rotation *= Quaternion.Slerp(
            Quaternion.identity,
            horizontalRotation,
            Mathf.Abs(turn) * (MIN_CAMERA_TURN_SPEED + turnSpeed * 2) * Time.deltaTime
        );

        // rotate vertically
        transform.rotation *= Quaternion.Slerp(
            Quaternion.identity,
            verticalRotation,
            Mathf.Abs(turnV) * (MIN_CAMERA_TURN_SPEED + turnSpeed * 2) * Time.deltaTime
        );

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        
        // code below needed to limit camera angle rotation

        // 300 - 50
        // 0 - 90, 270 - 360
        float val = eulerRotation.x;

        if (val > 0 && val < 90)
        {
            // was 70, but aim camera needs less
            val = Mathf.Clamp(val, 0, 50);
        }
        else if (val > 200)
        // 270 - 360 range
        {
            val = Mathf.Clamp(val, 290, 360);
        }


        transform.rotation = Quaternion.Euler(val, eulerRotation.y, 0);
    }

    public void setTurnSpeed(float val)
    {
        turnSpeed = val;
    }
}
