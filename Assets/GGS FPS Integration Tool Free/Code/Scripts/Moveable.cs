using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts
{
    public class Moveable : MonoBehaviour
    {
        [SerializeField] Vector3 _TargetPosition = Vector3.zero;
        [SerializeField] float _TransitionDuration = 1f;

        public bool IsMovingTowardsTarget { get; set; } = false;

        Vector3 InitialPosition { get; set; }
        float CurrentInterpolation { get; set; }

        void Start()
        {
            InitialPosition = transform.localPosition;
        }

        void Update()
        {
            Utilities.General.ErrorChecking.NumericChecker.ThrowIfEqual(_TransitionDuration, "Transition Duration", 0, "", gameObject, this);

            if (IsMovingTowardsTarget)
            {
                CurrentInterpolation += Time.deltaTime / _TransitionDuration;
            }
            else
            {
                CurrentInterpolation -= Time.deltaTime / _TransitionDuration;
            }

            CurrentInterpolation = Mathf.Clamp01(CurrentInterpolation);

            transform.localPosition = Vector3.Lerp(InitialPosition, _TargetPosition, CurrentInterpolation);
        }
    }
}

