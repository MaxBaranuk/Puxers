using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using ResourcesControl;
using UniRx;

namespace GameLogic
{
    public class GameManager : MonoBehaviour
    {
        public Game CurrentGame;
        public static GameSettings Settings;
        public Ball BallPrefab;
        private readonly Queue<Ball> _ballsPool = new Queue<Ball>();
        private ResourceHolder _resourcesHolder;
        private AudioSource _mainMusic;  
        private List<Ball> _ballsOnScene = new List<Ball>(); 

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
        }

        private void InitGame()
        {
            for (var i = 0; i < 12; i++)
            {
                _ballsPool.Enqueue(Instantiate(BallPrefab));
            }
        }

        private void AddBall()
        {
            
        }

        private Vector2 RandomizePosition()
        {
            Vector2 pos;
            do
            {
                pos = new Vector2(Random.Range(-4.5f, 4.5f), Random.Range(-3.5f, 3.5f));
            } while (isOnEmptyPlace(pos));
            return pos;
        }

        private bool isOnEmptyPlace(Vector2 pos)
        {
            _ballsOnScene.ForEach(ball =>
            {
                if(Vector2.Distance(ball.transform.localPosition, pos) < 1)
                    return; 
            });
            return true;
        }
    }
}