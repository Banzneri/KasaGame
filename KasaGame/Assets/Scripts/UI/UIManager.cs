using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
	[HideInInspector]
    public bool hasKey = false;
    [SerializeField] private Image goldKey;

    private GameObject[] hearts;
	private MyCharManager player;

	private float heartCount = 3f;

	private GameObject heart1;
	private GameObject heart2;
	private GameObject heart3;

	// Use this for initialization
	void Start () {
		hearts = GameObject.FindGameObjectsWithTag("Heart");
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharManager>();
		heart1 = GameObject.Find("1heart");
		heart2 = GameObject.Find("2heart");
		heart3 = GameObject.Find("3heart");
	}
	
	// Update is called once per frame
	void Update () {
		if (hasKey && !goldKey.enabled)
		{
			PickupKey();
		}
		if (player.Health == 0f && heartCount > 0f)
		{
			heartCount = 0f;
			heart1.SetActive(false);
			heart2.SetActive(false);
			heart3.SetActive(false);
		}
		else if (player.Health == 1f && heartCount > 1f)
		{
			heartCount = 1f;
			heart2.SetActive(false);
			heart3.SetActive(false);
		}
		else if (player.Health == 2f && heartCount > 2)
		{
			heartCount = 2f;
			heart3.SetActive(false); 
		}
		
		if (player.Health == 3f && heartCount < 3)
		{
			heartCount = 3;
			RefreshHearts();
		}
    }

	void RefreshHearts() 
	{
		for (int i = 0; i < hearts.Length; i++)
		{
			if (!hearts[i].activeSelf)
			{
				hearts[i].SetActive(true);
			}		
		}
	}

	public void PickupKey()
	{
		Debug.Log("Picked up key!");
		goldKey.enabled = true;
		hasKey = true;
	}

	public void LoseKey()
	{
		Debug.Log("Used key!");
		goldKey.enabled = false;
		hasKey = false;
	}
}
