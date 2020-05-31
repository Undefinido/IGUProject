using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGSoundScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

    private bool setNotDestroyed = false;
    // Play global
    private static BGSoundScript instance = null;
    public static BGSoundScript Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        Scene scene = SceneManager.GetActiveScene();
        if(scene.name == "Main Menu")
        {
            DontDestroyOnLoad(this.gameObject);
            setNotDestroyed = true;
        }        
    }
    // Update is called once per frame
    void Update () {
        Scene scene = SceneManager.GetActiveScene();
        //Only playable on level chooser
        if (scene.name != "LevelChooserMenu" && scene.name != "Main Menu" && scene.name != "MainMenuLabyrinth" && setNotDestroyed)
        {
            Destroy(this.gameObject);
        }
    }
}
