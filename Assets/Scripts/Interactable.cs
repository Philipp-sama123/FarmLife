using UnityEngine;

namespace KrazyKatgames
{
    public class Interactable : MonoBehaviour
    {
        // What are interactables (?)
        public string interactableText;

        [SerializeField] protected Collider interactableCollider;

        protected virtual void Awake()
        {
            // 
            if (interactableCollider == null)
                interactableCollider = GetComponent<Collider>();
        }
        protected virtual void Start()
        {
        }

        public virtual void Interact(PlayerManager player)
        {
            Debug.Log("YOU HAVE INTERACTED!");

            interactableCollider.enabled = false;
            player.playerInteractionManager.RemoveInteractionFromList(this);
        }
        public virtual void OnTriggerEnter(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                player.playerInteractionManager.AddInteractionToList(this);
            }
        }
        public virtual void OnTriggerExit(Collider other)
        {
            PlayerManager player = other.GetComponent<PlayerManager>();
            if (player != null)
            {
                player.playerInteractionManager.RemoveInteractionFromList(this); 
                PlayerUIManager.instance.playerUIPopUpManager.CloseAllPopupWindows();
            }
        }
    }
}