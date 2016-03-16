using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

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

//    public Sprite[] balls;
    public GameObject ball;
    public GameObject bonus;
    public GameObject empty;
    public GameObject info;
    public GameObject ballsInfo;
    public GameObject pointsInfo;
    public GameObject losePanel;

    GameObject selectedBall;
    Vector2 startSwipePos;
    Vector3 startMousePos;
    public int currentCombo;

    public int moveCounter;
    public int totalBalls = 0;
    public int totalPoints = 0;
    public Color[] colors;

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
        colors = new Color[12];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1);
        }

        moveKeys = new Hashtable();
        moveKeys[0] = 0;
        for (int i = 0; i < 6; i++) {
            createBall();
        }
        StartCoroutine(bonusSpawn());
	}
	
	void Update () {
        if (totalBalls > 12) loseGame();
        ballsInfo.GetComponent<Text>().text = "" + totalBalls + "/12";
        pointsInfo.GetComponent<Text>().text = "" + totalPoints;
        //             mobileInput();
                       editorInput();
        if (Input.GetKeyDown(KeyCode.Escape)) GameObject.Find("UIManager").GetComponent<UIManager>().pause();
        }

    public void createBall() {
        StartCoroutine(spawnItem(ball));
        totalBalls++;
    }

    public void createBonus() {
        StartCoroutine(spawnItem(bonus));
    }

    public IEnumerator addBalls() {

        yield return new WaitForSeconds(1.5f);
        if (Random.value > 0.5|totalBalls>9) createBall();
        else {
            createBall();
            createBall();
        }
    }

     void editorInput() {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.mousePosition)), Vector2.zero);
            if ((hit.collider) != null) {
                if (hit.collider.tag == "Ball") {
                    OnSelect(hit.transform.gameObject);
                    selectedBall = hit.collider.gameObject;
                    startMousePos = Input.mousePosition;
                } 
                
            }           
        }
        if (Input.GetMouseButton(0)&& selectedBall)
        {
             Vector3 dir = Input.mousePosition - startMousePos;
            info.GetComponent<Text>().text = "" + dir.magnitude;
            if (dir.magnitude > 25) OnRotate(selectedBall, dir);
            else DisableRotate(selectedBall, dir);
        }

        if (Input.GetMouseButtonUp(0)&& selectedBall)
        {            
            Vector3 dir = Input.mousePosition - startMousePos;

            if (dir.magnitude > 25)
            {
                if (dir.magnitude > 50f) dir = dir.normalized*50;
                OnThrow(selectedBall, dir/2);
            }
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
                    startSwipePos = Input.GetTouch(0).position;
                }
            }
        }
        if (Input.GetTouch(0).phase == TouchPhase.Moved && selectedBall)
        {
            Vector3 dir = Input.GetTouch(0).position - startSwipePos;
            info.GetComponent<Text>().text = "" + dir;
            OnRotate(selectedBall, dir);
        }
        if (Input.GetTouch(0).phase == TouchPhase.Ended && selectedBall)
        {
            Vector2 dir = Input.GetTouch(0).position - startSwipePos;
            if (dir.magnitude > 0.5f)
            {
                if (dir.magnitude > 25f) dir = dir.normalized * 25;
                OnThrow(selectedBall, dir);
            }
            OnDeselect(selectedBall);
            selectedBall = null;

        }
    }
    void loseGame() {
        SaveLoad.save(totalPoints);
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
