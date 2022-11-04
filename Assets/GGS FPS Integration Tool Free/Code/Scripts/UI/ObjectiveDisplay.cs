using UnityEngine;
using UnityEngine.UI;

namespace GGSFPSIntegrationTool.Scripts.UI
{
    public class ObjectiveDisplay : MonoBehaviour
    {
        [SerializeField] Text _ObjectiveText;
        [SerializeField] Image _ObjectiveTextBackground;

        [Space]

        [SerializeField] Color[] _AvaliableMessageColours;

        // Static properties do not work properly in UnityEvent parameters
        public static string MessageText { get; set; }
        public static Color MessageColour { get; set; }
        public static bool AreCountsDisplayed { get; set; }
        public static int CurrentCompletionCount { get; set; }
        public static int TargetCompletionCount { get; set; }
        
        public static float ExtraBackgroundWidth { get; set; } = 22f;

        protected float LastBackgroundWidth { get; private set; } = 0;

        void Awake()
        {
            // Ensures static properties are set to default values on Awake
            MessageText = "Undefined";
            MessageColour = _ObjectiveText.color;
            AreCountsDisplayed = true;
            CurrentCompletionCount = 0;
            TargetCompletionCount = 0;
        }

        void Update()
        {
            float backgroundWidth = _ObjectiveText.rectTransform.rect.width;

            if (backgroundWidth != LastBackgroundWidth)
            {
                _ObjectiveTextBackground.rectTransform.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Horizontal, backgroundWidth + ExtraBackgroundWidth
                    );
            }
            
            LastBackgroundWidth = backgroundWidth;

            if (AreCountsDisplayed)
            {
                _ObjectiveText.text = GetObjectiveTextWithCounts();
            }
            else
            {
                
                _ObjectiveText.text = MessageText;
            }

            _ObjectiveText.color = MessageColour;
        }

        protected virtual string GetObjectiveTextWithCounts()
        {
            return MessageText + " (" + CurrentCompletionCount + "/" + TargetCompletionCount + ")";
        }

        // UnityEvents-Compatable functions
        public void SetMessageText(string text)
        {
            MessageText = text;
        }
        public void SetMessageColour(int avaliableColourIndex)
        {
            MessageColour = _AvaliableMessageColours[avaliableColourIndex];
        }
        public void SetAreCountsDisplayed(bool areCountsDisplayed)
        {
            AreCountsDisplayed = areCountsDisplayed;
        }
        public void SetCurrentCompletionCount(int count)
        {
            CurrentCompletionCount = count;
        }
        public void SetTargetCompletionCount(int count)
        {
            TargetCompletionCount = count;
        }
    }
}

