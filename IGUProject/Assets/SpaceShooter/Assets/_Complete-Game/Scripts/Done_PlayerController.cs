using UnityEngine;
using System.Collections;
using Leap;
using System.Collections.Generic;
using Leap.Unity;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Done_Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
    public float speed;
    public float tilt;
    public Done_Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

    public Vector antPosition;
    public Vector actPosition;
    public Hand handplayer;
    public List<Finger> fingers;


    public LeapServiceProvider provider;

 
    private void Start()
    {
        actPosition = handplayer.PalmPosition;
        antPosition = handplayer.PalmPosition;
    }

    void Update()
    {

        Frame frame = provider.CurrentFrame;
        if (frame != null)
        {
            List<Hand> hands = frame.Hands;
            if (hands.Count == 0)
            {
                this.gameObject.transform.position = Vector3.zero;
                //gameObject.SetActive(false);
                return;
            }
            handplayer = hands[0];
            //actPosition = handplayer.PalmPosition;
            //antPosition = handplayer.PalmPosition;
            int numberFingers = 0;
            fingers = handplayer.Fingers;
            foreach (Finger f in fingers)
            {
                if (f.IsExtended)
                {
                    numberFingers++;
                }
            }
       
            if (handplayer.GetIndex().IsExtended 
                 && Time.time > nextFire )
            {
                nextFire = Time.time + fireRate;
                Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                GetComponent<AudioSource>().Play();
            }

            //if (Input.GetKey("space") && Time.time > nextFire)
            //{
            //    nextFire = Time.time + fireRate;
            //    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            //    GetComponent<AudioSource>().Play();
            //}
        }


    }

    void FixedUpdate()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal"); // ENTRE -1 Y 1
        //float moveVertical = Input.GetAxis("Vertical");
        
        Frame frame = provider.CurrentFrame;
        if (frame != null)
        {
           
            float moveHorizontal = 0;
            float moveVertical = 0;
            List<Hand> hands = frame.Hands;
            if (hands.Count == 0)
            {
                this.gameObject.transform.position = Vector3.zero;
                //gameObject.SetActive(false);
                return;
            }
            gameObject.SetActive(true);
            handplayer = hands[0];
            actPosition = handplayer.PalmPosition;
            if (actPosition.y > antPosition.y && Mathf.Abs(actPosition.y - antPosition.y) > 0.0006) // HACIA ARRIBA
            {
                //Debug.Log("VOY ARRIBA");
                moveVertical = (Mathf.Abs(actPosition.y - antPosition.y)) * 185 ;
            }
            if (actPosition.y < antPosition.y && Mathf.Abs(actPosition.y - antPosition.y) > 0.0006) // HACIA ABAJO
            {
                //Debug.Log("VOY ABAJO");
                moveVertical = (Mathf.Abs(actPosition.y - antPosition.y)) * 185;
                moveVertical = -moveVertical;
            }
            if (actPosition.x > antPosition.x && Mathf.Abs(actPosition.x - antPosition.x) > 0.0005) // HACIA DERECHA
            {
                //Debug.Log("VOY DERECHA");
                moveHorizontal = (Mathf.Abs(actPosition.x - antPosition.x)) * 240;
            }
            if (actPosition.x < antPosition.x && Mathf.Abs(actPosition.x - antPosition.x) > 0.0005) // HACIA IZQUIERDA
            {
                //Debug.Log("VOY IZQUIERDA");
                moveHorizontal = (Mathf.Abs(actPosition.x - antPosition.x)) * 240;
                moveHorizontal = -moveHorizontal;
            }
            //Debug.Log("MOV X: "+moveHorizontal);
            //Debug.Log("MOV Y: "+moveVertical);
            Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            antPosition = actPosition;
            //Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            GetComponent<Rigidbody>().velocity = movement * speed;
            //transform.Translate(moveHorizontal * speed, 0, moveVertical * speed);
            GetComponent<Rigidbody>().position = new Vector3
            (
                Mathf.Clamp(GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(GetComponent<Rigidbody>().position.z, boundary.zMin, boundary.zMax)
            );
            Debug.Log("Position X "+GetComponent<Rigidbody>().position.x);
            Debug.Log("Position Y "+GetComponent<Rigidbody>().position.z);
            //GetComponent<Rigidbody>().rotation = Quaternion.Euler(0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
        }
        

        
    }
}
