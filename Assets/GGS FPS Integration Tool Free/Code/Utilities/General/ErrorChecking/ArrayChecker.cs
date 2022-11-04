using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.ErrorChecking
{
    /// <summary>
    /// Used for throwing errors if arrays are unacceptable.
    /// </summary>
    public static class ArrayChecker
    {
        /// <summary>
        /// Throw an error if there are no elements in the array.
        /// </summary>
        /// <typeparam name="T">Data type of the array to check.</typeparam>
        /// <typeparam name="U">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="arrayName">Array name used in the information for the user about the cause of the error.</param>
        /// <param name="array">Array to check.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfNoElements<T, U>(string arrayName, T[] array, string instructiveText = "", Object sourceObject = null, U component = null) where U : Component
        {
            if (array.Length <= 0)
            {
                string informativeText = "Array '" + arrayName + "' must have elements.";

                Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
            }
        }
        /// <summary>
        /// Throw an error if there are no elements in the array.
        /// </summary>
        /// <typeparam name="T">Data type of the array to check.</typeparam>
        /// <typeparam name="U">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="array">Array to check.</param>
        /// <param name="informativeText">Information for the user about the cause of the error.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfNoElements<T, U>(T[] array, string informativeText = "", string instructiveText = "", Object sourceObject = null, U component = null) where U : Component
        {
            if (array.Length <= 0)
            {
                if (informativeText == "")
                {
                    informativeText = "Array must have elements.";
                }

                Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
            }
        }

        // Might not work with UnityEvents, try anyway.
        /// <summary>
        /// Throw an error if the array contains a null element.
        /// </summary>
        /// <typeparam name="T">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="arrayName">Array name used in the information for the user about the cause of the error.</param>
        /// <param name="array">Array to check.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfElementIsNull<T>(string arrayName, Object[] array, string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            ThrowIfNoElements(arrayName, array,  "", sourceObject, component);

            foreach (Object o in array)
            {
                if (o == null)
                {
                    string informativeText = "Array '" + arrayName + "' cannot contain null elements.";

                    Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
                }
            }
        }
        /// <summary>
        /// Throw an error if the array contains a null element.
        /// </summary>
        /// <typeparam name="T">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="array">Array to check.</param>
        /// <param name="informativeText">Information for the user about the cause of the error.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfElementIsNull<T>(Object[] array, string informativeText = "", string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            ThrowIfNoElements(array, "", "", sourceObject, component);

            foreach (Object o in array)
            {
                if (o == null)
                {
                    if (informativeText == "")
                    {
                        informativeText = "Array cannot contain null elements.";
                    }

                    Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
                }
            }
        }

        /// <summary>
        /// Throw an error if the index is over the array's range.
        /// </summary>
        /// <typeparam name="T">Data type of the array to check.</typeparam>
        /// <typeparam name="U">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="array">Array to check.</param>
        /// <param name="arrayName">Array name used in the information for the user about the cause of the error.</param>
        /// <param name="index">Index to check.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfIndexOverRange<T, U>(T[] array, string arrayName, int index, string instructiveText = "", Object sourceObject = null, U component = null) where U : Component
        {
            ThrowIfNoElements(arrayName, array, "", sourceObject, component);

            if (index >= array.Length)
            {
                string informativeText =
                    "Index must be less than length of Array '" + arrayName + "'." + '\n' +
                    "Index: " + index + '\n' +
                    "Length: " + array.Length;

                Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
            }
        }
        /// <summary>
        /// Throw an error if the index is over the array's range.
        /// </summary>
        /// <typeparam name="T">Data type of the array to check.</typeparam>
        /// <typeparam name="U">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="array">Array to check.</param>
        /// <param name="index">Index to check.</param>
        /// <param name="informativeText">Information for the user about the cause of the error.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfIndexOverRange<T, U>(T[] array, int index, string informativeText = "", string instructiveText = "", Object sourceObject = null, U component = null) where U : Component
        {
            ThrowIfNoElements(array, "", "", sourceObject, component);

            if (index >= array.Length)
            {
                if (informativeText == "")
                {
                    informativeText = "Index must be less than length of Array." + '\n' +
                    "Index: " + index + '\n' +
                    "Length: " + array.Length;
                }

                Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
            }
        }

        /// <summary>
        /// Throw an error if the index is less than zero.
        /// </summary>
        /// <typeparam name="T">Data type of the array to check.</typeparam>
        /// <typeparam name="U">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="array">Array to check.</param>
        /// <param name="arrayName">Array name used in the information for the user about the cause of the error.</param>
        /// <param name="index">Index to check.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfIndexUnderZero<T, U>(T[] array, string arrayName, int index, string instructiveText = "", Object sourceObject = null, U component = null) where U : Component
        {
            ThrowIfNoElements(arrayName, array, "", sourceObject, component);

            if (index < 0)
            {
                string informativeText =
                    "Index for Array '" + arrayName + "' must not be less than zero." + '\n' +
                    "Index: " + index;

                Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
            }
        }
        /// <summary>
        /// Throw an error if the index is less than zero.
        /// </summary>
        /// <typeparam name="T">Data type of the array to check.</typeparam>
        /// <typeparam name="U">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="array">Array to check.</param>
        /// <param name="index">Index to check.</param>
        /// <param name="informativeText">Information for the user about the cause of the error.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfIndexUnderZero<T, U>(T[] array, int index, string informativeText = "", string instructiveText = "", Object sourceObject = null, U component = null) where U : Component
        {
            ThrowIfNoElements(array, "", "", sourceObject, component);

            if (index < 0)
            {
                if (informativeText == "")
                {
                    informativeText = 
                        "Index for Array must not be less than zero." + '\n' +
                        "Index: " + index;
                }

                Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
            }
        }

        /// <summary>
        /// Throw an error if the index is over the array's range or less than zero.
        /// </summary>
        /// <typeparam name="T">Data type of the array to check.</typeparam>
        /// <typeparam name="U">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="array">Array to check.</param>
        /// <param name="arrayName">Array name used in the information for the user about the cause of the error.</param>
        /// <param name="index">Index to check.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfIndexOutOfRange<T, U>(T[] array, string arrayName, int index, string instructiveText = "", Object sourceObject = null, U component = null) where U : Component
        {
            ThrowIfNoElements(array, arrayName, "", sourceObject, component);

            ThrowIfIndexOverRange(array, arrayName, index, instructiveText, sourceObject, component);
            ThrowIfIndexUnderZero(array, arrayName, index, instructiveText, sourceObject, component);
        }
        /// <summary>
        /// Throw an error if the index is over the array's range or less than zero.
        /// </summary>
        /// <typeparam name="T">Data type of the array to check.</typeparam>
        /// <typeparam name="U">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="array">Array to check.</param>
        /// <param name="index">Index to check.</param>
        /// <param name="informativeText">Information for the user about the cause of the error.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfIndexOutOfRange<T, U>(T[] array, int index, string informativeText, string instructiveText = "", Object sourceObject = null, U component = null) where U : Component
        {
            ThrowIfNoElements(array, "", "", sourceObject, component);

            ThrowIfIndexOverRange(array, index, informativeText, instructiveText, sourceObject, component);
            ThrowIfIndexUnderZero(array, index, informativeText, instructiveText, sourceObject, component);
        }

    }
}