using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{

    private Animator animator = null;
    private LevelManager levelManager; //Reference to the level's level manager
    private string doorAnim = null;
    private bool isClose = false;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DoorAnimations();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && isClose == false && levelManager.objectiveList.Length == 1)
        {

            isClose = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && isClose == true && levelManager.objectiveList.Length == 1)
        {
            isClose = false;
        }
    }


    private void DoorAnimations()
    {
        if (isClose == false)
        {
            animator.SetBool(doorAnim, false);
        }
        else
        {
            animator.SetBool(doorAnim, true);
        }
    }

    public void setDoorAnim(string anim)
    {
        doorAnim = anim;
    }

    private void OnEnable()
    {
        setDoorAnim("PlayerNear");
    }

}
