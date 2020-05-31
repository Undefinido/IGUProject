using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour {

    // Use this for initialization
    //void Start () {

    //}

    //// Update is called once per frame
    //void Update () {

    //}
    public static int score = 0;

    public int TallyScore()
    {

        //checking if the pin is standing up (if it's not, the pin's rotation will be not 0)
        if (transform.rotation.x < 1 && transform.rotation.z < 1)
        {
            //pin is standing up (do nothing)
        }
        else
        {
            //pin is knocked over
            score++;
        }
        return score;
    }
}
