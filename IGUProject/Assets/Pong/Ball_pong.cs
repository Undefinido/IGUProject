using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Leap;
using Leap.Unity;
using Leap.Unity.Attributes;
using UnityEngine.SceneManagement;

public class Ball_pong : MonoBehaviour
{

    public int directionX;
    public int directionY;
    public int ballDirection;
    private float oldPosition;
    public Hand handplayer;
    public List<Finger> fingers;
    public bool started;
    public bool finished;
    public Canvas inicio;
    public Canvas final;
    public Camera cam;

    public float speed;
    //PUNTUACION

    public Text scoreText;
    public int player1Score;
    public int cpuScore;
    public int maxScore;
    //GANADOR

    public Text winner;


    public ParticleSystem sparks;

    public LeapServiceProvider provider;

    // Start is called before the first frame update
    void Start()
    {
        cpuScore = 0;
        player1Score = 0;
        scoreText.text = player1Score.ToString() + " - " + cpuScore.ToString();
        inicio.gameObject.SetActive(true);
        final.gameObject.SetActive(false);
        started = false;
        finished = false;
        //Frame frame = provider.CurrentFrame;
        maxScore = 5;
        //if (started) MoveBall();
        oldPosition = transform.position.x;
        ResetBall();
    }

    //provoca que la bola se mueva en una direccion
    void MoveBall()
    {
        speed = Random.Range(20, 25);

        //Escoge dirección de partida
        directionX = Random.Range(0, 2);
        //derecha
        if (directionX == 0)
        {
            directionX = 1;
        }//izquierda
        else
        {
            directionX = -1;
        }

        directionY = Random.Range(0, 2);
        if (directionY == 0)
        {
            directionY = 1;
        }
        else
        {
            directionY = -1;
        }

        GetComponent<Rigidbody>().velocity = new Vector3(speed * directionX, speed * directionY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            volverAlMenu();
        }
        if (!finished)
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
                        iniciarPong();
                    }
                }
            }
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
            //-1 izquierda, 1 derecha
            if (transform.position.x > oldPosition)
            {
                ballDirection = 1;
            }
            else
            {
                ballDirection = -1;
            }

            scoreText.text = player1Score.ToString() + " - " + cpuScore.ToString();

            if (player1Score == maxScore)
            {
                winner.text = "Player wins";
                winner.gameObject.SetActive(true);
                final.gameObject.SetActive(true);
                finished = true;
                StartCoroutine(resultadosFinales());
            }

            if (cpuScore == maxScore)
            {
                winner.text = "CPU wins";
                winner.gameObject.SetActive(true);
                final.gameObject.SetActive(true);
                finished = true;
                StartCoroutine(resultadosFinales());
            }
        }
        else
        {
            final.gameObject.SetActive(true);
            StartCoroutine(resultadosFinales());
        }

    }

    public void iniciarPong()
    {
        inicio.gameObject.SetActive(false);
        MoveBall();
        started = true;
    }

    //al marcar gol, la bola se pone al centro
    void ResetBall()
    {
        //transform.localPosition = Vector3.zero;
        transform.localPosition = new Vector3(0, 0, 0);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision objeto)
    {
        if (objeto.collider.tag == "Player1Goal")
        {
            player1Score++;
            ResetBall();
            if (player1Score < maxScore) {
                
                Invoke("MoveBall", 2);
            }
                
        }

        if (objeto.collider.tag == "CpuGoal")
        {
            cpuScore++;
            ResetBall();
            if (cpuScore < maxScore)
            {
                
                Invoke("MoveBall", 2);
            }
                
        }

        if (objeto.collider.tag == "Player" || objeto.collider.tag == "CPU Player")
        {
            sparks.Play();
        }
    }

    public IEnumerator resultadosFinales()
    {
        yield return new WaitForSeconds(2.0f);
        Debug.Log("XDDD LOOL LMAO");
        winner.gameObject.SetActive(false);
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
                    reiniciarPong();
                }
                else if (numberFingers == 2 || Input.GetKey("2"))
                {
                    volverAlMenu();
                }
            }
        }
    }

    public void reiniciarPong()
    {
        Debug.Log("REINICIAAR");
        final.gameObject.SetActive(false);
        Start();
    }

    public void volverAlMenu()
    {
        SceneManager.LoadScene("Main_menu/Scenes/LevelChooserMenu");
    }
}


