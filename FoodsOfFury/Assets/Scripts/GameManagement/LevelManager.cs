using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//----------------------------------------------------------------------------------------
// Author(s): Joshua Rechkemmer, Abdon J. Puente IV
//
// Description: This class handles completion of objectives and the end of the level
//      when all objectives are done
//----------------------------------------------------------------------------------------
public class LevelManager : MonoBehaviour
{
    [HideInInspector]
    public static bool gameOver = false;

    public Objective[] objectiveList;    //list of all of the objectives for the level

    private PlayerManager lives;        //Reference to the PlayerManager
    private Health  playerHealth;       //the player's heath
    private GameObject player;          //Reference to the player
    private ScreenFade fadeScreen;      //Reference to FadeScreen
    private Animator faintAnim;
    private PlayerMovementTwo movement;

    private int currentObjInd;          //index in objectiveList of current obj.
    private int doneCount;              //Number of completed objectives

    //public float faintDelay = 2.0f;
    public bool       inControlObjTxt = true;
    public Text       objectiveTxt;       //The text for the objective UI
    public GameObject endGameMenu;        //UI elements for the level completion
    public GameObject winMenu;            //Reference to the Win Menu UI
    public GameObject loseMenu;           //Reference to the Lose Menu UI
    public float      waitTime = 2f;      //Wait time for the popup to come up in seconds
    public Transform currentRespawnPoint; //The current point that the player will respawn at
    private Coroutine cFade;
    private bool      winSound = true;
    private bool      loseSound = true;
    private bool      hasDied = false;

    void Awake()
    {
        Time.timeScale = 1;     //this resets the timescale after switching scenes

        playerHealth = GameObject.FindGameObjectWithTag( "Player" ).GetComponentInParent<Health>(); //get the reference to the player's health componet
        faintAnim = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<Animator>();
        player = GameObject.Find("Player_PM2");
        fadeScreen = GameObject.Find("FadeScreen").GetComponentInParent<ScreenFade>();

        currentRespawnPoint = GameObject.Find("StartingPoint").GetComponent<Transform>();

        objectiveTxt = GameObject.Find("Objective_Text").GetComponent<Text>();

        currentObjInd = 0;
        updateObjectiveText();

        if ( objectiveList.Length > 1 )
        {
            for ( int ind = 1; ind < objectiveList.Length; ind++ )
            {
                objectiveList[ind].gameObject.SetActive( false );
            }
        }

    }

    void Start()
    {
        lives = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerManager>();
        movement = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerMovementTwo>();
        gameOver = false;
    }

    void Update()
    {
        //If the text currently displayed does not match the current obj.'s text
        //      update the objective text box
        if ( currentObjInd < objectiveList.Length-1 && !objectiveTxt.text.Equals(objectiveList[currentObjInd].message ))
        {
            updateObjectiveText();
        }

        //if the number of done obj. is equal to total number of obj
        //      the level is complete. go to next lvl
        if ( doneCount >= objectiveList.Length )
        {
            winningSound();

            Debug.Log( "All objectives done. Level is completed" );
            winMenu.gameObject.SetActive( true );
            setEndMessage(); //Activate the UI
        }

        //If the player died
        //  restart level
        if (lives.fullyDied && !hasDied)
        {
            gameOver = true;
            losingSound();
            hasDied = true;
            Debug.Log("Player has died");
            loseMenu.gameObject.SetActive( true );
            setEndMessage(); //Activate the UI
        }
        else if (playerHealth.amount <= 0 && !lives.fullyDied && cFade == null)
        {
            
            faintAnim.SetTrigger("Death");
            movement.stopInput(10.0f, true, true);
            player.GetComponent<PlayerManager>().addSwitchDelay(10.0f);
            cFade = StartCoroutine(FaintTimer());
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
        if ( Array.IndexOf( objectiveList, obj ) != -1 )
        {
            Debug.Log( "An Objective has been completed" );
            doneCount++;
            
            
            //check if out of bounds
            if ( currentObjInd < objectiveList.Length )
            {
                currentObjInd++;
                //objectiveList[currentObjInd - 1].gameObject.SetActive( false ); //disable completed obj
                objectiveList[currentObjInd].gameObject.SetActive( true );      //enable current obj
                
                updateObjectiveText();
            }
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

    public void updateObjectiveText()
    {
        if ( inControlObjTxt )
            objectiveTxt.text = objectiveList[currentObjInd].message;       //update objective UI
    }

    public void winningSound()
    {
        if (winSound == true)
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

    public void setRespawnPoint(Transform newPoint)
    {
        if (newPoint != null) //if the newPoint is not empty
        {
            currentRespawnPoint = newPoint; //set currentRespawnPoint to newPoint
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

    private IEnumerator FaintTimer()
    {
        AudioManager.Instance.playRandom(transform.position, "Rollo_Lose_01", "Rollo_Lose_02");

        yield return new WaitForSeconds(2);

        fadeScreen.DeathFade();
        player.transform.position = currentRespawnPoint.position;
        playerHealth.Revive();
        movement.stopInput(0.0f, false, false);
        player.GetComponent<PlayerManager>().addSwitchDelay(0.0f);
        fadeScreen.Restart();

        cFade = null;
    }


}
