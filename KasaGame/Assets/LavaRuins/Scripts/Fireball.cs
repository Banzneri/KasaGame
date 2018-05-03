using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    // speed of the fireball
    private float _Speed;

    // Lifetime of the fireball
    private float _LifeTime;

	// If ball is dying, make it to collapse
	private bool _Collapse;

    // Initializes the fireball, called from FireballSpawner upon instantiation
    public void InitializeObject(Vector3 position, float speed, float lifetime, float size)
    {
        transform.localPosition = position;
        _Speed = speed;
        _LifeTime = lifetime;
        transform.localScale = new Vector3(size, size, size);
    }

	// Use this for initialization
	void Start () {
		_Collapse = false;
	}
	
	// Update is called once per frame
	void Update () {

		transform.Rotate (new Vector3 (50f, 25f, 30f) * Time.deltaTime);
		transform.Translate(transform.InverseTransformDirection(Vector3.up) * _Speed * Time.deltaTime);

		if (_Collapse) {
			transform.localScale -= new Vector3 (9f, 9f, 9f) * Time.deltaTime;

			if (transform.localScale.y < 0.001f) {
				Destroy (gameObject);
			}
				
			return;
		}
			
        _LifeTime -= Time.deltaTime;
        if(_LifeTime <= 0)
        {
			_Collapse = true;
        }
	}


}
