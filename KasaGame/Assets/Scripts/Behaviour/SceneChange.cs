using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour {
	[SerializeField] private string level;
    [SerializeField] private GameObject[] completedObjs;
    [SerializeField] private Material completedMaterial;

    [SerializeField]
    private bool teleporterEnabled = true;
    
    void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && teleporterEnabled)
		{
			GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SaveScene();
			GameObject.FindGameObjectWithTag("SceneHandler").GetComponent<SceneHandler>().SavePlayer();
			MySceneManager.LoadLevel(level);
		}
	}

    public void ToggleTeleporter(bool onOff)
    {
        teleporterEnabled = onOff;
        if (teleporterEnabled) {
            //GetComponent<MeshRenderer>().enabled = true;
        }else {
            //GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void SetTeleporterCompleted()
    {
        foreach (var item in completedObjs)
        {
            item.GetComponent<Renderer>().material = completedMaterial;
        }
    }

    public bool GetTeleporterEnabled()
    {
        return teleporterEnabled;
    }
}
