using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace KrazyKatGames
{
    public class WorldItemDatabase : MonoBehaviour
    {
        public static WorldItemDatabase Instance;

        public EquippableItem unarmedEquippable;

        [Header("Weapons")]
        [SerializeField] List<EquippableItem> weapons = new();

        [Header("Equipment")]

        //  A List of all the items in Game
        [Header("Items")]
        private List<Item> items = new List<Item>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            //  add all weapons to weapons list
            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }


            //  assign all of the items a unique ID
            for (int i = 0; i < items.Count; i++)
            {
                items[i].itemID = i;
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        public EquippableItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapon => weapon.itemID == ID);
        }
    }
}