using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGSFPSIntegrationTool.Scripts.UI
{
    public class GeneralUI : MonoBehaviour
    {
        [SerializeField] GameObject[] _Menus;

        public void ChangeMenu(int index)
        {
            foreach (GameObject g in _Menus)
            {
                g.SetActive(false);
            }

            _Menus[index].SetActive(true);
        }
    }
}