using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGSFPSIntegrationTool.Scripts
{
    public class TargetFallCounter : MonoBehaviour
    {
        [SerializeField] GameObject[] _Targets;

        [Space]

        [SerializeField] UnityEvent _OnTargetFall;
        [SerializeField] UnityEvent _OnAllTargetsFallen;

        public float FallenTargetDot { get; set; } = 0.9f;

        short NumberOfFallenTargets { get; set; }
        bool[] HasTargetFallenArray { get; set; }
        bool HasReportedAllFallen { get; set; }

        void Awake()
        {
            NumberOfFallenTargets = 0;
            HasTargetFallenArray = new bool[_Targets.Length];

            for (short i = 0; i < HasTargetFallenArray.Length; i++)
            {
                HasTargetFallenArray[i] = false;
            }
        }

        void Start()
        {
            // Calling via Start() ensures default values are set (from ObjectiveDisplay)
            // before overriding with these values
            UI.ObjectiveDisplay.MessageText = "Targets Down";
            UI.ObjectiveDisplay.TargetCompletionCount = (short)_Targets.Length;
        }

        void Update()
        {
            NumberOfFallenTargets = (short)Utilities.General.Array.BoolChecker.NumberOfStates(true, HasTargetFallenArray);
            UI.ObjectiveDisplay.CurrentCompletionCount = NumberOfFallenTargets;

            for (short i = 0; i < _Targets.Length; i++)
            {
                if (!HasTargetFallenArray[i])
                {
                    if (Vector3.Dot(_Targets[i].transform.up, Vector3.up) < FallenTargetDot)
                    {
                        _OnTargetFall?.Invoke();

                        HasTargetFallenArray[i] = true;
                    }
                }
            }

            if (Utilities.General.Array.BoolChecker.AreAllOfState(true, HasTargetFallenArray) && !HasReportedAllFallen)
            {
                _OnAllTargetsFallen?.Invoke();

                HasReportedAllFallen = true;
            }
        }

    }

}
