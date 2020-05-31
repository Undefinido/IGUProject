using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    /*
    This script is used for pre-made labyrinths, don't work for random generated labs 
    */
    public GameObject player;   //public variable to store a reference to the player game object

    private Vector3 offset;     //private variable to store the offset distance between the player and the camera

	// Use this for initialization
	void Start () {
        offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    //guarantees that runs after all items have been processed, runs every frame
    private void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
