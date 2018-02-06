using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class VerticalMoveTrap : MonoBehaviour {
    [SerializeField] private float speed = 5f;
    [SerializeField] private float delay = 2f;
    [SerializeField] private float initialDelay = 1f;

    private float minY;
    private float maxY;
    private bool goingUp = false;
    private bool goingDown = true;
    private float delayCounter = 0f;
    private float initialDelayCounter = 0f;

	void Start () {
        minY = transform.position.y - transform.localScale.y;
        maxY = transform.position.y;
    }
	
	// Update is called once per frame
	void Update () {
        if (initialDelayCounter < initialDelay)
        {
            initialDelayCounter += Time.deltaTime;
        }

        if (initialDelayCounter > initialDelay)
        {
            delayCounter += Time.deltaTime;
            if (goingUp)
            {
                float y = transform.position.y + speed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
                if (transform.position.y > maxY)
                {
                    goingDown = true;
                    goingUp = false;
                }
            }
            else if (goingDown)
            {
                float y = transform.position.y - speed * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, y, transform.position.z);
                if (transform.position.y < minY)
                {
                    goingDown = false;
                }
            }

            if (delayCounter > delay)
            {
                goingUp = true;
                delayCounter = 0f;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        vThirdPersonController controller = collision.gameObject.GetComponent<vThirdPersonController>();
        if (collision.gameObject.tag.Equals("Player") && !controller.isFlying)
        {
            Rigidbody rigidbody = controller.GetComponent<Rigidbody>();
            controller.isFlying = true;
            rigidbody.AddForce(Vector3.up * 2.5f, ForceMode.VelocityChange);
            controller.speed = -0.5f;
            Debug.Log("Collision");
        }
    }
}
