using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LabGameManager : MonoBehaviour {

    public GameObject youWonWindow;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))    // Continues the game one level ahead
        {
            goBackToMenu();
        }
    }
    /* First time it loads (from tutorial), starts easy difficulty
                    Difficulty
                    ----------------
                    Muy facil -> 6x6
                    Facil -> 7x7
                    Intermedio -> 8x8
                    Dificil -> 9x9
                    Muy dificil -> 10x10
                    
                    Increases difficulty one row/column                     
                    */
    public static void nextLevel()
    {
            MazeLoader.mazeRows++;
            MazeLoader.mazeColumns++;
            SalidaLab.hasFinished = false;
            SceneManager.LoadScene("Game_Scenes/Labyrinth/Labyrinth_2_map");
    }

    public static void goBackToMenu()
    {
        MazeLoader.mazeColumns = 0;
        MazeLoader.mazeRows = 0;
        SalidaLab.hasFinished = false;        
        SceneManager.LoadScene("Game_Scenes/Labyrinth/MainMenuLabyrinth");
    }
}
