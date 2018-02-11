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
        [SerializeField] private GameObject _effect;
        private Vector3 _startTouchPosition;
        public static Vector3 Size;
        public ReactiveProperty<int> Value = new ReactiveProperty<int>(1);
        private TextMesh _valueInfo;

        private Rigidbody2D _rigidbody;
        public Collider2D Collider;
        private SpriteRenderer _image;
        private AudioSource _audioSource;
        private int _moveKey;

        private Vector3 _previousPosition;
        private Vector3 _previousVelosity;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            Collider = GetComponent<Collider2D>();
            _image = GetComponent<SpriteRenderer>();
            _valueInfo = GetComponentInChildren<TextMesh>();
            _audioSource = GetComponent<AudioSource>();
            
            GameManager.Settings.SoundsOn.Subscribe(b => _audioSource.mute = !b);
            
            Value.Subscribe(i =>
            {                          
                if (i > 14)
                {
                    gameObject.SetActive(false);
                    return;
                }

                _valueInfo.text = Mathf.Pow(2, i).ToString(CultureInfo.InvariantCulture);
                _image.sprite = ResourceHolder.Instanse.GetBallImage(i);
            });
#if UNITY_EDITOR     
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
                .Subscribe(x =>
                {
                    var dir = x - _startTouchPosition;
                    Throw(dir);
                });
#endif
            this.OnCollisionEnter2DAsObservable()
                .Subscribe(d =>
                {
                    Collide(d.collider);
                });
        }

        public void IncreaseValue(int value)
        {
            var combo = ++GameManager.ComboHolder[_moveKey];
                
            if(combo > 1 && _moveKey != 0)
                GameManager.ShowCombo(combo);

            GameManager.Instanse.CurrentGame.CurrentPlayer.Value.Score.Value 
                += (int) Mathf.Pow(2, Value.Value) * combo;
            Value.Value += value;
        }

        public void SetValue(int value)
        {
            Value.Value = value;
        }

        private async void Throw(Vector3 dir)
        {
            var force = dir.magnitude;
            if (force > Size.x * 2) dir = dir * Size.x * 2 / force;
            _effect.SetActive(true);
            
            _rigidbody.AddForce(- dir * 20, ForceMode2D.Impulse);
            _rigidbody.drag = 0.2f;
            _moveKey = ++GameManager.CurrentThrow;
            GameManager.ComboHolder.Add(_moveKey, 0);

            await Task.Delay(250);
                    
            _rigidbody.drag = 3f;
             
            while (_rigidbody.drag > .9f)
            {
                _rigidbody.drag -= 0.2f;
                await Task.Delay(50);
            }
                
            _rigidbody.drag = 0.9f;
            await NextStep();
            _effect.SetActive(false);   
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
            _audioSource.Play();
            if(ball == null)
                return;

            if (ball.Value.Value != Value.Value)
            {
                ball._moveKey = _moveKey = Mathf.Max(_moveKey, ball._moveKey);
                return;
            }       

            if (ball._previousVelosity.magnitude > _previousVelosity.magnitude)
            {
                ball.IncreaseValue(1);
                ball.transform.position = ball._previousPosition;
                ball._rigidbody.velocity = ball._previousVelosity;
                gameObject.SetActive(false);

            }
            else
            {
                IncreaseValue(1);
                transform.position = _previousPosition;
                _rigidbody.velocity = _previousVelosity;
                ball.gameObject.SetActive(false);
            }

           
        }
    }
}