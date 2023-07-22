using UnityEngine;


public class GameLoader : MonoBehaviour
{
        public GameSystemsManager gameSystemsManager;
        private void Awake()
        {
                if (GameSystemsManager.instance == null)
                {
                        Instantiate(gameSystemsManager);
                }
        }
}
