using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerMG : MonoBehaviour {
    /*
        This script is used only in random generated labyrinths, only works in them
    */
    private GameObject player;   //public variable to store a reference to the player game object
    public GameObject gameManager; //contains rows and columns

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //guarantees that runs after all items have been processed, runs every frame
    private void LateUpdate()
    {       
        player = GameObject.Find("Player");
        float distance = 15 + (MazeLoader.mazeRows * 3.0f); //ml.mazeRows; if not static
        transform.position = player.transform.position + new Vector3(0, distance, 0);        
    }        

}
