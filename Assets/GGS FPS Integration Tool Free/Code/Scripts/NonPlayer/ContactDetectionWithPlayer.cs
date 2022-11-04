using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts.NonPlayer
{
    [RequireComponent(typeof(Collider))]
    public class ContactDetectionWithPlayer : MonoBehaviour
    {
        public bool IsPlayerInTrigger { get; private set; } = false;

        public Collider TargetPlayerCollider { get; private set; }
        public Player.PlayerDeath TargetPlayerDeath { get; private set; }

        // Prevents the need to populate addional fields
        public void ApplyTargetPlayerDataOnAwake(Collider targetPlayerCollider, Player.PlayerDeath targetPlayerDeath)
        {
            TargetPlayerCollider = targetPlayerCollider;
            TargetPlayerDeath = targetPlayerDeath;
        }

        void OnDisable()
        {
            IsPlayerInTrigger = false;
        }

        void Update()
        {
            if (TargetPlayerDeath.IsDead)
            {
                IsPlayerInTrigger = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other == TargetPlayerCollider)
            {
                IsPlayerInTrigger = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other == TargetPlayerCollider)
            {
                IsPlayerInTrigger = false;
            }
        }
    }
}

