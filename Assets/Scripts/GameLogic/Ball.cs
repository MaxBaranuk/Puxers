using System.Globalization;
using System.Threading.Tasks;
using ResourcesControl;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace GameLogic
{
    public class Ball : MonoBehaviour
    {
       
        private Vector3 _startTouchPosition;
        public static Vector3 Size;
        public ReactiveProperty<int> Value = new ReactiveProperty<int>(1);
        private TextMesh _valueInfo;

        private Rigidbody2D _rigidbody;
        public Collider2D Collider;
        private SpriteRenderer _image;
        private int _moveKey;

        private Vector3 _previousPosition;
        private Vector3 _previousVelosity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
            _image = GetComponent<SpriteRenderer>();
            _valueInfo = GetComponentInChildren<TextMesh>();
            
            Value.Subscribe(i =>
            {              
                var combo = ++GameManager.ComboHolder[_moveKey];
                if(combo > 1 && _moveKey != 0)
                    GameManager.ShowCombo(combo);

                if (i > 14)
                {
                    gameObject.SetActive(false);
                    return;
                }

                
                _valueInfo.text = Mathf.Pow(2, i).ToString(CultureInfo.InvariantCulture);
                _image.sprite = ResourceHolder.Instanse.GetBallImage(i);
            });
            
            this.OnMouseDownAsObservable()
                .SelectMany(_ =>
                {
                    _startTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    return gameObject.UpdateAsObservable();
                })
                .TakeUntil(gameObject.OnMouseUpAsObservable())
                .Select(_ => Camera.main.ScreenToWorldPoint(Input.mousePosition))
                .RepeatUntilDestroy(this)
                .Subscribe(x => { });
            
            this.OnMouseUpAsObservable()
                .Select(_ => Camera.main.ScreenToWorldPoint(Input.mousePosition))
                .Subscribe(async x =>
                {
                    var dir = x - _startTouchPosition;
                    float force = dir.magnitude;
                    if (force > Size.x * 2) dir = dir * Size.x * 2 / force;
                    GetComponent<Rigidbody2D>().AddForce(- dir * 15, ForceMode2D.Impulse);
                    _moveKey = ++GameManager.CurrentThrow;
                    GameManager.ComboHolder.Add(_moveKey, 0);
                    await NextStep();
                });

            this.OnCollisionEnter2DAsObservable()
                .Subscribe(d =>
                {
                    Collide(d.collider);
                });
        }

        public void IncreaseValue(int value)
        {
            Value.Value += value;
        }

        public void SetValue(int value)
        {
            Value.Value = value;
        }
        
        private async Task NextStep()
        {
            await GameManager.Instanse.GenerateHoldersWithDelay(2, 3000); 
            GameManager.Instanse.CurrentGame.ChangePlayer();
        }

        private void FixedUpdate()
        {
            _previousPosition = transform.position;
            _previousVelosity = _rigidbody.velocity;
        }

        private void OnDisable()
        {
            GameManager.Instanse?.RemoveBall(this);
        }

        private void Collide(Collider2D other)
        {
            var ball = other.gameObject.GetComponent<Ball>();
            if(ball == null)
                return;

            if (ball.Value.Value != Value.Value)
            {
                ball._moveKey = _moveKey = Mathf.Max(_moveKey, ball._moveKey);
                return;
            }       

            if (ball._previousVelosity.magnitude > _previousVelosity.magnitude)
            {
                ball.Value.Value++;
                ball.transform.position = ball._previousPosition;
                ball._rigidbody.velocity = ball._previousVelosity;
                gameObject.SetActive(false);

            }
            else
            {
                Value.Value++;
                transform.position = _previousPosition;
                _rigidbody.velocity = _previousVelosity;
                ball.gameObject.SetActive(false);
            }

            GameManager.Instanse.CurrentGame.CurrentPlayer.Value.Score.Value += (int) Mathf.Pow(2, Value.Value);
        }
    }
}