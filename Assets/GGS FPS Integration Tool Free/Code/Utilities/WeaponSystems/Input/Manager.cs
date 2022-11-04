using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.WeaponSystems.Input
{
    public class Manager
    {
        public string FireButtonName { get; set; }
        public string AutoFireButtonName { get; set; }
        public string ReloadButtonName { get; set; }
        public string SwitchButtonName { get; set; }
        public string AimButtonName { get; set; }
        public string RunButtonName { get; set; }
        public string[] SelectionSwitchButtonNames { get; set; }

        public string MovementYAxisName { get; set; }

        public bool IsFireDetected { get; private set; }
        public bool IsAutoFireDetected { get; private set; }
        public bool IsReloadDetected { get; private set; }
        public bool IsSwitchDetected { get; private set; }
        public bool IsAimDetected { get; private set; }
        public bool IsRunDetected { get; private set; }
        public bool IsForwardMovementDetected { get; private set; }

        public bool[] IsSelectionSwitchDetectedArray { get; set; }


        public Manager(
            string fireButtonName,
            string autoFireButtonName,
            string reloadButtonName,
            string switchButtonName,
            string aimButtonName,
            string runButtonName,
            string[] selectionSwitchButtonNames,
            string movementYAxisName
            )
        {
            FireButtonName = fireButtonName;
            AutoFireButtonName = autoFireButtonName;
            ReloadButtonName = reloadButtonName;
            SwitchButtonName = switchButtonName;
            AimButtonName = aimButtonName;
            RunButtonName = runButtonName;
            SelectionSwitchButtonNames = selectionSwitchButtonNames;
            MovementYAxisName = movementYAxisName;

            IsSelectionSwitchDetectedArray = new bool[SelectionSwitchButtonNames.Length];
        }

        public void Update()
        {
            IsFireDetected = UnityEngine.Input.GetButtonDown(FireButtonName);
            IsAutoFireDetected = UnityEngine.Input.GetButton(AutoFireButtonName);
            IsReloadDetected = UnityEngine.Input.GetButtonDown(ReloadButtonName);
            IsSwitchDetected = UnityEngine.Input.GetButtonDown(SwitchButtonName);
            IsAimDetected = UnityEngine.Input.GetButton(AimButtonName);
            IsRunDetected = UnityEngine.Input.GetButton(RunButtonName);

            for (int i = 0; i < SelectionSwitchButtonNames.Length; i++)
            {
                IsSelectionSwitchDetectedArray[i] = UnityEngine.Input.GetButtonDown(SelectionSwitchButtonNames[i]);
            }

            // Simplified ternary conditional operator
            IsForwardMovementDetected = UnityEngine.Input.GetAxis(MovementYAxisName) > 0f;
        }
    }
}