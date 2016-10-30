using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuScene : MonoBehaviour {

    Text scoreInfo;
    GameObject startPanel;
    GameObject aboutPanel;
    GameObject howToPanel;
    GameObject settingsPanel;
    GameObject shopPanel;


	// Use this for initialization
	void Start () {
        SaveLoad.loadGame();
        startPanel = GameObject.Find("Canvas").transform.FindChild("Panel").FindChild("StartPanel").gameObject;
        scoreInfo = GameObject.Find("Canvas").transform.FindChild("Panel").FindChild("CentralPanel").FindChild("Points").GetComponent<Text>();
        aboutPanel = GameObject.Find("Canvas").transform.FindChild("Panel").FindChild("AboutPanel").gameObject;
        howToPanel = GameObject.Find("Canvas").transform.FindChild("Panel").FindChild("HowToPanel").gameObject;
        settingsPanel = GameObject.Find("Canvas").transform.FindChild("Panel").FindChild("Settings").gameObject;
        shopPanel = GameObject.Find("Canvas").transform.FindChild("Panel").FindChild("Shop").gameObject;
        scoreInfo.text = ""+ GameSettings.state.bestScore;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}

    public void OpenStartPanel() {
        startPanel.SetActive(true);
    }

    public void startGame() {
        GameSettings.state.gameType = GameState.GameType.Single;
        SceneManager.LoadScene(1);
    }

    public void startMultiplayerGame()
    {
        GameSettings.state.gameType = GameState.GameType.TwoPlayers;
        SceneManager.LoadScene(1);
    }

    public void switchMusic()
    {
        if (MainMusic.Source().isPlaying)
            MainMusic.Source().Stop();
        else
            MainMusic.Source().Play();
    }

    public void ToMenu() {
        aboutPanel.SetActive(false);
        howToPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    public void OpenAboutPanel() {
        aboutPanel.SetActive(true);
    }

    public void OpenHowToPanel() {
        howToPanel.SetActive(true);
    }

    public void OpenSettingsPanel() {
        settingsPanel.SetActive(true);
    }

    public void OpenShopPanel() {
        shopPanel.SetActive(true);
    }
}
