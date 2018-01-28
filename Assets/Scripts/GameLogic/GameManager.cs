using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using ResourcesControl;
using ScriptsOld;
using Smooth.Foundations.Slinq;
using UniRx;

namespace GameLogic
{
    public class GameManager : MonoBehaviour
    {
        public Game CurrentGame;
        public static GameSettings Settings;
        public Ball BallPrefab;
        [SerializeField] private Transform _ballsHolder;

        
        public BackgroundController bContr;
        private readonly Queue<Ball> _ballsPool = new Queue<Ball>();
        private ResourceHolder _resourcesHolder;
        private AudioSource _mainMusic;
        private readonly List<Ball> _ballsOnScene = new List<Ball>(); 

        private void Awake()
        {
            
            Settings = SaveLoad.LoadGame();
            _mainMusic = GetComponent<AudioSource>();
            Settings.MusicOn.Subscribe(b => _mainMusic.mute = !b);
            InitGame();
        }

        public void StartNewGame(Game game)
        {
            CurrentGame = game;
            Slinqable.Repeat(0, 11).ForEach(_ => AddBall());
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

        private void AddBall()
        {
            var ball = _ballsPool.Dequeue();
            _ballsOnScene.Add(ball);
            ball.transform.localPosition = RandomizePosition(ball);
            ball.gameObject.SetActive(true);
        }

        private Vector2 RandomizePosition(Ball ball)
        {
            var range = new Vector2(bContr.BackgroundSize.x - ball.BallSize.x * ball.transform.localScale.x / 2,
                bContr.BackgroundSize.y - ball.BallSize.x * ball.transform.localScale.y / 2);
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