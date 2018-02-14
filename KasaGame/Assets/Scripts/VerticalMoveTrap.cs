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
}
