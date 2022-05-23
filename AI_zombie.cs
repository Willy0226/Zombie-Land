using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.FirstPerson;

public class AI_zombie : MonoBehaviour
{
    // var
    public float patrolSpeed = 0.5f; // patrol speed
    public float chaseSpeed = 1f; // chase speed
    public float manicSpeed = 2.5f; // manic speed
    public float health = 60f; // zombie health
    public float damage = 10f; // zomibe attack damage
    public float manicDamage = 15; // manic attack damage
    public float knockbackForce = 30f; // zombie knockback
    private float fov = 120f; // field of vision
    private float viewDistance = 10f; // vision distance
    private float orgin_x; // orginal x-axis (for the self destroy)
    private float orgin_z; // orginal z-axis (for the self destroy)
    public GameObject[] bloods; // blood array

    //public int randomMoney = Random.Range(20, 50);  // get random money after zombie die

    // timer
    private float destroyTime = 20f; // self destroy timer

    public float attackTime = 0f; // current attack timer
    public float startingTime = 0.5f; // default attack timer

    public float detectTime = 0f; // detecting timer
    private float loseHold = 3f; // count down hold if zombie lose the player after stop detecting

    public float manicTime = 0f; // current manic timer
    private float endTime = 45f; // default manic timer

    // flag
    public bool isDetecting = false;
    public bool isAware = false;
    public bool isAttack = false;
    public bool isDead = false;
    public bool isManic = false;
    private bool getNewPosition = false;

    //radius
    private float attackRadius = 2f; // attackRadius
    private float patrolRadius = 7f; // patrolRadius

    // invoke
    private FirstPersonController fpsc;
    public Vector3 patrolPoint;
    private NavMeshAgent agent;
    private Renderer renderer;
    private Animator animator;
    private HealthSystem healthSystem;
    private PlayerGameManager PM;
    private Collectible CM;

    //----------------------------------------------------------------------------------------------//
    //----------------------------------------Founction---------------------------------------------//
    //----------------------------------------------------------------------------------------------//


    // Radius color set up
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red; // radius color
        Gizmos.DrawWireSphere(transform.position, attackRadius); // attack radius
        Gizmos.DrawWireSphere(transform.position, patrolRadius); // possible patrol radius
    }

    // starting set up
    public void Start()
    {
        fpsc = GameObject.FindGameObjectWithTag("Player").GetComponent<FirstPersonController>();
        healthSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthSystem>();
        PM = GameObject.Find("GameManager").GetComponent<PlayerGameManager>();
        agent = GetComponent<NavMeshAgent>();
        renderer = GetComponent<Renderer>();
        animator = GetComponent<Animator>();

        CM = GameObject.Find("Collectibles").GetComponent<Collectible>();

        patrolPoint = RandomPoint();

        attackTime = startingTime; // attack delay timer initialize
        manicTime = endTime; // manic  delay timer initialize
        getNewPosition = true; // set to trur to get first init position
    }

    public void Update()
    {
        if (isDead == false)
        {
            SearchForPlayer();
        } // always search the player(vision)

        if (isManic == true)
        {
            manicTime -= 1 * Time.deltaTime; // manic time countdown

            Manic(); // manic

            if (manicTime <= 0) // reset once time reachs 0
            {
                isManic = false;
                manicTime = endTime;
            }
        }
        else if (isAware == true && isDead == false)
        {
            Chasing(); // chase
            Attacking(); // attack
            Detecting(); // detect
        }
        else if (isDead == false)
        {
            Patroling(); // patrol
        }

        if (isManic == false)
        {
            OnHardModeChecker(); // check if it is manic now
        }

        if (getNewPosition == true)
        {
            StartCoroutine(getPosition()); // get position for the self destroy detection
            StartCoroutine(getPosition2()); // get position for the self destroy detection
            return;
        }
    }

    // damage delay
    private void DamageDelay()
    {
        // Get the distance to the player
        float distance = Vector3.Distance(fpsc.transform.position, transform.position);

        attackTime -= 1 * Time.deltaTime; // timer count down

        // if timer = 0 and player still in the attack radius
        if (attackTime <= 0 && (distance <= attackRadius))
        {
            if (isManic == true)
            {
                healthSystem.DamageToPlayer(manicDamage);
            }
            else if (isManic == false)
            {
                healthSystem.DamageToPlayer(damage);
            }
            attackTime = startingTime;
        }
    }

    // face target
    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5);
    }

    // vision (if zombie not in the one of thoses condition, then we define isDetecting = false; once !isDetecting, the timer start counting.)
    public void SearchForPlayer()
    {
        if (
            Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(fpsc.transform.position))
            < fov / 2f
        ) // vision angle condition
        {
            if (Vector3.Distance(fpsc.transform.position, transform.position) < viewDistance) // vision distance condition
            {
                RaycastHit hit;
                if (Physics.Linecast(transform.position, fpsc.transform.position, out hit, -1)) // not detecting player condition
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        OnAware();
                    }
                    else
                    {
                        isDetecting = false;
                    }
                }
                else
                {
                    isDetecting = false;
                }
            }
            else
            {
                isDetecting = false;
            }
        }
        else
        {
            isDetecting = false;
        }
    }

    // chase
    private void Chasing()
    {
        agent.SetDestination(fpsc.transform.position); // chase player
        agent.speed = chaseSpeed; // speed change
        FaceTarget(fpsc.transform.position); // face player
        animator.SetBool("Aware", true);
    }

    // attack
    private void Attacking()
    {
        // Get the distance to the player
        float distance = Vector3.Distance(fpsc.transform.position, transform.position);

        // if it's in the attack range
        if (distance <= attackRadius)
        {
            isAttack = true;
            DamageDelay(); // attack(but delay)

            animator.SetBool("Attack", true);
        }
        else
        {
            isAttack = false;
            attackTime = startingTime; // reset delay timer once player out of attack radius

            animator.SetBool("Attack", false);
        }
    }

    // detect
    private void Detecting()
    {
        // if zombie in aware state but not detecting player
        if (!isDetecting)
        {
            detectTime += Time.deltaTime; // detecing timer start count down

            // if timer exceeds holder then zombie lose the player
            if (detectTime >= loseHold)
            {
                isAware = false; // lose player
                detectTime = 0f; //  detecting timer reset
            }
        }
    }

    // patrol
    private void Patroling()
    {
        agent.speed = patrolSpeed; // move with patrol speed
        animator.SetBool("Aware", false);

        // else if zombie position to random point < setting range (3f)
        if (Vector3.Distance(transform.position, patrolPoint) < 2.5f)
        {
            patrolPoint = RandomPoint(); // set up a new random point
        }
        else // else keep toward that destination
        {
            agent.SetDestination(patrolPoint);
        }
    }

    // manic mode code
    private void Manic()
    {
        agent.SetDestination(fpsc.transform.position); // chase player
        FaceTarget(fpsc.transform.position); // face player

        agent.speed = manicSpeed; // speed change

        animator.SetBool("Aware", true);

        Attacking(); // attack
    }

    // Aware flag
    public void OnAware()
    {
        isAware = true;
        isDetecting = true;
        detectTime = 0f;
    }

    // Attack flag
    public void OnAttack()
    {
        isAttack = true;
    }

    // Manic flag
    public void OnManic()
    {
        isManic = true;
    }

    // Hard mode check
    private void OnHardModeChecker()
    {
        if (PM.zombieKilled != 0 && PM.zombieKilled % PM.ZombiesToHardModeRound == 0)
        {
            isManic = true;
        }
    }

    // random patrol point
    private Vector3 RandomPoint()
    {
        // make a random point in the sphere radius
        Vector3 randomPoint = (Random.insideUnitSphere * patrolRadius) + transform.position;

        // set up a random point inside the navmesh map
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomPoint, out navHit, patrolRadius, -1);

        // return a new random point in the navmesh map
        return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
    }

    // zombie knock back
    public void KnockBack()
    {
        if (isDead == true)
        {
            agent.transform.position += transform.forward * -1 * Time.deltaTime * knockbackForce; // -1 = backward
        }
    }

    // zombie blooding
    public void Blooding()
    {
        GameObject blooding = Instantiate(
            bloods[Random.Range(0, bloods.Length)],
            new Vector3(
                agent.transform.position.x,
                agent.transform.position.y - 0.9f,
                agent.transform.position.z
            ),
            agent.transform.rotation
        );
        Destroy(blooding, 60f);
    }

    // zombie got shoot
    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    // zombie dead
    private void Die()
    {
        // bool setting
        isDead = true;
        isAware = false;
        isAttack = false;
        isDetecting = false;
        agent.isStopped = true;
        agent.GetComponent<Collider>().enabled = false;
        agent.velocity = Vector3.zero;
        agent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        animator.SetBool("Dead", true);

        PM.zombieKilled++; // kill number +1
        PM.GetMoney(60); // get money
        PM.Days(); // check days

        //Spawn Collectible
        CM.SpawnCollectible(gameObject.transform);

        Destroy(gameObject, 10f); // destroy zombie after 10 sec

        return;
    }

    // get zombie position (hold the thread for a certain time)
    private IEnumerator getPosition()
    {
        orgin_x = gameObject.transform.position.x; // get orginal x position
        orgin_z = gameObject.transform.position.z; // get orginal z position

        getNewPosition = false; // disabled bool to stop updating position

        yield return new WaitForSeconds(destroyTime); // wait for next update

        selfDestroy(); // self destroy detect
    }

    // get zombie position (hold the thread for a certain time)
    private IEnumerator getPosition2()
    {
        orgin_x = gameObject.transform.position.x; // get orginal x position
        orgin_z = gameObject.transform.position.z; // get orginal z position

        getNewPosition = false;

        yield return new WaitForSeconds(4); // wait for next update

        resetPath();
    }

    // reset path
    private void resetPath()
    {
        // compute the distance between orgin position to current position => sqrt((x1 - o1)^2 + (x2 - o2)^2)
        double deltaDistance = Mathf.Sqrt(
            Mathf.Pow(transform.position.x - orgin_x, 2)
                + Mathf.Pow(transform.position.z - orgin_z, 2)
        );

        // if not move out of a certian range
        if (deltaDistance <= 0.1f)
        {
            Debug.Log("Reset path due to detecting not move in a certain time");
            patrolPoint = RandomPoint(); // set up a new random point
        }
    }

    // self destroy
    private void selfDestroy()
    {
        // compute the distance between orgin position to current position => sqrt((x1 - o1)^2 + (x2 - o2)^2)
        double deltaDistance = Mathf.Sqrt(
            Mathf.Pow(transform.position.x - orgin_x, 2)
                + Mathf.Pow(transform.position.z - orgin_z, 2)
        );

        // Get the distance from zombie to the player
        float distance = Vector3.Distance(fpsc.transform.position, transform.position);

        // if not move out of a certian range and not in attack range
        if (deltaDistance <= 0.1f && distance >= attackRadius)
        {
            Debug.Log("Self destroy due to detecting not move in a certain time");
            Destroy(gameObject);
        }
        // else restart the loop
        else
        {
            getNewPosition = true;
        }
    }
}
