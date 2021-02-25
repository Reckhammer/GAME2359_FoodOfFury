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
    public  float           aggroRange  = 10;   //The distance in unity units to start aggressive behavior when player is in range
    public  float           patrolTime  = 15;   //The wait time in seconds before moving to next waypoint
    public  Transform[]     waypoints;          //Locations that the npc will travel to

    private int     index = 0;                  //Index of current waypoint
    private float   agentSpeed;                 //NavMesh movement speed. Maximum movement speed of enemy

    private Transform       player;             //Reference to the player's transform
    private Animator        animator;           //Reference to animator component
    private NavMeshAgent    agent;              //Reference to NavMeshAgent component

    void Awake()
    {
        //Initilize the script's variables
        //animator = GetComponent<Animator>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag( "Player" ).transform;

        if ( agent != null )
        {
            agentSpeed = agent.speed;
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

    //----------------------------------------------------------------------------------------
    // patrol() - increment the current index of the waypoints array
    //
    //----------------------------------------------------------------------------------------
    private void patrol()
    {
        //if the index is at the last index of the array, set it to 0
        //else add 1 to the current index
        index = index == waypoints.Length - 1 ? 0 : index++;
    }

    private void checkStatus()
    {
        //Patrol behavior
        agent.destination = waypoints[index].position;  //Tell the navmesh to move the enemy to the waypoint
        agent.speed = agentSpeed / 2;   //Set the movement to a walking pace

        //Aggro behavior
        //Check if the player is w/in aggroRange
        if ( player != null && Vector3.Distance( transform.position, player.position ) < aggroRange )
        {
            agent.destination = player.position;    //Tell the navmesh to move to the player
            agent.speed = agentSpeed;   //Set speed to their maximum speed
        }
    }
}
