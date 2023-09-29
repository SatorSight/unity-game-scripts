using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private Camera camera;
    private GameObject healthBar;
    private GameObject canvas;
    private GameObject myHealthBar;
    private GameObject innerBar;
    private GameObject player;
    private Animator anim;
    private Rigidbody body;
    private GameObject[] blood;
    private float damagedStarts = 0f;

    public LayerMask IgnoreMe;
    public float health = 5f;
    private UnityEngine.AI.NavMeshAgent agent;

    private RaycastHit hitInfo = new RaycastHit();

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        anim = GetComponent<Animator>();
        healthBar = GameObject.Find("EnemyHealthBar");
        canvas = GameObject.Find("EnemyCanvas");
        player = GameObject.FindWithTag("Player");

        prepareBlood();

        myHealthBar = Instantiate(healthBar);
        myHealthBar.transform.SetParent(canvas.transform, false);

        body = GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void Update()
    {
        if (isDead())
        {
            return;
        }


        attackIfPlayerIsNear();

        handleMoveAnimation();

        if (myHealthBar)
        {
            handleHealthBar();
        }
        
        if (health < 0)
        {
            die();
        }
    }

    private void attackIfPlayerIsNear()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // if far from player, move, otherwise don't move and attack
        if (distance > 1f)
        {
            anim.SetBool("Attacking", false);
            agent.isStopped = false;

            if (playerInSight())
            {
                // hitInfo is set inside playerInSight function
                Collider col = hitInfo.collider;
                if (col && col.tag == "Player")
                {
                    agent.destination = player.transform.position;
                }
            }
        }
        else
        {
            AttackPlayer();
            agent.isStopped = true;
        }
    }

    private bool playerInSight()
    {
        Vector3 rayFromMe = transform.position;
        rayFromMe.y += 1f;

        // (un)comment to debug ray
        Debug.DrawRay(rayFromMe, (player.transform.position - transform.position), Color.red);
        return Physics.Raycast(rayFromMe, (player.transform.position - transform.position), out hitInfo, 1000f, ~IgnoreMe);
    }

    private void handleMoveAnimation()
    {
        if (agent.velocity.magnitude > 2f)
        {
            anim.SetBool("Moving", true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }
    }

    private void AttackPlayer()
    {
        anim.SetBool("Attacking", true);
    }

    private void handleHealthBar()
    {
        myHealthBar.transform.position = transform.position;
        Vector3 healthBarPos = myHealthBar.transform.position;
        healthBarPos.y += 2f;
        myHealthBar.transform.position = healthBarPos;

        innerBar = myHealthBar.transform.Find("HealthBar/HealthBarInner").gameObject;

        RectTransform rect = (RectTransform)innerBar.GetComponent(typeof(RectTransform));
        rect.offsetMin = new Vector2(5f - health, rect.offsetMin.y);
        myHealthBar.transform.rotation = Quaternion.LookRotation((transform.position - camera.transform.position).normalized);
    }

    public void damagedWithKatana()
    {
        if (Time.time > damagedStarts + 0.5f)
        {
            damagedStarts = Time.time;
            health -= 2f;
            anim.SetBool("Damaged", true);

            displayBlood();

            Vector3 fromPlayerVec = player.transform.position - body.transform.position;

            fromPlayerVec.y = 0f;
            fromPlayerVec.Normalize();
            body.AddForce(-fromPlayerVec * 50, ForceMode.Impulse);
            StartCoroutine(Damaged());
        }
    }

    private bool isDead()
    {
        return anim.GetBool("Dead");
    }

    private void die()
    {
        anim.SetBool("Moving", false);
        anim.SetBool("Dead", true);
        Object.Destroy(myHealthBar);
        body.isKinematic = true;
        agent.isStopped = true;
    }

    public void prepareBlood()
    {
        blood = new GameObject[3];

        GameObject globalBlood = GameObject.Find("BloodSpray");
        GameObject blood1 = Instantiate(globalBlood);
        GameObject blood2 = Instantiate(globalBlood);
        GameObject blood3 = Instantiate(globalBlood);


        blood1.transform.SetParent(transform, false);
        blood2.transform.SetParent(transform, false);
        blood3.transform.SetParent(transform, false);

        Vector3 tempPos = transform.position;
        tempPos.y += 1f; // 1.25, 1.5
        blood1.transform.position = tempPos;
        tempPos.y += 0.25f;
        blood2.transform.position = tempPos;
        tempPos.y += 0.25f;
        blood3.transform.position = tempPos;


        blood1.transform.Rotate(0, 45, 0);
        blood2.transform.Rotate(0, 0, 0);
        blood3.transform.Rotate(0, -45, 0);

        blood[0] = blood1;
        blood[1] = blood2;
        blood[2] = blood3;

        for (int i = 0; i < 3; i++)
        {
            blood[i].active = false;
        }
    }

    public void displayBlood()
    {
        for(int i = 0; i < 3; ++i)
        {
            blood[i].active = true;
        }
    }

    public void hideBlood()
    {
        for (int i = 0; i < 3; ++i)
        {
            blood[i].active = false;
        }
    }

    IEnumerator Damaged()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Damaged", false);
        hideBlood();
    }
}
