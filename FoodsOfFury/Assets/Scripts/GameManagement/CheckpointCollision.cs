using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//----------------------------------------------------------------------------------------
// Author: Abdon J. Puente IV
//
// Description: This class sets the checkpoints
//----------------------------------------------------------------------------------------


public class CheckpointCollision : MonoBehaviour
{

    private LevelManager levelManager; //Reference to the level's level manager
    private Transform respawnPoint; //Reference to child obj. Respawn Point

    // Start is called before the first frame update
    void Start()
    {
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();  //Sets the reference to the eventManager obj
        respawnPoint = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            levelManager.setRespawnPoint(respawnPoint);
            Debug.Log("Checkpoint set!");
        }
    }
}
