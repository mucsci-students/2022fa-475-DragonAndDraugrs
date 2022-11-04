using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.Array
{
    /// <summary>
    /// Used to extract members as an array from implementing classes (of this interface) that are in an array.
    /// </summary>
    /// <typeparam name="T">Data type of the target memeber.</typeparam>
    public interface IMemberAccessibleToArray<T>
    {
        /// <summary>
        /// Property that is called when a member is being accessed.
        /// </summary>
        T GetMemberForArray { get; }
    }
}
