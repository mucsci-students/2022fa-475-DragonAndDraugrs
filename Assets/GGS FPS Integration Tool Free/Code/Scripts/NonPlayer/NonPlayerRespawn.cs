using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts.NonPlayer
{
    public class NonPlayerRespawn : Respawn
    {
        [SerializeField] GameObject[] _NonPlayerCharacters;
        [SerializeField] bool _WillAutoRespawn = true;
        [SerializeField] float _AutoRespawnDelayInSeconds;

        NonPlayerDeath CurrentNonPlayerDeath { get; set; }
        Health CurrentHealth { get; set; }

        Utilities.General.Time.DownCounter AutoRespawnCountDown { get; set; } = new Utilities.General.Time.DownCounter();

        protected override void OnDrawGizmos()
        {
            Utilities.General.Gizmo.PositionDrawer.DrawPositions(_RespawnPoints, Color.red);
        }

        void Awake()
        {
            Utilities.General.ErrorChecking.ArrayChecker.ThrowIfElementIsNull("Non Player Characters", _NonPlayerCharacters, "", gameObject, this);
        }

        void Start()
        {
            bool wasEnabledBefoerStart = false;

            // Ensures that gameObjects that were enabled in Edit Mode (before Start) are recorded in SpawnedGameObjectsList,
            // thus prevents issue with automatic spawning systems
            // GameObjects enabled in Edit Mode that resemble Instantiated respawn gameObjects are excluded from the automatic
            // spawning systems
            foreach (GameObject g in _NonPlayerCharacters)
            {
                if (g.activeSelf)
                {
                    SpawnedGameObjectsList.Add(g);
                    wasEnabledBefoerStart = true;
                }
            }

            if (_WillSpawnOnStart && !wasEnabledBefoerStart)
            {
                RespawnAtNextPoint();
            }
        }

        void Update()
        {
            UpdateRespawn();

            AutoRespawnCountDown.Update();
        }

        void UpdateRespawn()
        {
            if (_WillAutoRespawn)
            {
                NonPlayerDeath nonPlayerDeath;

                // Remove nulled list elements, as such spawned gameObject would have died & despawned
                for (short i = 0; i < SpawnedGameObjectsList.Count; i++)
                {
                    nonPlayerDeath = SpawnedGameObjectsList[i].GetComponent<NonPlayerDeath>();

                    if (nonPlayerDeath != null)
                    {
                        if (nonPlayerDeath.IsDespawnedDead)
                        {
                            SpawnedGameObjectsList.RemoveAt(i);
                        }
                    }
                }

                // Start count to spawn more gameObjects
                if (SpawnedGameObjectsList.Count < _NonPlayerCharacters.Length)
                {
                    if (!AutoRespawnCountDown.HasStarted)
                    {
                        AutoRespawnCountDown.Start(_AutoRespawnDelayInSeconds);
                    }
                }

                // Once count is finished, spawn the gameObject
                if (AutoRespawnCountDown.HasEnded)
                {
                    RespawnAtNextPoint();
                }
            }
        }

        protected override GameObject FindGameObjectToSpawn()
        {
            foreach (GameObject g in _NonPlayerCharacters)
            {
                CurrentNonPlayerDeath = g.GetComponent<NonPlayerDeath>();

                if (CurrentNonPlayerDeath != null)
                {
                    if (CurrentNonPlayerDeath.IsDespawnedDead || !g.activeSelf)  // ? use IsDespawnDead instead of IsDead?
                    {
                        return g;
                    }
                }
            }

            // Null is return if all spawnable entities are alrady active when a Respawn function is called
            // outside of automatic respawning system
            return null;
        }

        protected override void SpawnGameObject(GameObject gameObjectToSpawn, Vector3 position, Quaternion rotation)
        {
            // Prevents null gameObjectToSpawn when FindGameObjectToSpawn() returns null
            if (gameObjectToSpawn == null)
            {
                return;
            }

            CurrentHealth = gameObjectToSpawn.GetComponent<Health>();

            if (CurrentHealth != null)
            {
                CurrentHealth.ResetHealth();
            }

            CurrentNonPlayerDeath = gameObjectToSpawn.GetComponent<NonPlayerDeath>();

            if (CurrentNonPlayerDeath != null)
            {
                CurrentNonPlayerDeath.DisableDeath();
            }

            UnityEngine.AI.NavMeshAgent navMeshAgent = gameObjectToSpawn.GetComponent<UnityEngine.AI.NavMeshAgent>();

            if (navMeshAgent != null)
            {
                navMeshAgent.Warp(position);
            }

            gameObjectToSpawn.transform.position = position;
            gameObjectToSpawn.transform.rotation = rotation;

            if (!gameObjectToSpawn.activeSelf)
            {
                gameObjectToSpawn.SetActive(true);
            }

            SpawnedGameObjectsList.Add(gameObjectToSpawn);
        }
    }
}