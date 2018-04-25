using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class McGuffinScript : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed;

    public enum McGuffinOrigin { Level1, Level2, Level3 };

    [SerializeField]
    public McGuffinOrigin mcGuffinOrigin;

    Renderer renderer;

    public bool found = false;

    // Use this for initialization
    void Start () {

        renderer = GetComponent<Renderer>();

        Init();
	}

    private void Init()
    {
        switch (mcGuffinOrigin)
        {
            case McGuffinOrigin.Level1:
                found = Game.GetGameData().redMacGuffin;
                renderer.material.color = Color.red;
                break;
            case McGuffinOrigin.Level2:
                found = Game.GetGameData().greenMacGuffin;
                renderer.material.color = Color.green;
                break;
            case McGuffinOrigin.Level3:
                found = Game.GetGameData().blueMacGuffin;
                renderer.material.color = Color.blue;
                break;
        }
        renderer.enabled = found;
    }
	
	// Update is called once per frame
	void Update () {
        RotateMcGuffin(rotationSpeed);

        transform.Translate(Vector3.forward * Mathf.Sin(Time.time) / 200);

    }

    void RotateMcGuffin(float speed)
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
