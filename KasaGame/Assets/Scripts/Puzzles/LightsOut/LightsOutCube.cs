using Invector.CharacterController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOutCube : MonoBehaviour {
    public LightsOutCube[] adjacents;
    public bool activated;
    private Light _light;
    private MeshRenderer _renderer;
    private GameObject _player;
    private bool _playerOnTop;
    private bool _playerJustEntered = false;
    public AudioSource _soundOn;
    public AudioSource _soundOff;
    private LightsOut _manager;

    private void Start()
    {
        _manager = gameObject.GetComponentInParent<LightsOut>();
        _light = GetComponent<Light>();
        _renderer = GetComponent<MeshRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player");

        if (activated)
        {
            _light.enabled = true;
            _renderer.material.color = Color.green;
        }
        else
        {
            _light.enabled = false;
            _renderer.material.color = Color.grey;
        }
    }


    public void Change()
    {
        if (activated)
        {
            if (!_soundOff.isPlaying)
            {
                _soundOff.Play();
            }
            activated = false;
            _light.enabled = false;
            _renderer.material.color = Color.grey;
            _manager.activeCubes--;
        }
        else
        {
            if (!_soundOn.isPlaying)
            {
                _soundOn.Play();
            }
            activated = true;
            _light.enabled = true;
            _renderer.material.color = Color.green;
            _manager.activeCubes++;
        }
    }

    private void Update()
    {
        RaycastHit hit;
        Ray downward = new Ray(_player.transform.position, _player.transform.TransformDirection(new Vector3(0, -0.5f, 0)));

        Debug.DrawRay(_player.transform.position, _player.transform.TransformDirection(new Vector3(0, -0.5f, 0)), Color.red);

        if (Physics.Raycast(downward, out hit, 1))
        {
            if (hit.collider == transform.GetComponent<BoxCollider>())
            {
                _playerOnTop = true;
            }
        }
        else
        {
            _playerOnTop = false;
            _playerJustEntered = false;
        }

        if (_playerOnTop == true && _playerJustEntered == false)
        {
            _playerJustEntered = true;
            Change();
            for (int i = 0; i < adjacents.Length; i++)
            {
                adjacents[i].Change();
            }
        }
    }
}
