using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.Console
{
    /// <summary>
    /// Logs and displays messages to the console.
    /// </summary>
    public class Logger
    {
        static string LogIntro { get; } = "GGSFPSIntegrationTool Console:";

        // Throwing exception stops execution unlike Debug.LogError, thus prevents following Unity errors from occuring.
        /// <summary>
        /// Throw an error that stops further execution and display a detailed error message in the console.
        /// </summary>
        /// <typeparam name="T">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="informativeText">Text that informs the user of something.</param>
        /// <param name="instructiveText">Text that instructs the user to do something.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the message.</param>
        /// <param name="component">Component that relates to the occurrence of the message.</param>
        public static void ThrowException<T>(string informativeText, string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            throw new System.Exception(GenerateText(informativeText, instructiveText, sourceObject, component));
        }

        /// <summary>
        /// Display a detailed warning message in the console.
        /// </summary>
        /// <typeparam name="T">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="informativeText">Text that informs the user of something.</param>
        /// <param name="instructiveText">Text that instructs the user to do something.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the message.</param>
        /// <param name="component">Component that relates to the occurrence of the message.</param>
        public static void LogWarning<T>(string informativeText, string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            Debug.LogWarning(GenerateText(informativeText, instructiveText, sourceObject, component), sourceObject);
        }

        /// <summary>
        /// Display a detailed message in the console.
        /// </summary>
        /// <typeparam name="T">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="informativeText">Text that informs the user of something.</param>
        /// <param name="instructiveText">Text that instructs the user to do something.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the message.</param>
        /// <param name="component">Component that relates to the occurrence of the message.</param>
        public static void LogMessage<T>(string informativeText, string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            Debug.Log(GenerateText(informativeText, instructiveText, sourceObject, component), sourceObject);
        }

        static string GenerateText<T>(string informativeText, string instructiveText, Object sourceObject, T component)
        {
            string
                objectData = "",
                componentData = "";

            if (sourceObject != null)
            {
                if (sourceObject is GameObject)
                {
                    objectData = "GameObject: " + sourceObject.name + '\n';
                }
                else if (sourceObject is ScriptableObject)
                {
                    objectData = "ScriptableObject: " + sourceObject.name + '\n';
                }
                else
                {
                    objectData = "Object: " + sourceObject.name + '\n';
                }
            }

            if (component != null)
            {
                componentData = "Component: " + typeof(T).ToString() + '\n';
            }

            if (informativeText != "")
            {
                if (sourceObject == null && component == null)
                {
                    informativeText = informativeText + '\n';
                }
                else
                {
                    informativeText = '\n' + informativeText + '\n';
                }
            }

            if (instructiveText != "")
            {
                instructiveText = '\n' + instructiveText + '\n';
            }

            return
                LogIntro + '\n' +
                objectData +
                componentData +
                informativeText +
                instructiveText;
        }
    }
}

