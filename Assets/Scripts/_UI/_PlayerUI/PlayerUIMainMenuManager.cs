using System.Collections;
using System.Collections.Generic;
using KrazyKatGames;
using UnityEngine;

public class PlayerUIMainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuUI;
    public void CloseMainMenu()
    {
        mainMenuUI.SetActive(false);
        PlayerUIManager.instance.mainMenuWindowIsOpen = false;
    }
    public void OpenMainMenu()
    {
        mainMenuUI.SetActive(true);
        PlayerUIManager.instance.mainMenuWindowIsOpen = true;
    }
}