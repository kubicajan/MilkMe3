using Code.Persona;
using Code.Persona.Blueprints;
using Persona;
using Persona.Blueprints;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Living.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerBase playerBase;
        private Farmer farmer;
        private Mage mage;
        private Warrior warrior;
        private Archer archer;

        private PersonaAbstract currentPersona;
        private int currentPersonaNumber = 1;

        private const int NUMBER_OF_PERSONAS = 4;

        private Keyboard keyboard;

        private void Awake()
        {
            keyboard = Keyboard.current;
        }

        private void Start()
        {
            farmer = GetComponent<Farmer>();
            mage = GetComponent<Mage>();
            warrior = GetComponent<Warrior>();
            archer = GetComponent<Archer>();
            playerBase = GetComponent<PlayerBase>();

            farmer.Initialize(playerBase);
            archer.Initialize(playerBase);
            mage.Initialize(playerBase);
            warrior.Initialize(playerBase);

            SwapToPersona(farmer);
        }

        private void Update()
        {
            if (!playerBase.canMove || keyboard == null)
                return;

            // Swap persona (O)
            if (keyboard.oKey.wasPressedThisFrame)
            {
                switch (currentPersonaNumber % NUMBER_OF_PERSONAS)
                {
                    case 1: SwapToPersona(warrior); break;
                    case 2: SwapToPersona(mage); break;
                    case 3: SwapToPersona(archer); break;
                    case 0: SwapToPersona(farmer); break;
                }

                currentPersonaNumber++;
                Debug.Log("Switched to " + currentPersona.PersonaName);
            }

            currentPersona.MovePotentially();

            if (keyboard.fKey.wasPressedThisFrame)
                currentPersona.Interact();

            if (keyboard.xKey.wasPressedThisFrame)
                currentPersona.Heal();

            if (keyboard.cKey.wasPressedThisFrame)
                currentPersona.BaseAttack();

            if (keyboard.eKey.wasPressedThisFrame)
                currentPersona.SecondAbility();

            if (keyboard.qKey.wasPressedThisFrame)
                currentPersona.FirstAbility();

            if (keyboard.bKey.wasPressedThisFrame)
                currentPersona.Build();

            if (keyboard.rKey.wasPressedThisFrame)
                currentPersona.CommitSuicide();

            if (keyboard.spaceKey.wasPressedThisFrame)
                currentPersona.Jump();

            if (keyboard.leftShiftKey.wasPressedThisFrame)
                currentPersona.Dash();
        }

        private void FixedUpdate()
        {
            if (currentPersona.GetMovement().x != 0)
            {
                currentPersona.Move();
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

            GetComponent<SpriteRenderer>().sprite = currentPersona.GetSkin();
        }
    }
}