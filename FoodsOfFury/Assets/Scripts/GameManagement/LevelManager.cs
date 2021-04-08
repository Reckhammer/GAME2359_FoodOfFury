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

    public Text     objective;          //The text for the objective UI
    public Image    endGameMenu;        //UI elements for the level completion
    public Image    winMenu;            //Reference to the Win Menu UI
    public Image    loseMenu;           //Reference to the Lose Menu UI
    public float    waitTime = 2f;      //Wait time for the popup to come up in seconds
    private bool    winSound = true;
    private bool    loseSound = true;

    void Awake()
    {
        Time.timeScale = 1;     //this resets the timescale after switching scenes

        playerHealth = GameObject.FindGameObjectWithTag( "Player" ).GetComponentInParent<Health>(); //get the reference to the player's health componet
    }

    void Update()
    {
        //if there are no more objectives in the list
        //      the level is complete. go to next lvl
        if ( objectiveList.Count == 0 )
        {
            winningSound();

            Debug.Log( "All objectives done. Level is completed" );
            winMenu.gameObject.SetActive( true );
            setEndMessage(); //Activate the UI
        }

        //If the player died
        //  restart level
        if ( playerHealth.amount <= 0 )
        {
            losingSound();

            Debug.Log( "Player has died" );
            loseMenu.gameObject.SetActive( true );
            setEndMessage(); //Activate the UI
        }

        objective.text = objectiveList[0].message;
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
    public void setEndMessage()
    {


        StartCoroutine( DelayedMenu( waitTime ));
        

    }

    public void winningSound()
    {
        if (winSound)
        {
            AudioManager.Instance.playRandom(transform.position, "Rollo_Win_01", "Rollo_Win_02");
            winSound = false;
        }
    }

    public void losingSound()
    {
        if (loseSound == true)
        {
           AudioManager.Instance.playRandom(transform.position, "Rollo_Lose_01", "Rollo_Lose_02");
           loseSound = false;
        }
        
    }



    private IEnumerator DelayedMenu( float waiter )
    {
        yield return new WaitForSeconds( waiter );

        endGameMenu.gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        PauseMenu.gameIsPaused = true;

        GameObject.Find("Game_Canvas").GetComponent<PauseMenu>().Pause(false);
        GameObject pauseMenu = GameObject.Find("PauseGroup");
        pauseMenu.gameObject.SetActive(false);

        Time.timeScale = 0; //this makes everything stop. Need to do this when switching scenes

    }
}
