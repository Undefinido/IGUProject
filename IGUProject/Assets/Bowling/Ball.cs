using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Leap;
using Leap.Unity;
using UnityEngine.SceneManagement;

public class Ball : MonoBehaviour {

    public Text puntuacion;
    public Text turnos;
    public Vector3 startPosition;
    public Quaternion startRotation;
    public int scoreTirada;
    public int scoreTurno;
    public int nTirada;
    public int nTurno;
    public static int maxTurnos = 10;
    public int totalScore;
    public int[] resultCasillero;
    public GameObject[] pins;
    public bool isSpare;
    public bool isStrike;
    public int [] bAfterStrike;
    public int [] bAfterSpare;
    public int afterSt;
    public int afterSp;
    public int[,] casilleroPuntos;
    public Vector3[] position = new Vector3[10];
    public Quaternion[] rot = new Quaternion[10];
    public bool[] collided;

    public bool started;
    public bool finished;
    public Canvas inicio;
    public Canvas final;

    public Hand handplayer;
    public List<Finger> fingers;

    public LeapServiceProvider provider;
    // Use this for initialization
    void Start () {
        //this.gameObject.SetActive(false);
        inicio.gameObject.SetActive(true);
        final.gameObject.SetActive(false);
        started = false;
        finished = false;
        resultCasillero = new int[maxTurnos];
        casilleroPuntos = new int[maxTurnos, 3];
        afterSt = 0;
        afterSp = 0;
        totalScore = 0;
        scoreTurno = 0;
        isSpare = false;
        isStrike = false;
        bAfterSpare = new int[1];
        bAfterStrike = new int[2];
        nTurno = 1;
        nTirada = 1;
        collided = new bool[10];
        startPosition = this.transform.position;
        pins = GameObject.FindGameObjectsWithTag("pin") as GameObject[];
        int i = 0;
        puntuacion.text = "Puntuación = 0";
        turnos.text = "Turno: "+nTurno+" \nTirada: 1 \nDerribados: ";
        foreach (GameObject pin in pins)
        { // ARREGLAR FALLO DE QUE NO SALGAN TODOS LOS BOLOS AL REINICIAR PARTIDA
            pin.gameObject.SetActive(true);
            collided[i] = false;
            position[i] = pin.transform.position;
            pin.transform.rotation = Quaternion.identity;
            i++;

        }
    }
	
	// Update is called once per frame
	void Update () {
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
                        iniciarJuego();
                    }
                }
            }

            if (Input.GetKey("1"))
            {
                iniciarJuego();
            }
            return;
        }
        if ((this.gameObject.transform.position.y < 0 && this.gameObject.transform.position.z > 4 && nTurno <= 10) || Input.GetKey("down"))
        {
            StartCoroutine(Wait()); // fin de tirada 
            
        }
        if (Input.GetKey("space"))
        {
            this.gameObject.transform.position = startPosition;
            this.gameObject.transform.rotation = Quaternion.identity;

            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
        if (finished)
        {
            Debug.Log("SALUROS");
            final.gameObject.SetActive(true);
            StartCoroutine(pantallaFinal());
            
        }

    }


    public IEnumerator Wait()
    {
        // wait for 5 seconds (the f makes the number we enter a float)

        yield return new WaitForSeconds(4.0f);
        scoreTirada = 0;
        int i = 0;
        foreach (GameObject pin in pins)
        {
           
            if (pin.transform.up.y < 0.8f && !collided[i])
            {
                scoreTirada++;
                pin.SetActive(false);
                collided[i] = true ;
            }
            else
            {
                pin.gameObject.transform.rotation = Quaternion.identity;
                pin.gameObject.transform.position = position[i];
            }
            i++;

        }
        scoreTurno += scoreTirada;
        if (nTurno != 10 && nTirada < 3) casilleroPuntos[nTurno - 1, nTirada - 1] = scoreTirada;
        if (nTurno == 10 ) 
            casilleroPuntos[9, nTirada - 1] = scoreTirada;

        if (scoreTirada == 10 && nTirada == 1 && nTurno != 10)
        {
            nTirada++;
        }
        
        if (nTurno < 10)
        {
            totalScore = 0;
            for (int j = 0; j < nTurno; j++)
            {
                if (casilleroPuntos[j, 0] + casilleroPuntos[j, 1] < 10)
                {
                    resultCasillero[j] = totalScore + casilleroPuntos[j, 0] + casilleroPuntos[j, 1];
                }
                else
                {
                    if (esStrike(casilleroPuntos, j))
                    {
                        if (esStrike(casilleroPuntos, j + 1))
                        {
                            resultCasillero[j] = totalScore + casilleroPuntos[j, 0] + casilleroPuntos[j, 1] + casilleroPuntos[j + 1, 0] + casilleroPuntos[j + 2, 0];
                        }
                        else
                        {
                            resultCasillero[j] = totalScore + casilleroPuntos[j, 0] + casilleroPuntos[j, 1] + casilleroPuntos[j + 1, 0] + casilleroPuntos[j + 1, 1];
                        }
                    }
                    else if (esSpare(casilleroPuntos, j))
                    {
                        resultCasillero[j] = totalScore + casilleroPuntos[j, 0] + casilleroPuntos[j, 1] + casilleroPuntos[j + 1, 0];
                    }
                }
                totalScore = resultCasillero[j];
            }
        }
        if (nTirada == 1)
        {
            nTirada++;
            turnos.text = "Turno: " + nTurno + "\nTirada: " + nTirada + "\nDerribados: " + scoreTirada;
            if (scoreTirada == 10 && nTurno == 10)
            {
                Debug.Log(scoreTirada);
                i = 0;
                Debug.Log("HOLAAA");
                foreach (GameObject pin in pins)
                {
                    pin.transform.position = position[i];
                    pin.transform.rotation = Quaternion.identity;
                    pin.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    pin.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    pin.SetActive(true);
                    collided[i] = false;
                    i++;
                }
            }
        }

        else if (nTirada == 2 && nTurno != 10)
        {
            i = 0;
            foreach (GameObject pin in pins)
            {
                pin.transform.position = position[i];
                pin.transform.rotation = Quaternion.identity;
                pin.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                pin.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                pin.SetActive(true);
                collided[i] = false;
                i++;
            }
            if (nTurno == 10 && scoreTurno >= 10) {
                nTirada++;
                turnos.text = "Turno: " + nTurno + "\nTirada: " + nTirada + "\nDerribados: " + scoreTirada;
            }
            else 
            {
                nTirada = 1;
                nTurno++;
                scoreTurno = 0;
                turnos.text = "Turno: " + nTurno + "\nTirada: " + nTirada + "\nDerribados: " + scoreTirada;
            }

             
        }else if (nTurno == 10)
        {
            if (scoreTirada == 10 || (scoreTurno == 10 && scoreTirada < 10) || (scoreTirada == 0 && scoreTurno == 10))
            {
                Debug.Log("xdd");
                i = 0;
                foreach (GameObject pin in pins)
                {
                    pin.transform.position = position[i];
                    pin.transform.rotation = Quaternion.identity;
                    pin.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    pin.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                    pin.SetActive(true);
                    collided[i] = false;
                    i++;
                }
            }
            else if (scoreTurno < 10) 
            {
                nTurno++;
            }
            nTirada++;
            turnos.text = "Turno: " + nTurno + "\nTirada: " + nTirada + "\nDerribados: " + scoreTirada;
            if (nTirada == 4) nTurno++;
        }       
        else
        {
            nTurno++;
        }

        


        puntuacion.text = "Puntuación :" + totalScore;

        if (nTurno >= 11)
        {
            if (casilleroPuntos[8, 0] + casilleroPuntos[8, 1] < 10)
            {
                //resultCasillero[8] = totalScore + casilleroPuntos[8, 0] + casilleroPuntos[8, 1];
            }
            else
            {
                if (esStrike(casilleroPuntos, 8))
                {
                    resultCasillero[8] = totalScore + casilleroPuntos[9, 0] + casilleroPuntos[9, 1];
                }
                else if (esSpare(casilleroPuntos, 8))
                {
                    resultCasillero[8] = totalScore + casilleroPuntos[9, 0];
                }
            }
            totalScore = resultCasillero[8] + casilleroPuntos[9, 0] + casilleroPuntos[9, 1] + casilleroPuntos[9, 2];
            turnos.text = "Derribados: " + scoreTirada;
            puntuacion.text = "Puntuación final: " + totalScore;
            finished = true;

        }

        this.gameObject.SetActive(false);
        this.gameObject.transform.position = startPosition;
        this.gameObject.transform.rotation = Quaternion.identity;

        this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        this.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        this.gameObject.SetActive(true);

    }

    public bool esStrike(int [,] cPuntos, int i)
    {
        if (cPuntos[i,0] == 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool esSpare(int[,] cPuntos, int i)
    {
        if (cPuntos[i, 0] + cPuntos[i, 1] == 10)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public IEnumerator pantallaFinal()
    {
        yield return new WaitForSeconds(2.0f);
        Frame frame = provider.CurrentFrame;
        if (frame != null)
        {
            //List<Hand> hands = frame.Hands;
            //if (hands.Count == 0)
            //{
            //    return;
            //}
            //int numberFingers = 0;
            //handplayer = hands[0];
            //fingers = handplayer.Fingers;
            //if (!started)
            //{
            //    foreach (Finger f in fingers)
            //    {
            //        if (f.IsExtended)
            //        {
            //            numberFingers++;
            //        }
            //    }
            //    if (handplayer.GetThumb().IsExtended && numberFingers == 1)
            //    {
            //        inicio.gameObject.SetActive(false);
            //        MoveBall();
            //        started = true;
            //    }
            //}
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
                if (handplayer.GetThumb().IsExtended && numberFingers == 1)
                {
                    reiniciarJuego();
                }
                else if (numberFingers == 2 || Input.GetKey("2"))
                {
                    volverAlMenu();
                }
            }
        }
    }

    public void reiniciarJuego()
    {
        Debug.Log("REINICIAAR");
        final.gameObject.SetActive(false);
        inicio.gameObject.SetActive(true);
        int i = 0;
        foreach (GameObject pin in pins)
        {
            pin.transform.position = position[i];
            pin.transform.rotation = Quaternion.identity;
            pin.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            pin.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            pin.SetActive(true);
            collided[i] = false;
            i++;
        }
        Start();
    }

    public void iniciarJuego()
    {
        inicio.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        started = true;
    }

    public void volverAlMenu()
    {
        SceneManager.LoadScene("Main_menu/Scenes/LevelChooserMenu");
    }
}
