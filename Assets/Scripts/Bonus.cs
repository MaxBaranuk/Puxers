using UnityEngine;
using System.Collections;
using System;

public class Bonus : MonoBehaviour {

    public enum Type {Multiple, Same };
    public int value;
//    TextMesh valText;
    GameScene manager;
    public Type type;
    Transform animation;
//    Sprite[] multipleImages;
//    Sprite[] multipleAnimations;
//    Sprite[] sameImages;
//    Sprite[] sameAnimations;


//    public Sprite[] images;
    // Use this for initialization
    void Start () {
        animation = transform.FindChild("Animation");


        Destroy(gameObject, 6);

        //!!! ONLY 1
        type =(Type) UnityEngine.Random.Range(0,1);


        manager = GameObject.Find("SceneManager").GetComponent<GameScene>();
//        valText = GetComponentInChildren<TextMesh>();
//        gameObject.GetComponent<SpriteRenderer>().sprite = images[(int)type];
        switch ((int)type) {
            case 0:
                value = UnityEngine.Random.Range(1, 5)*2;
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("SynergyCombo/"+value/2);
                animation.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("SynergyCombo/" + value / 2+"a");
                //                valText.text = "x" + value;
                break;
            case 1:
                value = (int)Math.Pow(2, UnityEngine.Random.Range(1, 6));
//                valText.text = "" + value;
                break;
            //case 2:
            //    valText.text = "B";
            //    break;
        }
        StartCoroutine(spawn());
    }
	
	// Update is called once per frame
	void Update () {
        animation.Rotate(Vector3.forward);

    }

    IEnumerator spawn(){
        while (transform.localScale.x < 0.15)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.01f, transform.localScale.y + 0.01f, 0.2f);
            yield return new WaitForSeconds(0.01f);
            
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ball") {
            manager.currentCombo = 1;
            
            switch (type) {
                case Type.Multiple:
                    coll.gameObject.GetComponent<Ball>().changeValue(value);
                    break;
                case Type.Same:
                    GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
                    Debug.Log("size" + balls.Length);
                    for (int i = 0; i < balls.Length; i++){
                        Debug.Log(balls[i]);
                        balls[i].GetComponent<Ball>().setValue(value);
                    }
                    break;
                //case Type.Bomb:
                //    value = coll.gameObject.GetComponent<Ball>().getValue();
                //    GameObject[] ballss = GameObject.FindGameObjectsWithTag("Ball");
                    
                //    for (int i = 0; i<ballss.Length; i++)
                //    {                     
                //        if (ballss[i].GetComponent<Ball>().getValue() == value) ballss[i].GetComponent<Ball>().StartCoroutine(ballss[i].GetComponent<Ball>().resizeBall());
                //    }
                //    break;

            }
            Destroy(gameObject);
        }
    }
}
