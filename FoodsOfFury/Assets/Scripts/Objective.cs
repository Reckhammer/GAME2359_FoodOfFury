using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enumeration that helps manage the behavior of the objective
    //Defines what is the task for the objective
public enum ObjectiveType { Waypoint, Rescue }; //Enemy Wave???

//----------------------------------------------------------------------------------------
// Author: Joshua Rechkemmer
//
// Description: This class handles the behavior of an event the player needs to complete
//          to beat the level and sends a message to the LevelManager obj when completed
//----------------------------------------------------------------------------------------
public class Objective : MonoBehaviour
{
    

    public ObjectiveType    objectiveType;      //The type of objective of THIS obj
    public Health           health;             //Health component for the rescue cage obj

    private LevelManager lvlManager;            //Reference to the levelManager obj. for the level

   void Awake()
    {
        lvlManager = GameObject.Find( "LevelManager" ).GetComponent<LevelManager>();    //Get the reference to the lvlManager in the scene

        //if this is a rescue objective type
        //      Get a reference to the health component/script
        if ( objectiveType == ObjectiveType.Rescue )
            health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if ( objectiveType == ObjectiveType.Rescue && health.amount <= 0f )
        {
            lvlManager.setCompleted( this );
        }
    }

    void OnTriggerEnter( Collider other )
    {
        //If this is a waypoint objective type and the player is collided w/ the waypoint's trigger
        //      Send message to levelManager
        if ( objectiveType == ObjectiveType.Waypoint && other.gameObject.tag == "Player" )
        {
            lvlManager.setCompleted( this );
            gameObject.SetActive( false );
        }
    }
}
