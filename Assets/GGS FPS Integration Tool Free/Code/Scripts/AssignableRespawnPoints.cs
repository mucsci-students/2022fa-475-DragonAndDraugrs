using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts
{
    public class AssignableRespawnPoints : MonoBehaviour
    {
        [SerializeField] Respawn _RespawnToAffect;
        [SerializeField] Transform[] _NewRespawnPoints;

        void OnDrawGizmos()
        {
            Utilities.General.Gizmo.PositionDrawer.DrawPositions(_NewRespawnPoints, Color.blue);
        }

        public void ApplyNewRespawnPoints()
        {
            _RespawnToAffect.ChangeRespawnPoints(_NewRespawnPoints);
        }
    }
}
