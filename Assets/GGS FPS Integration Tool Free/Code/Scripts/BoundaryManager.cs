using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts
{
    public class BoundaryManager : MonoBehaviour
    {
        [SerializeField] Vector3 _Centre = Vector3.zero;
        [SerializeField] Vector3 _Dimensions = Vector3.one;

        public string PlayerGameObjectsTag { get; set; } = "Player";
        public string DeathBodyGameObjectsTag { get; set; } = "Deathbody";
        public string LooseGameObjectsTag { get; set; } = "Loose";
        
        public int GameObjectPositionCheckPerFrame { get; set; } = 1;
        List<GameObject> BoundaryRestrictedGameObjects { get; set; } = new List<GameObject>();
        
        int RemainingChecksForFrame { get; set; } = 0;
        int CurrentCheckIndex { get; set; } = 0;

        void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
            Gizmos.DrawCube(_Centre, _Dimensions);

            Gizmos.color = new Color(1f, 0f, 0f, 1f);
            Gizmos.DrawWireCube(_Centre, _Dimensions);
        }

        void Awake()
        {
            BoundaryRestrictedGameObjects.AddRange(GameObject.FindGameObjectsWithTag(PlayerGameObjectsTag));
            BoundaryRestrictedGameObjects.AddRange(GameObject.FindGameObjectsWithTag(DeathBodyGameObjectsTag));
            BoundaryRestrictedGameObjects.AddRange(GameObject.FindGameObjectsWithTag(LooseGameObjectsTag));
        }

        void Update()
        {
            while(RemainingChecksForFrame > 0)
            {
                bool isOutOfBounds = false;

                Vector3 position = 
                    BoundaryRestrictedGameObjects[CurrentCheckIndex].transform.position;

                if (
                        position.x > _Centre.x + _Dimensions.x / 2f 
                        || 
                        position.x < _Centre.x - _Dimensions.x / 2f
                    )
                {
                    isOutOfBounds = true;
                }
                else if (
                        position.y > _Centre.y + _Dimensions.y / 2f
                        ||
                        position.y < _Centre.y - _Dimensions.y / 2f
                    )
                {
                    isOutOfBounds = true;
                }
                else if (
                        position.z > _Centre.z + _Dimensions.z / 2f
                        ||
                        position.z < _Centre.z - _Dimensions.z / 2f
                    )
                {
                    isOutOfBounds = true;
                }

                if (isOutOfBounds)
                {
                    GameObject currentGameObject = BoundaryRestrictedGameObjects[CurrentCheckIndex];

                    
                    if (currentGameObject.tag == PlayerGameObjectsTag)
                    {
                        Health health = currentGameObject.GetComponent<Health>();
                        if (health != null)
                        {
                            health.SetHealth(health.MinimumHealth);
                        }
                    }
                    else if (currentGameObject.tag == DeathBodyGameObjectsTag)
                    {
                        Rigidbody rigidbody = currentGameObject.GetComponent<Rigidbody>();
                        if (rigidbody != null)
                        {
                            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
                        }
                    }
                    else if (currentGameObject.tag == LooseGameObjectsTag)
                    {
                        currentGameObject.SetActive(false);
                        BoundaryRestrictedGameObjects.RemoveAt(CurrentCheckIndex);
                    }

                }

                CurrentCheckIndex++;

                if (CurrentCheckIndex >= BoundaryRestrictedGameObjects.Count)
                {
                    CurrentCheckIndex = 0;
                }

                RemainingChecksForFrame--;
            }

            RemainingChecksForFrame = GameObjectPositionCheckPerFrame;
        }
    }
}
