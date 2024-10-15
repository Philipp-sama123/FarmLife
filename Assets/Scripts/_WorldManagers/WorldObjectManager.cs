using UnityEngine;

namespace KrazyKatGames
{
    public class WorldObjectManager : MonoBehaviour
    {
        public static WorldObjectManager instance;

        // 4. When the fog walls are spawned, add them to the world fog wall list
        // 5. Grab the correct fogwall from the list on the boss manager when the boss is being initialized

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
        }

        // public void SpawnObject(NetworkObjectSpawner networkObjectSpawner)
        // {
        //    
        // }
    }
}