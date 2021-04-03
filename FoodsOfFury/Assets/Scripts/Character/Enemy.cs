using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//----------------------------------------------------------------------------------------
// Author: Joshua Rechkemmer
//
// Description: This class controls the movement and actions of enemy npcs
//
//----------------------------------------------------------------------------------------
public class Enemy : MonoBehaviour
{
    //Enumeration to describe attacking behavior
    public enum EnemyType { Melee, Range }; //Maybe we could make an enemy with both melee and range attacks?

    public  EnemyType       attackType = EnemyType.Melee;   //Attack behavior of the enemy
    public  float           attackRange = 1f;               //The distance in unity units to start attacking
    public  float           aggroRange  = 10f;              //The distance in unity units to start aggressive behavior when player is in range
    public  float           patrolTime  = 15f;              //The wait time in seconds before moving to next waypoint
    public  float           attackRate  = 2f;               //The time between attacks
    public  float           projectileSpeed = 100f;         //Speed of the projectile
    public  Transform[]     waypoints;                      //Locations that the npc will travel to
    public  Transform       attackPoint;                    //Child gameObj that the projectiles will come from or that is the hitbox of melee
    public  GameObject      projectile;                     //GameObj. that will be shot out from attackPoint if it's a ranged enemy
    public  Animation       meleeAttackAnim;                //Melee attack animation for enemy

    private int     index = 0;                  //Index of current waypoint
    private float   agentSpeed;                 //NavMesh movement speed. Maximum movement speed of enemy
    private float   nextFire;                   //The next point in time where the enemy can attack again
    private float   oldHealth = 0;

    private Transform       player;             //Reference to the player's transform
    private Animator        animator;           //Reference to animator component
    private NavMeshAgent    agent;              //Reference to NavMeshAgent component

    void Awake()
    {
        //Initilize the script's variables
        //animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag( "Player" ).transform;

        oldHealth = GetComponent<Health>().amount;

        if ( agent != null )
        {
            agentSpeed = agent.speed;
        }

        //if there is now waypoints given, initialize a waypoint to be on the enemy's position
        if ( waypoints == null )
        {
            waypoints = new Transform[1];
            waypoints[0] = this.transform;
        }

        //Do the checkStatus function repeatedly
        InvokeRepeating( "checkStatus", 0, 0.5f );

        //Increment the waypoint if there is more than 1
        if ( waypoints.Length > 1 )
        {
            InvokeRepeating( "patrol", 0, patrolTime );
        }
    }

    void Update()
    {
        //put animator code here
    }

    // subscribe to Health.OnUpdate() event
    private void OnEnable()
    {
        Health health = GetComponent<Health>();
        health.OnUpdate += HealthUpdated;
    }

    // unsubscribe to Health.OnUpdate() event
    private void OnDisable()
    {
        Health health = GetComponent<Health>();
        health.OnUpdate -= HealthUpdated;
    }

    // Does health reactions
    private void HealthUpdated( float amount )
    {
        if (amount == 0) // // player died
        {
            onDeath();
        }
        else if (amount < oldHealth) // player damaged
        {
            print("Enemy was damaged!");
            //play hurt sounds
            // hurt animations?
        }

        oldHealth = amount;
    }

    //----------------------------------------------------------------------------------------
    // patrol() - increment the current index of the waypoints array
    //
    //----------------------------------------------------------------------------------------
    private void patrol()
    {
        //if the index is at the last index of the array, set it to 0
        //else add 1 to the current index
        index = index == ( waypoints.Length - 1 ) ? 0 : index + 1;
    }

    //----------------------------------------------------------------------------------------
    // attack() - handles the behavior when the enemy is attacking and damages player
    //
    //----------------------------------------------------------------------------------------
    private void attack()
    {
        //Check if the enemy can attack again
        if ( Time.time > nextFire )
        {
            //Use switch statement to jump to the code to do corresponding attack behavior
            switch( attackType )
            {
                case EnemyType.Melee:
                    //Collider[] hitEnemies = Physics.OverlapSphere( attackPoint.position, attackRange, LayerMask.NameToLayer( "Player" ) );   //See if the attackPoint is colliding with the player
                    //Debug.Log( "Enemy Hit player" );
                    //Insert damaging code here
                    print("attacking");
                    if (!meleeAttackAnim.isPlaying)
                    {
                        meleeAttackAnim.Play();
                        AudioManager.Instance.playRandom(transform.position, "IceCream_Cone_Attack01"); 
                    }
                    break;
                case EnemyType.Range:
                    GameObject projectileInst = Instantiate( projectile, attackPoint.position, transform.rotation ); //Create the projectile
                    Rigidbody projectileRB = projectileInst.GetComponent<Rigidbody>(); //Get a reference to its rigidbody

                    AudioManager.Instance.playRandom(transform.position, "Fry_Attack_01");

                    break;
            }

            nextFire = Time.time + attackRate;
        }
    }

    //----------------------------------------------------------------------------------------
    // checkStatus() - control and change the behavior of the enemy
    //
    //----------------------------------------------------------------------------------------
    private void checkStatus()
    {
        //Patrol behavior
        agent.destination = waypoints[index].position;  //Tell the navmesh to move the enemy to the waypoint
        agent.speed = agentSpeed / 2;   //Set the movement to a walking pace

        //print("distance: " + Vector3.Distance(transform.position, player.position));
        //Attack behavior
        //Check if the player is w/in attackRange
        if ( player != null && Vector3.Distance( transform.position, player.position ) < attackRange )
        {
            agent.speed = 0;
            transform.LookAt( player ); //Rotate the enemy to face the player
            attack();
        }
        //Aggro behavior
        //Check if the player is w/in aggroRange
        else if ( player != null && Vector3.Distance( transform.position, player.position ) < aggroRange )
        {
            agent.destination = player.position;    //Tell the navmesh to move to the player
            agent.speed = agentSpeed;   //Set speed to their maximum speed
        }
    }

    private void onDeath()
    {
        Destroy( gameObject );
    }
}
