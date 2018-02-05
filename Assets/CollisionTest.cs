using UnityEngine;

public class CollisionTest : MonoBehaviour
{
	private Collider2D _collider;
	// Use this for initialization
	void Start ()
	{
		_collider = GetComponent<Collider2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (_collider.IsTouchingLayers(8))
			Debug.Log("Collide");
		
//		if (_collider.GetContacts() > 0)
//			Debug.Log("Collide2");
	}
}
