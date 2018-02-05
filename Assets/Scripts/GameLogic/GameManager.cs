using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.UI;
using ScriptsOld;
using Smooth.Slinq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameLogic
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Transform _ballsHolder;
        [SerializeField] private Text _comboText;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private UiManager _uiManager;
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private BallHolder _ballHolderPrefab;
        [SerializeField] private BackgroundController _backgroundController;
        
        public static GameManager Instanse;
        public Game CurrentGame;
        public static GameSettings Settings;
        public UiManager UiManager => _uiManager;
        
        public static int CurrentThrow = 1;
        public static Dictionary<int, int> ComboHolder = new Dictionary<int, int>();
 
        private readonly Queue<Ball> _ballsPool = new Queue<Ball>();
        private readonly Queue<BallHolder> _ballHoldersPool = new Queue<BallHolder>();
        private readonly List<Ball> _ballsOnScene = new List<Ball>();
        private readonly List<BallHolder> _holdersOnScene = new List<BallHolder>();
        
        private AudioSource _mainMusic;       
        private Vector3 _spawnRange;

        public Collider2D[] SceneBallColliders
            => _ballsOnScene.Slinq()
                .Select(o => o.GetComponent<Ball>())
                .Select(ball => ball.Collider).ToList().ToArray();

        private void Awake()
        {           
            if (Instanse == null) Instanse = GetComponent<GameManager>();
            Settings = SaveLoad.LoadGame();
            _mainMusic = GetComponent<AudioSource>();
            Settings.MusicOn.Subscribe(b => _mainMusic.mute = !b);
            Ball.Size = Vector3.one * 1.2f;           
        }

        private void Start()
        {
            _spawnRange = new Vector2(_backgroundController.BackgroundSize.x - Ball.Size.x * _ballPrefab.transform.localScale.x / 2,
                _backgroundController.BackgroundSize.y - Ball.Size.y * _ballPrefab.transform.localScale.y / 2);
            InitGame();
        }

        public async void StartNewGame(Game game)
        {
            CurrentGame = game;
            CurrentThrow = 0;
            ComboHolder = new Dictionary<int, int> {{CurrentThrow, 0}};
            _ballsOnScene.ForEach(RemoveBall);
            await AddBalls(6);
        }

        public static async void ShowCombo(int value)
        {
            Instanse._comboText.text = $"x{value}";
            Instanse._comboText.gameObject.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(2));
            Instanse._comboText.gameObject.SetActive(false);
        }

        private async Task AddBalls(int count)
        {

            for (var i = 0; i < count; i++)
            {
                await Task.Delay(50);
                var ball = _ballsPool.Dequeue();
                _ballsOnScene.Add(ball); 
                ball.Value.Value = Random.Range(1, 6);
                ball.transform.localPosition = RandomizePosition();
                ball.gameObject.SetActive(true);
            }
        }

        public void AddBall(Vector3 position, int value)
        {
            if (_ballsPool.Count == 0)
            {
                Lose();
                return;
            }
           
            var ball = _ballsPool.Dequeue();
            _ballsOnScene.Add(ball);              
            ball.Value.Value = value;
            ball.transform.localPosition = position;
            ball.gameObject.SetActive(true);
        }

        public async Task GenerateHoldersWithDelay(int count, int delay)
        {
            await Task.Delay(delay);
            _holdersOnScene.ForEach(ball => ball.Activate());
            _holdersOnScene.Clear();
            Slinqable.Repeat(0, count).ForEach(_ =>
            {              
                var ball = _ballHoldersPool.Dequeue();
                _holdersOnScene.Add(ball);              
                ball.transform.localPosition = RandomizePosition();
                ball.gameObject.SetActive(true);
            });       
        }
        
        public void RemoveBall(Ball ball)
        {
            _ballsOnScene.Remove(ball);
            _ballsPool.Enqueue(ball);
        }

        public void RemoveBallHolder(BallHolder ball)
        {
            _ballHoldersPool.Enqueue(ball);
        }

        

        private void InitGame()
        {
            Slinqable.Repeat(0,12).ForEach(_ =>
            {
                var ball = Instantiate(_ballPrefab);
                ball.transform.parent = _ballsHolder;
                _ballsPool.Enqueue(ball);
            });
           
            Slinqable.Repeat(0,2).ForEach(_ =>
            {
                var ball = Instantiate(_ballHolderPrefab);
                ball.transform.parent = _ballsHolder;
                _ballHoldersPool.Enqueue(ball);
            });
        }
        
        private async void Lose()
        {
            _losePanel.SetActive(true);
            await Task.Delay(2000);
            _losePanel.SetActive(false);
            _uiManager.OpenStartPanel();
        }

        private Vector2 RandomizePosition()
        {
            var count = 0;
            Vector2 pos;
            do
            {
                pos = new Vector2(Random.Range( -_spawnRange.x, _spawnRange.x), Random.Range(-_spawnRange.y, _spawnRange.y));
                count++;
            } while (!IsOnEmptyPlace(pos) || count > 10);
            if (count > 10) Debug.Log("Randomize error");
            return pos;
        }

        private bool IsOnEmptyPlace(Vector2 pos)
        {
            return _ballsOnScene.Slinq()
                .Select(ball => ball.transform)
                .All((ball, size) => Vector2.Distance(ball.localPosition, pos) > size, Ball.Size.x)
                && _holdersOnScene.Slinq()
                          .Select(ball => ball.transform)
                          .All((ball, size) => Vector2.Distance(ball.localPosition, pos) > size, Ball.Size.x);
        }
    }
}