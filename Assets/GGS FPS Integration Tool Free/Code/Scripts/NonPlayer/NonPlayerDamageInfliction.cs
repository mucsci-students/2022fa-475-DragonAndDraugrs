using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts.NonPlayer
{
    [RequireComponent(typeof(Collider), typeof(AudioSource))]
    public class NonPlayerDamageInfliction : MonoBehaviour
    {
        [SerializeField] Animator _NonPlayerAnimator;
        [SerializeField] Health _NonPlayerHealth;

        [Space]

        [SerializeField] Collider _TargetPlayerEnvironmentCollider;
        [SerializeField] Health _TargetPlayerHealth;

        [Space]

        [SerializeField] float _DamageHealthLoss = 10f;
        [SerializeField] GameObject _BloodSplatter;
        
        public float BloodSplatterLifeTime { get; set; } = 1f;

        public Utilities.General.Range PitchRange { get; set; } = new Utilities.General.Range(0.9f, 1.2f);

        public bool IsDamaging { get; private set; } = false;
        AudioSource AudioSourceProperty { get; set; }


        Utilities.General.Time.DownCounter DamageInflictionCountDown { get; set; } 
            = new Utilities.General.Time.DownCounter();

        void Awake()
        {
            AudioSourceProperty = GetComponent<AudioSource>();
        }

        // Prevents damaging from continuing after NonPlayer dies and respawns
        // This was caused by the OnTriggerExit not being called during NonPlayer death
        void OnDisable()
        {
            IsDamaging = false;
        }

        void Update()
        {
            DamageInflictionCountDown.Update();
            
            if (_TargetPlayerHealth.IsDead)
            {
                IsDamaging = false;
            }

            _NonPlayerAnimator.SetBool("IsHitting", IsDamaging);
        }

        void OnTriggerEnter(Collider collider)
        {
            if (_TargetPlayerEnvironmentCollider == collider)
            {
                IsDamaging = true;
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (_TargetPlayerEnvironmentCollider == collider)
            {
                IsDamaging = false;
            }
        }

        public void InflictDamageToTarget()
        {
            if (IsDamaging && !_NonPlayerHealth.IsDead)
            {
                _TargetPlayerHealth.InflictDamage(_DamageHealthLoss, null);

                AudioSourceProperty.pitch = PitchRange.RandomValue;
                AudioSourceProperty.Play();

                GameObject hitInstance = Instantiate(_BloodSplatter, transform.position, transform.rotation);
                Destroy(hitInstance, BloodSplatterLifeTime);
            }
        }
    }
}