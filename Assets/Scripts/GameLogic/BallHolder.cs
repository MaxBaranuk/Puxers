using System.Globalization;
using ResourcesControl;
using UnityEngine;

namespace GameLogic
{
	public class BallHolder : MonoBehaviour {

		private TextMesh _valueInfo;
		private Collider2D _collider;
		private SpriteRenderer _image;
		private int _value;

		private void Awake()
		{
			_collider = GetComponent<Collider2D>();
			_image = GetComponent<SpriteRenderer>();
			_valueInfo = GetComponentInChildren<TextMesh>();
		}

		private void OnEnable()
		{
			_collider.enabled = true;
			_value = Random.Range(1, 6);
			_valueInfo.text = Mathf.Pow(2, _value).ToString(CultureInfo.InvariantCulture);
			_image.sprite = ResourceHolder.Instanse.GetBallImage(_value);
		}

		public void Activate()
		{
			var contacts = _collider.GetContacts(GameManager.Instanse.SceneBallColliders);
			_collider.enabled = false;
			if (contacts == 0)
				GameManager.Instanse.AddBall(transform.localPosition, _value);
			GameManager.Instanse.RemoveBallHolder(this);
			gameObject.SetActive(false);
		}
	}
}
