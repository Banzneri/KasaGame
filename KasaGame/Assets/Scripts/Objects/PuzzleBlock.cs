using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlock : MonoBehaviour
{
    private GameObject _player;
    private float _originalJumpHeight;
    private bool goDown;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if (goDown && transform.position.y > -40)
        {
            transform.Translate(Vector3.down * 10 * Time.deltaTime);
        }

        RaycastHit hit;
        Ray downward = new Ray(_player.transform.position, _player.transform.TransformDirection(new Vector3(0, -0.1f, 0)));

        if (Physics.Raycast(downward, out hit, 1f))
        {
            if (hit.collider.transform.GetComponent<BoxCollider>())
            {
                if (Input.GetButtonDown("Jump"))
                {
                    goDown = true;
                }

            }
        }
    }
}
