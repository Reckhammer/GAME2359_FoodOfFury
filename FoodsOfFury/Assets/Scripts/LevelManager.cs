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
    public ArrayList objectiveList = new ArrayList();    //list of all of the objectives for the level

    public Image    endGameMenu;        //UI elements for the level completion
    public float    waitTime = .75f;    //Wait time for the popup to come up in seconds

    void Awake()
    {
        Time.timeScale = 1;     //this resets the timescale after switching scenes
    }

    void Update()
    {
        //if there are no more objectives in the list
        //      the level is complete. go to next lvl
        if ( objectiveList.Count == 0 )
        {
            Debug.Log( "All objectives done. Level is completed" );
            setEndMessage();

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
    public void setEndMessage()
    {
        StartCoroutine( ClickerEnd( waitTime ));
        endGameMenu.gameObject.SetActive( true );

        Time.timeScale = 0; //this makes everything stop. Need to do this when switching scenes
    }

    private IEnumerator ClickerEnd( float waiter )
    {
        yield return new WaitForSeconds( waiter );
    }
}
