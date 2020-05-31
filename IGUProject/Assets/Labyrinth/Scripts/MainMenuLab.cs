using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Leap;


public class MainMenuLab : MonoBehaviour {

    Controller controller;
    public Hand handplayer;
    public List<Finger> fingers;
    public GameObject MainMenu2;
    private int wait;

    // Use this for initialization
    void Start () {
        controller = new Leap.Controller();
        wait = 0;
	}
	
	// Update is called once per frame
	void Update () {
        Frame frame = controller.Frame();
        if (frame != null && wait == 100)
        {
            List<Hand> hands = frame.Hands;
            if (hands.Count == 0)
            {   //keyboard support
                if (Input.GetKeyDown("1"))    // Continues the game one level ahead
                {
                    PlayTutorial();
                }
                else if (Input.GetKeyDown("2"))  // Leaves the game
                {
                    PlayMuyFacil();

                }
                else if (Input.GetKeyDown("3"))
                {
                    PlayFacil();
                }
                else if (Input.GetKeyDown("4"))
                {
                    MainMenu2.SetActive(true);
                    this.gameObject.SetActive(false);
                }
                else if (Input.GetKeyDown("5"))
                {
                    GoBackToLeapifySelector();
                }         
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
            if (numberFingers == 1)    // Continues the game one level ahead
            {
                PlayTutorial();
            }
            else if (numberFingers == 2)  // Leaves the game
            {
                PlayMuyFacil();

            }
            else if (numberFingers == 3)
            {
                PlayFacil();
            }
            else if (numberFingers == 4)
            {
                MainMenu2.SetActive(true);
                this.gameObject.SetActive(false);
            }
            else if (numberFingers == 5)
            {
                GoBackToLeapifySelector();
            }
        }
        if (wait == 100) wait = 0;
        else wait++;
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("Game_Scenes/Labyrinth/Labyrinth");
    }

    /*
                    Difficulty
                    ----------------
                    Muy facil -> 6x6
                    Facil -> 7x7
                    Intermedio -> 8x8
                    Dificil -> 9x9
                    Muy dificil -> 10x10
                    
                    Increases difficulty one row/column                     
    */
    public void PlayMuyFacil()
    {
        MazeLoader.mazeColumns = 6;
        MazeLoader.mazeRows = 6;
        SceneManager.LoadScene("Game_Scenes/Labyrinth/Labyrinth_2_map");
    }

    public void PlayFacil()
    {
        MazeLoader.mazeColumns = 7;
        MazeLoader.mazeRows = 7;
        SceneManager.LoadScene("Game_Scenes/Labyrinth/Labyrinth_2_map");
    }

    public void GoBackToLeapifySelector()
    {
        SceneManager.LoadScene("Main_menu/Scenes/LevelChooserMenu");
        //SceneManager.LoadScene("Main_menu/Scenes/LevelChooserMenu");
    }
}
