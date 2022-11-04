using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Utilities.General
{
    /// <summary>
    /// Used for moddifing names of GameObjects. 
    /// </summary>
    public static class ObjectNamer
    {
        /// <summary>
        /// Shorten GameObject name from the end by a number of characters. 
        /// Designed to remove '(Clone)' after instantiating GameObjects.
        /// </summary>
        /// <param name="gameObjectToNameShorten">GameObject to shorten name.</param>
        /// <param name="numberOfCharactersToShortenBy">Number of characters to shorten name by, from its end.</param>
        public static void ShortenName(GameObject gameObjectToNameShorten, byte numberOfCharactersToShortenBy = 7)
        {
            gameObjectToNameShorten.name = gameObjectToNameShorten.name.Remove(
                gameObjectToNameShorten.name.Length - numberOfCharactersToShortenBy
                );
        }
    }
}

