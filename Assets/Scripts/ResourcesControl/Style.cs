using UnityEngine;

namespace ResourcesControl
{
    public abstract class Style
    {
        public enum Type { Origin, Synergy}

        public abstract Type StyleType { get; }
        public abstract Sprite Background { get; }
        public abstract Sprite Ball0 { get; }
        public abstract Sprite Ball1 { get; }
        public abstract Sprite Ball2 { get; }
        public abstract Sprite Ball3 { get; }
        public abstract Sprite Ball4 { get; }
        public abstract Sprite Ball5 { get; }
        public abstract Sprite Ball6 { get; }
        public abstract Sprite Ball7 { get; }
        public abstract Sprite Ball8 { get; }
        public abstract Sprite Ball9 { get; }
        public abstract Sprite Ball10 { get; }
        public abstract Sprite Ball11 { get; }
        public abstract Sprite Ball12 { get; }
        public abstract Sprite BallCrown { get; }
        public abstract string RootFolderName { get; }
    }
}