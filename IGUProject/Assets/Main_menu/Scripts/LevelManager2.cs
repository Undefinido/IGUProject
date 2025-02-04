﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.SceneManagement;

public class LevelManager2 : MonoBehaviour {

    public Hand handplayer;
    public List<Finger> fingers;
    public LeapServiceProvider provider;

    public GameObject mainMenu1;
    public GameObject mainMenu2;

    private int wait;
    // Start is called before the first frame update
    void Start()
    {
        wait = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //keyboard support
        if(wait == 100)
        {
            if (Input.GetKeyDown("1"))
            {
                cargarSpaceShooter();
            }
            if (Input.GetKeyDown("2"))
            {
                cargarLaberinto();
            }
            if (Input.GetKeyDown("4"))
            {
                this.gameObject.SetActive(false);
                (this.GetComponent(typeof(LevelManager2)) as MonoBehaviour).enabled = false;
                mainMenu1.SetActive(true);
                (mainMenu1.GetComponent(typeof(LevelManager)) as MonoBehaviour).enabled = true;
            }
            if (Input.GetKeyDown("5"))
            {
                volverMenuPrincipal();
            }
        }

        Frame frame = provider.CurrentFrame;
        if (frame != null && wait == 100)
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
            if (numberFingers == 1 || Input.GetKeyDown("1"))
            {
                cargarSpaceShooter();
            }
            if (numberFingers == 2 || Input.GetKeyDown("2"))
            {
                cargarLaberinto();
            }
            if ( numberFingers == 4 || Input.GetKeyDown("4"))
            {
                this.gameObject.SetActive(false);
                (this.GetComponent(typeof(LevelManager2)) as MonoBehaviour).enabled = false;
                mainMenu1.SetActive(true);
                (mainMenu1.GetComponent(typeof(LevelManager)) as MonoBehaviour).enabled = true;
            }
            if (numberFingers == 5 || Input.GetKeyDown("5"))
            {
                volverMenuPrincipal();
            }
        }
        if (wait == 100) wait = 0;
        else wait++;

    }

    public void LevelLoader(string gameName)
    {
        SceneManager.LoadScene(gameName);
    }

    public void cargarSpaceShooter()
    {
        LevelLoader("Game_Scenes/Done_Main");
    }

    public void cargarLaberinto()
    {
        LevelLoader("Game_Scenes/Labyrinth/MainMenuLabyrinth");
    }

    public void volverMenuPrincipal()
    {
        SceneManager.LoadScene("Main_menu/Scenes/Main Menu");
    }
}
