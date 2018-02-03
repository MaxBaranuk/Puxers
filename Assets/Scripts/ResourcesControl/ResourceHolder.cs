using System.Collections.Generic;
using Assets.Scripts.GameLogic;
using Assets.Scripts.ResourcesControl;
using GameLogic;
using Smooth.Foundations.PatternMatching.GeneralMatcher;
using UnityEngine;
using Smooth.Slinq;

namespace ResourcesControl
{
    public class ResourceHolder : MonoBehaviour
    {
        private OriginStyle _origin;
        private SynergyStyle _synergy;
        public static ResourceHolder Instanse;
        
        void Awake()
        {
            if (Instanse == null) Instanse = GetComponent<ResourceHolder>();
            _origin = new OriginStyle();
            _synergy = new SynergyStyle();
        }

        public Sprite GetBallImage(int value)
        {
            var res = GameManager.Settings.CurrentStyle.Value
                .MatchTo<Style.Type, Style>()
                .With(Style.Type.Origin).Return(_origin)
                .With(Style.Type.Synergy).Return(_synergy)
                .Result();
            return value.MatchTo<int, Sprite>()
                .With(1).Return(res.Ball0)
                .With(2).Return(res.Ball1)
                .With(3).Return(res.Ball2)
                .With(4).Return(res.Ball3)
                .With(5).Return(res.Ball4)
                .With(6).Return(res.Ball5)
                .With(7).Return(res.Ball6)
                .With(8).Return(res.Ball7)
                .With(9).Return(res.Ball8)
                .With(10).Return(res.Ball9)
                .With(11).Return(res.Ball10)
                .With(12).Return(res.Ball11)
                .With(13).Return(res.Ball12)
                .Result();
        }
    }
}