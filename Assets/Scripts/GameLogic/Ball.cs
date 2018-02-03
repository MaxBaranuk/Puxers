using System.Threading.Tasks;
using ResourcesControl;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Scripts.GameLogic
{
    public class Ball : MonoBehaviour
    {
       
        private Vector3 _strartTouchPosition;
        public static Vector3 Size;
        public ReactiveProperty<int> Value = new ReactiveProperty<int>(1);
        private TextMesh _valueInfo;

        private Rigidbody2D _rigidbody;
        private Collider2D _collider;
        private SpriteRenderer _image;
        private int _moveKey;
        private bool _isActive;
        private bool _isOverlap;
              
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _image = GetComponent<SpriteRenderer>();
            _valueInfo = GetComponentInChildren<TextMesh>();

            GameManager.ActivateBalls.Subscribe(_ =>
            {
                if(!_isActive)
                    Active();
            });
            
            Value.Subscribe(i =>
            {
                _valueInfo.text = i.ToString();
                _image.sprite = ResourceHolder.Instanse.GetBallImage(i);
                var combo = ++GameManager.ComboHolder[_moveKey];
                if(combo > 1 && _moveKey != 0)
                    GameManager.ShowCombo(combo);
            });
            
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
                    if (force > Size.x * 2) dir = dir * Size.x * 2 / force;
                    GetComponent<Rigidbody2D>().AddForce(- dir * 15, ForceMode2D.Impulse);
                    _moveKey = ++GameManager.CurrentThrow;
                    GameManager.ComboHolder.Add(_moveKey, 0);
                    GameManager.Instanse.AddBalls(2, 3000);
                });

            this.OnTriggerEnter2DAsObservable()
                .Subscribe(c =>
                {
                    _collider.isTrigger = false;
                    Collide(c);

                });
            
            this.OnCollisionExit2DAsObservable()
                .Subscribe(d =>
                {
                    _collider.isTrigger = true;
                });
        }

        private void OnEnable()
        {
            Value.Value = Random.Range(1, 6);
            _isActive = false;
        }

        private void Active()
        {
            _isActive = true;
            _image.color = Color.white;
        }

        private void OnDisable()
        {
            _collider.isTrigger = true;
            _image.color = new Color(1, 1, 1, 0.5f);
            GameManager.BallsPool.Enqueue(this);
        }

        private void Collide(Collider2D other)
        {
            Debug.Log("Collide");
            var ball = other.gameObject.GetComponent<Ball>();
            if(ball == null)
                return;

            if (ball.Value.Value != Value.Value)
            {
                ball._moveKey = _moveKey = Mathf.Max(_moveKey, ball._moveKey);
                return;
            }       

            if (ball._rigidbody.velocity.magnitude > _rigidbody.velocity.magnitude)
            {
                ball.Value.Value++;
                gameObject.SetActive(false);

            }
            else
            {
                Value.Value++;
                ball.gameObject.SetActive(false);
            }
        }
    }
}