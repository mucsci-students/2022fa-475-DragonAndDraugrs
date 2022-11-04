using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // Needed to check Dictionary elements by index using ElementAt()

namespace GGSFPSIntegrationTool.Scripts
{
    public class CursorManager : MonoBehaviour
    {
        public static bool IsCursorLocked { get; private set; }
        public static Dictionary<MonoBehaviour, bool> IsCursorLockingAuthorised { get; private set; }

        void Awake()
        {
            IsCursorLocked = false;
            IsCursorLockingAuthorised = new Dictionary<MonoBehaviour, bool>();
        }

        void Update()
        {
            UpdateCursorLocking();

            // Cursor locking
            if (IsCursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        
        void UpdateCursorLocking()
        {
            IsCursorLocked = true;
            foreach (bool b in IsCursorLockingAuthorised.Values)
            {
                if (!b)
                {
                    IsCursorLocked = false;
                    break;
                }
            }
        }

            // ? Should be moved to General namespace   
        public static void ReportCursorLockingAuthorisation(
            MonoBehaviour monoBehaviourScript, 
            bool isCursorLockingAuthorised
            )
        {
            if (IsCursorLockingAuthorised.ContainsKey(monoBehaviourScript))
            {
                IsCursorLockingAuthorised[monoBehaviourScript] = isCursorLockingAuthorised;
            }
            else
            {
                IsCursorLockingAuthorised.Add(monoBehaviourScript, isCursorLockingAuthorised);
            }
        }
    }
}