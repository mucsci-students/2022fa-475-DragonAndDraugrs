using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GGSFPSIntegrationTool.Scripts
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float _MaximumHealth = 100f;
        [SerializeField] float _MinimumHealth = -100f;
        [SerializeField] float _StartHealth = 100f;

        [SerializeField] bool _HasLives = false;
        [SerializeField] short _MaximumLives = 3;
        [SerializeField] short _StartLives = 1;

        // Fields made public for externalised initialisations for respawning destroyed player
        [SerializeField] public UnityEvent _OnDamage;
        [SerializeField] public UnityEvent _OnDeath;
        [SerializeField] public UnityEvent _OnDeathWithLives;
        [SerializeField] public UnityEvent _OnDeathWithoutLives;

        public bool IsDead { get; private set; } = false;
        public float HealthCount { get; private set; }

        public float MaximumHealth
        {
            get => _MaximumHealth;
            private set => _MaximumHealth = value;
        }
        public float MinimumHealth
        {
            get => _MinimumHealth;
            private set => _MinimumHealth = value;
        }

        public short LivesCount { get; private set; }

        Utilities.General.Time.DownCounter DelayedResetHealthCountDown { get; set; } = 
            new Utilities.General.Time.DownCounter();
        Utilities.General.Time.DownCounter DelayedResetLivesCountDown { get; set; } = 
            new Utilities.General.Time.DownCounter();

        void Awake()
        {
            HealthCount = _StartHealth;
            LivesCount = _StartLives;
        }

        void Update()
        {
            if (DelayedResetHealthCountDown.HasEnded)
            {
                ResetHealth();
            }

            if (DelayedResetLivesCountDown.HasEnded)
            {
                ResetLives();
            }

            DelayedResetHealthCountDown.Update();
            DelayedResetLivesCountDown.Update();

            HealthCount = Mathf.Clamp(HealthCount, _MinimumHealth, _MaximumHealth);
            LivesCount = (short)Mathf.Clamp(LivesCount, 0, _MaximumLives);

            if (HealthCount <= 0f)
            {
                if (!IsDead)
                {
                    if (_HasLives)
                    {
                        if (LivesCount > 0)
                        {
                            _OnDeathWithLives.Invoke();

                            LivesCount--;
                        }
                        else
                        {
                            _OnDeathWithoutLives.Invoke();
                        }
                    }
                    else
                    {
                        _OnDeath.Invoke();
                    }

                    IsDead = true;
                }
            }
            else
            {
                IsDead = false;
            }
        }

        public void InflictDamage(float healthLoss, Collider hitCollider)
        {
            if (healthLoss <= 0f) 
            { 
                return; 
            }

            if (hitCollider != null)
            {
                ColliderDamageMultiplier multiplierComponent =
                    hitCollider.gameObject.GetComponent<ColliderDamageMultiplier>();

                if (multiplierComponent != null)
                {
                    healthLoss *= multiplierComponent.DamageMultiplication;
                }
            }

            HealthCount -= healthLoss;
            _OnDamage.Invoke();
        }

        public void AddHealth(float amountToAdd)
        {
            if (amountToAdd <= 0f)
            {
                return;
            }

            HealthCount += amountToAdd;

            if (HealthCount > _MaximumHealth)
            {
                HealthCount = _MaximumHealth;
            }
        }

        public void AddLives(short amountToAdd)
        {
            if (amountToAdd <= 0)
            {
                return;
            }

            LivesCount += amountToAdd;

            if (LivesCount > _MaximumLives)
            {
                LivesCount = _MaximumLives;
            }
        }

        public void SetHealth(float health)
        {
            HealthCount = health;
        }

        public void ResetHealth()
        {
            HealthCount = _StartHealth;
        }

        public void ResetHealthWithDelay(float delayInSeconds)
        {
            if (!DelayedResetHealthCountDown.HasStarted)
            {
                DelayedResetHealthCountDown.Start(delayInSeconds);
            }
        }

        public void SetLives(int lives)
        {
            LivesCount = (short)lives;
        }

        public void ResetLives()
        {
            LivesCount = _StartLives;
        }

        public void ResetLivesWithDelay(float delayInSeconds)
        {
            if (!DelayedResetLivesCountDown.HasStarted)
            {
                DelayedResetLivesCountDown.Start(delayInSeconds);
            }
        }

        public void DestroyGameObject(float secondsUntilDestruction)
        {
            Destroy(gameObject, secondsUntilDestruction);
        }
    }
}
