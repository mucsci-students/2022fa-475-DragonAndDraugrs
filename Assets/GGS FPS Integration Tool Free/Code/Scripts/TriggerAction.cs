using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGSFPSIntegrationTool.Scripts
{
    public class TriggerAction : MonoBehaviour
    {
        [SerializeField] Collider[] _TargetColliders;

        [Space]

        [SerializeField] UnityEvent _OnTriggerEnter;
        [SerializeField] UnityEvent _OnTriggerExit;
        [SerializeField] UnityEvent _OnTriggerStay;

        void OnTriggerEnter(Collider other)
        {
            if (DoesColliderMatch(other, _TargetColliders))
            {
                _OnTriggerEnter?.Invoke();
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (DoesColliderMatch(other, _TargetColliders))
            {
                _OnTriggerExit?.Invoke();
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (DoesColliderMatch(other, _TargetColliders))
            {
                _OnTriggerStay?.Invoke();
            }
        }

        bool DoesColliderMatch(Collider detectedCollider, Collider[] targetColliders)
        {
            foreach (Collider c in targetColliders)
            {
                if (c == null)
                {
                    continue;
                }

                if (detectedCollider == c)
                {
                    return true;
                }
            }

            return false;
        }
    }
}