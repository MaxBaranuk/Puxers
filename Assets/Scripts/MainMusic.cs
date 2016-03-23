using UnityEngine;
using System.Collections;

public class MainMusic : MonoBehaviour {

    public static AudioSource source;
    public static bool AudioBegin;
	// Use this for initialization
    void Awake()
    {
        if (!AudioBegin)
        {
            source = GetComponent<AudioSource>();
            if(GameSettings.state.MusicOn) source.Play();
            DontDestroyOnLoad(gameObject);
            AudioBegin = true;
        }
    }
}
