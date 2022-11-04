using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts
{
    [CreateAssetMenu(fileName = "New Ammo", menuName = "Ammo")]
    public class Ammo : ScriptableObject
    {
        static class Tooltips
        {
            public const string
                StartAmount = "Total ammo of this type avaliable at start.",
                AmmoIcon = "Icon used in UI to resemble this type of ammo.";
        }

        [Tooltip(Tooltips.StartAmount)] public int StartAmount = 10;
        [Tooltip(Tooltips.AmmoIcon)] public Sprite AmmoIcon;
    }
}