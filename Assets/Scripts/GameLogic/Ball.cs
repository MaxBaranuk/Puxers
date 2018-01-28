using ResourcesControl;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GameLogic
{
    public class Ball : MonoBehaviour
    {
        private Vector3 _strartTouchPosition;
        public Vector3 BallSize;
        private void Awake()
        {
            GameManager.Settings.CurrentStyle.Subscribe(SetStyle);
            BallSize = GetComponent<SpriteRenderer>().sprite.bounds.size;
            this.OnMouseDownAsObservable()
                .SelectMany(_ =>
                {
                    _strartTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    return gameObject.UpdateAsObservable();
                })
                .TakeUntil(gameObject.OnMouseUpAsObservable())
                .Select(_ => Camera.main.ScreenToWorldPoint(Input.mousePosition))
                .RepeatUntilDestroy(this)
                .Subscribe(x => { });
            
            this.OnMouseUpAsObservable()
                .Select(_ => Camera.main.ScreenToWorldPoint(Input.mousePosition))
                .Subscribe(x =>
                {
                    var dir = x - _strartTouchPosition;
                    float force = dir.magnitude;
                    Debug.Log(force);
                if (force > BallSize.x * 2) dir = dir * BallSize.x * 2 / force;
                    GetComponent<Rigidbody2D>().AddForce(- dir * 15, ForceMode2D.Impulse);
                    
                });
        }

        private void SetStyle(Style.Type style)
        {
            
        }
    }
}