using UnityEngine;

public class MenuManagerScript : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject aboutMenu;
    public GameObject settingsMenu;
    public GameObject canvas;

    void Start()
    {
        mainMenu.SetActive(true);
        aboutMenu.SetActive(false);
        settingsMenu.SetActive(false);
        //DontDestroyOnLoad (canvas);
    }

    public void Back() 
    {
        mainMenu.SetActive(true);
        aboutMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void About() 
    {
        mainMenu.SetActive(false);
        aboutMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void Settings() 
    {
        mainMenu.SetActive(false);
        aboutMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
