using System;
using KrazyKatGames;
using UnityEngine;

public class PlayerUIMainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject mainMenuUI;

    [HideInInspector] public PlayerUIInventoryManager inventoryManager;
    [HideInInspector] public PlayerUIEquipmentManager equipmentManager;
    public void CloseMainMenu()
    {
        mainMenuUI.SetActive(false);
        
        PlayerUIManager.instance.mainMenuWindowIsOpen = false;
    }
    public void OpenMainMenu()
    {
        mainMenuUI.SetActive(true);
        PlayerUIManager.instance.mainMenuWindowIsOpen = true;

        InitializeSubMenus();
    }
    private void InitializeSubMenus()
    {
        if (!inventoryManager)
            inventoryManager = GetComponentInChildren<PlayerUIInventoryManager>();
        if (!equipmentManager)
            equipmentManager = GetComponentInChildren<PlayerUIEquipmentManager>();
    }
}