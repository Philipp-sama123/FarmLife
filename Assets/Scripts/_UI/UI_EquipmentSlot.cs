using UnityEngine;
using UnityEngine.UI;

namespace KrazyKatGames
{
    public class UI_EquipmentSlot : MonoBehaviour
    {
        [SerializeField] public Image image;
        public Item item;


        public void LoadItem(Item itemToLoad)
        {
            if (itemToLoad != null)
                image.sprite = itemToLoad.itemIcon;
            else
                Debug.LogWarning("No itemIcon (!)");
            item = itemToLoad;
        }
    }
}