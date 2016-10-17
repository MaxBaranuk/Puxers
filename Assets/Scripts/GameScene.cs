using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScene : MonoBehaviour {

    public delegate void OnClickEvent(GameObject g);
    public delegate void OnThrowEvent(GameObject g, Vector3 dir);
    public event OnThrowEvent OnThrow;
    public event OnClickEvent OnSelect;
    public event OnClickEvent OnDeselect;
    public event OnThrowEvent OnRotate;
    public event OnThrowEvent DisableRotate;

    private static GameScene instance;
    public Hashtable moveKeys;
    public GameObject ball;
    public GameObject [] bonus;
    public GameObject empty;
    public GameObject info;
    public bool isPlayerOne = true;
//    public GameObject ballsInfo;
    public GameObject pointsInfo;
    public GameObject p1;
    public GameObject p2;
    public GameObject pointsInfo2;
    public GameObject losePanel;
    

    GameObject selectedBall;
    Vector3 startSwipePos;
    Vector3 startMousePos;
    public int currentCombo;

    public int moveCounter;
    public int totalBalls = 0;
    public int pointsOne = 0;
    public int pointsTwo = 0;
    //    public Color[] colors;

   
    public GameObject lifesPanel;
    GameObject[] lifes;

    public static GameScene Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindObjectOfType(typeof(GameScene)) as GameScene;
            return instance;
        }

    }
    
    void Start () {

        //float height = Camera.main.orthographicSize * 2.0f;
        //float width = height * Screen.width / Screen.height;
//        GameObject walls = GameObject.Find("Table");
 //       walls.transform.localScale = new Vector3((Screen.width/Screen.height)/1.6f, walls.transform.localScale.y, 0.1f);

        if (GameSettings.state.gameType == GameState.GameType.TwoPlayers) {
            pointsInfo2.SetActive(true);
            p2.SetActive(true);
            pointsInfo2.GetComponent<Text>().color = new Color(1, 1, 1, 0.2f);
            p2.GetComponent<Text>().color = new Color(1, 1, 1, 0.2f);
        } 
        //colors = new Color[8];
        //for (int i = 0; i < colors.Length; i++)
        //{
        //    colors[i] = new Color(Random.value, Random.value, Random.value, 1);
        //}
        lifes = new GameObject[12];
        for (int i = 0; i < 12; i++) {
            lifes[i] = lifesPanel.GetComponent<RectTransform>().Find(""+(i+1)).gameObject;
        }
        moveKeys = new Hashtable();
        moveKeys[0] = 0;
        for (int i = 0; i < 6; i++) {
            createBall();
        }
        StartCoroutine(bonusSpawn());
	}
	
	void Update () {
        if (totalBalls > 8) loseGame();
        showLifes();
        //        ballsInfo.GetComponent<Text>().text = "" + totalBalls + "/12";
        if (GameSettings.state.gameType == GameState.GameType.Single) {
            pointsInfo.GetComponent<Text>().text = "" + pointsOne;
 //           pointsInfo2.GetComponent<Text>().text = "" + pointsTwo;
        }          
        else {
            pointsInfo.GetComponent<Text>().text = "" + pointsOne;
            pointsInfo2.GetComponent<Text>().text = "" + pointsTwo;
            
        }
       
  //                   mobileInput();

        editorInput();
        if (Input.GetKeyDown(KeyCode.Escape)) GameObject.Find("UIManager").GetComponent<UIManager>().pause();
        }

    void showLifes() {
        for (int i = 0; i < 8; i++) {
            Color c = lifes[i].GetComponent<Image>().color;
            c.a = 1;
            if (i < totalBalls) c.a = 1;
            else c.a = 0.2f;
            lifes[i].GetComponent<Image>().color = c;
        }
    }

    public void changePlayer() {
            
        if (isPlayerOne)
        {
            StartCoroutine(setPlayerTwoThrow());
            
        }
        else {
            StartCoroutine(setPlayerOneThrow());
        }
        isPlayerOne = !isPlayerOne;
    }

    IEnumerator setPlayerOneThrow() {
        float a = 0.2f;
        while (a < 1f) {
            pointsInfo.GetComponent<Text>().color = new Color(1, 1, 1, a);
            pointsInfo2.GetComponent<Text>().color = new Color(1, 1, 1, 1.2f-a);
            p1.GetComponent<Text>().color = new Color(1, 1, 1, a);
            p2.GetComponent<Text>().color = new Color(1, 1, 1, 1.2f - a);
            a += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    IEnumerator setPlayerTwoThrow(){
        float a = 0.2f;
        while (a < 1f)
        {
            pointsInfo2.GetComponent<Text>().color = new Color(1, 1, 1, a);
            pointsInfo.GetComponent<Text>().color = new Color(1, 1, 1, 1.2f - a);
            p2.GetComponent<Text>().color = new Color(1, 1, 1, a);
            p1.GetComponent<Text>().color = new Color(1, 1, 1, 1.2f - a);
            a += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void createBall() {
        StartCoroutine(spawnItem(ball));
        totalBalls++;
    }

    public void createBonus() {
        StartCoroutine(spawnItem(bonus[Random.Range(0,3)]));
    }

    public IEnumerator addBalls() {

        yield return new WaitForSeconds(2f);
        createBall();

        //if (Random.value > 0.5|totalBalls>6) createBall();
        //else {
        //    createBall();
        //    createBall();
        //}
    }

     void editorInput() {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.mousePosition)), Vector2.zero);
            if ((hit.collider) != null) {
                if (hit.collider.tag == "Ball") {
                    OnSelect(hit.transform.gameObject);
                    selectedBall = hit.collider.gameObject;
                    startMousePos = Camera.main.ScreenToWorldPoint((Input.mousePosition));
                } 
                
            }           
        }
        if (Input.GetMouseButton(0)&& selectedBall)
        {
             Vector3 dir = Camera.main.ScreenToWorldPoint((Input.mousePosition)) - startMousePos;
//            info.GetComponent<Text>().text = "" + dir.magnitude;
            float force = dir.magnitude;
            if (force > 2.5f) dir = dir * 2.5f / force;
            OnRotate(selectedBall, dir);
//            if (dir.magnitude > 25) OnRotate(selectedBall, dir);
//            else DisableRotate(selectedBall, dir);
        }

        if (Input.GetMouseButtonUp(0)&& selectedBall)
        {            
            Vector3 dir = Camera.main.ScreenToWorldPoint((Input.mousePosition)) - startMousePos;
            float force = dir.magnitude;
            if (force > 2.5f) dir = dir * 2.5f / force;
           
            OnThrow(selectedBall, dir * 15);
            OnDeselect(selectedBall);
            selectedBall = null;
            
        }
    }
    void mobileInput() {
        if (Input.GetTouch(0).phase==TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
            if ((hit.collider) != null)
            {
                if (hit.collider.tag == "Ball") {
                    OnSelect(hit.transform.gameObject);
                    selectedBall = hit.collider.gameObject;
                    startSwipePos = Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position));
                }
            }
        }
        if (Input.GetTouch(0).phase == TouchPhase.Moved && selectedBall)
        {
            Vector3 dir = Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)) - startSwipePos;
//            info.GetComponent<Text>().text = "" + dir;
            float force = dir.magnitude;
            if (force > 2.5f) dir = dir * 2.5f / force;
            OnRotate(selectedBall, dir);
        }


        if (Input.GetTouch(0).phase == TouchPhase.Ended && selectedBall)
        {
            Vector3 dir = Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)) - startSwipePos;
            float force = dir.magnitude;
            if (force > 2.5f) dir = dir * 2.5f / force;

            OnThrow(selectedBall, dir * 15);
            //            if (dir.magnitude > 0.5f)
            //            {
            //                if (dir.magnitude > 25f) dir = dir.normalized * 25;

            ////                GetComponent<AudioSource>().PlayOneShot(contactSound);
            //            }
            OnDeselect(selectedBall);
            selectedBall = null;

        }
    }
    void loseGame() {
        SaveLoad.saveGame(Mathf.Max(pointsOne, pointsTwo));
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Ball");
        for (var i = 0; i < gameObjects.Length; i++) {
            Destroy(gameObjects[i]);
        }
        losePanel.SetActive(true);
    }
    IEnumerator bonusSpawn() {
        while (true) {
            yield return new WaitForSeconds(Random.Range(20, 60));
            createBonus();
        }
    }

    IEnumerator spawnItem(GameObject item) {
        Vector2 targetPosition;
        bool isEmp = true;
        do
        {
            targetPosition = new Vector2(Random.Range(-4, 4), Random.Range(-2.5f, 2.5f));
            GameObject tr = (GameObject) Instantiate(empty, targetPosition, Quaternion.identity);
            yield return new WaitForSeconds(0.05f);

            isEmp = tr.GetComponent<Empty>().getEnter();
 
            Debug.Log("empty " + isEmp+""+ targetPosition);
            Destroy(tr);
            yield return new WaitForSeconds(0.05f);
        } while (!isEmp);

        Instantiate(item, targetPosition, Quaternion.identity);
       
    }
    IEnumerator balling() {
        while (true)
        {
            createBall();
            yield return new WaitForSeconds(5);
        }
    }
}
