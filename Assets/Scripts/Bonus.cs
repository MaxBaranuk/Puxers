using UnityEngine;
using System.Collections;
using System;

public class Bonus : MonoBehaviour {

    public enum Type {Bonus, MegaBonus, Bomb};
    public int value;
    TextMesh valText;
    GameScene manager;
    public Type type;
    public Sprite[] images;
    // Use this for initialization
    void Start () {
        Destroy(gameObject, 6);
        type =(Type) UnityEngine.Random.Range(0,3);
        manager = GameObject.Find("SceneManager").GetComponent<GameScene>();
        valText = GetComponentInChildren<TextMesh>();
        gameObject.GetComponent<SpriteRenderer>().sprite = images[(int)type];
        switch ((int)type) {
            case 0:
                value = UnityEngine.Random.Range(2, 6);
                valText.text = "x" + value;
                break;
            case 1:
                value = (int)Math.Pow(2, UnityEngine.Random.Range(1, 6));
                valText.text = "" + value;
                break;
            case 2:
                valText.text = "B";
                break;
        }
        StartCoroutine(spawn());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator spawn()
    {
        while (transform.localScale.x < 0.15)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.01f, transform.localScale.y + 0.01f, 0.2f);
            yield return new WaitForSeconds(0.01f);
            
        }
 //       GetComponent<CircleCollider2D>().isTrigger = false;
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ball") {
            manager.currentCombo = 1;
            
            switch (type) {
                case Type.Bonus:
                    coll.gameObject.GetComponent<Ball>().changeValue(value);
                    break;
                case Type.MegaBonus:
                    GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
                    for (int i = 0; i < balls.Length; i++){
                        balls[i].GetComponent<Ball>().setValue(value);
                    }
                    break;
                case Type.Bomb:
                    value = coll.gameObject.GetComponent<Ball>().getValue();
                    GameObject[] ballss = GameObject.FindGameObjectsWithTag("Ball");
                    for (int i = 0; i<ballss.Length; i++)
                    {
                        if (ballss[i].GetComponent<Ball>().getValue() == value) ballss[i].GetComponent<Ball>().StartCoroutine(ballss[i].GetComponent<Ball>().resizeBall());
                    }
                    break;

            }

            //coll.gameObject.GetComponent<Ball>().changeValue(value/2);
            //manager.totalPoints += coll.gameObject.GetComponent<Ball>().getValue();
            Destroy(gameObject);
        }
    }
}
