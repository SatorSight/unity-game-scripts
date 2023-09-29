using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCube : MonoBehaviour
{
    public Animator anim;
    private float attackStarts = 0f;

    void OnTriggerStay(Collider other)
    {
        if (anim.GetBool("Attacking") == true)
        {
            if (other.gameObject.tag == "Player")
            {
                if(Time.time - attackStarts > 2f)
                {
                    GirlMover player = (GirlMover)other.gameObject.GetComponent(typeof(GirlMover));
                    player.damagedByEnemy();

                    attackStarts = Time.time;
                }
            }
        }
    }
}
