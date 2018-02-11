﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Smooth.Foundations.PatternMatching.GeneralMatcher;
using Smooth.Slinq;
using UI;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;
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
        [SerializeField] private Transform _lifesHolder;
        [SerializeField] private Color _activeLife;
        [SerializeField] private Color _inactiveLife;
        [SerializeField] private Bonus _bonusPrefab;
        
        public static GameManager Instanse;
        public Game CurrentGame;
        public static GameSettings Settings;
        public UiManager UiManager => _uiManager;
        
        public static int CurrentThrow = 1;
        public static Dictionary<int, int> ComboHolder = new Dictionary<int, int>();
        public bool IsGameRunning;
        
        private readonly Queue<Ball> _ballsPool = new Queue<Ball>();
        private readonly Queue<BallHolder> _ballHoldersPool = new Queue<BallHolder>();
        private readonly List<Ball> _ballsOnScene = new List<Ball>();
        private readonly List<BallHolder> _holdersOnScene = new List<BallHolder>();
        private Bonus _bonus;
        private Image [] _lifeImages;
        
       
        private readonly IntReactiveProperty _lifes = new IntReactiveProperty();
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
            _lifeImages = _lifesHolder.GetComponentsInChildren<Image>();
            _lifes.Subscribe(i =>
            {
                _lifeImages.Slinq()
                    .ForEach((image, lifes) => 
                        image.color = image.transform.parent.GetSiblingIndex() <= lifes
                        ? _activeLife
                        : _inactiveLife, i);
            });
        }

        public async void StartNewGame(Game game)
        {
            CurrentGame = game;
            CurrentThrow = 0;
            _lifes.Value = _ballsOnScene.Count;
            ComboHolder = new Dictionary<int, int> {{CurrentThrow, 0}};
            
            await AddBalls(6);
            IsGameRunning = true;
            StartCoroutine(SpawnBonus());
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
                _lifes.Value++;
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
            _lifes.Value++;
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
            Slinqable.Repeat(0, count).ForEach((_, holders) =>
            {              
                var ball = _ballHoldersPool.Dequeue();
                holders.Add(ball);              
                ball.transform.localPosition = RandomizePosition();
                ball.gameObject.SetActive(true);
            }, _holdersOnScene);       
        }
        
        public void RemoveBall(Ball ball)
        {
            _ballsOnScene.Remove(ball);
            _ballsPool.Enqueue(ball);
            _lifes.Value--;
        }

        private IEnumerator SpawnBonus()
        {
            var delay = Random.Range(10, 50);
            yield return new WaitForSeconds(delay);
            
            while (IsGameRunning)
            {     
                _bonus.transform.position = RandomizePosition();
                _bonus.gameObject.SetActive(true);
                delay = Random.Range(10, 50);
                yield return new WaitForSeconds(delay);
            }
        }

        public void RemoveBallHolder(BallHolder ball)
        {
            _ballHoldersPool.Enqueue(ball);
        }

        public void SetSameValueToAll(int value)
        {
            _ballsOnScene.Slinq()
                .ForEach((ball, val) => ball.SetValue(val), value);
        }

        private void InitGame()
        {
            Slinqable.Repeat(0,12).ForEach(_ =>
            {
                var ball = Instantiate(_ballPrefab);
                ball.transform.parent = _ballsHolder;
                _ballsPool.Enqueue(ball);
                _lifes.Value++;
            });
           
            Slinqable.Repeat(0,2).ForEach(_ =>
            {
                var ball = Instantiate(_ballHolderPrefab);
                ball.transform.parent = _ballsHolder;
                _ballHoldersPool.Enqueue(ball);
            });
            
            _bonus = Instantiate(_bonusPrefab);
        }
        
        private async void Lose()
        {
            IsGameRunning = false;
            _losePanel.SetActive(true);
            _losePanel.GetComponentInChildren<Text>().text =
                CurrentGame.CurrentGameType.MatchTo<Game.GameType, string>()
                    .With(Game.GameType.Single).Return("GAME\nOVER")
                    .With(Game.GameType.TwoPlayers).Return($"Player {CurrentGame.CurrentPlayer.Value.Order}" +
                                                           "LOSE")
                    .Result();
            
            var balls = _ballsOnScene.ToArray();
            foreach (var t in balls)
                t.gameObject.SetActive(false);
            ComboHolder.Clear();
            
            StopAllCoroutines();
            Settings.BestScore.Value = CurrentGame.CurrentPlayer.Value.Score.Value;
            SaveLoad.SaveGame(Settings.BestScore.Value);
            await Task.Delay(2000);
            _losePanel.SetActive(false);
            _uiManager.OpenStartPanel();
        }

        private Vector3 RandomizePosition()
        {
            var count = 0;
            Vector3 pos;
            do
            {
                pos = new Vector3(Random.Range( -_spawnRange.x, _spawnRange.x), 
                    Random.Range(-_spawnRange.y, _spawnRange.y), -0.04f);
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