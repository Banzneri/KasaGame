using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour, ITriggerObject<IActionObject>
{
    private List<PuzzleBlock> _blocks = new List<PuzzleBlock>();
    private bool unlocked = false;
    public int downedBlocks = 0;
    public GameObject[] actionObjects;
    private AudioSource complete;

    // Use this for initialization
    void Start () {
        complete = GetComponent<AudioSource>();

        foreach (Transform trans in gameObject.transform)
        {
            _blocks.Add(trans.gameObject.GetComponentInChildren<PuzzleBlock>());
            if (trans.gameObject.GetComponentInChildren<PuzzleBlock>().goDown == true)
            {
                downedBlocks++;
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (downedBlocks == _blocks.Count && unlocked == false)
        {
            unlocked = true;
            TriggerAll();
            complete.Play();
        }
        else if (downedBlocks != _blocks.Count && unlocked == true)
        {
            unlocked = false;
            TriggerAll();
        }

        Debug.Log(downedBlocks);
    }

    public void Trigger(IActionObject actionObject)
    {
        actionObject.Action();
    }

    public void TriggerAll()
    {
        for (int i = 0; i < actionObjects.Length; i++)
        {
            actionObjects[i].GetComponent<IActionObject>().Action();
        }
    }
}
