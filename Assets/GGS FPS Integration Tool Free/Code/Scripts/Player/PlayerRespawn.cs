using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts.Player
{
    public class PlayerRespawn : Respawn
    {
        [SerializeField] GameObject _CharacterGameObject;
        [SerializeField] Transform _CharacterEnvironmentCamera;

        CharacterController CharacterController { get; set; }
       
        protected override void OnDrawGizmos()
        {
            Utilities.General.Gizmo.PositionDrawer.DrawPositions(_RespawnPoints, Color.blue);
        }

        protected override GameObject FindGameObjectToSpawn()
        {
            return _CharacterGameObject;
        }

        protected override void SpawnGameObject(GameObject gameObjectToSpawn, Vector3 position, Quaternion rotation)
        {
            // Prevents characterController from overriding transform just after respawning,
            // causing disruption to respawing
            CharacterController = _CharacterGameObject.GetComponent<CharacterController>();
            if (CharacterController != null)
            {
                CharacterController.enabled = false;
            }

            // ? will eventually need to reset weapon variables as well?
            gameObjectToSpawn.transform.position = position;
            gameObjectToSpawn.transform.rotation = rotation;
            _CharacterEnvironmentCamera.rotation = rotation;
            
            if (CharacterController != null)
            {
                CharacterController.enabled = false;
            }
        }
    }
}