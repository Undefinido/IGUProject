using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using UnityEngine.SceneManagement;

public class MainMenu2 : MonoBehaviour {

    Controller controller;
    public Hand handplayer;
    public List<Finger> fingers;
    public GameObject MainMenu1;
    private int wait;

    // Use this for initialization
    void Start () {
        controller = new Controller();
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
                    PlayIntermedio();
                }
                else if (Input.GetKeyDown("2"))  // Leaves the game
                {
                    PlayDificil();

                }
                else if (Input.GetKeyDown("3"))
                {
                    PlayMuyDificil();
                }
                else if (Input.GetKeyDown("4"))
                {
                    MainMenu1.SetActive(true);
                    this.gameObject.SetActive(false);
                }
                else if (Input.GetKeyDown("5"))
                {
                    GoBackToLeapifySelector();
                }
            }
            else
            {
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
                    PlayIntermedio();
                }
                else if (numberFingers == 2)  // Leaves the game
                {
                    PlayDificil();

                }
                else if (numberFingers == 3)
                {
                    PlayMuyDificil();
                }
                else if (numberFingers == 4)
                {
                    MainMenu1.SetActive(true);
                    this.gameObject.SetActive(false);
                }
                else if (numberFingers == 5)
                {
                    GoBackToLeapifySelector();
                }
            }
        }
        if (wait == 100) wait = 0;
        else wait++;
    }

    public void PlayIntermedio()
    {
        MazeLoader.mazeColumns = 8;
        MazeLoader.mazeRows = 8;
        SceneManager.LoadScene("Game_Scenes/Labyrinth/Labyrinth_2_map");
    }

    public void PlayDificil()
    {
        MazeLoader.mazeColumns = 9;
        MazeLoader.mazeRows = 9;
        SceneManager.LoadScene("Game_Scenes/Labyrinth/Labyrinth_2_map");
    }

    public void PlayMuyDificil()
    {
        MazeLoader.mazeColumns = 10;
        MazeLoader.mazeRows = 10;
        SceneManager.LoadScene("Game_Scenes/Labyrinth/Labyrinth_2_map");
    }

    public void GoBackToLeapifySelector()
    {
        SceneManager.LoadScene("Main_menu/Scenes/LevelChooserMenu");
    }
}
