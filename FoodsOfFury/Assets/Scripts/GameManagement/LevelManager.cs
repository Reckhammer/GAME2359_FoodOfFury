using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//----------------------------------------------------------------------------------------
// Author: Joshua Rechkemmer
//
// Description: This class handles completion of objectives and the end of the level
//      when all objectives are done
//----------------------------------------------------------------------------------------
public class LevelManager : MonoBehaviour
{
    public List<Objective> objectiveList = new List<Objective>();    //list of all of the objectives for the level

    private Health  playerHealth;       //the player's heath


    public Image    endGameMenu;        //UI elements for the level completion
    public Text     endGameText;        //The text for the UI
    public Button   nextLvlBtn;         //Reference to the next level button ui
    public Button   restartBtn;         //Reference to the restart button ui
    public float    waitTime = .75f;    //Wait time for the popup to come up in seconds

    void Awake()
    {
        Time.timeScale = 1;     //this resets the timescale after switching scenes

        playerHealth = GameObject.FindGameObjectWithTag( "Player" ).GetComponent<Health>(); //get the reference to the player's health componet
    }

    void Update()
    {
        //if there are no more objectives in the list
        //      the level is complete. go to next lvl
        if ( objectiveList.Count == 0 )
        {
            Debug.Log( "All objectives done. Level is completed" );
            setEndMessage( "Level Complete" ); //Activate the UI
            nextLvlBtn.gameObject.SetActive( true );
        }

        //If the player died
        //  restart level
        if ( playerHealth.amount <= 0 )
        {
            Debug.Log( "Player has died" );
            setEndMessage( "You have Expired" ); //Activate the UI
            restartBtn.gameObject.SetActive( true );
        }

    }

    //-----------------------------------------------------------------------------------
    // setCompleted() - this method searches for the reference to the obj. in the
    //          ArrayList and removes it from the list.
    //          This should be used in the Objectives class whenever the event
    //          is completed
    //
    public void setCompleted( Objective obj )
    {
        if ( objectiveList.Contains( obj ) )
        {
            Debug.Log( "An Objective has been completed" );
            objectiveList.Remove( obj );
        }
        else
        {
            Debug.Log( "ERROR: An Unlisted Objective has been completed" );
        }
    }

    //-----------------------------------------------------------------------------------
    // setEndMessage() - this 
    public void setEndMessage( string endGameMessage )
    {
        StartCoroutine( ClickerEnd( waitTime ));
        endGameMenu.gameObject.SetActive( true );
        endGameText.text = endGameMessage;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseMenu.gameIsPaused = true;

        GameObject.Find("Game_Canvas").GetComponent<PauseMenu>().Pause();
        GameObject pauseMenu = GameObject.Find( "PauseGroup" );
        pauseMenu.gameObject.SetActive( false );

        Time.timeScale = 0; //this makes everything stop. Need to do this when switching scenes
    }

    private IEnumerator ClickerEnd( float waiter )
    {
        yield return new WaitForSeconds( waiter );
    }
}
