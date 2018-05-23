using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMover : MonoBehaviour {
    public float moveDistance = 25;
    public float speed = 2;
    private bool goingUp;
    private float startY;
    public bool randomSpeed = true;
    public float waitTime = 0;
    private float timeCounter = 0;
    public bool startGoingUp;
    private float endY;
    private float endX;
    public bool horizontal = false;
    private float startX;
    public float preMove = 0;

	// Use this for initialization
	void Start () {
        if (randomSpeed)
        {
            float random = Random.Range(2, 6);
            speed = random;
        }
        
        if(startGoingUp)
        {
            endY = transform.position.y + moveDistance;
            endX = transform.position.x + moveDistance;
            goingUp = true;
        }
        else
        {
            endY = transform.position.y - moveDistance;
            endX = transform.position.x - moveDistance;
            goingUp = false;
        }

        startY = transform.position.y;
        startX = transform.position.x;

        if(!goingUp)
        {
            transform.Translate(Vector3.down * preMove * Time.deltaTime);
        }
        else
        {
            transform.Translate(Vector3.up * preMove * Time.deltaTime);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (timeCounter >= waitTime)
        {
            if (!goingUp)
            {
                transform.Translate(Vector3.down * speed * Time.deltaTime);

                if ((!horizontal && startGoingUp && transform.position.y <= startY) || (!horizontal && !startGoingUp && transform.position.y <= endY) || (horizontal && startGoingUp && transform.position.x <= startX) || (horizontal && !startGoingUp && transform.position.x <= endX))
                {
                    goingUp = true;

                    timeCounter = 0;

                    if (randomSpeed)
                    {
                        float random = Random.Range(2, 6);
                        speed = random;
                    }
                }
            }
            else if (goingUp)
            {
                transform.Translate(Vector3.up * speed * Time.deltaTime);

                if ((!horizontal && startGoingUp && transform.position.y >= endY) || (!horizontal && !startGoingUp && transform.position.y >= startY) || (horizontal && startGoingUp && transform.position.x >= endX) || (horizontal && !startGoingUp && transform.position.x >= startX))
                {
                    goingUp = false;

                    timeCounter = 0;

                    if (randomSpeed)
                    {
                        float random = Random.Range(2, 6);
                        speed = random;
                    }
                }
            }
        }

        timeCounter++;
    }
}
