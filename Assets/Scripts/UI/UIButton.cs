using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UiButton : Button
    {
        protected override void Awake()
        {
            var source = gameObject.AddComponent<AudioSource>();
            
        }
    }
}