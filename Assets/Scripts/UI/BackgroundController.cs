using UnityEngine;

namespace UI
{
	public class BackgroundController : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _playArea;
		public Vector3 BackgroundSize => Extends;
		public Vector3 Extends;

		private void Awake () {
			Resize();
		}

		private void Resize()
		{
			var sr = GetComponent<SpriteRenderer>();
			if (sr == null) return;

			transform.localScale = Vector3.one;

			
			var width = sr.sprite.bounds.size.x;
			var height = sr.sprite.bounds.size.y;

			var worldScreenHeight = Camera.main.orthographicSize * 2f;
			var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

			var xWidth = transform.localScale;
			xWidth.x = worldScreenWidth / width;
			transform.localScale = xWidth;
			var yHeight = transform.localScale;
			yHeight.y = worldScreenHeight / height;
			transform.localScale = yHeight;
			Extends = new Vector3(_playArea.bounds.extents.x,
				_playArea.bounds.extents.y,
				_playArea.bounds.extents.z);
		}
	}
}
