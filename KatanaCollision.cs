using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaCollision : MonoBehaviour
{
    private Animator playerAnim;
    public AudioClip zombieHitSound;
    private AudioSource audioSource;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        playerAnim = player.GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (playerAnim.GetBool("Attacking") == true)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                EnemyController other = (EnemyController)collision.gameObject.GetComponent(typeof(EnemyController));
                other.damagedWithKatana();
                audioSource.PlayOneShot(zombieHitSound);
            }
            else
            {
                Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
            }
        }
    }
}
