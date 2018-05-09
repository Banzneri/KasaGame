using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlock : MonoBehaviour, IActionObject
{
    private GameObject _player;
    public bool goDown = false;
    private float _startY;
    private bool _playerOnTop = false;
    private bool _downedMan;
    private PuzzleManager manager;

    private void Start()
    {
        manager = gameObject.GetComponentInParent<PuzzleManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(_player.GetComponent<MyCharManager>().Health <= 0)
        {
            goDown = false;
        }

        if (goDown && transform.position.y > -40)
        {
            transform.Translate(Vector3.down * 30 * Time.deltaTime);
        }
        else if (!goDown && transform.position.y < _startY)
        {
            transform.Translate(Vector3.up * 30 * Time.deltaTime);
        }

        RaycastHit hit;
        Ray downward = new Ray(_player.transform.position + new Vector3(0, 1f, 0), _player.transform.TransformDirection(new Vector3(0, -1f, 0)));

        Debug.DrawRay(_player.transform.position + new Vector3(0, 1f, 0), _player.transform.TransformDirection(new Vector3(0, -1f, 0)), Color.red);

        if (Physics.Raycast(downward, out hit, 2f))
        {
            if (hit.collider == transform.GetComponent<BoxCollider>())
            {
                _playerOnTop = true;
            }
            else if (_playerOnTop == true)
            {
                goDown = true;
                _playerOnTop = false;
            }
        }
        else if (_playerOnTop)
        {
                goDown = true;
                _playerOnTop = false;
        }

        if (!_downedMan && goDown)
        {
            _downedMan = true;
            manager.downedBlocks++;
        }

        if (_downedMan && !goDown)
        {
            manager.downedBlocks--;
            _downedMan = false;
        }
}

    public void Action()
    {
        if (goDown)
        {
            goDown = false;
        }
    }
}
