using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//----------------------------------------------------------------------------------------
// Author: Joshua Rechkemmer, Jose Villanueva
//
// Description: This class controls the movement and actions of enemy npcs
//
//----------------------------------------------------------------------------------------
public class nEnemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyLoot
    {
        public GameObject item;
        [Range(0, 100)]
        public float dropChancePercentage;
    };

    public float attackRange = 1f;          //The distance in unity units to start attacking
    public float aggroRange = 10f;          //The distance in unity units to start aggressive behavior when player is in range
    //public float patrolTime = 15f;        //The wait time in seconds before moving to next waypoint
    public Transform[] waypoints;           //Locations that the npc will travel to
    public EnemyLoot[] loot;                //Items that the enemy can drop when killed
    public GameObject hitParticle;          //Particle System effect to make appear when hit
    public GameObject poofPartical;         //Poof particle
    public MonoBehaviour attackScript;

    private int index = 0;                  //Index of current waypoint
    private float agentSpeed;               //NavMesh movement speed. Maximum movement speed of enemy
    private float oldHealth;
    private bool isDead = false;

    private Transform player;               //Reference to the player's transform
    private Animator animator;              //Reference to animator component
    private NavMeshAgent agent;             //Reference to NavMeshAgent component
    public Renderer render;
    private Color ogColor;
    private float colorDelay = 1.0f;

    public delegate void EnemyEvent(string message);
    public event EnemyEvent enemyEvent;

    void Start()
    {
        //Initilize the script's variables
        oldHealth = GetComponent<nHealth>().amount;
        ogColor = render.material.color;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agentSpeed = agent.speed;

        //if there is no waypoints given, initialize a waypoint to be on the enemy's position
        if (waypoints.Length == 0)
        {
            waypoints = new Transform[1];
            waypoints[0] = this.transform;
        }
        else // do patroll
        {
            agent.destination = waypoints[index].position;  //Tell the navmesh to move the enemy to the waypoint
            agent.speed = agentSpeed / 2;                   //Set the movement to a walking pace
        }
    }

    void Update()
    {
        if (animator != null)
        {
            //If the current agent speed is zero,
            //      then it is idle, so play idle animation
            //Else
            //      it is moving, so play run animation
            if (agent.speed == 0)
            {
                animator.SetBool("IsIdle", true);
                animator.SetBool("IsMoving", false);
            }
            else
            {
                animator.SetBool("IsIdle", false);
                animator.SetBool("IsMoving", true);
            }
        }

        if (!isDead)
        {
            checkStatus();
        }

        //print("current state: " + currentState);
    }

    // subscribe to Health.OnUpdate() event
    private void OnEnable()
    {
        nHealth health = GetComponent<nHealth>();
        health.OnUpdate += HealthUpdated;
    }

    // unsubscribe to Health.OnUpdate() event
    private void OnDisable()
    {
        nHealth health = GetComponent<nHealth>();
        health.OnUpdate -= HealthUpdated;
    }

    // Does health reactions
    private void HealthUpdated(float amount)
    {
        ////print("Enemy health updated " + amount + " " + oldHealth);
        if (amount == 0) // enemy died
        {
            isDead = true;
            onDeath();
            render.material.SetColor("_BaseColor", Color.red);
            StartCoroutine(RendererTimer());
        }
        else if (amount < oldHealth) // enemy damaged
        {
            ////print("Enemy was damaged!");
            animator.SetTrigger("Hit"); //play hurt animation
            AudioManager.Instance.playRandom(transform.position, "Enemy_Hurt_01");    //play hurt sounds

            //Do the red flash on their renderer
            render.material.SetColor("_BaseColor", Color.red);
            Vector3 particlePos = transform.position;
            particlePos.y = particlePos.y + 1f;
            Instantiate(hitParticle, particlePos, transform.rotation);
            StartCoroutine(RendererTimer());
        }

        oldHealth = amount;
    }

    private IEnumerator RendererTimer()
    {
        float passed = 0.0f;

        while (passed < colorDelay)
        {
            passed += Time.deltaTime;
            yield return null;
        }

        render.material.SetColor("_BaseColor", ogColor);
    }

    //----------------------------------------------------------------------------------------
    // patrol() - check position and increment the current index of the waypoints array
    //
    //----------------------------------------------------------------------------------------
    private void patrol()
    {
        if (Vector3.Distance(transform.position, waypoints[index].position) < 0.3f)
        {
            index = (index == (waypoints.Length - 1)) ? 0 : index + 1; // increment index
        }
        agent.destination = waypoints[index].position;  //Tell the navmesh to move the enemy to the waypoint
        agent.speed = agentSpeed / 2;                   //Set the movement to a walking pace
    }

    //----------------------------------------------------------------------------------------
    // checkStatus() - control and change the behavior of the enemy
    //
    //----------------------------------------------------------------------------------------
    private void checkStatus()
    {
        //Attack behavior
        //Check if the player is w/in attackRange
        if (player != null && Vector3.Distance(transform.position, player.position) < attackRange)
        {
            agent.speed = 0;
            Vector3 dir = player.position;
            dir.y = transform.position.y;
            transform.LookAt(dir); //Rotate the enemy to face the player

            if (!attackScript.enabled)
            {
                attackScript.enabled = true;
            }
        }
        //Aggro behavior
        //Check if the player is w/in aggroRange
        else if (player != null && Vector3.Distance(transform.position, player.position) < aggroRange)
        {
            agent.destination = player.position;    //Tell the navmesh to move to the player
            agent.speed = agentSpeed;               //Set speed to their maximum speed
        }
        else
        {
            patrol();

            if (attackScript.enabled) // need to turn off attack script
            {
                attackScript.enabled = false;
            }
        }
    }

    private void dropLoot()
    {
        if (loot.Length != 0)
        {
            foreach (EnemyLoot drop in loot)
            {
                if (Random.Range(0, 100) <= drop.dropChancePercentage)
                {
                    Instantiate(drop.item, transform.position + new Vector3(0,1,0), transform.rotation); //Create the obj at the enemy's location
                }
            }
        }
    }

    private void onDeath()
    {
        attackScript.enabled = false;
        GetComponent<Collider>().enabled = false; //Turn off their collider
        agent.enabled = false;
        dropLoot();
        animator.SetTrigger("Death"); //Play the animation
        AudioManager.Instance.playRandom(transform.position, "Enemy_KO_01"); //Play Sound
        StartCoroutine(DelayedPoof(3.5f));
        StartCoroutine(DelayedDestruction(5)); //Wait 5 secs to destroy the enemy
    }

    private IEnumerator DelayedPoof(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(Instantiate(poofPartical, transform.position, poofPartical.transform.rotation), 1.5f);
    }

    private IEnumerator DelayedDestruction(float waiter)
    {
        yield return new WaitForSeconds(waiter);
        AudioManager.Instance.playRandom(transform.position, "Vegetable_Char_Poof_01");
        Destroy(gameObject);
    }

    public void sendEvent(string eventName)
    {
        if (enemyEvent != null)
        {
            enemyEvent(eventName);
        }
    }
}