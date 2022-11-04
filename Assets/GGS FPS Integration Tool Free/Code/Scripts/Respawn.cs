using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGSFPSIntegrationTool.Scripts
{
    public abstract class Respawn : MonoBehaviour 
    {
        public enum RespawnType : byte { Reset, Instantiate };
        public enum RespawnPointOrder : byte { Random, Sequential };

        [SerializeField] protected bool _WillSpawnOnStart;

        [SerializeField] protected Transform[] _RespawnPoints;
        [SerializeField] protected RespawnPointOrder _RespawnPointOrder = RespawnPointOrder.Random;

        [SerializeField] protected bool _WillSpawnOnGround = false;
        [SerializeField] protected float _PivotToBottomHeight = 0.5f;
        [SerializeField] protected float _RandomisedHorizontalRadiusOffset = 0f;
        [SerializeField] protected float _RandomisedVerticalUpwardsOffset = 0f;
        [SerializeField] protected float _RandomisedYRotationOffset = 0f;
        [SerializeField] protected UnityEvent _OnSpawn;

        protected short NextSequentialRespawnPointIndex { get; set; } = 0;
        protected List<GameObject> SpawnedGameObjectsList { get; set; } = new List<GameObject>();

        protected virtual void OnDrawGizmos()
        {
            Utilities.General.Gizmo.PositionDrawer.DrawPositions(_RespawnPoints, Color.green);
        }

        public void RespawnAtNextPoint()
        {
            if (_RespawnPointOrder == RespawnPointOrder.Sequential)
            {
                RespawnAtSpecificPoint(NextSequentialRespawnPointIndex);

                NextSequentialRespawnPointIndex++;

                if (NextSequentialRespawnPointIndex >= _RespawnPoints.Length)
                {
                    NextSequentialRespawnPointIndex = 0;
                }
            }
            else
            {
                // Min is inclusive & max is exclusive
                RespawnAtSpecificPoint((short)Random.Range(0, _RespawnPoints.Length));
            }
        }

        public void RespawnAtSpecificPoint(short respawnPointIndex)
        {
            SpawnGameObject(
                FindGameObjectToSpawn(), 
                GetSpawnPosition(respawnPointIndex), 
                GetSpawnQuaternion(respawnPointIndex)
                );

            // Any public Respawn function will also call this
            _OnSpawn.Invoke();
        }

        public void ChangeRespawnPoints(Transform[] newRespawnPoints)
        {
            _RespawnPoints = newRespawnPoints;
        }


        // Used to select a gameObject from spawning 
        // Such system would vary in functionality thus needs implementing in derived classes
        protected abstract GameObject FindGameObjectToSpawn();

        // Process of spawning involves similar variables but different approaches to executing such operation
        // Such system would also vary and needs implementing in derived classes
        protected abstract void SpawnGameObject(GameObject gameObjectToSpawn, Vector3 position, Quaternion rotation);

        protected Vector3 GetSpawnPosition(short respawnPointIndex)
        {
            if (_WillSpawnOnGround)
            {
                return GetGroundedSpawnPosition(respawnPointIndex, _PivotToBottomHeight);
            }
            else
            {
                return GetUngroundedSpawnPosition(respawnPointIndex);
            }
        }

        protected Vector3 GetUngroundedSpawnPosition(short respawnPointIndex)
        {
            Vector2 horizontalNormal = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
                );

            horizontalNormal.Normalize();

            Vector3 position = new Vector3(
                Random.Range(0f, _RandomisedHorizontalRadiusOffset) * horizontalNormal.x,
                Random.Range(0f, _RandomisedVerticalUpwardsOffset),
                Random.Range(0f, _RandomisedHorizontalRadiusOffset) * horizontalNormal.y
                );

            return _RespawnPoints[respawnPointIndex].position + position;
        }

        protected Vector3 GetGroundedSpawnPosition(
                short respawnPointIndex,
                float pivotToBottomHeight,
                float raycastDownwardsRange = 5f,
                float raycastAbovePivotStartHeight = 1f
            )
        {
            Vector3 offsetPositon = GetUngroundedSpawnPosition(respawnPointIndex);

            // Upwards offset unneeded & prevents it from confusing ground positioning
            offsetPositon.y = _RespawnPoints[respawnPointIndex].position.y;

            RaycastHit[] hits = Physics.RaycastAll(
                    offsetPositon + (Vector3.up * raycastAbovePivotStartHeight),
                    Vector3.down,
                    raycastDownwardsRange
                );

            bool isSpawnedGameObjectHit;

            foreach (RaycastHit h in hits)
            {
                isSpawnedGameObjectHit = false;

                // Continues to next RaycastHit, if the spawn point collider was hit
                if (h.collider.gameObject == _RespawnPoints[respawnPointIndex].gameObject)
                {
                    continue;
                }

                // Check if any spawned gameObjects were hit, if so, this RaycastHit can be dismissed 
                if (SpawnedGameObjectsList.Count > 0)
                {
                    foreach (GameObject g in SpawnedGameObjectsList)
                    {
                        if (h.collider.gameObject == g)
                        {
                            isSpawnedGameObjectHit = true;
                            break;
                        }
                    }
                }

                // Dismiss any hits on spawned gameObject, anything else is a vaild hit
                if (!isSpawnedGameObjectHit)
                {
                    return h.point + (Vector3.up * pivotToBottomHeight);
                }
            }

            // Use spawn point position if no valid colliders were found
            return _RespawnPoints[respawnPointIndex].position;
        }

        protected Quaternion GetSpawnQuaternion(short respawnPointIndex)
        {
            Quaternion rotation = Quaternion.Euler(
                0f,
                Random.Range(-_RandomisedYRotationOffset, _RandomisedYRotationOffset),
                0f
                );

            return _RespawnPoints[respawnPointIndex].rotation * rotation;
        }
    }
}