using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        
        public Ball BallPrefab;
        [SerializeField] private Transform _ballsHolder;
        [SerializeField] private Text _comboText;
        
        public BackgroundController BContr;
       
        private readonly Queue<Ball> _ballsPool = new Queue<Ball>();
        private ResourceHolder _resourcesHolder;
        private AudioSource _mainMusic;
        private readonly List<Ball> _ballsOnScene = new List<Ball>(); 

        private void Awake()
        {           
            if (Instanse == null) Instanse = GetComponent<GameManager>();
            Settings = SaveLoad.LoadGame();
            _mainMusic = GetComponent<AudioSource>();
            Settings.MusicOn.Subscribe(b => _mainMusic.mute = !b);
            InitGame();
        }

        public void StartNewGame(Game game)
        {
            CurrentGame = game;
            CurrentThrow = 0;
            ComboHolder = new Dictionary<int, int>();
            ComboHolder.Add(CurrentThrow, 0);
            AddBalls(6, 500);
        }

        public static async void ShowCombo(int value)
        {
            Instanse._comboText.text = value.ToString();
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
                _ballsPool.Enqueue(ball);
            }
        }

        public async void AddBalls(int count, int delay)
        {
            await Task.Delay(delay);
            Slinqable.Repeat(0, count).ForEach(_ =>
            {
                var ball = _ballsPool.Dequeue();
                _ballsOnScene.Add(ball);
                ball.transform.localPosition = RandomizePosition(ball);
                ball.gameObject.SetActive(true);
            });
            
        }

        private Vector2 RandomizePosition(Ball ball)
        {
            var range = new Vector2(BContr.BackgroundSize.x - ball.BallSize.x * ball.transform.localScale.x / 2,
                BContr.BackgroundSize.y - ball.BallSize.x * ball.transform.localScale.y / 2);
            Vector2 pos;
            do
            {
                pos = new Vector2(Random.Range( -range.x, range.x), Random.Range(-range.y, range.y));
            } while (!IsOnEmptyPlace(pos));
            return pos;
        }

        private bool IsOnEmptyPlace(Vector2 pos)
        {
            return _ballsOnScene.Slinq()
                .All(ball => Vector2.Distance(ball.transform.localPosition, pos) > ball.BallSize.x);
        }
    }
}