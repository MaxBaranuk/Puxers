using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

    TextMesh valText;
//    bool isChecked;
    int value = 1;
    int moveKey = 0;
    GameScene manager;
    public Sprite crown;
//    TextMesh velInf;
    public Vector2 velosity;
   

    void Awake()
    {
        manager = GameObject.Find("SceneManager").GetComponent<GameScene>();
        valText = GetComponentInChildren<TextMesh>();
        GameScene.Instance.OnThrow += OnThrow;
        GameScene.Instance.OnSelect += OnSelect;
        GameScene.Instance.OnDeselect += OnDeselect;
        GameScene.Instance.DisableRotate += DisableRotate;
        GameScene.Instance.OnRotate += OnRotate;
        StartCoroutine(spawn());

        
    }
    void OnDisable()
    {
        
    }

    void Start () {
 //       velInf = transform.Find("vel").GetComponent<TextMesh>();
        value =(int) Math.Pow(2, UnityEngine.Random.Range(0,6));
        changeValue(1);
    }
	
	void Update () {
//        velInf.text = "" + moveKey;
        velosity = GetComponent<Rigidbody2D>().velocity;
    }

    public void changeValue(int v) {

        for (int i = 0; i < v; i++) {
            value *= 2;
        }
     
        valText.text = "" + value;
        if (value >= 4096) {
            StartCoroutine(resizeBall());
            gameObject.GetComponent<SpriteRenderer>().sprite = crown;
        } 
        //       else gameObject.GetComponent<SpriteRenderer>().sprite = manager.balls[(int)(Math.Log(value, 2) - 1)];
        else gameObject.GetComponent<SpriteRenderer>().color = manager.colors[(int)(Math.Log(value, 2) - 1)];

    }

    public void setValue(int v)
    {
        value = v;
        valText.text = "" + value;
        if (value >= 4096) StartCoroutine(resizeBall());
//        else gameObject.GetComponent<SpriteRenderer>().sprite = manager.balls[(int)(Math.Log(value, 2) - 1)];
        else gameObject.GetComponent<SpriteRenderer>().color = manager.colors[(int)(Math.Log(value, 2) - 1)];
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Ball" && coll.gameObject.GetComponent<Ball>().value == value)
       {

            manager.currentCombo = (int)manager.moveKeys[moveKey];
            manager.currentCombo++;
            manager.moveKeys[moveKey] = manager.currentCombo;
            if (manager.currentCombo > 1) StartCoroutine(showCombo(manager.currentCombo));

            if (coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > GetComponent<Rigidbody2D>().velocity.magnitude)
            {
                coll.gameObject.GetComponent<Ball>().changeValue(1);
                gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                StartCoroutine(resizeBall());
//                gameObject.GetComponent<Ball>().delete();
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
            else {
                changeValue(1);
                coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                coll.gameObject.GetComponent<Ball>().StartCoroutine(coll.gameObject.GetComponent<Ball>().resizeBall());
                coll.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        else {
            if (coll.gameObject.tag != "Empty"&& coll.gameObject.tag != "Bonus") {
                contactBall(coll);
            }
           
        }
    }

    void OnTriggerStay2D(Collider2D coll) {
        if (coll.gameObject.tag == "Ball" && coll.gameObject.GetComponent<Ball>().value == value)
        {

 //           manager.currentCombo = (int) manager.moveKeys[moveKey];
 //           manager.currentCombo++;
//            manager.moveKeys[moveKey] = manager.currentCombo;
//            if (manager.currentCombo > 1) StartCoroutine(showCombo(manager.currentCombo));

            if (coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > GetComponent<Rigidbody2D>().velocity.magnitude)
            {
                coll.gameObject.GetComponent<Ball>().changeValue(1);
                gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                StartCoroutine(resizeBall());
                //                gameObject.GetComponent<Ball>().delete();
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
            else {
                changeValue(1);
                coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                coll.gameObject.GetComponent<Ball>().StartCoroutine(coll.gameObject.GetComponent<Ball>().resizeBall());
                coll.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        else {
            if (coll.gameObject.tag != "Empty" && coll.gameObject.tag != "Bonus")
            {
                contactBall(coll);
            }

        }
    }

    void contactBall(Collider2D c) {
        GetComponent<Collider2D>().isTrigger = false;
        c.isTrigger = false;     
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
        if (coll.gameObject.tag == "Ball") {
            coll.collider.isTrigger = true;
            if (moveKey > coll.gameObject.GetComponent<Ball>().moveKey) coll.gameObject.GetComponent<Ball>().moveKey = moveKey;
            else moveKey = coll.gameObject.GetComponent<Ball>().moveKey;
        } 
        else coll.collider.isTrigger = false;
    }

    void OnThrow(GameObject g, Vector3 dir) {
        if (g == gameObject)
        {
            g.GetComponent<Rigidbody2D>().AddForce(-dir, ForceMode2D.Impulse);
            manager.StartCoroutine(manager.addBalls());           
            manager.moveCounter++;
            manager.moveKeys.Add(manager.moveCounter, 0);
            moveKey = manager.moveCounter;
        }
    }
    void OnSelect(GameObject g)
    {
        if (g == gameObject) {
//            isChecked = true;
            g.GetComponentInChildren<BallAnimation>().setPressed();
        }
    }
    void OnDeselect(GameObject g)
    {
        if (g == gameObject)
        {
//            isChecked = false;
            g.GetComponentInChildren<BallAnimation>().setUnpressed();
        }
    }

    void OnRotate(GameObject g, Vector3 dir) {
        if (g == gameObject)
        {
            g.GetComponentInChildren<BallAnimation>().setPulling(dir);
        }
    }

    void DisableRotate(GameObject g, Vector3 dir)
    {
        if (g == gameObject)
        {
            g.GetComponentInChildren<BallAnimation>().setUnPulling();
        }
    }

    IEnumerator spawn() {
        while (transform.localScale.x < 0.15)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.01f, transform.localScale.y + 0.01f, 0.2f);
            yield return new WaitForSeconds(0.01f);
        }
        GetComponent<CircleCollider2D>().enabled = true;
    }

    //void delete() {
    //    Destroy(gameObject);
    //}

    public IEnumerator resizeBall() {
        if (value > 4096) yield return new WaitForSeconds(1);
        while (transform.localScale.x > 0) {
            transform.localScale = new Vector3(transform.localScale.x - 0.01f, transform.localScale.y - 0.01f, 0.2f);
            yield return new WaitForSeconds(0.01f);
        }
        manager.totalPoints += value* manager.currentCombo;
        manager.totalBalls--;
        GameScene.Instance.OnThrow -= OnThrow;
        GameScene.Instance.OnSelect -= OnSelect;
        GameScene.Instance.OnDeselect -= OnDeselect;
        GameScene.Instance.OnRotate -= OnRotate;
        GameScene.Instance.DisableRotate -= DisableRotate;
        Destroy(gameObject);
     
    }

    IEnumerator showCombo(int combo) {
        manager.info.GetComponent<Text>().text = "combo x" + combo;
        yield return new WaitForSeconds(3f);
        manager.info.GetComponent<Text>().text = "";
    }
    public int getValue() {

        return value;
    }
}
