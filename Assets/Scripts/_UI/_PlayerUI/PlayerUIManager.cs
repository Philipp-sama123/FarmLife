using UnityEngine;

namespace KrazyKatGames
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;

        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        [HideInInspector] public PlayerUIPopUpManager playerUIPopUpManager;
        [HideInInspector] public PlayerUIMainMenuManager playerUIMainMenuManager;
        private PlayerManager player;


        [Header("UI Flags")]
        public bool menuWindowIsOpen = false;
        public bool popupWindowIsOpen = false;
        public bool mainMenuWindowIsOpen = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
            playerUIPopUpManager = GetComponentInChildren<PlayerUIPopUpManager>();
            playerUIMainMenuManager = GetComponentInChildren<PlayerUIMainMenuManager>();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            player = PlayerInputManager.instance.player;
            Debug.Log("player " + player);
        }

        private void Update()
        {
        }
        public void CloseAllMenuWindows()
        {
            // playerUIMainMenuManager.CloseMainMenu();
            // playerUICharacterMenuManager.CloseCharacterMenu();
            // playerUIEquipmentManager.CloseEquipmentManagerMenu();
        }
    }
}