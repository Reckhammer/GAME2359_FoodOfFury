using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiObjective : MonoBehaviour
{
    public Objective[] objectives;      //Array of child Objectives

    private Objective       objComponent;   //Reference to our own objective component
    private LevelManager    lvlManager;     //Reference to levelManager GameObject
    private int             doneCount;      //Number of child obj. that are completed
    private int             oldCount;       //Integer to check if there was a change in doneCount
    private string          baseMessage;    //The base part of the objective message

    void Awake()
    {
        lvlManager = GameObject.Find( "LevelManager" ).GetComponent<LevelManager>();    //Get the reference to the lvlManager in the scene
        objComponent = GetComponent<Objective>();

        getDoneCount();
        oldCount = doneCount;
        baseMessage = objComponent.message;
        getMessage();
    }

    // Update is called once per frame
    void Update()
    {
        //Only do something if not done
        if ( !objComponent.isDone )
        {
            //If doneCoun is equal to the total number of obj
            //      this objective is completed
            //Else if doneCount is greater than oldCount
            //      update message
            if ( doneCount == objectives.Length )
            {
                lvlManager.setCompleted( objComponent );
                objComponent.isDone = true;
            }
            else if ( getDoneCount() > oldCount )
            {
                objComponent.message = getMessage();

            }
        }
    }

    private int getDoneCount()
    {
        doneCount = 0;

        //iterate thru the list of objectives and count up all the ones that are done
        foreach ( Objective obj in objectives )
        {
            if ( obj.isDone )
            {
                doneCount++;
            }
        }

        return doneCount;
    }

    private string getMessage()
    {
        string newMessage = baseMessage + " " + doneCount + "/" + objectives.Length;
        objComponent.message = newMessage;
        return newMessage;
    }
}
