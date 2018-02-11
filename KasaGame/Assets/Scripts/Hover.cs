using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour {
	enum SinCos {
		Sin,
		Cos
	};
	[SerializeField] private float min = -1f;
	[SerializeField] private float max = 1f;
	[SerializeField] private float speed = 0.5f;
	[SerializeField] private SinCos sinCos;
	private float randomDelay;
	private float delayCounter = 0;
	private Vector3 lowPoint;
	private Vector3 highPoint;
	private float curSpeed = 1;
	private Vector3 originalPos;
	// Use this for initialization
	void Start () {
		randomDelay = Random.Range(0.5f, 2f);
		originalPos = transform.position;
		highPoint = new Vector3(originalPos.x, originalPos.y + max, originalPos.z);
		lowPoint = new Vector3(originalPos.x, originalPos.y + min, originalPos.z);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (delayCounter < randomDelay)
		{
			delayCounter += Time.deltaTime;
		} else
		{
			float yPos = sinCos.Equals(SinCos.Sin) ? Mathf.Sin(Time.time) : Mathf.Cos(Time.time); 
			this.transform.position = new Vector3(transform.position.x, transform.position.y + yPos * Time.deltaTime * speed, transform.position.z);
		}
	}
}
