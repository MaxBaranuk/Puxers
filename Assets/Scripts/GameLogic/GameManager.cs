using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.UI;
using GameLogic;
using ResourcesControl;
using ScriptsOld;
using Smooth.Slinq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts.GameLogic
{
    public class GameManager : MonoBehaviour
    {
        public Game CurrentGame;
        public static GameSettings Settings;
        public static int CurrentThrow = 1;
        public static Dictionary<int, int> ComboHolder = new Dictionary<int, int>();
        public static GameManager Instanse;
        public static readonly ReactiveCommand ActivateBalls = new ReactiveCommand();
        public Ball BallPrefab;
        [SerializeField] private Transform _ballsHolder;
        [SerializeField] private Text _comboText;
        [SerializeField] private GameObject _losePanel;
        [SerializeField] private UiManager _uiManager;
        public BackgroundController BContr;
       
        public static readonly Queue<Ball> BallsPool = new Queue<Ball>();
        private ResourceHolder _resourcesHolder;
        private AudioSource _mainMusic;
        private readonly List<Ball> _ballsOnScene = new List<Ball>();
        private Vector3 _spawnRange;

        private void Awake()
        {           
            if (Instanse == null) Instanse = GetComponent<GameManager>();
            Settings = SaveLoad.LoadGame();
            _mainMusic = GetComponent<AudioSource>();
            Settings.MusicOn.Subscribe(b => _mainMusic.mute = !b);
 //           Ball.Size = BallPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size;
            Ball.Size = Vector3.one * 1.2f;           
        }

        private void Start()
        {
            _spawnRange = new Vector2(BContr.BackgroundSize.x - Ball.Size.x * BallPrefab.transform.localScale.x / 2,
                BContr.BackgroundSize.y - Ball.Size.y * BallPrefab.transform.localScale.y / 2);
            InitGame();
        }

        public async void StartNewGame(Game game)
        {
            CurrentGame = game;
            CurrentThrow = 0;
            ComboHolder = new Dictionary<int, int> {{CurrentThrow, 0}};
            await AddBalls(6, 500);
            ActivateBalls.Execute();
        }

        public static async void ShowCombo(int value)
        {
            Instanse._comboText.text = $"x{value}";
            Instanse._comboText.gameObject.SetActive(true);
            await Task.Delay(TimeSpan.FromSeconds(2));
            Instanse._comboText.gameObject.SetActive(false);
        }

        private void InitGame()
        {
            for (var i = 0; i < 12; i++)
            {
                var ball = Instantiate(BallPrefab);
                ball.transform.parent = _ballsHolder;
                BallsPool.Enqueue(ball);
            }
        }

        public async Task AddBalls(int count, int delay)
        {
            await Task.Delay(delay);
            ActivateBalls.Execute();
            Slinqable.Repeat(0, count).ForEach(_ =>
            {
                if (BallsPool.Count == 0)
                {
                    Lose();
                    return;
                }

                var ball = BallsPool.Dequeue();
                _ballsOnScene.Add(ball);
                ball.transform.localPosition = RandomizePosition();
                ball.gameObject.SetActive(true);
            });
            
        }

        private async void Lose()
        {
            _losePanel.SetActive(true);
            await Task.Delay(2);
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
            if(count > 10) Debug.Log("Randomize error");
            return pos;
        }

        private bool IsOnEmptyPlace(Vector2 pos)
        {
            return _ballsOnScene.Slinq()
                .All(ball => Vector2.Distance(ball.transform.localPosition, pos) > Ball.Size.x);
        }
    }
}