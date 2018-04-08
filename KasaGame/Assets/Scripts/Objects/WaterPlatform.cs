using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPlatform : MonoBehaviour
{

    private GameObject _player;
    private float _originalJumpHeight;
    private bool _goDown = false;
    private Vector3 _originalPosition;
    private Animator _anim;
    private AudioSource _soundEffect;
    public float sinkDepth = 5;


    // Use this for initialization
    void Start()
    {
        _soundEffect = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
        _originalPosition = transform.position;
        _player = GameObject.FindGameObjectWithTag("Player");
        _originalJumpHeight = _player.GetComponent<JumpManager>().JumpHeight;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray downward = new Ray(_player.transform.position, _player.transform.TransformDirection(new Vector3(0, -0.5f, 0)));

        if (Physics.Raycast(downward, out hit, 1f))
        {
            if (hit.collider == GetComponent<SphereCollider>())
            {
                _anim.enabled = false;

                if (!_soundEffect.isPlaying)
                {
                    _soundEffect.Play();
                }
                _player.GetComponent<JumpManager>().StopJumping();
                _player.GetComponent<JumpManager>().ScaleJump(1.5f);
                _player.GetComponent<vThirdPersonController>().SpecialJump();
                _goDown = true;
            }
        }

        if (_player.GetComponent<vThirdPersonController>().jumpCounter == 0)
        {
            _player.GetComponent<vThirdPersonController>().jumpHeight = _originalJumpHeight;
        }

        if(transform.position.y > _originalPosition.y - sinkDepth && _goDown)
        {
            transform.position -= new Vector3(0, 1f, 0) * 10f * Time.deltaTime;
        }
        
        if(transform.position.y < _originalPosition.y - sinkDepth && _goDown)
        {
            _goDown = false;
        }

        if(transform.position.y < _originalPosition.y && !_goDown)
        {
            transform.position += new Vector3(0, 1f, 0) * 3f * Time.deltaTime;
        }

        if(transform.position.y >= _originalPosition.y && !_goDown && !_anim.enabled)
        {
            _anim.Play("BeachBall", -1, 0f);
            _anim.enabled = true;
        }
    }


}
