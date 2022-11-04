using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts
{
    

    public class Collectable : MonoBehaviour
    {
        public enum CollectionType
        {
            Weapon,
            Ammo,
            Health,
            Lives
        }
        public enum DespawnType { Disable, Destroy }

        [SerializeField] CollectionType _CollectionType = CollectionType.Weapon;
        [SerializeField] Weapon _Weapon;
        [SerializeField] Ammo _Ammo;

        [SerializeField] bool _Enable = true;
        [SerializeField] int _AmmoInWeapon;
        [SerializeField] int _AddToAmmoTotal;
        [SerializeField] float _HealthToAdd = 10f;
        [SerializeField] short _LivesToAdd = 1;

        [SerializeField] DespawnType _DespawnType = DespawnType.Disable;
        [SerializeField] GameObject _AfterCollectionObject;
        [SerializeField] float _AfterCollectionDespawnTime = 2f;

        Utilities.Collectable.Manager CollectableManager { get; set; }

        void Awake()
        {
            CollectableManager = new Utilities.Collectable.Manager(
                (byte)_CollectionType,
                gameObject,
                _Weapon,
                _Ammo,
                _Enable,
                _AmmoInWeapon,
                _AddToAmmoTotal,
                _HealthToAdd,
                _LivesToAdd,
                (byte)_DespawnType,
                _AfterCollectionObject,
                _AfterCollectionDespawnTime
                );
        }

        void OnTriggerEnter(Collider other)
        {
            CollectableManager.DetectColliderEntry(other);
        }
    }
}