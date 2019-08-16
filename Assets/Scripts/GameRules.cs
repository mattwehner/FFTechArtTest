using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameRules : MonoBehaviour
    {
        //Accessible Variables
        public static GameRules Instance;
    
        private bool _gameStart = false;
        public bool HasGameStarted { get { return _gameStart; } }

        private bool _gameEnd = false;
        public bool HasGameEnded { get { return _gameEnd; } }

        //Inspector Variables
        [SerializeField]
        private Camera _mainCamera;
        [SerializeField]
        private CinemachineVirtualCamera _victoryView;
        [SerializeField]
        private CinemachineVirtualCamera _loseView;

        [Header("Squishy Parameters")]
        [SerializeField]
        private float _squishyJumpForce = 10000f;
        public float SquishyJumpForce { get { return _squishyJumpForce; } }

        [Header("Missile Parameters")]
        [SerializeField]
        private Transform _missileParent;
        public Transform MissileParent { get { return _missileParent; } }

        [SerializeField]
        private float _missileThrust = 100f;
        public float MissileThrust { get { return _missileThrust; } }
	
        [SerializeField]
        private float _missileSpawnTimer = 2f;
        public float MissileSpawnTimer { get { return _missileSpawnTimer; } }

        [SerializeField]
        private int _missileSpawnCount = 10;
        public int MissileSpawnCount { get { return _missileSpawnCount; } }

        [SerializeField]
        private float _missileAccuracy = 1f;
        public float MissileAccuracy { get { return _missileAccuracy; } }

        [SerializeField]
        private float _missileExplosionSize = 1f;
        public float MissileExplosionSize { get { return _missileExplosionSize; } }

        [SerializeField]
        private float _missileExplosionTime = 1f;
        public float MissileExplosionTime { get { return _missileExplosionTime; } }


        [Header("Mine Parameters")]
        [SerializeField]
        private Transform _mineParent;
        public Transform MineParent { get { return _mineParent; } }

        [SerializeField]
        private float _mineDetonationTimer = 2f;
        public float MineDetonationTimer { get { return _mineDetonationTimer; } }

        [SerializeField]
        private float _mineExplosionSize = 2f;
        public float MineExplosionSize { get { return _mineExplosionSize; } }

        [SerializeField]
        private float _mineExplosionTime = 2f;
        public float MineExplosionTime { get { return _mineExplosionTime; } }

        [Header("Explosion Parameters")]
        [SerializeField]
        private Transform _explosionParent;
        public Transform ExplosionParent { get { return _explosionParent; } }


        [Header("Endgame UI")]
        [SerializeField]
        private GameObject _endgameCanvas;
        [SerializeField]
        private GameObject _endgameVictory;
        [SerializeField]
        private GameObject _endgameGameOver;
        [SerializeField]
        private GameObject _endgameScreenshot;


        //Internal Variables
        private int _missilesDestroyed = 0;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            //Check for end game conditions
            if (!_gameEnd && _missilesDestroyed >= _missileSpawnCount)
            {
                GameVictory();
            }
        }

        public void MissileDestroyed()
        {
            _missilesDestroyed++;
        }

        //Game is lost
        public void GameOver(Collision collidedWith)
        {
            //Activate Lose Camera
            _loseView.gameObject.SetActive(true);
        
            //Add Object to LoseView Group
            CinemachineTargetGroup _loseTargerGroup = _loseView.GetComponentInChildren<CinemachineTargetGroup>();
            _loseTargerGroup.AddMember(collidedWith.transform, 1, 0);

            StartCoroutine(CaptureScreenshotThenCleanupScene());

            _endgameCanvas.SetActive(true);
            _endgameGameOver.SetActive(true);
            _endgameVictory.SetActive(false);
            _gameEnd = true;
            Debug.Log("Game Over!");
        }
    
        //Game is won
        public void GameVictory()
        {
            //Activate Victory Camera
            _victoryView.gameObject.SetActive(true);
            StartCoroutine(CaptureScreenshotThenCleanupScene());

            _endgameCanvas.SetActive(true);
            _endgameGameOver.SetActive(false);
            _endgameVictory.SetActive(true);
            _gameEnd = true;
            Debug.Log("Victory!");
        }

        //Reset the game
        public void ResetGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        public void BeginLevel()
        {
            _gameStart = true;
            MissileSpawner.Instance.StartSpawning();
        }

        private IEnumerator CaptureScreenshotThenCleanupScene()
        {
            var dimensions = _endgameScreenshot.GetComponent<RectTransform>().rect;
            ScreenshotUtilities.Capture(
                (int)dimensions.width,
                (int)dimensions.height, 
                _endgameScreenshot.GetComponent<Image>());

            yield return new WaitUntil(() => ScreenshotUtilities.ScreenshotCaptured);

            MissileSpawner.Instance.Cleanup();
        
            //Switch to Main Camera
            _loseView.gameObject.SetActive(false);
            _victoryView.gameObject.SetActive(false);
        }
    }
}
