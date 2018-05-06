using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour {

    [SerializeField]
    float speed;

    [SerializeField]
    Transform target;

    Vector3 direction;

    float minDistance;
    float maxDistance;

    Vector3 distanceToPlayer;
    
	void Start () {
        minDistance = transform.localScale.x * 10;
        maxDistance = minDistance * 2;
        
        direction = new Vector3(Random.value, Random.value, Random.value);

        speed = speed / (transform.localScale.x/2);
        transform.rotation = Quaternion.AngleAxis(Random.Range(0, 360), direction);
	}
	
	void Update () {
        distanceToPlayer = target.position - transform.position;
        
        if(distanceToPlayer.magnitude < minDistance) {
            transform.Translate(-distanceToPlayer.normalized * Time.deltaTime * speed*10);
        }
        else if(distanceToPlayer.magnitude > maxDistance) {
            transform.Translate(distanceToPlayer.normalized * Time.deltaTime * speed*10);
        }else {
            transform.RotateAround(target.position, Vector3.up, Time.deltaTime * speed);
            transform.Rotate(direction * Time.deltaTime * speed);
        }
        
        
	}
}
