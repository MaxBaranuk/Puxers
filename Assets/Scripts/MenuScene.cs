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
