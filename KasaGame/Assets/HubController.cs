using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubController : MonoBehaviour {

    [SerializeField]
    GameObject[] levelTeleporters;
    
	void Start () {
        ToggleTeleporter(levelTeleporters[3], false);
        ToggleTeleporter(levelTeleporters[4], false);
        ToggleTeleporter(levelTeleporters[5], false);

        ToggleTeleporter(6, false);
        ToggleTeleporter(7, false);
        ToggleTeleporter(8, false);
    }
	
	void Update () {
		
	}

    SceneChange FindSceneChange(GameObject teleporterPrefab)
    {
        SceneChange sceneChange = teleporterPrefab.GetComponentInChildren<SceneChange>();
        return sceneChange;
    }

    SceneChange FindSceneChange(int teleporterIndex)
    {
        if (teleporterIndex > levelTeleporters.Length-1) {
            Debug.LogError("Attempted to toggle teleporter index " + teleporterIndex + " when there are " + (levelTeleporters.Length - 1) + " indexes");
            return null;
        }else {
            SceneChange sceneChange = levelTeleporters[teleporterIndex].GetComponentInChildren<SceneChange>();
            return sceneChange;
        }
    }

    public void ToggleTeleporter(GameObject teleporter, bool onOff)
    {
        FindSceneChange(teleporter).ToggleTeleporter(onOff);
    }

    public void ToggleTeleporter(int teleporterIndex, bool onOff)
    {
        if(teleporterIndex > levelTeleporters.Length - 1 ) {
            Debug.LogError("Attempted to toggle teleporter index " + teleporterIndex + " when there are " + (levelTeleporters.Length - 1) + " indexes");
            return;
        }
        FindSceneChange(teleporterIndex).ToggleTeleporter(onOff);
    }
}
