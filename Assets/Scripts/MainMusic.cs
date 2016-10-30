using UnityEngine;
using System.Collections;

public class MainMusic : MonoBehaviour {

    static AudioSource source;
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

    public static AudioSource Source() {

        if (source == null) {
          GameObject g = Instantiate(Resources.Load<GameObject>("Prefabs/MainMusic"));
          source = g.GetComponent<AudioSource>();
        }
        return source;
    }
}
