using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGSFPSIntegrationTool.Scripts
{
    

    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        public enum CollisionAction { Despawn, Drop, Stick };

        [SerializeField] CollisionAction _CollisionAction = CollisionAction.Despawn;

        [SerializeField] bool _IsPostCollisionLifetimeEnabled = false;
        [SerializeField] float _WholeLifetime = 3f;
        [SerializeField] float _PreCollisionLifetime = 3f;
        [SerializeField] float _PostCollisionLifetime = 3f;

        [SerializeField] bool _WillDetonateOnCollision = false;
        [SerializeField] bool _WillDetonateOnLifetimeEnd = false;
        [SerializeField] GameObject _GameObjectToSpawnOnDetonation;

        [SerializeField] bool _IsDetonationGameObjectLifetimeLimited = true;
        [SerializeField] float _DetonationGameObjectLifetime = 4f;

        [SerializeField] float _DamageOnFirstCollision = 0f;
        [SerializeField] float _MaximumDetonationAreaDamage = 0f;
        [SerializeField] float _DetonationAreaRadius = 3f;
        [SerializeField] float _DetonationExplosionForce = 0f;

        [SerializeField] UnityEvent _OnSpawn;
        [SerializeField] UnityEvent _OnCollision;
        [SerializeField] UnityEvent _OnLifetimeEnd;

        public float RigidbodyExplosionUpwardsModifier { get; set; } = 1f;

        Rigidbody RigidbodyProperty { get; set; }  
        bool HasFirstCollisionHappened { get; set; } = false;
        bool HasStuckToCollider { get; set; } = false;
        Utilities.General.Time.DownCounter LifetimeCountDown { get; set; }
            = new Utilities.General.Time.DownCounter();

        GameObject StickPoint { get; set; }

        void Awake()
        {
            RigidbodyProperty = GetComponent<Rigidbody>();
        }

        void Start()
        {
            if (_CollisionAction != CollisionAction.Despawn && _IsPostCollisionLifetimeEnabled)
            {
                LifetimeCountDown.Start(_PreCollisionLifetime);
            }
            else
            {
                LifetimeCountDown.Start(_WholeLifetime);
            }

            _OnSpawn.Invoke();
        }

        void Update()
        {
            if (LifetimeCountDown.HasEnded)
            {
                if (_WillDetonateOnLifetimeEnd)
                {
                    SpawnDetonationGameObject();
                }

                _OnLifetimeEnd.Invoke();

                if (StickPoint != null)
                {
                    Destroy(StickPoint);
                }

                Destroy(gameObject);
            }

            UpdateSticking();

            LifetimeCountDown.Update();
        }

        void OnCollisionEnter(Collision collision)
        {
            if (!HasFirstCollisionHappened)
            {
                CollisionDamage(collision);

                if (_WillDetonateOnCollision)
                {
                    SpawnDetonationGameObject();
                }

                if (_IsPostCollisionLifetimeEnabled)
                {
                    LifetimeCountDown.Start(_PostCollisionLifetime);
                }
            }

            if (_CollisionAction == CollisionAction.Despawn)
            {
                Destroy(gameObject);
            }
            else if (_CollisionAction == CollisionAction.Stick)
            {
                StickToCollider(collision);
            }

            HasFirstCollisionHappened = true;
            
            _OnCollision.Invoke();
        }

        void UpdateSticking()
        {
            if (HasStuckToCollider)
            {
                if (StickPoint != null && StickPoint.activeInHierarchy)
                {
                    transform.position = StickPoint.transform.position;
                    transform.rotation = StickPoint.transform.rotation;
                }
                else
                {
                    RigidbodyProperty.freezeRotation = false;
                    RigidbodyProperty.detectCollisions = true;

                    HasStuckToCollider = false;
                }
            }
        }

        void SpawnDetonationGameObject()
        {
            GameObject instance = 
                Instantiate(
                    _GameObjectToSpawnOnDetonation, 
                    transform.position, 
                    transform.rotation
                    );

            Utilities.General.ObjectNamer.ShortenName(instance);

            if (_MaximumDetonationAreaDamage > 0f)
            {
                ExplosionDamage();
            }

            if (_DetonationExplosionForce > 0f)
            {
                ExplosionEffect();
            }

            if (_IsDetonationGameObjectLifetimeLimited)
            {
                Destroy(instance, _DetonationGameObjectLifetime);
            }
        }

        void CollisionDamage(Collision collision)
        {
            Health health = collision.gameObject.GetComponentInParent<Health>();

            if (health != null)
            {
                health.InflictDamage(_DamageOnFirstCollision, collision.collider);
            }
        }

        void ExplosionDamage()
        {
            Collider[] effectedColliders = Physics.OverlapSphere(transform.position, _DetonationAreaRadius);

            foreach (Collider c in effectedColliders)
            {
                Health health = c.gameObject.GetComponent<Health>();

                if (health != null)
                {
                    Collider[] effectedChildColliders = c.gameObject.GetComponentsInChildren<Collider>();

                    float highestColliderDamage = 0f;

                    foreach (Collider cc in effectedChildColliders)
                    {
                        float distance = Vector3.Distance(transform.position, cc.transform.position);
                        float distanceInterpolation = distance / _DetonationAreaRadius;

                        // Prevents values over 1, caused by disconnect between Distance & OverlapSphere
                        distanceInterpolation = Mathf.Clamp01(distanceInterpolation);

                        float colliderDamage = _MaximumDetonationAreaDamage - (_MaximumDetonationAreaDamage * distanceInterpolation);

                        ColliderDamageMultiplier damageMultiplierComponent = cc.GetComponent<ColliderDamageMultiplier>();

                        if (damageMultiplierComponent != null)
                        {
                            colliderDamage *= damageMultiplierComponent.DamageMultiplication;
                        }

                        if (colliderDamage > highestColliderDamage)
                        {
                            highestColliderDamage = colliderDamage;
                        }
                    }

                    health.InflictDamage(highestColliderDamage, null);
                }
            }
        }

        void ExplosionEffect()
        {
            Collider[] effectedColliders = Physics.OverlapSphere(transform.position, _DetonationAreaRadius); ;
            Rigidbody currentRigidbody;

            foreach (Collider c in effectedColliders)
            {
                currentRigidbody = c.attachedRigidbody;

                if (currentRigidbody != null)
                {
                    currentRigidbody.AddExplosionForce(
                        _DetonationExplosionForce,
                        transform.position,
                        _DetonationAreaRadius,
                        RigidbodyExplosionUpwardsModifier,
                        ForceMode.Impulse
                        );
                }
            }
        }

        void StickToCollider(Collision collision)
        {
            if (HasStuckToCollider)
            {
                return;
            }

            RigidbodyProperty.freezeRotation = true;
            RigidbodyProperty.detectCollisions = false;

            StickPoint = new GameObject("StickPoint");

            StickPoint.transform.position = transform.position;
            StickPoint.transform.rotation = transform.rotation;

            StickPoint.transform.parent = collision.transform;

            HasStuckToCollider = true;
        }
    }
}