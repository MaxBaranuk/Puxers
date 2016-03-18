using UnityEngine;
using System.Collections;

public class GameSceneGUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void switchMusic() {
        if (MainMusic.source.isPlaying)
            MainMusic.source.Pause();
        else
            MainMusic.source.Play();
    }
}


