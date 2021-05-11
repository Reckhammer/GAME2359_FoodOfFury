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
    public GameObject activeParticle;
    private LevelManager levelManager; //Reference to the level's level manager
    private Transform respawnPoint; //Reference to child obj. Respawn Point
    private Animator animator = null; // reference to animator
    private string flagAnim = null;
    private bool isActive = false;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        activeParticle.gameObject.SetActive(false);
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();  //Sets the reference to the eventManager obj
        respawnPoint = transform.GetChild(0);
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (respawnPoint == levelManager.currentRespawnPoint && isActive == false)
        {
            print("Active");
            activeParticle.gameObject.SetActive(true);
            isActive = true;
        }
        else if (respawnPoint != levelManager.currentRespawnPoint && isActive == true)
        {
            print("Inactive");
            activeParticle.gameObject.SetActive(false);
            isActive = false;
        }

        FlagAnimations();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            levelManager.setRespawnPoint(respawnPoint);
            AudioManager.Instance.playRandom(transform.position, "Checkpoint_Flag_01");
            Debug.Log("Checkpoint set!");
        }
    }

    private void FlagAnimations()
    {
        if (isActive == false)
        {
            animator.SetBool(flagAnim, false);
        }
        else
        {
            animator.SetBool(flagAnim, true);
        }
    }

    public void setFlagAnim(string anim)
    {
        flagAnim = anim;
    }

    private void OnEnable()
    {
        setFlagAnim("flagActive");
    }

}
