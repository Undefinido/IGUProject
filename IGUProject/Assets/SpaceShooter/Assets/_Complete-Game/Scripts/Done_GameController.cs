using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using Leap.Unity;
using Leap;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;


public class Done_GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;

    public Text scoreText;
    public Text restartText;
    public Text gameOverText;

    private bool gameOver;
    private bool restart;
    private int score;

    public bool started;
    public bool finished;
    public Canvas inicio;
    public Canvas final;

    public Hand handplayer;
    public List<Finger> fingers;

    public LeapServiceProvider provider;

    void Start()
    {
        this.gameObject.SetActive(true);
        final.gameObject.SetActive(false);
        gameOver = false;
        restartText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        if (started)StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            volverAlMenu();
        }
        if (!started)
        {
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
                if (!started)
                {
                    foreach (Finger f in fingers)
                    {
                        if (f.IsExtended)
                        {
                            numberFingers++;
                        }
                    }
                    if ((handplayer.GetThumb().IsExtended && numberFingers == 1) || Input.GetKey("1"))
                    {
                        inicio.gameObject.SetActive(false);
                        this.gameObject.SetActive(true);
                        started = true;
                        Start();
                    }
                }
            }
            return;
        }
        if (gameOver)
        {
            StartCoroutine(pantallaFinal());
        }
        
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true && !gameOver)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);
            if (gameOver)
            {
                break;
            }
        }
    }

    private IEnumerator pantallaFinal()
    {
        yield return new WaitForSeconds(2.0f);
        Frame frame = provider.CurrentFrame;
        if (frame != null)
        {           
            List<Hand> hands = frame.Hands;
            if (hands.Count == 0)
            {
                yield return new WaitForSeconds(0.0f);
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
                if ((handplayer.GetThumb().IsExtended && numberFingers == 1) || Input.GetKey("1"))
                {
                    reiniciarJuego();
                }
                else if (numberFingers == 2 || Input.GetKey("2"))
                {
                    //started = false;
                    //final.gameObject.SetActive(false);
                    volverAlMenu();
                }
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void GameOver() // AÑADIR PANTALLA FINAL
    {
        //gameOverText.text = "Game Over!";
        gameOver = true;
        final.gameObject.SetActive(true);
        StartCoroutine(pantallaFinal());
    }


    public void iniciarJuego()
    {
        inicio.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        started = true;
        Start();
    }

    public void reiniciarJuego()
    {
        final.gameObject.SetActive(false);
        inicio.gameObject.SetActive(true);
        started = false;
        //Start();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //cargarEscena();
    }

    public IEnumerator cargarEscena()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void volverAlMenu()
    {
        SceneManager.LoadScene("Main_menu/Scenes/LevelChooserMenu");
    }
}