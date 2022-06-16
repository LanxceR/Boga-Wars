using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject MainMenu;

    public void ButtonPressed()
    {

    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
