using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using Leap.Unity.Attributes;
using System;
using UnityEngine.SceneManagement;

public class LabMovement : MonoBehaviour
{
    public Hand antPosition, actPosition;
    public Hand handplayer;
    public List<Finger> fingers;
    Controller controller;
    Scene scene;
    bool started = false;
    public GameObject inicio;

    // Start is called before the first frame update
    void Start()
    {
        controller = new Controller();
        scene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void Update()
    {
        Frame frame = controller.Frame();
        if (!started && scene.name == "Labyrinth") //is tutorial
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
            if ((handplayer.GetThumb().IsExtended && numberFingers == 1) || Input.GetKey("1"))
            {
                iniciarJuego();
            }
            return;
        }

        if (frame != null)
        {
            List<Hand> hands = frame.Hands;
            if (hands.Count == 0)
            {
                transform.eulerAngles = new Vector3(0f, 0f, 0f);    //poner a una posicion default
                return;
            }            
            
            actPosition = hands[0];
            

            Quaternion quat = actPosition.Basis.rotation.ToQuaternion();//* Time.deltaTime;
            quat.z = -quat.z;

            //movement fixs, just in case it goes crazy

            //inclinación vertical
            if (quat.x > 0.2f)
            {
                quat.x = 0.2f;
            }
            else if (quat.x < -0.25f)
            {
                quat.x = -0.25f;
            }

            //rotacion mano 
            if (quat.y > 0.3f)
            {
                quat.y = 0.3f;
            }
            else if (quat.y < -0.3f)
            {
                quat.y = -0.3f;
            }

            //inclinación lateral 
            if (quat.z > 0.3f)
            {
                quat.z = 0.3f;
            }
            else if (quat.z < - 0.3f) {
                quat.z = -0.3f;
            }

            transform.rotation = quat;

            antPosition = actPosition;
        }
    }

    public void iniciarJuego()
    {
        inicio.gameObject.SetActive(false);
        started = true;
    }
}
