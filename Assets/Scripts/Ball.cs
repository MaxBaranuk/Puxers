﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

    Sprite [] ballImages;
    TextMesh valText;
    int value = 1;
    int moveKey = 0;
    GameScene manager;
    Sprite crown;
    public GameObject effect;
//    TextMesh velInf;
    public Vector2 velosity;
    public AudioClip contactSound;
    public AudioClip shotSound;

    void Awake()
    {
        string styleFolder = "";
        if (GameSettings.state.style == GameState.StyleType.Custom) styleFolder = "BallsCustom";
        if (GameSettings.state.style == GameState.StyleType.Synergy) styleFolder = "BallsSynergy";
        ballImages = new Sprite[12];
        for (int i = 0; i < 12; i++) {
            ballImages[i] = Resources.Load<Sprite>(styleFolder+"/"+i);
        }
        crown = Resources.Load<Sprite>(styleFolder + "/crown");

        manager = GameObject.Find("SceneManager").GetComponent<GameScene>();
        valText = GetComponentInChildren<TextMesh>();
        GameScene.Instance.OnThrow += OnThrow;
        GameScene.Instance.OnSelect += OnSelect;
        GameScene.Instance.OnDeselect += OnDeselect;
        GameScene.Instance.DisableRotate += DisableRotate;
        GameScene.Instance.OnRotate += OnRotate;
        StartCoroutine(spawn());        
    }
    
    void Start () {
//        velInf = transform.Find("vel").GetComponent<TextMesh>();
        value =(int) Math.Pow(2, UnityEngine.Random.Range(1,6));
        changeValue(1);
    }
	
	void Update () {
//        velInf.text = "" + moveKey;
        velosity = GetComponent<Rigidbody2D>().velocity;
    }

    //IEnumerator changeColor(Color c) {
    //    Color prevCol = gameObject.GetComponent<SpriteRenderer>().color;
    //    float stepR = (c.r - prevCol.r)/50;
    //    float stepG = (c.g - prevCol.g)/50;
    //    float stepB = (c.b - prevCol.b)/50;
    //    for (int i = 0; i < 50; i++) {
    //        prevCol = new Color(prevCol.r+stepR, prevCol.g + stepG, prevCol.b + stepB);
    //        gameObject.GetComponent<SpriteRenderer>().color = prevCol;
    //        gameObject.transform.Find("ring").GetComponent<SpriteRenderer>().color = prevCol;
    //        yield return new WaitForSeconds(0.01f);
    //    }
    //    gameObject.GetComponent<SpriteRenderer>().color = c;
    //    gameObject.transform.Find("ring").GetComponent<SpriteRenderer>().color = c;
    //}
    
    public void changeValue(int v) {

//        for (int i = 0; i < v; i++) {
            value *= v;
//        } 
        valText.text = "" + value;
        if (value >= 4096)
        {
            StartCoroutine(resizeBall());
            gameObject.GetComponent<SpriteRenderer>().sprite = crown;
        }
        else {
            try {
                gameObject.GetComponent<SpriteRenderer>().sprite = ballImages[(int)(Math.Log(value, 2) - 1)];
            }
            catch (Exception) {
                Debug.Log("index: "+ (int)(Math.Log(value, 2) - 1));
            }
        } 
        //            StartCoroutine(changeColor(manager.colors[(int)(Math.Log(value, 2) - 1)]));
        Instantiate(effect, transform.position, Quaternion.identity);
    }

    public void setValue(int v)
    {
        value = v;
        valText.text = "" + value;
        if (value >= 4096) StartCoroutine(resizeBall());
        else gameObject.GetComponent<SpriteRenderer>().sprite = ballImages[(int)(Math.Log(value, 2) - 1)];
//        StartCoroutine(changeColor(manager.colors[(int)(Math.Log(value, 2) - 1)]));
        Instantiate(effect, transform.position, Quaternion.identity);
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Ball")
        {
            if (moveKey > coll.gameObject.GetComponent<Ball>().moveKey) coll.gameObject.GetComponent<Ball>().moveKey = moveKey;
            else moveKey = coll.gameObject.GetComponent<Ball>().moveKey;
        }

        if (coll.gameObject.tag == "Ball" && coll.gameObject.GetComponent<Ball>().value == value)
        {
            if (coll.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > GetComponent<Rigidbody2D>().velocity.magnitude)
            {
                coll.gameObject.GetComponent<Ball>().changeValue(1);
                gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                StartCoroutine(resizeBall());
                manager.currentCombo = (int)manager.moveKeys[coll.gameObject.GetComponent<Ball>().moveKey];
                manager.currentCombo++;
                manager.moveKeys[coll.gameObject.GetComponent<Ball>().moveKey] = manager.currentCombo;
                if (manager.currentCombo > 1) StartCoroutine(showCombo(manager.currentCombo));
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
            else {
                manager.currentCombo = (int)manager.moveKeys[moveKey];
                manager.currentCombo++;
                manager.moveKeys[moveKey] = manager.currentCombo;
                if (manager.currentCombo > 1) StartCoroutine(showCombo(manager.currentCombo));
                changeValue(1);
                coll.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
                coll.gameObject.GetComponent<Ball>().StartCoroutine(coll.gameObject.GetComponent<Ball>().resizeBall());
                coll.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            }
        }
        else if (coll.gameObject.tag != "Empty" && coll.gameObject.tag != "Bonus") contactBall(coll);
//        else GetComponent<AudioSource>().PlayOneShot(contactSound);
        
    }

    void contactBall(Collider2D c) {
        GetComponent<Collider2D>().isTrigger = false;
        c.isTrigger = false;     
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        GetComponent<AudioSource>().PlayOneShot(contactSound);
        GetComponent<CircleCollider2D>().isTrigger = true;
        if (coll.gameObject.tag == "Ball") coll.collider.isTrigger = true;
        else coll.collider.isTrigger = false;
    }

    void OnThrow(GameObject g, Vector3 dir) {
        if (g == gameObject)
        {
            if (GameSettings.state.gameType == GameState.GameType.TwoPlayers) GameScene.Instance.changePlayer();
//            if (GameScene.Instance.isPlayerOne) 
            GetComponent<AudioSource>().PlayOneShot(shotSound);
            g.GetComponent<Rigidbody2D>().AddForce(-dir, ForceMode2D.Impulse);
            manager.StartCoroutine(manager.addBalls());           
            manager.moveCounter++;
            manager.moveKeys.Add(manager.moveCounter, 0);
            moveKey = manager.moveCounter;
        }
    }
    void OnSelect(GameObject g){
        if (g == gameObject) g.GetComponentInChildren<BallAnimation>().setPressed();       
    }

    void OnDeselect(GameObject g){
        if (g == gameObject) g.GetComponentInChildren<BallAnimation>().setUnpressed();
    }

    void OnRotate(GameObject g, Vector3 dir) {
        if (g == gameObject) g.GetComponentInChildren<BallAnimation>().setPulling(dir);
    }

    void DisableRotate(GameObject g, Vector3 dir){
        if (g == gameObject) g.GetComponentInChildren<BallAnimation>().setUnPulling();
    }

    IEnumerator spawn() {
        while (transform.localScale.x < 0.15)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.01f, transform.localScale.y + 0.01f, 0.2f);
            yield return new WaitForSeconds(0.01f);
        }
        GetComponent<CircleCollider2D>().enabled = true;
        gameObject.tag = "Ball";
    }

    public IEnumerator resizeBall() {
        if (value > 4096) yield return new WaitForSeconds(1);
        GetComponent<CircleCollider2D>().isTrigger = true;
        while (transform.localScale.x > 0) {
            transform.localScale = new Vector3(transform.localScale.x - 0.04f, transform.localScale.y - 0.04f, 0.2f);
            yield return new WaitForSeconds(0.01f);
        }

        if (GameSettings.state.gameType == GameState.GameType.Single) manager.pointsOne += value * manager.currentCombo;
        else {
            if(moveKey%2==1) manager.pointsOne += value * manager.currentCombo;
            else manager.pointsTwo += value * manager.currentCombo;
        } 
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
