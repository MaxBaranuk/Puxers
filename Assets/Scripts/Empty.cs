using UnityEngine;
using System.Collections;

public class Empty : MonoBehaviour {

    bool isEmpty = true;
//    GameScene manager;
	// Use this for initialization
	void Start () {
//        manager = GameObject.Find("SceneManager").GetComponent<GameScene>();

    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D coll)
    {
        isEmpty = false;
        Debug.Log("try fail");
    }

    public bool getEnter() {
        return isEmpty;
    }
}
