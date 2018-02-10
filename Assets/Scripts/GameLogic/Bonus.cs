﻿using System.Globalization;
using System.Threading.Tasks;
using Smooth.Foundations.PatternMatching.GeneralMatcher;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    public class Bonus : MonoBehaviour 
    {
        [SerializeField] private GameObject _sameEffect;
        [SerializeField] private GameObject _multipleEffect;
        
        private enum Type {Multiple, Same };
        private int _value;
        private Type _type;
        private SpriteRenderer _image;
        private TextMesh _text;

        private void Awake()
        {
            _image = GetComponent<SpriteRenderer>();
            _text = GetComponentInChildren<TextMesh>();
            this.OnTriggerEnter2DAsObservable()
                .Select(c => c.GetComponent<Ball>())
                .Where(ball => ball != null)
                .Subscribe(ball =>
                {
                    _type.Match()
                        .With(Type.Same).Do(_ =>GameManager.Instanse.SetSameValueToAll(_value))
                        .With(Type.Multiple).Do(_ => ball.IncreaseValue(_value))
                        .Exec();
                    gameObject.SetActive(false);
                });
        }

        private async void OnEnable()
        {
            _type = Random.value > 0.5f
                ? Type.Multiple
                : Type.Same;
            _value = Random.Range(2, 5);
            
            _type.Match()
                .With(Type.Multiple).Do(_ =>
                {
                    _multipleEffect.SetActive(true);
                    _sameEffect.SetActive(false);
                    _text.text = "x" + _value;
                })
                .With(Type.Same).Do(_ =>
                {
                    _multipleEffect.SetActive(false);
                   _sameEffect.SetActive(true);
                    _text.text = Mathf.Pow(2, _value).ToString(CultureInfo.InvariantCulture);
                })
                .Exec();
            await Task.Delay(4000);
            gameObject.SetActive(false);
        }
    }
}
