using UnityEngine;
using System.Collections;

public class BallAnimation : MonoBehaviour {

    bool isChecked;
    public Sprite ring;
    public Sprite ringArr;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void setPressed() {
        StartCoroutine(pressed());
    }

    public void setUnpressed() {
        StartCoroutine(unpressed());
    }

    public void setPulling(Vector3 dir) {
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle+90, Vector3.forward);
        gameObject.GetComponent<SpriteRenderer>().sprite = ringArr;
        //        transform.rotation = Quaternion.Euler(dir);
    }

    public void setUnPulling()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = ring;
        //var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
        //        transform.rotation = Quaternion.Euler(dir);
    }

    IEnumerator pressed() {
        isChecked = true;
        while (transform.localScale.x < 7f) {
            transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y + 0.05f, 0.2f);
            if (!isChecked) break;
            yield return new WaitForSeconds(0.01f);
        }
//        gameObject.GetComponent<SpriteRenderer>().sprite = ringArr;
    }

    IEnumerator unpressed()
    {
        isChecked = false;
        while (transform.localScale.x > 3f)
        {
            transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y - 0.1f, 0.2f);
            yield return new WaitForSeconds(0.005f);
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = ring;
    }


}
