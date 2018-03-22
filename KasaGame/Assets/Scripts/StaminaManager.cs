using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Invector.CharacterController;

public class StaminaManager : MonoBehaviour {
	[SerializeField] private Scrollbar scrollbar;
	[SerializeField] private float staminaUse = 100f;
	[SerializeField] private float staminaRegen = 20f;
	private MyCharManager player;

	private bool regenerating = false;
	private bool reachedZero = false;
	private bool sprinting = false;
	private Color originalColor;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharManager>();
		originalColor = scrollbar.colors.normalColor;
	}
	
	// Update is called once per frame
	void Update () {
		scrollbar.size = player.CurrentStamina / player.MaxStamina;
		HandleStamina();
	}

	void LateUpdate()
	{
		if (reachedZero)
		{
			player.GetComponent<vThirdPersonController>().isSprinting = false;
		}
	}

	public void HandleStamina() 
	{
		vThirdPersonController cc = player.GetComponent<vThirdPersonController>();
		float curStam = player.CurrentStamina;
		float dt = Time.deltaTime;

		if (Input.GetButtonDown("Sprint") && !reachedZero && cc.isGrounded)
		{
			sprinting = true;
			regenerating = false;
		}
		if (Input.GetButtonUp("Sprint"))
		{
			sprinting = false;
			regenerating = true;
		}

		if (sprinting && !regenerating && !reachedZero)
		{
			curStam -= dt * staminaUse;

			if (curStam < 0f)
			{
				ChangeColor(Color.gray);
				reachedZero = true;
				curStam = 0f;
				regenerating = true;
			}
		}
		if (regenerating)
		{
			curStam += dt * staminaRegen;

			if (curStam > player.MaxStamina) 
			{ 
				if (reachedZero)
				{
					ChangeColor(originalColor);
					reachedZero = false;
				}
				regenerating = false;
				curStam = player.MaxStamina; 
			}
		}
		player.CurrentStamina = curStam;
	}

	void ChangeColor(Color color)
	{
		ColorBlock cb = scrollbar.colors;
		cb.normalColor = color;
		scrollbar.colors = cb;
	}
}
