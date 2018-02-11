using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class VerticalMoveTrap : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float delay = 2f;
    [SerializeField] private float initialDelay = 1f;

    private Vector3 minY;
    private Vector3 maxY;
    private bool goingUp = false;
    private bool goingDown = true;
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

        if (initialDelayCounter > initialDelay)
        {
            delayCounter += Time.deltaTime;
            if (goingUp)
            {
                Vector3 movement = Vector3.MoveTowards(transform.position, maxY, Time.deltaTime * speed);
                transform.position = movement;
                if (transform.position == maxY)
                {
                    goingDown = true;
                    goingUp = false;
                }
            }
            else if (goingDown)
            {
                Vector3 movement = Vector3.MoveTowards(transform.position, minY, Time.deltaTime * speed);
                transform.position = movement;
                if (transform.position == minY)
                {
                    goingDown = false;
                }
            }

            if (delayCounter > delay)
            {
                GetComponent<AudioSource>().Play();
                goingUp = true;
                delayCounter = 0f;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        vThirdPersonController controller = collision.gameObject.GetComponent<vThirdPersonController>();
        if (collision.gameObject.tag.Equals("Player"))
        {
            Rigidbody rigidbody = controller.GetComponent<Rigidbody>();
            controller.isFlying = true;
            controller.isMovable = false;
            rigidbody.AddForce(Vector3.up * 1.5f, ForceMode.VelocityChange);
            controller.speed = -0.5f;
            Debug.Log("Collision");
        }
    }

    void OnCollisionEnter(Collision other)
    {
        MyCharManager player = other.gameObject.GetComponent<MyCharManager>();

        if (other.gameObject.tag.Equals("Player") && !player.GetComponent<vThirdPersonController>().isFlying)
        {
            if (goingDown || goingUp)
            {
                player.TakeDamage();   
            }
        }
    }
}
