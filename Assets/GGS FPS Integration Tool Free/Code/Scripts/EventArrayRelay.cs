using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGSFPSIntegrationTool.Scripts
{
    // This is needed as the Animation Events does not allow functions/properties from child GameObject scripts
    public class EventArrayRelay : MonoBehaviour
    {
        [SerializeField] UnityEvent[] Functions;

        public void InvokeFromArray(int index)
        {
            Utilities.General.ErrorChecking.ArrayChecker.ThrowIfNoElements("Functions", Functions, "", gameObject, this);

            Functions[index]?.Invoke();
        }
    }
}