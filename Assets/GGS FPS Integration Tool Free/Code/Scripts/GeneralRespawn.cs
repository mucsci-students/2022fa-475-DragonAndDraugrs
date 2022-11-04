using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts
{
    public class GeneralRespawn : Respawn
    {
        [SerializeField] RespawnType _RespawnType = RespawnType.Reset;
        [SerializeField] List<GameObject> _GameObjectsToEnable;
        [SerializeField] GameObject _GameObjectToSpawn;

        [SerializeField] bool _WillAutoRespawn = false;
        [SerializeField] short _MaximumSpawnedGameObjectsInExistance = 1;
        [SerializeField] float _AutoRespawnDelayInSeconds = 3f;

        short DelayedRespawnPointIndex { get; set; } = -1; // Defaults to -1 when not in use so standard RespawnAtPoint is called instead

        Utilities.General.Time.DownCounter DelayCountDown { get; set; } = new Utilities.General.Time.DownCounter();
        Utilities.General.Time.DownCounter AutoRespawnDelayCountDown { get; set; } = new Utilities.General.Time.DownCounter();

        void Awake()
        {
            if (_RespawnType == RespawnType.Reset)
            {
                Utilities.General.ErrorChecking.ArrayChecker.ThrowIfElementIsNull("Game Objects To Enable", _GameObjectsToEnable.ToArray(), "", gameObject, this);
            }

            Utilities.General.ErrorChecking.ArrayChecker.ThrowIfElementIsNull("Respawn Points", _RespawnPoints, "", gameObject, this);
        }

        void Start()
        {
            bool wasEnabledBefoerStart = false;

            // Ensures that gameObjects that were enabled in Edit Mode (before Start) are recorded in SpawnedGameObjectsList,
            // thus prevents issue with automatic spawning systems
            // GameObjects enabled in Edit Mode that resemble Instantiated respawn gameObjects are excluded from the automatic
            // spawning systems
            if (_RespawnType == RespawnType.Reset)
            {
                foreach (GameObject g in _GameObjectsToEnable)
                {
                    if (g.activeSelf)
                    {
                        SpawnedGameObjectsList.Add(g);
                        wasEnabledBefoerStart = true;
                    }
                }
            }

            if (_WillSpawnOnStart && !wasEnabledBefoerStart)
            {
                RespawnAtNextPoint();
            }
        }

        void Update()
        {
            UpdateDelayedRespawn();

            UpdateAutoRespawn();

            DelayCountDown.Update();
            AutoRespawnDelayCountDown.Update();
        }

        void UpdateDelayedRespawn()
        {
            if (DelayCountDown.HasEnded)
            {
                if (DelayedRespawnPointIndex == -1)
                {
                    RespawnAtNextPoint();
                }
                else
                {
                    RespawnAtSpecificPoint(DelayedRespawnPointIndex);
                    DelayedRespawnPointIndex = -1;
                }
            }
        }

        void UpdateAutoRespawn()
        {
            if (_WillAutoRespawn)
            {
                // Remove nulled list elements, as such spawned gameObject would have been destroyed or disabled
                for (short i = 0; i < SpawnedGameObjectsList.Count; i++)
                {
                    if (
                            (_RespawnType == RespawnType.Reset && !SpawnedGameObjectsList[i].activeSelf)
                            ||
                            (_RespawnType == RespawnType.Instantiate && SpawnedGameObjectsList[i] == null)
                        )
                    {
                        SpawnedGameObjectsList.RemoveAt(i);
                    }
                }

                // Start count to spawn more gameObjects
                if (
                        (_RespawnType == RespawnType.Reset && SpawnedGameObjectsList.Count < _GameObjectsToEnable.Count)
                        ||
                        (_RespawnType == RespawnType.Instantiate && SpawnedGameObjectsList.Count < _MaximumSpawnedGameObjectsInExistance)
                    )
                {
                    if (!AutoRespawnDelayCountDown.HasStarted)
                    {
                        AutoRespawnDelayCountDown.Start(_AutoRespawnDelayInSeconds);
                    }
                }

                // Once count is finished, spawn the gameObject
                if (AutoRespawnDelayCountDown.HasEnded)
                {
                    RespawnAtNextPoint();
                }
            }
        }

        public void DelayedRespawnAtNextPoint(float delayInSeconds)
        {
            if (!DelayCountDown.HasStarted)
            {
                DelayCountDown.Start(delayInSeconds);
            }
        }

        // This cannot be called via UnityEvents fields as it has two parameters, call via scripts instead
        public void DelayedRespawnAtSpecificPoint(short respawnPointIndex, float delayInSeconds)
        {
            if (!DelayCountDown.HasStarted)
            {
                DelayCountDown.Start(delayInSeconds);
                DelayedRespawnPointIndex = respawnPointIndex;
            }
        }

        protected override GameObject FindGameObjectToSpawn()
        {
            GameObject gameObjectToSpawn = null;

            if (_RespawnType == RespawnType.Reset)
            {
                // Chooses a disabled gameObject from _GameObjectsToEnable to be re-enabled
                foreach (GameObject g in _GameObjectsToEnable)
                {
                    if (!g.activeSelf)
                    {
                        gameObjectToSpawn = g;
                        break;
                    }
                }

                // Null gameObjectToSpawn would happen through manual calls of a Respawn function
                if (gameObjectToSpawn == null)
                {
                    // For preventing additional gameObject from spawning
                    return null;
                }
            }
            else
            {
                gameObjectToSpawn = _GameObjectToSpawn;
            }

            return gameObjectToSpawn;
        }

        protected override void SpawnGameObject(GameObject gameObjectToSpawn, Vector3 position, Quaternion rotation)
        {
            if (_RespawnType == RespawnType.Reset)
            {
                gameObjectToSpawn.transform.position = position;
                gameObjectToSpawn.transform.rotation = rotation;

                if (!gameObjectToSpawn.activeSelf)
                {
                    gameObjectToSpawn.SetActive(true);
                }

                SpawnedGameObjectsList.Add(gameObjectToSpawn);
            }
            else
            {
                GameObject instantiatedGameObject = Instantiate(gameObjectToSpawn, position, rotation);

                // Remove "(Clone)" from newly spawned gameObject
                Utilities.General.ObjectNamer.ShortenName(instantiatedGameObject);

                SpawnedGameObjectsList.Add(instantiatedGameObject);
            }
        }
    }
}
