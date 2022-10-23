using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectMenuManagerUI : MonoBehaviour
{

    private LevelSelectManager levelSelect;

    public TextMeshProUGUI level1Text;
    public Button level2Button;
    public TextMeshProUGUI level2Text;
    public Button level3Button;
    public TextMeshProUGUI level3Text;

    // Start is called before the first frame update
    void Start()
    {
        levelSelect = FindObjectsOfType<LevelSelectManager>()[0];

        if (levelSelect.level1Time != -1)
        {
            level1Text.text = levelSelect.level1Time + "s";
        }
        if (levelSelect.level2Time != -1)
        {
            level2Text.text = levelSelect.level2Time + "s";
        }
        if (levelSelect.level3Time != -1)
        {
            level3Text.text = levelSelect.level3Time + "s";
        }

        if (levelSelect.level1Time < 51.0f && levelSelect.level1Time != -1.0f)
        {
            level2Button.interactable = true;
        }
        if (levelSelect.level2Time < 120.0f && levelSelect.level2Time != -1.0f)
        {
            level3Button.interactable = true;
        }
    }

}
