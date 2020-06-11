using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Leap;
using Leap.Unity;

public class LevelManager : MonoBehaviour
{

    public Hand handplayer;
    public List<Finger> fingers;
    public LeapServiceProvider provider;

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
        if(wait == 100)
        {
            if (Input.GetKeyDown("1"))
            {
                cargarBolera();
            }
            if (Input.GetKeyDown("2"))
            {
                cargarPong();
            }
            if (Input.GetKeyDown("3"))
            {
                cargarPPT();
            }
            if (Input.GetKeyDown("4"))
            {
                this.gameObject.SetActive(false);
                (this.GetComponent(typeof(LevelManager)) as MonoBehaviour).enabled = false;
                mainMenu2.SetActive(true);
                (mainMenu2.GetComponent(typeof(LevelManager2)) as MonoBehaviour).enabled = true;
            }
            if (Input.GetKeyDown("escape"))
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
            if (Input.GetKeyDown("1") || numberFingers == 1)
            {
                cargarBolera();
            }
            if (Input.GetKeyDown("2") || numberFingers == 2)
            {
                cargarPong();
            }
            if (Input.GetKeyDown("3") || numberFingers == 3)
            {
                cargarPPT();
            }
            if (Input.GetKeyDown("4") || numberFingers == 4)
            {
                this.gameObject.SetActive(false);
                (this.GetComponent(typeof(LevelManager)) as MonoBehaviour).enabled = false;
                mainMenu2.SetActive(true);
                (mainMenu2.GetComponent(typeof(LevelManager2)) as MonoBehaviour).enabled = true;
            }

        }
        if (wait == 100) wait = 0;
        else wait++;
        
    }

    public void LevelLoader(string gameName)
    {
    	SceneManager.LoadScene(gameName);
    }

    public void cargarBolera()
    {
        LevelLoader("Game_Scenes/Bolera");
    }

    public void cargarPong()
    {
        LevelLoader("Game_Scenes/SampleScene");
    }

    public void cargarPPT()
    {
        LevelLoader("Game_Scenes/PPT");
    }

    public void volverMenuPrincipal()
    {
        SceneManager.LoadScene("Main_menu/Scenes/Main Menu");
    }
}
