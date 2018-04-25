using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubController : MonoBehaviour {

    [SerializeField]
    GameObject[] levelTeleporters;

    [SerializeField]
    GameObject[] mcGuffins;

	void Start () {
        // Todo: Add logic that checks which levels have been completed, and enable/disable
        // corresponding teleporters

        //Testing methods, these should probably be done with a loop
        ToggleTeleporter(levelTeleporters[3], false);
        ToggleTeleporter(levelTeleporters[4], false);
        ToggleTeleporter(levelTeleporters[5], false);
        ToggleTeleporter(6, false);
        ToggleTeleporter(7, false);
        ToggleTeleporter(8, false);

        //Set all mcGuffins to false
        for (int i = 0; i < mcGuffins.Length; i++) {
            ToggleMcGuffin(i, false);
        }
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

    public void ToggleTeleporter(GameObject teleporterPrefab, bool onOff)
    {
        FindSceneChange(teleporterPrefab).ToggleTeleporter(onOff);
    }

    public void ToggleTeleporter(int teleporterIndex, bool onOff)
    {
        if(teleporterIndex > levelTeleporters.Length - 1 ) {
            Debug.LogError("Attempted to toggle teleporter index " + teleporterIndex + " when the highest index is " + (levelTeleporters.Length - 1));
            return;
        }
        FindSceneChange(teleporterIndex).ToggleTeleporter(onOff);
    }

    public bool GetTeleporterEnabled(GameObject teleporterPrefab)
    {
        bool tEnabled = FindSceneChange(teleporterPrefab).GetTeleporterEnabled();
        return tEnabled;
    }

    public bool GetTeleporterEnabled(int teleporterIndex)
    {
        bool tEnabled = FindSceneChange(teleporterIndex).GetTeleporterEnabled();
        return tEnabled;
    }

    public void ToggleMcGuffin(GameObject mcGuffinPrefab, bool visible)
    {
        mcGuffinPrefab.GetComponent<MeshRenderer>().enabled = visible;
    }

    public void ToggleMcGuffin(int mcGuffinIndex, bool visible)
    {
        if (mcGuffinIndex > mcGuffins.Length - 1) {
            Debug.LogError("Attempted to toggle McGuffin at index " + mcGuffinIndex + " when the highest index is " + (mcGuffins.Length - 1));
            return;
        }
        mcGuffins[mcGuffinIndex].GetComponent<MeshRenderer>().enabled = visible;
    }

}
