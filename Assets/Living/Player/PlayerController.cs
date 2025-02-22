using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private PlayerBase playerBase;
	private Farmer farmer;
	private Mage mage;
	private Warrior warrior;
	private Archer archer;
	private PersonaAbstract currentPersona;
	private int currentPersonaNumber = 1;

	const int NUMBER_OF_PERSONAS = 4;

	private void Start()
	{
		farmer = GetComponent<Farmer>();
		mage = GetComponent<Mage>();
		warrior = GetComponent<Warrior>();
		archer = GetComponent<Archer>();
		playerBase = GetComponent<PlayerBase>();
		SwapToPersona(farmer);
		farmer.Initialize(playerBase);
		archer.Initialize(playerBase);
		mage.Initialize(playerBase);
		warrior.Initialize(playerBase);
	}


	private void Update()
	{
		if (playerBase.canMove)
		{
			if (Input.GetKeyDown("o"))
			{
				switch (currentPersonaNumber % NUMBER_OF_PERSONAS)
				{
					case 1:
						SwapToPersona(warrior);
						break;
					case 2:
						SwapToPersona(mage);
						break;
					case 3:
						SwapToPersona(archer);
						break;
					case 0:
						SwapToPersona(farmer);
						break;
				}

				currentPersonaNumber++;
				Debug.Log("switched to" + currentPersona.PersonaName);
			}

			currentPersona.MovePotentially();

			if (Input.GetKeyDown("f"))
			{
				currentPersona.Interact();
			}

			if (Input.GetKeyDown("c"))
			{
				currentPersona.BaseAttack();
			}

			if (Input.GetKeyDown("e"))
			{
				currentPersona.SecondAbility();
			}

			if (Input.GetKeyDown("q"))
			{
				currentPersona.FirstAbility();
			}

			if (Input.GetKeyDown("b"))
			{
				currentPersona.Build();
			}

			if (Input.GetKeyDown("x"))
			{
				//BulletAttack();
			}

			if (Input.GetKeyDown("r"))
			{
				currentPersona.CommitSuicide();
			}

			if (Input.GetKeyDown("z"))
			{
				//StartCoroutine(LaserAttack());
			}

			if (Input.GetButtonDown("Jump"))
			{
				currentPersona.Jump();
			}

			if (Input.GetKeyDown(KeyCode.LeftShift))
			{
				currentPersona.Dash();
			}
		}
	}

	private void SwapToPersona(PersonaAbstract persona)
	{
		if (currentPersona != null)
		{
			currentPersona.SwapFromMe();
		}

		currentPersona = persona;
		currentPersona.SwapToMe();
		gameObject.GetComponent<SpriteRenderer>().sprite = currentPersona.GetSkin();
	}

	private void FixedUpdate()
	{
		if (currentPersona.GetMovement().x != 0)
		{
			currentPersona.Move();
		}
	}
}