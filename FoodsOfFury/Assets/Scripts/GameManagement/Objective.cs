using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Enumeration that helps manage the behavior of the objective
    //Defines what is the task for the objective
public enum ObjectiveType { Waypoint, Rescue, Multi }; //Enemy Wave???

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

    public bool isDone = false;                //Boolean if the objective is done
    public bool needKey = false;               //Boolean if player need key

    private LevelManager lvlManager;            //Reference to the levelManager obj. for the level
    private AudioSource cageSounds;              //Cage Sounds -Brian

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
        if (objectiveType == ObjectiveType.Rescue && other.gameObject.tag == "Player"  && other.gameObject.GetComponent<Inventory>() != null && !isDone)
        {
            //if player hits the interact key AND has a key
            //      Objective is complete
            Inventory player = other.gameObject.GetComponent<Inventory>();

            if ( Input.GetKeyDown( KeyCode.Mouse0 ) && player.keyCount > 0 )
            {
                lvlManager.setCompleted( this );
                GetComponent<MeshRenderer>().enabled = false;   //Disable cage visual
                transform.Find( "Cage Collider" ).GetComponent<Collider>().enabled = false; //Disable cage collider

                ////print("saved!!!");
                player.addKey( -1 );
                UIManager.instance.updateKeyUI();
                AudioManager.Instance.playRandom(transform.position, "Cage_Unlock_01"); //Cage unlock sound -Brian
                AudioManager.Instance.playRandom(transform.position, "Vegetable_Yeah_01", "Vegetable_Yeah_02");

                if ( GetComponentInChildren<Animator>() != null )
                {
                    GetComponentInChildren<Animator>().SetBool( "IsRescued", true );
                }

                //StartCoroutine( DelayedDestruction( 5 ) );
                isDone = true;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse0) && player.keyCount < 0)
            {
                needKey = true;
                StartCoroutine(KeyUITimer());
            }

        }
    }

    private IEnumerator KeyUITimer()
    {
        yield return new WaitForSeconds(3);
        needKey = false;
    }

    private IEnumerator DelayedDestruction( float waiter )
    {
        yield return new WaitForSeconds(waiter);
        AudioManager.Instance.playRandom(transform.position, "Vegetable_Char_Poof_01");
        gameObject.SetActive( false );
    }
}
