using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubController : MonoBehaviour {

    [SerializeField]
    GameObject[] levelTeleporters;
    [SerializeField] GameObject endGameTeleporter;

    [SerializeField]
    GameObject[] mcGuffins;

	void Awake () {
        // Todo: Add logic that checks which levels have been completed, and enable/disable
        // corresponding teleporters

        //Testing methods, these should probably be done with a loop
        GameData data = Game.GetGameData();
        for (int i = 3; i < 10; i++)
        {
            ToggleTeleporter(i, false);
        }
        if (data.level1done && data.level2done && data.level3done)
        {
            int max = data.level4done && data.level5done && data.level6done ? 9: 6;
            for (int i = 3; i < max; i++) 
                ToggleTeleporter(i, true);
        }
        SetCompletedPortals();
    }

    private void SetCompletedPortals()
    {
        for (int i = 0; i < 10; i++)
        {
            if (Game.HasFinishedLevel(i+1))
            {
                Debug.Log("HAS FINISHED!!!");
                FindSceneChange(i).SetTeleporterCompleted();
            }
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
