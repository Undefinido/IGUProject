using System.Collections;
using System.Collections.Generic;
using Leap;
using Leap.Unity;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Game_PPT : MonoBehaviour {

    public LeapServiceProvider provider;
    public Hand handplayer;
    public List <Finger> fingers;
    public UnityEngine.UI.Image resP;
    public UnityEngine.UI.Image resCPU;
    public Sprite inicial;
    public Sprite rock;
    public Sprite paper;
    public Sprite scissors;
    public Sprite inicial_m;
    public Sprite rock_m;
    public Sprite paper_m;
    public Sprite scissors_m;
    public Text tm;
    public Text puntuacion;
    public Text resultados;
    public Camera cam;
    private float timer;
    private int done;
    private int puntosJugador;
    private int puntosCPU;
    private int maxPuntos;
    public bool started;
    public bool finished;
    public Canvas inicio;
    public Canvas final;
    private System.Random rnd = new System.Random();
    // Use this for initialization
    void Start () {
        puntosJugador = 0;
        puntosCPU = 0;
        maxPuntos = 3;
        done = 0;
        started = false;
        finished = false;
        resP.sprite = inicial;
        resCPU.sprite = inicial_m;
        inicio.gameObject.SetActive(true);
        final.gameObject.SetActive(false);
        puntuacion.text = "PUNTUACIÓN: " + puntosJugador + " - " + puntosCPU;
        puntuacion.gameObject.SetActive(true);
    }

	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey("escape"))
        {
            volverAlMenu();
        }
        if (finished)
        {
            if (puntosJugador == maxPuntos)
            {
                final.gameObject.SetActive(true);
                StartCoroutine(Resultados("jugador")); // fin de tirada  
            }
            if (puntosCPU == maxPuntos)
            {
                final.gameObject.SetActive(true);
                StartCoroutine(Resultados("cpu")); // fin de tirada  
            }
        }
        else
        if (started && !finished)
        {
            //PRUEBAS CÁMARA
            if (Input.GetKey("left"))
            {
                cam.transform.Rotate(new Vector3(0.0f, -1.0f, 0.0f));
            }
            if (Input.GetKey("right"))
            {
                cam.transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
            }
            if (Input.GetKey("up"))
            {
                cam.transform.Translate(new Vector3(0.0f, 0.0f, 1.0f));
            }
            if (Input.GetKey("down"))
            {
                cam.transform.Translate(new Vector3(0.0f, 0.0f, -1.0f));
            }
            if (puntosJugador == maxPuntos)
            {
                finished = true;
                final.gameObject.SetActive(true);
                StartCoroutine(Resultados("jugador")); // fin de tirada  
                return;
            }
            if (puntosCPU == maxPuntos)
            {
                finished = true;
                final.gameObject.SetActive(true);
                StartCoroutine(Resultados("cpu")); // fin de tirada  
                return;
            }
            timer += Time.deltaTime;
            int seconds = (int)timer % 60;
            int seconds3 = seconds % 4;
            if (!finished)
            {
                tm.text = seconds3.ToString();
            }
            if (seconds3 == 0 && !finished && started)
            {
                tm.text = "YA";
            }

            if (seconds3 != 0)
            {
                done = 0;
                resP.sprite = inicial;
                resCPU.sprite = inicial_m;
            }

            if (tm.text == "YA" && done == 0 && !finished && started)
            {
                comprobarResultado();
                done = 1;
            }
        }
        else
        {
            Frame frame = provider.CurrentFrame;

            if (frame != null)
            {
                List<Hand> hands = frame.Hands;
                if (hands.Count == 0)
                {
                    return;
                }
                int nExtendFing = 0;
                handplayer = hands[0];
                fingers = handplayer.Fingers;
                foreach (Finger f in fingers)
                {
                    if (f.IsExtended)
                    {
                        nExtendFing++;
                    }
                }
                if ((handplayer.GetThumb().IsExtended && nExtendFing == 1) || Input.GetKey("1"))
                {
                    inicio.gameObject.SetActive(false);
                    started = true;
                }
            }
        }
    }

    void comprobarResultado()
    {
        int opt = rnd.Next(1, 4);
        int optJ = 0;
        int nExtendFing = 0;
        Frame frame = provider.CurrentFrame;

        if (frame != null)
        {
            List<Hand> hands = frame.Hands;
            if (hands.Count == 0)
            {
                return;
            }
            handplayer = hands[0];
            fingers = handplayer.Fingers;
            foreach (Finger f in fingers)
            {
                if (f.IsExtended)
                {
                    nExtendFing++;
                }
            }

            if (nExtendFing == 0)
            {
                resP.sprite = rock;
                optJ = 1;
            }
            if (nExtendFing == 5)
            {
                resP.sprite = paper;
                optJ = 2;
            }
            if (nExtendFing == 2)
            {
                resP.sprite = scissors;
                optJ = 3;
            }
            switch (opt)
            {
                case 1: resCPU.sprite = rock_m; break;
                case 2: resCPU.sprite = paper_m; break;
                case 3: resCPU.sprite = scissors_m; break;
            }

            if(optJ == opt)
            {
                // empate
                return;
            }
            if (optJ == 2 && opt == 1)
            {
                // gano
                puntosJugador++;
            }
            if (optJ == 1 && opt == 2)
            {
                // pierdo
                puntosCPU++;
            }
            if (optJ == 3 && opt == 2)
            {
                // gano
                puntosJugador++;
            }
            if (optJ == 2 && opt == 3)
            {
                // pierdo
                puntosCPU++;
            }
            if (optJ == 1 && opt == 3)
            {
                // gano
                puntosJugador++;
            }
            if (optJ == 3 && opt == 1)
            {
                // pierdo
                puntosCPU++;
            }
            
            puntuacion.text = "PUNTUACIÓN: " + puntosJugador + " - " + puntosCPU;

        }
    }

    public IEnumerator Resultados(string ganador)
    {
        yield return new WaitForSeconds(2.0f);
        if (ganador == "jugador")
        {
            puntuacion.text = "GANADOR: JUGADOR";
        }
        if (ganador == "cpu")
        {
            puntuacion.text = "GANADOR: CPU";
        }
        //yield return new WaitForSeconds(2.0f);
        Debug.Log("XDDD LOOL LMAO");
        resultados.text = "FINAL: " + puntosJugador + " - " + puntosCPU;
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
                    reiniciarPPT();
                }
                else
                if (numberFingers == 2 || Input.GetKey("2"))
                {
                    volverAlMenu();
                }
            }

        }
    }

    public void reiniciarPPT()
    {
        Debug.Log("REINICIAAR");
        final.gameObject.SetActive(false);
        Start();
    }

    public void volverAlMenu()
    {
        SceneManager.LoadScene("Main_menu/Scenes/LevelChooserMenu");
    }
    //public IEnumerator Esperar()
    //{
    //    yield return new WaitForSeconds(2.0f);
        
    //}


}

