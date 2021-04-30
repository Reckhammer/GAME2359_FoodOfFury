using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshPlatDestroy : MonoBehaviour
{
    void Awake()
    {
        //Create an array of all of the gameobjects that are used for the navmesh but aren't needed visually
        GameObject[] navMeshPlatforms = GameObject.FindGameObjectsWithTag( "NavMeshAsst" );

        //Loop thru all of the objects in the array
        foreach ( GameObject obj in navMeshPlatforms )
        {
            Destroy( obj ); //Destroy them
        }
    }
}
