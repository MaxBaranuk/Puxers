using ResourcesControl;
using UniRx;
using UnityEngine;

namespace GameLogic
{
    public class Ball : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.Settings.CurrentStyle.Subscribe(SetStyle);
        }

        private void SetStyle(Style.Type style)
        {
            
        }

        public void SetSelected()
        {
            
        }
    }
}