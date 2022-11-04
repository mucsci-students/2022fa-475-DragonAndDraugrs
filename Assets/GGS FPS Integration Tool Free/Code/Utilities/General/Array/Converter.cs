using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.Array
{
    /// <summary>
    /// Extracts data from arrays in specific ways.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Finds the index of an array element.
        /// </summary>
        /// <typeparam name="T">Data type of the array elements.</typeparam>
        /// <param name="targetElement">Element to find in the array.</param>
        /// <param name="elements">Array to search.</param>
        /// <returns>Returns the element's array index.</returns>
        public static int ElementToIndex<T>(T targetElement, T[] elements)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                // Comparing generic objects cannot involve '=='
                if (targetElement.Equals(elements[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Extracts members from inside array elements, then applies them to their own array.
        /// </summary>
        /// <typeparam name="T">Data type of the target members.</typeparam>
        /// <param name="objectsToAccess">Array to extract the members from.</param>
        /// <returns>Returns array of the retrieved members.</returns>
        public static T[] ApplyMembersToArray<T>(IMemberAccessibleToArray<T>[] objectsToAccess)
        {
            T[] output = new T[objectsToAccess.Length];

            for (int i = 0; i < output.Length; i++)
            {
                output[i] = objectsToAccess[i].GetMemberForArray;
            }

            return output;
        }
    }
}