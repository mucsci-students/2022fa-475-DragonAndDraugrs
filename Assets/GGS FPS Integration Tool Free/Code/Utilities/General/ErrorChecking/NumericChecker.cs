using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General.ErrorChecking
{
    /// <summary>
    /// Used for throwing errors if numeric values are unacceptable.
    /// </summary>
    public static class NumericChecker
    {
        enum ThrowCondition
        {
            Equal,
            OverMaximum,
            UnderMimimum
        }

        /// <summary>
        /// Throw an error if variable is equal to disallowed value.
        /// </summary>
        /// <typeparam name="T">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="variableToCheck">Variable to check.</param>
        /// <param name="variableName">Variable name used in the information for the user about the cause of the error.</param>
        /// <param name="disallowedValue">The comparing value that will causes an error.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfEqual<T>(float variableToCheck, string variableName, float disallowedValue, string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            ThrowIf(ThrowCondition.Equal, variableToCheck, variableName, disallowedValue, instructiveText, sourceObject, component);
        }
        /// <summary>
        /// Throw an error if variable is equal to disallowed value.
        /// </summary>
        /// <typeparam name="T">Data type of the component. Use 'Component' if the 'component' parameter is unused.</typeparam>
        /// <param name="variableToCheck">Variable to check.</param>
        /// <param name="disallowedValue">The comparing value that will causes an error.</param>
        /// <param name="informativeText">Information for the user about the cause of the error.</param>
        /// <param name="instructiveText">Instructions for the user to resolve the related issue.</param>
        /// <param name="sourceObject">Object that relates to the occurrence of the error.</param>
        /// <param name="component">Component that relates to the occurrence of the error.</param>
        public static void ThrowIfEqual<T>(float variableToCheck, float disallowedValue, string informativeText = "", string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            ThrowIf(ThrowCondition.Equal, variableToCheck, disallowedValue, informativeText, instructiveText, sourceObject, component);
        }


        public static void ThrowIfOverMaximum<T>(float variableToCheck, string variableName, float maximumValueAllowed, string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            ThrowIf(ThrowCondition.OverMaximum, variableToCheck, variableName, maximumValueAllowed, instructiveText, sourceObject, component);
        }
        public static void ThrowIfOverMaximum<T>(float variableToCheck, float maximumValueAllowed, string informativeText = "", string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            ThrowIf(ThrowCondition.OverMaximum, variableToCheck, maximumValueAllowed, informativeText, instructiveText, sourceObject, component);
        }

        public static void ThrowIfUnderMimimum<T>(float variableToCheck, string variableName, float minimumValueAllowed, string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            ThrowIf(ThrowCondition.UnderMimimum, variableToCheck, variableName, minimumValueAllowed, instructiveText, sourceObject, component);
        }
        public static void ThrowIfUnderMimimum<T>(float variableToCheck, float minimumValueAllowed, string informativeText = "", string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            ThrowIf(ThrowCondition.UnderMimimum, variableToCheck, minimumValueAllowed, informativeText, instructiveText, sourceObject, component);
        }

        static void ThrowIf<T>(ThrowCondition throwCondition, float variableToCheck, string variableName, float comparingValue, string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            string informativeText;

            switch (throwCondition)
            {
                case ThrowCondition.Equal:

                    if (variableToCheck == comparingValue)
                    {
                        informativeText = "Variable '" + variableName + "' cannot be equal to " + comparingValue + ".";

                        Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
                    }

                    break;
                case ThrowCondition.OverMaximum:

                    if (variableToCheck > comparingValue)
                    {
                        informativeText = "Variable '" + variableName + "' cannot be more than " + comparingValue + ".";

                        Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
                    }

                    break;
                case ThrowCondition.UnderMimimum:

                    if (variableToCheck < comparingValue)
                    {
                        informativeText = "Variable '" + variableName + "' cannot be less than " + comparingValue + ".";

                        Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
                    }

                    break;
            }
        }
        static void ThrowIf<T>(ThrowCondition throwCondition, float variableToCheck, float comparingValue, string informativeText = "", string instructiveText = "", Object sourceObject = null, T component = null) where T : Component
        {
            switch (throwCondition)
            {
                case ThrowCondition.Equal:

                    if (variableToCheck == comparingValue)
                    {
                        if (informativeText == "")
                        {
                            informativeText = "Variable cannot be equal to " + comparingValue + ".";
                        }

                        Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
                    }

                    break;
                case ThrowCondition.OverMaximum:

                    if (variableToCheck > comparingValue)
                    {
                        if (informativeText == "")
                        {
                            informativeText = "Variable cannot be more than " + comparingValue + ".";
                        }

                        Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
                    }

                    break;
                case ThrowCondition.UnderMimimum:

                    if (variableToCheck < comparingValue)
                    {
                        if (informativeText == "")
                        {
                            informativeText = "Variable cannot be less than " + comparingValue + ".";
                        }

                        Console.Logger.ThrowException(informativeText, instructiveText, sourceObject, component);
                    }

                    break;
            }
        }
    }
}