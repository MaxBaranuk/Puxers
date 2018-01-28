using UniRx;

namespace GameLogic
{
    public class Game
    {
        public enum GameType { Single, TwoPlayers };

        private GameType _currentGameType;
        public readonly IntReactiveProperty Player1Score = new IntReactiveProperty();
        public readonly IntReactiveProperty Player2Score = new IntReactiveProperty();

        public Game()
        {
            Player1Score.Value = 0;
            Player2Score.Value = 0;
        }
        
        public Game(GameType type)
        {
            _currentGameType = type;      
        }

        public void SetGameType(GameType type)
        {
            _currentGameType = type;
        }
    }
}