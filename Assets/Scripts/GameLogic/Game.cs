using Smooth.Foundations.PatternMatching.GeneralMatcher;
using UniRx;

namespace GameLogic
{
    public class Game
    {
        public enum GameType { Single, TwoPlayers };

        private GameType _currentGameType;
        private readonly Player _player1 = new Player(Type.One);
        private readonly Player _player2 = new Player(Type.Two);
        public ReactiveProperty<Player> CurrentPlayer;
        
        public Game()
        {
            CurrentPlayer = new ReactiveProperty<Player>(_player1);
            _player1.Score.Subscribe(i => GameManager.Instanse.UiManager.Player1Score.text = i.ToString());
            _player2.Score.Subscribe(i => GameManager.Instanse.UiManager.Player2Score.text = i.ToString());
            CurrentPlayer.Subscribe(player =>
            {
 //               GameManager.Instanse.UiManager.Player1Score.
                player.Order.Match()
                    .With(Type.One).Do(_ => { })
                    .With(Type.Two).Do(_ => { })
                    .Exec();
                
            });
        }
        
        public Game(GameType type)
        {
            _currentGameType = type;      
        }

        public void SetGameType(GameType type)
        {
            _currentGameType = type;
        }
        
        public void ChangePlayer()
        {
            if (_currentGameType == GameType.Single)
                return;

            CurrentPlayer.Value = CurrentPlayer.Value == _player1 
                ? _player2 
                : _player1;
        }
    }
}