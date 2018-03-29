using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlatform : MonoBehaviour
{

    private GameObject _player;
    private float _time;
    private float _originalJumpHeight;
    private bool _goDown = false;


    // Use this for initialization
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _originalJumpHeight = _player.GetComponent<vThirdPersonController>().jumpHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.GetComponent<vThirdPersonController>().isGrounded &&
           _player.GetComponent<vThirdPersonController>().groundHit.collider.gameObject == transform.gameObject)
        {
            _player.GetComponent<vThirdPersonController>().jumpHeight = _originalJumpHeight * 1.5f;
            _player.GetComponent<vThirdPersonController>().Jump();
            _goDown = true;
        }

        if (_player.GetComponent<vThirdPersonController>().jumpCounter == 0)
        {
            _player.GetComponent<vThirdPersonController>().jumpHeight = _originalJumpHeight;
        }

        if(transform.position.y > -20 && _goDown)
        {
            transform.position -= new Vector3(0, 1f, 0) * 10f * Time.deltaTime;
        }
        
        if(transform.position.y <= -20 && _goDown)
        {
            _goDown = false;
        }

        if(transform.position.y < -8 && !_goDown)
        {
            transform.position += new Vector3(0, 1f, 0) * 2f * Time.deltaTime;
        }
          
    }


}
