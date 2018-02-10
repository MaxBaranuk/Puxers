using System;
using GameLogic;
using Smooth.Foundations.PatternMatching.GeneralMatcher;
using UniRx;
using UnityEngine;

namespace UI
{
    public class InputController : MonoBehaviour
    {
        private Vector3 _startTouchPosition;
        private Ball _currentBall;

#if UNITY_ANDROID
        private void Update()
        {
            if(!GameManager.Instanse.IsGameRunning)
                return;
            
            var input = Input.GetTouch(0);
            input.phase.Match()
                .With(TouchPhase.Began).Do(_ =>
                {
                    _currentBall = null;
                    var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
                    _startTouchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    if (hit.collider != null)
                        _currentBall = hit.collider.GetComponent<Ball>();
                })
                .With(TouchPhase.Ended).Do(_ =>
                {
                    if (_currentBall == null)
                        return;

                    var dir = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - _startTouchPosition;
                    _currentBall.Throw(dir);
                })
                .Exec();
        }
#endif        
    }
}