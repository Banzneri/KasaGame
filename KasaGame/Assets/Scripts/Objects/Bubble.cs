using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour {
    private GameObject _player;
    public BubbleSpawner originSpawner;
    private float _originalJumpHeight;
    public float _floatHeight = 20;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _originalJumpHeight = originSpawner.originalPlayerJumpHeight;
    }

    // Update is called once per frame
    void Update () {

        RaycastHit hit;
        Ray downward = new Ray(_player.transform.position, _player.transform.TransformDirection(new Vector3(0, -0.5f, 0)));

        if (Physics.Raycast(downward, out hit, 1f))
        {
            if (hit.collider == GetComponent<SphereCollider>())
            {
                originSpawner.PlayEffect();
                _player.GetComponent<JumpManager>().StopJumping();
                _player.GetComponent<JumpManager>().RevertToOriginalSettings();
                _player.GetComponent<JumpManager>().SetBubbleJump();
                _player.GetComponent<vThirdPersonController>().SpecialJump();
                GameObject.Destroy(gameObject);
            }
        }

        //Bubbles like to float
        transform.position += new Vector3(0, 1f, 0) * 3f * Time.deltaTime;

        //Destroy old bubbles
        if(transform.position.y > _floatHeight)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
