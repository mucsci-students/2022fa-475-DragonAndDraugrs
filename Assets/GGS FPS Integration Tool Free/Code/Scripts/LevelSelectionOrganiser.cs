using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace GGSFPSIntegrationTool.Scripts
{
    [Serializable]
    class SceneDetails
    {
        public bool IsVisible;
        public int SceneIndex;
        public Sprite ButtonSprite;
        public string Title;
        public string Description;

        // For Editor
        public bool IsFoldoutOpen;
    }

    public class LevelSelectionOrganiser : MonoBehaviour
    {
        [SerializeField] Button[] _LevelButtons = new Button[6];

        [SerializeField] Text _InfoMenuTitle;
        [SerializeField] Text _InfoMenuDescription;
        [SerializeField] Button _InfoMenuStartButton;

        [SerializeField] List<SceneDetails> _SceneDetails = new List<SceneDetails>();

        int SelectedSceneIndex { get; set; } = -1;

        void Awake()
        {
            Utilities.General.ErrorChecking.ArrayChecker.ThrowIfElementIsNull(
                "Level Buttons", 
                _LevelButtons, 
                "", 
                gameObject, 
                this
                );
        }

        void Start()
        {
            foreach (Button b in _LevelButtons)
            {
                b.gameObject.SetActive(false);
            }

            int buttonIndex = 0;
            
            for (byte i = 0; i < _SceneDetails.Count; i++)
            {
                if (_SceneDetails[i].IsVisible)
                {
                    byte indexForDelegate = i; // Prevent index-out-of-range errors when applying 'i' to AddListener delegate
                    _LevelButtons[buttonIndex].onClick.AddListener(delegate { ApplyDataToLevelInfoMenu(indexForDelegate); });

                    _LevelButtons[buttonIndex].transform.Find("Image").GetComponent<Image>().sprite = _SceneDetails[i].ButtonSprite;
                    _LevelButtons[buttonIndex].transform.Find("Text").GetComponent<Text>().text = _SceneDetails[i].Title;

                    _LevelButtons[buttonIndex].gameObject.SetActive(true);

                    buttonIndex++;
                }
            }
        }

        public void ApplyDataToLevelInfoMenu(int index)
        {
            SelectedSceneIndex = index;

            _InfoMenuTitle.text = _SceneDetails[SelectedSceneIndex].Title;
            _InfoMenuDescription.text = _SceneDetails[SelectedSceneIndex].Description;
        }

        // SceneManager.LoadScene(string name) doesn't work in builds
        // Resolved with using index version of function
        public void ChangeScene()
        {
            SceneManager.LoadScene(_SceneDetails[SelectedSceneIndex].SceneIndex);
        }

        // ? Move to Utilities.General?
        //string GetSceneNameFromPath(string path)
        //{
        //    string output = path;

        //    // Removes the '.unity' at the path's end
        //    output = output.Remove(output.Length - 6);

        //    // Remove the last '/' and everything before it
        //    for (int i = output.Length - 1; i >= 0; i--)
        //    {
        //        if (output[i] == '/')
        //        {
        //            // Secord parameter states number of characters to delete, hence the '+ 1'
        //            output = output.Remove(0, i + 1);
        //            break;
        //        }
        //    }

        //    return output;
        //}
    }
}