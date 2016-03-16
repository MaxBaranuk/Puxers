using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

    public GameObject losePanel;
    public GameObject pausePanel;
    public GameObject pauseButton;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void pause() {
        pausePanel.SetActive(true);
    }

    public void restart() {
        SceneManager.LoadScene(1);
    }

    public void home() {
        SceneManager.LoadScene(0);
    }

    public void musicShut() {

    }

    public void soundShut() {

    }

    public void pausePanelClose() {
        pausePanel.SetActive(false);
    }
}
