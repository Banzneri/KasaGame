using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;
using System;

public class VerticalMoveTrap : MonoBehaviour, IActionObject {
    public bool automatic;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float delay = 2f;
    [SerializeField] private float initialDelay = 1f;

    private Vector3 minY;
    private Vector3 maxY;
    [SerializeField] private bool goingUp = false;
    [SerializeField] private bool goingDown = true;
    private float delayCounter = 0f;
    private float initialDelayCounter = 0f;

	void Start () {
        minY = new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z);
        maxY = transform.position;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (initialDelayCounter < initialDelay)
        {
            initialDelayCounter += Time.deltaTime;
        }

        if (initialDelayCounter > initialDelay || !automatic)
        {
            delayCounter += Time.deltaTime;

            if (goingUp)
            {
                Vector3 movement = Vector3.MoveTowards(transform.position, maxY, Time.deltaTime * speed);
                transform.position = movement;
                if (transform.position == maxY && automatic)
                {
                    goingDown = true;
                    goingUp = false;
                }
            }
            else if (goingDown)
            {
                Vector3 movement = Vector3.MoveTowards(transform.position, minY, Time.deltaTime * speed);
                transform.position = movement;
                if (transform.position == minY && automatic)
                {
                    goingDown = false;
                }
            }
            else if (delayCounter > delay && automatic)
            {
                GetComponent<AudioSource>().Play();
                goingUp = true;
                delayCounter = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            MyCharManager player = other.gameObject.GetComponent<MyCharManager>();
            vThirdPersonController controller = other.gameObject.GetComponent<vThirdPersonController>();
            Rigidbody rigidbody = controller.GetComponent<Rigidbody>();

            if (!player.Immune && player.Health > 0)
            {
                if (goingDown || goingUp)
                {
                    controller.isFlying = true;
                    rigidbody.velocity = Vector3.zero;
                    rigidbody.AddForce(Vector3.up * 10 , ForceMode.VelocityChange);
                    player.TakeDamage();   
                }
            }
        }
    }

    public void Action()
    {

        GetComponent<AudioSource>().Play();

            if (goingUp)
            {
                    goingDown = true;
                    goingUp = false;
            }
            else if (goingDown)
            {
                    goingDown = false;
                    goingUp = true;     
            }
    }
}
