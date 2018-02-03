using UnityEngine;

namespace ScriptsOld
{
	public class BackgroundController : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _playArea;
		public Vector3 BackgroundSize => _extends;
		public Vector3 _extends;
		void Awake () {
			Resize();
		}

		private void Resize()
		{
			var sr = GetComponent<SpriteRenderer>();
			if (sr == null) return;

			transform.localScale = Vector3.one;

			
			var width = sr.sprite.bounds.size.x;
			var height = sr.sprite.bounds.size.y;

			float worldScreenHeight = Camera.main.orthographicSize * 2f;
			float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

			Vector3 xWidth = transform.localScale;
			xWidth.x = worldScreenWidth / width;
			transform.localScale = xWidth;
			Vector3 yHeight = transform.localScale;
			yHeight.y = worldScreenHeight / height;
			transform.localScale = yHeight;
			_extends = new Vector3(_playArea.bounds.extents.x,
				_playArea.bounds.extents.y,
				_playArea.bounds.extents.z);
		}

	}
}
