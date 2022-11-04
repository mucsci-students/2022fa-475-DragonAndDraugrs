using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.Gizmo
{
    /// <summary>
    /// Draws gizmos to visualise positions.
    /// </summary>
    public static class PositionDrawer
    {
        /// <summary>
        /// Standard gizmo colour alpha value used in DrawPositions functions.
        /// </summary>
        public const float DefaultAlpha = 0.75f;
        /// <summary>
        /// Standard gizmo sphere radius used in DrawPositions functions.
        /// </summary>
        public const float DefaultRadius = 0.5f;

        /// <summary>
        /// Draw sphere gizmos at positions. Call via OnDrawGizmos or OnDrawGizmosSelected MonoBehaviour functions.
        /// </summary>
        /// <param name="points">Transfroms that resemble the gizmo positions.</param>
        /// <param name="colour">Colour of the gizmo.</param>
        /// <param name="alpha">Gizmo colour alpha value.</param>
        /// <param name="radius">Gizmo sphere radius.</param>
        public static void DrawPositions(Transform[] points, Color colour, float alpha = DefaultAlpha, float radius = DefaultRadius)
        {
            Vector3[] pointPositions = new Vector3[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                if (points[i] != null)
                {
                    pointPositions[i] = points[i].position;
                }
            }

            DrawPositions(pointPositions, colour, alpha, radius);
        }
        /// <summary>
        /// Draw sphere gizmos at positions. Call via OnDrawGizmos or OnDrawGizmosSelected MonoBehaviour functions.
        /// </summary>
        /// <param name="points">Vectors that resemble the gizmo positions.</param>
        /// <param name="colour">Colour of the gizmo.</param>
        /// <param name="alpha">Gizmo colour alpha value.</param>
        /// <param name="radius">Gizmo sphere radius.</param>
        public static void DrawPositions(Vector3[] points, Color colour, float alpha = DefaultAlpha, float radius = DefaultRadius)
        {
            if (points == null)
            {
                return;
            }

            if (points.Length <= 0)
            {
                return;
            }

            alpha = Mathf.Clamp01(alpha);
            Gizmos.color = new Color(colour.r, colour.g, colour.b, alpha);

            foreach (Vector3 p in points)
            {
                Gizmos.DrawSphere(p, radius);
            }
        }
    }
}
