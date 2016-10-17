using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuScene : MonoBehaviour {

    public GameObject scoreInfo;
	// Use this for initialization
	void Start () {
        SaveLoad.loadGame();
        scoreInfo.GetComponent<Text>().text = ""+ GameSettings.state.bestScore;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
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
        if (MainMusic.source.isPlaying)
            MainMusic.source.Stop();
        else
            MainMusic.source.Play();
    }
}
