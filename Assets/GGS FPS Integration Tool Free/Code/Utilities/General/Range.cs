using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General
{
    /// <summary>
    /// Represents a range between two numeric values.
    /// </summary>
    public struct Range
    {
        float _Minimum, _Maximum;

        /// <summary>
        /// Random value from within range inclusively.
        /// </summary>
        public float RandomValue
        {
            get
            {
                return Random.Range(_Minimum, _Maximum);
            }
        }

        /// <summary>
        /// Constructor to initialise range.
        /// </summary>
        /// <param name="minimum">Minimum end of the range.</param>
        /// <param name="maximum">Maximum end of the range.</param>
        public Range(float minimum, float maximum)
        {
            _Minimum = minimum;
            _Maximum = maximum;
        }
    }
}
