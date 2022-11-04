using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts.Player
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField] GameObject _CharacterGameObject;
        [SerializeField] GameObject _EnvironmentCameraGameObject;
        [SerializeField] CapsuleCollider _EnvironmentCollider;
        [SerializeField] Camera _WeaponCamera;
        [SerializeField] WeaponSystems _WeaponSystems;
        [SerializeField] Animator _WeaponSpaceAnimator;
        
        [Space]

        [SerializeField] GameObject _DeathbodyGameObject;
        [SerializeField] PlayerRespawn _PlayerRespawn;
        
        [Space]

        [SerializeField] float _RespawnDelayDuration = 3f;

        public delegate void VoidDelegate();
        public static event VoidDelegate OnDeath;

        public bool IsDead { get; private set; } = false;
        public Utilities.General.Time.DownCounter RespawnCountDown { get; private set; }
            = new Utilities.General.Time.DownCounter();
        
        bool LastIsDead { get; set; } = false;

        PlayerController FPSPlayerControllerProperty { get; set; }
        Health HealthProperty { get; set; }
        CharacterController CharacterControllerProperty { get; set; }
        Camera EnvironmentCameraProperty { get; set; }
        AudioListener AudioListenerProperty { get; set; }
        Rigidbody DeathbodyRigidbody { get; set; }

        void Awake()
        {
            FPSPlayerControllerProperty = _CharacterGameObject.GetComponent<PlayerController>();
            HealthProperty = _CharacterGameObject.GetComponent<Health>();
            CharacterControllerProperty = _CharacterGameObject.GetComponent<CharacterController>();

            EnvironmentCameraProperty = _EnvironmentCameraGameObject.GetComponent<Camera>();
            AudioListenerProperty = _EnvironmentCameraGameObject.GetComponent<AudioListener>();

            DeathbodyRigidbody = _DeathbodyGameObject.GetComponent<Rigidbody>();
        }

        void Start()
        {
            _DeathbodyGameObject.SetActive(false);
        }

        void Update()
        {
            RespawnCountDown.Update();

            if (IsDead != LastIsDead)
            {
                CursorManager.ReportCursorLockingAuthorisation(this, !IsDead);
            }
            LastIsDead = IsDead;
            
        }

        public void EnableDeath()
        {
            // Prevent function being executed repeatedly
            if (IsDead)
            {
                return;
            }

            _DeathbodyGameObject.transform.position = _EnvironmentCameraGameObject.transform.position;
            _DeathbodyGameObject.transform.rotation = _EnvironmentCameraGameObject.transform.rotation;

            DeathbodyRigidbody.constraints = RigidbodyConstraints.None;

            DeathbodyRigidbody.velocity =
                CharacterControllerProperty.velocity +
                (_CharacterGameObject.transform.right * Random.Range(-1f, 1f));

            DespawnPlayer();
            _DeathbodyGameObject.SetActive(true);

            RespawnCountDown.Start(_RespawnDelayDuration);

            OnDeath?.Invoke();
            
            IsDead = true;
        }

        public void DisableDeath()
        {
            _DeathbodyGameObject.SetActive(false);
            SpawnPlayer();

            IsDead = false;
        }

        void DespawnPlayer()
        {
            ToggleComponents(false);

            // Destroy instantiated weapon gameObject
            Destroy(_WeaponSpaceAnimator.transform.GetChild(0).gameObject);
        }

        void SpawnPlayer()
        {
            _PlayerRespawn.RespawnAtNextPoint();

            ToggleComponents(true);

            // Reinitialise by calling Utilities level constructors, via their Initialise functions
            _WeaponSystems.Initialise();
            FPSPlayerControllerProperty.Initialise();
        }

        void ToggleComponents(bool isEnable)
        {
            FPSPlayerControllerProperty.enabled = isEnable;
            HealthProperty.enabled = isEnable;
            CharacterControllerProperty.enabled = isEnable;
            EnvironmentCameraProperty.enabled = isEnable;
            AudioListenerProperty.enabled = isEnable;
            _WeaponSystems.enabled = isEnable;
            _WeaponSpaceAnimator.enabled = isEnable;
            _EnvironmentCollider.enabled = isEnable;
            _WeaponCamera.enabled = isEnable;
        }
    }
}