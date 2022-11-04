using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.ErrorChecking
{
    /// <summary>
    /// Used for throwing errors if objects are unacceptable.
    /// </summary>
    public static class ObjectChecker
    {
        /// <summary>
        /// Throw an error if object equals null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectName">Object name used in the information for the user about the cause of the error.</param>
        /// <param name="objectToCheck">Object that will be checked.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfNull<T>(string objectName, Object objectToCheck, string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            if (objectToCheck != null)
            {
                return;
            }

            string informativeText = "Object '" + objectName + "' cannot be NULL.";

            Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
        }
        /// <summary>
        /// Throw an error if object equals null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectToCheck">Object that will be checked.</param>
        /// <param name="informativeText">Information for the user about the cause of the error.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfNull<T>(Object objectToCheck, string informativeText = "", string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            if (objectToCheck != null)
            {
                return;
            }

            if (informativeText == "")
            {
                informativeText = "Object cannot be NULL.";
            }

            Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
        }
    }
}