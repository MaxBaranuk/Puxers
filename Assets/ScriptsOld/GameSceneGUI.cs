//using Game;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//
//namespace ScriptsOld
//{
//    public class GameSceneGUI : MonoBehaviour {
//
//        GameObject soundButton;
//        GameObject musicButton;
//        Sprite musicOn;
//        Sprite musicOff;
//        Sprite soundOn;
//        Sprite soundOff;
//        GameObject shopPanel;
//
//        GameObject losePanel;
//        GameObject pausePanel;
//        GameObject pauseButton;
//        // Use this for initialization
//        void Start () {
//            musicOn = Resources.Load<Sprite>("Sprites/musicOn");
//            musicOff = Resources.Load<Sprite>("Sprites/musicOff");
//            soundOn = Resources.Load<Sprite>("Sprites/soundOn");
//            soundOff = Resources.Load<Sprite>("Sprites/soundOff");
//
//            losePanel = GameObject.Find("Canvas").transform.Find("Panel").Find("LosePanel").gameObject;
//            pausePanel = GameObject.Find("Canvas").transform.Find("Panel").Find("PausePanel").gameObject;
//            pauseButton = GameObject.Find("Canvas").transform.Find("Panel").Find("Panel").Find("BottomPanel").Find("Pause").gameObject;
//            shopPanel = GameObject.Find("Canvas").transform.Find("Panel").Find("Shop").gameObject;
//
//            soundButton = pausePanel.transform.Find("Panel").Find("Sound").gameObject;
//            musicButton = pausePanel.transform.Find("Panel").Find("Music").gameObject;
//
//
//            if (GameSettings.Record.MusicOn) musicButton.GetComponentInChildren<Image>().sprite = musicOn;
//            else musicButton.GetComponentInChildren<Image>().sprite = musicOff;
//            if (GameSettings.Record.SoundsOn) soundButton.GetComponentInChildren<Image>().sprite = soundOn;
//            else soundButton.GetComponentInChildren<Image>().sprite = soundOff;
//        }
//	
//        // Update is called once per frame
//        void Update () {
//	
//        }
//
//        public void switchMusic() {
//            if (MainMusic.Source().isPlaying)
//            {
//                MainMusic.Source().Pause();
//                musicButton.GetComponentInChildren<Image>().sprite = musicOff;
//                GameSettings.Record.MusicOn = false;
//            }
//
//            else {
//                MainMusic.Source().Play();
//                musicButton.GetComponentInChildren<Image>().sprite = musicOn;
//                GameSettings.Record.MusicOn = true;
//            }           
//        }
//
//        public void OpenShopPanel()
//        {
//            Time.timeScale = 0;
//            shopPanel.SetActive(true);
//        }
//
//        public void Restart() {
//            SceneManager.LoadScene(1);
//        }
//
//        public void Home() {
//            SceneManager.LoadScene(0);
//        }
//    
//        public void Pause()
//        {
//            Time.timeScale = 0;
//            pausePanel.SetActive(true);
//        }
//
//        //public void restart()
//        //{
//        //    SceneManager.LoadScene(1);
//        //}
//
//        //public void home()
//        //{
//        //    SceneManager.LoadScene(0);
//        //}
//
//        public void pausePanelClose()
//        {
//            Time.timeScale = 1;
//            pausePanel.SetActive(false);
//        }
//    }
//}
//
//
