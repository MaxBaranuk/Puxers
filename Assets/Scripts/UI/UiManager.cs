using Assets.Scripts.GameLogic;
using GameLogic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Game = GameLogic.Game;

namespace Assets.Scripts.UI
{
    public class UiManager : MonoBehaviour {

        [SerializeField] private Text _bestScoreInfo;
        [SerializeField] private Text _player1ScoreInfo;
        [SerializeField] private Text _player2ScoreInfo;
        [SerializeField] private GameObject _startPanel;
        [SerializeField] private GameObject _selectPlayModePanel;
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _gameUiPanel;
        
        private GameManager _gameManager;

        private void Awake()
        {       
            _gameManager = GetComponent<GameManager>();           
        }

        private void Start()
        {
            GameManager.Settings.BestScore.Subscribe(i => _bestScoreInfo.text = "" + GameManager.Settings.BestScore);
        }
//        private void Update () {
//            if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
//        }

        public void Play()
        {
            _startPanel.SetActive(false);
            _selectPlayModePanel.SetActive(true);
            _gameManager.CurrentGame = new Game();
            
        }

        public void GoToDeveloperPage()
        {
            Application.OpenURL(Constants.DeveloperPageLink);
        }
        
        public void OpenMainMenu()
        {
            _mainMenuPanel.SetActive(true);
        }

        public void StartGame(bool isSingle)
        {
            _gameManager.CurrentGame.SetGameType(isSingle ? Game.GameType.Single : Game.GameType.TwoPlayers);
            _selectPlayModePanel.SetActive(false);
            
            _gameManager.CurrentGame.Player1Score.Subscribe(score => _player1ScoreInfo.text = score.ToString());           
            _player2ScoreInfo.gameObject.SetActive(!isSingle);
            if (!isSingle)
            _gameManager.CurrentGame.Player2Score.Subscribe(score => _player2ScoreInfo.text = score.ToString());
            _gameUiPanel.SetActive(true);
            
            _gameManager.StartNewGame(_gameManager.CurrentGame);
        }
    }
}
