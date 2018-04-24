using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChange : MonoBehaviour {
	[SerializeField] private string level;

    [SerializeField]
    bool teleporterEnabled = true;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player" && teleporterEnabled)
		{
			MySceneManager.LoadLevel(level);
		}
	}

    public void ToggleTeleporter(bool onOff)
    {
        teleporterEnabled = onOff;
        if (teleporterEnabled) {
            GetComponent<MeshRenderer>().enabled = true;
        }else {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
