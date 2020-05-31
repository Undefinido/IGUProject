using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUPlayer : MonoBehaviour
{

    private GameObject ball;
    private Vector2 ballPos;

    private int topBound = 8;
    private int bottomBound = -8;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (ball == null)
        {
            ball = GameObject.FindGameObjectWithTag("Ball");
            
        }

        //si la bola se mueve hacia la derecha
        if (ball.GetComponent<Ball_pong>().ballDirection == 1)
        {
            ballPos = ball.transform.localPosition;

            //si no está en el límite y la pelota está por debajo
            if ( transform.localPosition.y > bottomBound && ballPos.y < transform.localPosition.y)
            {
                transform.localPosition += new Vector3(0, -speed, 0);
            }

            if(transform.localPosition.y < topBound && ballPos.y > transform.localPosition.y)
            {
                transform.localPosition += new Vector3(0, speed, 0);
            }
        }
    }
}
