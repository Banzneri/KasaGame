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
        ToggleParticles(teleporterPrefab, onOff);
        ToggleLights(teleporterPrefab, onOff);
    }

    public void ToggleTeleporter(int teleporterIndex, bool onOff)
    {
        if(teleporterIndex > levelTeleporters.Length - 1 ) {
            Debug.LogError("Attempted to toggle teleporter index " + teleporterIndex + " when the highest index is " + (levelTeleporters.Length - 1));
            return;
        }
        FindSceneChange(teleporterIndex).ToggleTeleporter(onOff);
        ToggleParticles(levelTeleporters[teleporterIndex], onOff);
        ToggleLights(levelTeleporters[teleporterIndex], onOff);
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

    void ToggleParticles(GameObject teleporterPrefab, bool onOff)
    {
        ParticleSystem[] parSystems;
        parSystems = teleporterPrefab.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem ps in parSystems)
        {
            if (!onOff)
            {
                ps.Stop();
            }
            else
            {
                ps.Play();
            }
        }
    }

    void ToggleLights(GameObject teleporterPrefab, bool onOff)
    {
        teleporterPrefab.GetComponentInChildren<Light>().enabled = onOff;
    }

}
