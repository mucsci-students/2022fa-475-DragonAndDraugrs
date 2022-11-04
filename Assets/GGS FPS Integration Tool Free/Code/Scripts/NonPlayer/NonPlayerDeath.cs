using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace GGSFPSIntegrationTool.Scripts.NonPlayer
{
    // ? refinements needed to disabling GameObjects and Componants?

    public class NonPlayerDeath : MonoBehaviour
    {
        [SerializeField] GameObject _CharacterGameObject;
        [SerializeField] GameObject _DamageInflictionGameObject;

        [Space]

        [SerializeField] float _DespawnAfterDeathDelay = 3f;

        [Space]

        [SerializeField] GameObject[] _GameObjectsToDisableOnDeath;
        [SerializeField] GameObject[] _GameObjectsToDisableOnDespawn;

        public delegate void VoidDelegate();
        public static event VoidDelegate OnDeath;

        public bool IsDead { get; private set; } = false;
        public bool IsDespawnedDead { get; private set; } = false;

        NonPlayerController CharacterNonPlayerController { get; set; }
        Health CharacterHealth { get; set; }
        NonPlayerAudio CharacterAudio { get; set; }
        CapsuleCollider CharacterCollider { get; set; }
        Animator CharacterAnimator { get; set; }
        NavMeshAgent CharacterNavMeshAgent { get; set; }

        NonPlayerDamageInfliction CharacterDamageInfliction { get; set; }
        CapsuleCollider CharacterDamageInflictionCollider { get; set; }

        // Used for resetting animator after despawn, ensuring animations start from Entry node
        RuntimeAnimatorController LastRuntimeAnimatorController { get; set; }

        bool AreComponentsInitialised { get; set; } = false;
        
        Utilities.General.Time.DownCounter DespawnCountDown { get; set; } = new Utilities.General.Time.DownCounter();

        void Awake()
        {
            InitialiseComponents();
        }

        void Update()
        {
            // Prevents NonPlayer from staying airborn during death
            if (IsDead && !CharacterNavMeshAgent.isOnOffMeshLink)
            {
                CharacterNavMeshAgent.enabled = false;
            }

            if (DespawnCountDown.HasEnded)
            {
                AfterDespawn();
            }

            DespawnCountDown.Update();
        }

        public void EnableDeath()
        {
            if (IsDead)
            {
                return;
            }

            // Some components are disabled
            CharacterHealth.enabled = false;
            CharacterCollider.enabled = false;
            CharacterDamageInfliction.enabled = false;
            CharacterDamageInflictionCollider.enabled = false;

            DespawnCountDown.Start(_DespawnAfterDeathDelay);

            CharacterAnimator.SetBool("IsDead", true);

            if (OnDeath != null)
            {
                OnDeath.Invoke();
            }

            foreach (GameObject g in _GameObjectsToDisableOnDeath)
            {
                g.SetActive(false);
            }


            IsDead = true;
        }

        void AfterDespawn()
        {
            // All components & gameObjects are disabled
            ToggleGameObject(false);
            ToggleComponents(false);

            CharacterAnimator.runtimeAnimatorController = null;

            foreach (GameObject g in _GameObjectsToDisableOnDespawn)
            {
                g.SetActive(false);
            }


            IsDespawnedDead = true;
        }

        public void DisableDeath()
        {
            InitialiseComponents();

            // All components & gameObjects are re-enabled
            ToggleGameObject(true);
            ToggleComponents(true);

            CharacterAnimator.runtimeAnimatorController = LastRuntimeAnimatorController;


            // ? refinements needed?
            foreach (GameObject g in _GameObjectsToDisableOnDeath)
            {
                g.SetActive(true);
            }
            foreach (GameObject g in _GameObjectsToDisableOnDespawn)
            {
                g.SetActive(true);
            }


            IsDead = false;
            IsDespawnedDead = false;
        }

        void ToggleGameObject(bool isEnable)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(isEnable);
            }
        }

        void ToggleComponents(bool isEnable)
        {
            CharacterNonPlayerController.enabled = isEnable;
            CharacterHealth.enabled = isEnable;
            CharacterAudio.enabled = isEnable;
            CharacterCollider.enabled = isEnable;
            CharacterAnimator.enabled = isEnable;
            CharacterNavMeshAgent.enabled = isEnable;

            CharacterDamageInfliction.enabled = isEnable;
            CharacterDamageInflictionCollider.enabled = isEnable;
        }

        // Ensures that component properties are initialised during external function calls 
        // when NonPlayer is currently disabled
        void InitialiseComponents()
        {
            if (AreComponentsInitialised)
            {
                return;
            }

            CharacterNonPlayerController = _CharacterGameObject.GetComponent<NonPlayerController>();
            CharacterHealth = _CharacterGameObject.GetComponent<Health>();
            CharacterAudio = _CharacterGameObject.GetComponent<NonPlayerAudio>();
            CharacterCollider = _CharacterGameObject.GetComponent<CapsuleCollider>();
            CharacterAnimator = _CharacterGameObject.GetComponent<Animator>();
            CharacterNavMeshAgent = _CharacterGameObject.GetComponent<NavMeshAgent>();

            CharacterDamageInfliction = _DamageInflictionGameObject.GetComponent<NonPlayerDamageInfliction>();
            CharacterDamageInflictionCollider = _DamageInflictionGameObject.GetComponent<CapsuleCollider>();

            LastRuntimeAnimatorController = CharacterAnimator.runtimeAnimatorController;

            AreComponentsInitialised = true;
        }
    }
}