using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GGSFPSIntegrationTool.Scripts.NonPlayer
{
    public class NonPlayerController : MonoBehaviour
    {
        [SerializeField] NonPlayerDamageInfliction _CharacterDamageInfliction;
        [SerializeField] ContactDetectionWithPlayer _CharacterPlayerContactDetection;

        [Space]

        [SerializeField] Transform _TargetPlayerTransform;
        [SerializeField] Player.PlayerDeath _TargetPlayerDeath;
        [SerializeField] Collider _TargetPlayerEnvironmentCollider;

        [Space]

        [SerializeField] float _AgentOffMeshLinkSpeed = 5f;

        Animator AnimatorProperty;
        NavMeshAgent NavMeshAgentProperty;

        Vector3 LastPosition { get; set; }
        float OriginalAgentSpeed { get; set; }

        void Awake()
        {
            _CharacterPlayerContactDetection.ApplyTargetPlayerDataOnAwake(
                _TargetPlayerEnvironmentCollider,
                _TargetPlayerDeath
                );

            AnimatorProperty = GetComponent<Animator>();
            NavMeshAgentProperty = GetComponent<NavMeshAgent>();

            OriginalAgentSpeed = NavMeshAgentProperty.speed;
        }

        void Update()
        {
            if (_CharacterPlayerContactDetection.IsPlayerInTrigger)
            {
                transform.position = LastPosition;
            }
            
            if (NavMeshAgentProperty != null && NavMeshAgentProperty.enabled)
            {
                if (_TargetPlayerTransform != null)
                {
                    if (!_TargetPlayerDeath.IsDead)
                    {
                        AnimatorProperty.SetBool("IsWalking", true);
                        NavMeshAgentProperty.SetDestination(_TargetPlayerTransform.position);
                    }
                    else
                    {
                        AnimatorProperty.SetBool("IsWalking", false);
                        NavMeshAgentProperty.SetDestination(transform.position);
                    }
                }

                // Deals with agents' speed during an off mesh link
                if (NavMeshAgentProperty.isOnOffMeshLink && NavMeshAgentProperty.speed == OriginalAgentSpeed)
                {
                    NavMeshAgentProperty.velocity = Vector3.zero;
                    NavMeshAgentProperty.speed = _AgentOffMeshLinkSpeed;
                }
                if (!NavMeshAgentProperty.isOnOffMeshLink && NavMeshAgentProperty.speed == _AgentOffMeshLinkSpeed)
                {
                    NavMeshAgentProperty.velocity = Vector3.zero;
                    NavMeshAgentProperty.speed = OriginalAgentSpeed;
                }
            }

            LastPosition = transform.position;
        }
    }
}


