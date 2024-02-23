using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaCollision : MonoBehaviour
{
    private Animator playerAnim;
    public AudioClip zombieHitSound;
    private AudioSource audioSource;

    private bool hitHappened = false;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerAnim = player.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    
    // TODO: need to ignore collision when not attacking
    
    private void OnTriggerStay(Collider other)
    {
        // other.
        if (playerAnim.GetBool("Slashing") == true)
        {
            if (other.gameObject.tag == "Enemy")
            {
                EnemyController enemy = (EnemyController)other.gameObject.GetComponent(typeof(EnemyController));
                enemy.damagedWithKatana();
                audioSource.PlayOneShot(zombieHitSound);
                hitHappened = true;
            } else if (other.gameObject.tag == "Destructible")
            {
                // TODO: generalize with some sort of Destructible interface
                CrashCrate crate = (CrashCrate)other.gameObject.GetComponent(typeof(CrashCrate));
                crate.destroyed();
            }
        }
        // Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            hitHappened = false;
        }
    }


    // private void OnTriggerEnter(Collider other)
    // {
    //     if (isCollisionWithPlayer(other))
    //     {
    //         Debug.Log("should zoom in camera");
    //         StartCoroutine(ChangeValueOverTime(DEFAULT_ZOOM, INSIDE_BUILDING_ZOOM));
    //         
    //         
    //         // camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, 0, INSIDE_BUILDING_ZOOM);    
    //     }
    // }
    //
    // private void OnTriggerExit(Collider other)
    // {
    //     if (isCollisionWithPlayer(other))
    //     {
    //         Debug.Log("should zoom out camera");
    //         StartCoroutine(ChangeValueOverTime(INSIDE_BUILDING_ZOOM, DEFAULT_ZOOM));
    //         // camera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(0, 0, DEFAULT_ZOOM);
    //     }
    // }
    //
    // private bool isCollisionWithPlayer(Collider collider)
    // {
    //     return collider.gameObject.layer == LayerMask.NameToLayer("Player");
    // }
}
