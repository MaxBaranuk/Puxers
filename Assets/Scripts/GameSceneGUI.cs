using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameSceneGUI : MonoBehaviour {

    GameObject soundButton;
    GameObject musicButton;
    Sprite musicOn;
    Sprite musicOff;
    Sprite soundOn;
    Sprite soundOff;
    GameObject shopPanel;

    GameObject losePanel;
    GameObject pausePanel;
    GameObject pauseButton;
    // Use this for initialization
    void Start () {
        musicOn = Resources.Load<Sprite>("Sprites/musicOn");
        musicOff = Resources.Load<Sprite>("Sprites/musicOff");
        soundOn = Resources.Load<Sprite>("Sprites/soundOn");
        soundOff = Resources.Load<Sprite>("Sprites/soundOff");

        losePanel = GameObject.Find("Canvas").transform.FindChild("Panel").FindChild("LosePanel").gameObject;
        pausePanel = GameObject.Find("Canvas").transform.FindChild("Panel").FindChild("PausePanel").gameObject;
        pauseButton = GameObject.Find("Canvas").transform.FindChild("Panel").FindChild("Panel").FindChild("BottomPanel").FindChild("Pause").gameObject;
        shopPanel = GameObject.Find("Canvas").transform.FindChild("Panel").FindChild("Shop").gameObject;

        soundButton = pausePanel.transform.FindChild("Panel").FindChild("Sound").gameObject;
        musicButton = pausePanel.transform.FindChild("Panel").FindChild("Music").gameObject;


        if (GameSettings.state.MusicOn) musicButton.GetComponentInChildren<Image>().sprite = musicOn;
        else musicButton.GetComponentInChildren<Image>().sprite = musicOff;
        if (GameSettings.state.soundsOn) soundButton.GetComponentInChildren<Image>().sprite = soundOn;
        else soundButton.GetComponentInChildren<Image>().sprite = soundOff;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void switchMusic() {
        if (MainMusic.Source().isPlaying)
        {
            MainMusic.Source().Pause();
            musicButton.GetComponentInChildren<Image>().sprite = musicOff;
            GameSettings.state.MusicOn = false;
        }

        else {
            MainMusic.Source().Play();
            musicButton.GetComponentInChildren<Image>().sprite = musicOn;
            GameSettings.state.MusicOn = true;
        }           
    }

    public void OpenShopPanel()
    {
        Time.timeScale = 0;
        shopPanel.SetActive(true);
    }

    public void Restart() {
        SceneManager.LoadScene(1);
    }

    public void Home() {
        SceneManager.LoadScene(0);
    }
    
    public void Pause()
    {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    //public void restart()
    //{
    //    SceneManager.LoadScene(1);
    //}

    //public void home()
    //{
    //    SceneManager.LoadScene(0);
    //}

    public void pausePanelClose()
    {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}


