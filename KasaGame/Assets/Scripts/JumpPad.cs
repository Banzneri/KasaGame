using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    private GameObject _player;
    private float _originalJumpHeight;
    private AudioSource _soundEffect;
    private Animator _anim;

    // Use this for initialization
    void Start () {
		_player = GameObject.FindGameObjectWithTag("Player");
        _originalJumpHeight = _player.GetComponent<vThirdPersonController>().jumpHeight;
        _soundEffect = GetComponent<AudioSource>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(_player.GetComponent<vThirdPersonController>().isGrounded &&
           _player.GetComponent<vThirdPersonController>().groundHit.collider.gameObject == transform.gameObject)
        {
            _player.GetComponent<vThirdPersonController>().jumpHeight = _originalJumpHeight * 2f;
            _player.GetComponent<vThirdPersonController>().Jump();

            if(!_soundEffect.isPlaying)
            {
                _soundEffect.Play();
                _anim.Play("PadJump");
            }
        }

        if (_player.GetComponent<vThirdPersonController>().jumpCounter == 0)
        {
            _player.GetComponent<vThirdPersonController>().jumpHeight = _originalJumpHeight;
        }
    }
}
