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
    private DamageOnTriggerEnter _damage;

	void Awake () {
        _minY = new Vector3(transform.position.x, transform.position.y - transform.localScale.y, transform.position.z);
        _maxY = transform.position;
        _damage = GetComponent<DamageOnTriggerEnter>();
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        HandleMoving();
        HandleDamage();
    }

    private void HandleDamage()
    {
        if (_goingDown || _goingUp)
        {
            if (!_damage._doesDamage)
            {
                _damage._doesDamage = true;
            }
        }
        else if (_damage.enabled)
        {
            Debug.Log("DamageDisabled");
            _damage._doesDamage = false;
        }
    }

    private void HandleMoving()
    {
        if (_initialDelayCounter < _initialDelay)
        {
            _initialDelayCounter += Time.deltaTime;
        }

        if (_initialDelayCounter > _initialDelay || !_automatic)
        {
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
            else
            {
                _delayCounter += Time.deltaTime;
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
