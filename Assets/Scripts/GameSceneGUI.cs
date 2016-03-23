using UnityEngine;
using UnityEngine.UI;
public class GameSceneGUI : MonoBehaviour {

    public GameObject soundButtonText;
    public GameObject musicButtonText;
	// Use this for initialization
	void Start () {
        if (GameSettings.state.MusicOn) musicButtonText.GetComponent<Text>().text = "MUSIC OFF";
        else musicButtonText.GetComponent<Text>().text = "MUSIC ON";
        if (GameSettings.state.soundsOn) soundButtonText.GetComponent<Text>().text = "SOUND OFF";
        else soundButtonText.GetComponent<Text>().text = "SOUND ON";
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void switchMusic() {
        if (MainMusic.source.isPlaying)
        {
            MainMusic.source.Pause();
            musicButtonText.GetComponent<Text>().text = "MUSIC ON";
        }

        else {
            MainMusic.source.Play();
            musicButtonText.GetComponent<Text>().text = "MUSIC OFF";
        }           
    }

    public void restart() {

    }
    public void toMenu() {

    }
    public void switchSound(){
    }
    }


