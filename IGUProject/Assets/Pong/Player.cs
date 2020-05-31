using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Leap;
using Leap.Unity;
using Leap.Unity.Attributes;


public class Player : MonoBehaviour
{
 
    public KeyCode up;
    public KeyCode down;
    public Vector antPosition;
    public Vector actPosition;
    public Hand handplayer;
    public List<Finger> fingers;
    float speed;
    float finalSpeed;

    public LeapServiceProvider provider;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.38f;
        Frame frame = provider.CurrentFrame;
        if (frame != null)
        {
            List<Hand> hands = frame.Hands;
            if (hands.Count == 0)
            {
                return;
            }
            handplayer = hands[0];
            actPosition = handplayer.PalmPosition;
            antPosition = handplayer.PalmPosition;
        }
            //Frame frame = controller.Frame();
            //handplayer = frame.Hand(0);
            
    }

    // Update is called once per frame
    void Update()
    {

        Frame frame = provider.CurrentFrame;
        if (frame != null)
        {
            List<Hand> hands = frame.Hands;
            if (hands.Count == 0)
            {
                return;
            }
            handplayer = hands[0];       
            //Debug.Log("X: "+handplayer.PalmPosition.x);
            //Debug.Log("Y: " + handplayer.PalmPosition.y);
            //Debug.Log("Z: " + handplayer.PalmPosition.z);
            actPosition = handplayer.PalmPosition;
            if (actPosition.y > antPosition.y && Mathf.Abs(actPosition.y - antPosition.y) > 0.0005) // HACIA ARRIBA
            {
                if (transform.localPosition.y > 8)
                {
                    finalSpeed = 0;
                }
                else
                {
                    finalSpeed = speed + (Mathf.Abs(actPosition.y - antPosition.y));
                }
                transform.Translate(0, finalSpeed, 0);
            }
            if (actPosition.y < antPosition.y && Mathf.Abs(actPosition.y - antPosition.y) > 0.0005) // HACIA ABAJO
            {
                if (transform.localPosition.y < -8)
                {
                    finalSpeed = 0;
                }
                else
                {
                    finalSpeed = speed + (Mathf.Abs(actPosition.y - antPosition.y));
                }
                transform.Translate(0, -finalSpeed, 0);
            }
            antPosition = actPosition;
        }

    }

    /* void Start()
    {
        //speed = 0.15f;
    }

    // Update is called once per frame
    void Update()
    {
        //Por defecto, up es la tecla W y down es S
        if (Input.GetKey(up))
        {
            //si no está en el límite, se mueve
            if (transform.localPosition.y < topBound)
            {
                transform.Translate(0, speed, 0);
            }
            
        }
        if (Input.GetKey(down))
        {

            if (transform.localPosition.y > bottomBound)
            {
                transform.Translate(0, -speed, 0);
            }
        }

    }*/
}
