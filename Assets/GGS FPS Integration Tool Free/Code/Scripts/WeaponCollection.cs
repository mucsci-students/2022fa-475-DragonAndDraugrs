using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts
{
    [System.Serializable]
    public class WeaponDetails :  Utilities.General.Array.IMemberAccessibleToArray<Weapon>
    {
        public Weapon WeaponScriptableObject;
        public bool IsEnabledOnStart;
        public bool IsLoadedOnStart;

        public Weapon GetMemberForArray
        {
            get => WeaponScriptableObject;
        }
    }

    [CreateAssetMenu(fileName = "New WeaponCollection", menuName = "WeaponCollection")]
    public class WeaponCollection : ScriptableObject
    {
        public List<WeaponDetails> WeaponDetailsList;
    }
}