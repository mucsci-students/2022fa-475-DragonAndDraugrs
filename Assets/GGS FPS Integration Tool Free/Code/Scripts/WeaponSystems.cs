using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UWS = GGSFPSIntegrationTool.Utilities.WeaponSystems;

namespace GGSFPSIntegrationTool.Scripts
{
    public class WeaponSystems : MonoBehaviour
    {
        static class Tooltips
        {
            public const string
                _CharacterController = "CharacterController of the Player.",
                _CameraRaySpawn = "Camera used to project firing ray via its Z axis.",
                _WeaponSpaceGameObject = "Child 'Weapon Space' GameObject of this GameObject.",

                _WeaponCollection = "WeaponCollection to use on this character.",
                _BloodSplatterImpact = "Prefab involving a Particle System that is played when a damagable GameObject is hit.",

                _FireButtonName = "Key or button used to fire weapon.",
                _AutoFireButtonName = "Key or button that is held down for automatic firing. Often the same as Input Auto Fire.",
                _ReloadButtonName = "Key or button used to reload weapon.",
                _SwitchButtonName = "Key or button used to switch weapon.",
                _AimButtonName = "Key or button used to aim weapon.",
                _RunButtonName = "Key or button that is held down to run.",
                _SelectionSwitchButtonNames = "Keys and/or buttons used for selecting a weapon to switch to. " + 
                "Order of elements in array correspond to order of weapons in WeaponCollection",

                _MovementYAxisName = "Name of axis used for forward and back character movements.",

                _MouseXInfluenceAxisName = "Name of axis specified in Input Manager for left and right mouse movements.",
                _MouseYInfluenceAxisName = "Name of axis specified in Input Manager for up and down mouse movements.",

                _FireRaycastIgnorableLayerNames = "Layers that Raycasts should ignore.";
        }

        [SerializeField] [Tooltip(Tooltips._CharacterController)] CharacterController _CharacterController;
        [SerializeField] [Tooltip(Tooltips._CameraRaySpawn)] Transform _CameraRaySpawn;
        [SerializeField] [Tooltip(Tooltips._WeaponSpaceGameObject)] GameObject _WeaponSpaceGameObject;

        [SerializeField] [Tooltip(Tooltips._WeaponCollection)] WeaponCollection _WeaponCollection;
        [SerializeField] [Tooltip(Tooltips._BloodSplatterImpact)] GameObject _BloodSplatterImpact;

        [Header("Button Names")]
        [SerializeField] [Tooltip(Tooltips._FireButtonName)] string _FireButtonName;
        [SerializeField] [Tooltip(Tooltips._AutoFireButtonName)] string _AutoFireButtonName;
        [SerializeField] [Tooltip(Tooltips._ReloadButtonName)] string _ReloadButtonName;
        [SerializeField] [Tooltip(Tooltips._SwitchButtonName)] string _SwitchButtonName;
        [SerializeField] [Tooltip(Tooltips._AimButtonName)] string _AimButtonName;
        [SerializeField] [Tooltip(Tooltips._RunButtonName)] string _RunButtonName;
        [SerializeField] [Tooltip(Tooltips._SelectionSwitchButtonNames)] string[] _SelectionSwitchButtonNames;

        [Header("Movement Axes")]
        [SerializeField] [Tooltip(Tooltips._MovementYAxisName)] string _MovementYAxisName;

        [Header("Mouse Influence Axes")]
        [SerializeField] [Tooltip(Tooltips._MouseXInfluenceAxisName)] string _MouseXInfluenceAxisName;
        [SerializeField] [Tooltip(Tooltips._MouseYInfluenceAxisName)] string _MouseYInfluenceAxisName;

        [Header("Layer Names")]
        [SerializeField] [Tooltip(Tooltips._FireRaycastIgnorableLayerNames)] LayerMask _FireRaycastIgnorableLayers;


        public UWS.Manager WeaponSpaceManager { get; set; }

        void Awake()
        {
            WeaponSpaceManagerAwake();
        }

        void Start()
        {
            WeaponSpaceManager.Start();
        }

        void Update()
        {
            if (UI.PauseMenu.IsGamePaused || UI.EndGameMenu.HasGameEnded)
            {
                return;
            }

            WeaponSpaceManager.Update();
        }

        public void Initialise()
        {
            WeaponSpaceManagerAwake();
            WeaponSpaceManager.Start();
        }

        void WeaponSpaceManagerAwake()
        {
            WeaponSpaceManager = new UWS.Manager(
                _CharacterController,
                _WeaponCollection,
                _WeaponSpaceGameObject.GetComponent<Animator>(),
                _WeaponSpaceGameObject.GetComponent<AudioSource>(),
                transform,
                _WeaponSpaceGameObject.transform,
                _CameraRaySpawn,
                _MouseXInfluenceAxisName,
                _MouseYInfluenceAxisName,
                _FireButtonName,
                _AutoFireButtonName,
                _ReloadButtonName,
                _SwitchButtonName,
                _AimButtonName,
                _RunButtonName,
                _SelectionSwitchButtonNames,
                _MovementYAxisName,
                _BloodSplatterImpact,
                _FireRaycastIgnorableLayers
                );
        }
    }
}