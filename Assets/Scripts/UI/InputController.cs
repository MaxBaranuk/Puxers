//using System;
//using GameLogic;
//using UniRx;
//using UnityEngine;
//
//namespace UI
//{
//    public class InputController : MonoBehaviour
//    {
//        private IObservable<Vector2> throwBall = Observable.Empty<Vector2>();
//        private Vector3 _startMousePos;
//
//        private void Awake()
//        {
//            var pointDownStream = Observable.EveryUpdate()
//                .Where(_ => Input.GetMouseButtonDown(0) | Input.GetTouch(0).phase==TouchPhase.Began)
//                .Subscribe(l => { });
//            
//            var pointUpStream = Observable.EveryUpdate()
//                .Where(_ => Input.GetMouseButtonUp(0) || Input.GetTouch(0).phase==TouchPhase.Ended);
//            
//            var pointStream = Observable.EveryUpdate()
//                .Where(_ => Input.GetMouseButton(0) || Input.GetTouch(0).phase==TouchPhase.Moved)
//                .Subscribe(l => { });
//        }
//
//        private void Update()
//        {
//         #if UNITY_EDITOR
//             EditorInput();
//         #else
//             mobileInput();
//         #endif
//        }
//
//        void EditorInput() {
//            if (Input.GetMouseButtonDown(0))
//            {
//                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.mousePosition)), Vector2.zero);
//                if ((hit.collider) != null) {
//                    if (hit.collider.CompareTag("Ball")) {
//                        hit.collider.gameObject.GetComponent<Ball>().SetSelected();
//                        _startMousePos = Camera.main.ScreenToWorldPoint((Input.mousePosition));
////                        OnSelect(hit.transform.gameObject);
////                        selectedBall = hit.collider.gameObject;
////                        _startMousePos = Camera.main.ScreenToWorldPoint((Input.mousePosition));
//                    } 
//                
//                }           
//            }
//            if (Input.GetMouseButton(0))
//            {
//                Vector3 dir = Camera.main.ScreenToWorldPoint((Input.mousePosition)) - _startMousePos;
////            info.GetComponent<Text>().text = "" + dir.magnitude;
//                float force = dir.magnitude;
//                if (force > 2.5f) dir = dir * 2.5f / force;
////                OnRotate(selectedBall, dir);
////            if (dir.magnitude > 25) OnRotate(selectedBall, dir);
////            else DisableRotate(selectedBall, dir);
//            }
//
//            if (Input.GetMouseButtonUp(0))
//            {         
////                if(Vector2.Distance(Input.mousePosition, _startMousePos)>0) ThrowBall
//                Vector3 dir = Camera.main.ScreenToWorldPoint((Input.mousePosition)) - _startMousePos;
//                float force = dir.magnitude;
//                if (force > 2.5f) dir = dir * 2.5f / force;
////           
////                OnThrow(selectedBall, dir * 15);
////                OnDeselect(selectedBall);
////                selectedBall = null;
//            
//            }
//        }
////        void MobileInput() {
////            if (Input.GetTouch(0).phase==TouchPhase.Began)
////            {
////                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
////                if ((hit.collider) != null)
////                {
////                    if (hit.collider.tag == "Ball") {
////                        OnSelect(hit.transform.gameObject);
////                        selectedBall = hit.collider.gameObject;
////                        startSwipePos = Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position));
////                    }
////                }
////            }
////            if (Input.GetTouch(0).phase == TouchPhase.Moved && selectedBall)
////            {
////                Vector3 dir = Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)) - startSwipePos;
//////            info.GetComponent<Text>().text = "" + dir;
////                float force = dir.magnitude;
////                if (force > 2.5f) dir = dir * 2.5f / force;
////                OnRotate(selectedBall, dir);
////            }
////
////
////            if (Input.GetTouch(0).phase == TouchPhase.Ended && selectedBall)
////            {
////                Vector3 dir = Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)) - startSwipePos;
////                float force = dir.magnitude;
////                if (force > 2.5f) dir = dir * 2.5f / force;
////
////                OnThrow(selectedBall, dir * 15);
////                //            if (dir.magnitude > 0.5f)
////                //            {
////                //                if (dir.magnitude > 25f) dir = dir.normalized * 25;
////
////                ////                GetComponent<AudioSource>().PlayOneShot(contactSound);
////                //            }
////                OnDeselect(selectedBall);
////                selectedBall = null;
////
////            }
////        }
//    }
//}