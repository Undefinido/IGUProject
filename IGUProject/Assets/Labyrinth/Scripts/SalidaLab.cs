using Leap;
using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaLab : MonoBehaviour {
    
    public static bool hasFinished = false;
    private bool initializedProvider = false;
    public Hand handplayer;
    public List<Finger> fingers;
    public Controller controller;

    public GameObject finishWindow, youWonWindow;
    private int wait;
	// Use this for initialization
	void Start () {
        wait = 0;
	}
	
	// Update is called once per frame
	void Update () {
        // Once finishes the lab, manages the decision of the user (to keep or to leave the game)
        if(wait == 100) {
            if (hasFinished)
            {
                if (initializedProvider == false)
                {
                    controller = new Controller();
                    initializedProvider = true;
                }
                Frame frame = controller.Frame();
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
                    if (Input.GetKey("1") || (handplayer.GetThumb().IsExtended && numberFingers == 1))    // Continues the game one level ahead
                    {
                        if (MazeLoader.mazeColumns == 0)
                        {
                            empezarModoFacil();
                        }
                        else
                        {
                            if (MazeLoader.mazeColumns < 10)
                            {  // If is not reached the max difficulty                               
                                LabGameManager.nextLevel();
                            }
                            else
                            {
                                LabGameManager.goBackToMenu();
                                return;
                            }
                        }
                    }
                    else if (Input.GetKey("2") || numberFingers == 2)  // Leaves the game
                    {
                        LabGameManager.goBackToMenu();
                    }
                }
            }
        }
        if (wait == 100) wait = 0;
        else wait++;
    }

    private void OnTriggerEnter(Collider other)
    {
        hasFinished = true;

        if (MazeLoader.mazeColumns < 10)
        {
            finishWindow.SetActive(true);
            (GameObject.Find("Lab").GetComponent(typeof(LabMovement)) as MonoBehaviour).enabled = false;            
        }
        else
        {
            youWonWindow.SetActive(true);
            (GameObject.Find("Lab").GetComponent(typeof(LabMovement)) as MonoBehaviour).enabled = false;
        }        
    }

    public void empezarModoFacil()
    {
        MazeLoader.mazeRows = 6;
        MazeLoader.mazeColumns = 6;
        hasFinished = false;    // the user has to do another lab
        SceneManager.LoadScene("Game_Scenes/Labyrinth/Labyrinth_2_map");
    }
}
