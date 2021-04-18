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

    public string           message;            //Text message telling the player what to do
    public ObjectiveType    objectiveType;      //The type of objective of THIS obj

    private bool isDone = false;             //Boolean if the objective is done

    private LevelManager lvlManager;            //Reference to the levelManager obj. for the level

   void Awake()
    {
        lvlManager = GameObject.Find( "LevelManager" ).GetComponent<LevelManager>();    //Get the reference to the lvlManager in the scene

        //if message is not initialized
        //      Set it to a default value
        if ( message == null )
        {
            message = "Objective message not initialized";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter( Collider other )
    {
        //If this is a waypoint objective type and the player is collided w/ the waypoint's trigger
        //      Send message to levelManager
        if ( objectiveType == ObjectiveType.Waypoint && other.gameObject.tag == "Player" )
        {
            lvlManager.setCompleted( this );
            gameObject.SetActive( false );
            isDone = true;
        }
        
    }

    void OnTriggerStay( Collider other )
    {
        if (objectiveType == ObjectiveType.Rescue && other.gameObject.tag == "Player")
        {
            //if player hits the interact key AND has a key
            //      Objective is complete
            GameObject player = other.gameObject;

            if (Input.GetKeyDown(KeyCode.F) && player.GetComponent<Inventory>().keyCount > 0)
            {
                lvlManager.setCompleted( this );
                GetComponent<MeshRenderer>().enabled = false;

                GetComponentInChildren<Animator>().SetBool("IsRescued", true);
                isDone = true;
            }

        }
    }
}
