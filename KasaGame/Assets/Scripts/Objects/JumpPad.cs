using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    private GameObject _player;
    private float _originalJumpHeight;
    public AudioSource _jump;
    public AudioSource _superJump;
    private Animator _anim;

    // Use this for initialization
    void Start () {
		_player = GameObject.FindGameObjectWithTag("Player");
        _originalJumpHeight = _player.GetComponent<JumpManager>().JumpHeight;
        _anim = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        RaycastHit hit;
        Ray downward = new Ray(_player.transform.position, _player.transform.TransformDirection(new Vector3(0, -0.5f, 0)));

        if (Physics.Raycast(downward, out hit, 1f))
        {
            if (hit.collider.gameObject == this.gameObject)
            {
                if (Input.GetButton("Jump"))
                {
                    _player.GetComponent<JumpManager>().ScaleJump(1.8f);

                    if (!_superJump.isPlaying)
                    {
                        _superJump.Play();
                        _anim.Play("PadJump");
                    }
                }
                else
                {
                    _player.GetComponent<JumpManager>().ScaleJump(1.5f);

                    if (!_jump.isPlaying)
                    {
                        _jump.Play();
                        _anim.Play("PadJump");
                    }
                }
                _player.GetComponent<JumpManager>().StopJumping();
                _player.GetComponent<vThirdPersonController>().SpecialJump();
            }
        }

        if (_player.GetComponent<vThirdPersonController>().jumpCounter == 0)
        {
            _player.GetComponent<JumpManager>().JumpHeight = _originalJumpHeight;
        }
    }
}
