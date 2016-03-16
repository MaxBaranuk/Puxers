using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MenuScene : MonoBehaviour {

    public GameObject scoreInfo;
	// Use this for initialization
	void Start () {
        SaveLoad.bestScore = SaveLoad.load();
        scoreInfo.GetComponent<Text>().text = ""+SaveLoad.bestScore;

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}

    public void startGame() {
        SceneManager.LoadScene(1);
    }
}
