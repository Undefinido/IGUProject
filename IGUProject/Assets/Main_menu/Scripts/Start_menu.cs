﻿using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_menu : MonoBehaviour {

    public Hand handplayer;
    public List<Finger> fingers;
    public LeapServiceProvider provider;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        Frame frame = provider.CurrentFrame;
        if (frame != null)
        {
            List<Hand> hands = frame.Hands;
            if (hands.Count == 0)
            {
                return;
            }
            int numberFingers = 0;
            handplayer = hands[0];
            fingers = handplayer.Fingers;
            foreach (Finger f in fingers)
            {
                if (f.IsExtended)
                {
                    numberFingers++;
                }
            }
            if (Input.GetKey("1") || numberFingers == 1)
            {
                cargarSelectorNiveles();
            }

        }                
    }

    public void cargarSelectorNiveles()
    {
        SceneManager.LoadScene("Main_menu/Scenes/LevelChooserMenu");
    }
}
