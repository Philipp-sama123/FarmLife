namespace KrazyKatGames
{
    public class PlayerSoundFXManager : CharacterSoundFXManager
    {
        private PlayerManager player;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
    }
}