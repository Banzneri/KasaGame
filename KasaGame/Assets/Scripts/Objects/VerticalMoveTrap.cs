using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;
using System;

public class VerticalMoveTrap : MonoBehaviour, IActionObject {
    public bool _automatic;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _delay = 2f;
    [SerializeField] private float _initialDelay = 1f;
    [SerializeField] private bool _goingUp = false;
    [SerializeField] private bool _goingDown = true;
    private float _delayCounter = 0f;
    private float _initialDelayCounter = 0f;
    private Vector3 _minY;
    private Vector3 _maxY;

	void Awake () {
        _minY = new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z);
        _maxY = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (_initialDelayCounter < _initialDelay)
        {
            _initialDelayCounter += Time.deltaTime;
        }

        if (_initialDelayCounter > _initialDelay || !_automatic)
        {
            _delayCounter += Time.deltaTime;

            if (_goingUp)
            {
                transform.position = Vector3.MoveTowards(transform.position, _maxY, Time.deltaTime * _speed);
                if (transform.position == _maxY && _automatic)
                {
                    _goingDown = true;
                    _goingUp = false;
                }
            }
            else if (_goingDown)
            {
                transform.position = Vector3.MoveTowards(transform.position, _minY, Time.deltaTime * _speed);
                if (transform.position == _minY && _automatic)
                {
                    _goingDown = false;
                }
            }
            else if (_delayCounter > _delay && _automatic)
            {
                GetComponent<AudioSource>().Play();
                _goingUp = true;
                _delayCounter = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HitPlayer(other);
    }

    void OnTriggerStay(Collider other)
    {
        HitPlayer(other);
    }

    void HitPlayer(Collider playerCollider) 
    {
        if (playerCollider.gameObject.tag.Equals("Player"))
        {
            MyCharManager player = playerCollider.gameObject.GetComponent<MyCharManager>();
            vThirdPersonController controller = player.GetComponent<vThirdPersonController>();
            Rigidbody rigidbody = player.GetComponent<Rigidbody>();
            
            if (!player.Immune && player.Health > 0)
            {
                if (_goingDown || _goingUp)
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
        if (_goingUp)
        {
                _goingDown = true;
                _goingUp = false;
        }
        else if (_goingDown)
        {
                _goingDown = false;
                _goingUp = true;     
        }
    }
}
