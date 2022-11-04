using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GGSFPSIntegrationTool.Scripts.UI
{
    public class GameplayDataDisplay : MonoBehaviour
    {
        [SerializeField] Text _TimeValueText;
        [SerializeField] Text _KillsValueText;
        [SerializeField] Text _DeathsValueText;

        void OnEnable()
        {
            _TimeValueText.text = GameplayDataManager.TimeToDisplay;
            _KillsValueText.text = GameplayDataManager.KillCount.ToString();
            _DeathsValueText.text = GameplayDataManager.DeathCount.ToString();
        }
    }
}
