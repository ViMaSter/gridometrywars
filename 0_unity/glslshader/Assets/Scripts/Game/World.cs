using UnityEngine;

namespace Game
{
    [ExecuteInEditMode]
    public class World : MonoBehaviour
    {
        private static World _instance;
        public static World Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                if (_instance != null)
                {
                    Debug.LogWarning("Trying to setup another instance of the world. Dropping it.");
                    return;
                }

                _instance = value;
            }
        }

        public Game.Map Map;

        void OnEnable()
        {
            InitialSetup();
        }

        void Awake()
        {
            InitialSetup();
        }

        void InitialSetup()
        {
            Instance = this;
            Map = new Game.Map();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(1.0f, 0.0f, 0.0f, 0.25f);
            Gizmos.DrawWireCube(Game.World.Instance.Map.Bounds.center, Game.World.Instance.Map.Bounds.size);
        }
    }
}