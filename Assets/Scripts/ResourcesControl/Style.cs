using UnityEngine;

namespace ResourcesControl
{
    public abstract class Style
    {
        public enum Type { Origin, Synergy}

        public abstract Type StyleType { get; }
        public Sprite Background { get; protected set; }
        public Sprite Ball0 { get; private set; }
        public Sprite Ball1 { get; private set; }
        public Sprite Ball2 { get; private set; }
        public Sprite Ball3 { get; private set; }
        public Sprite Ball4 { get; private set; }
        public Sprite Ball5 { get; private set; }
        public Sprite Ball6 { get; private set; }
        public Sprite Ball7 { get; private set; }
        public Sprite Ball8 { get; private set; }
        public Sprite Ball9 { get; private set; }
        public Sprite Ball10 { get; private set; }
        public Sprite Ball11 { get; private set; }
        public Sprite Ball12 { get; private set; }
        public Sprite BallCrown { get; protected set; }
        protected abstract string RootFolderName { get; }

        protected void Init()
        {
            Background = Resources.Load<Sprite>(RootFolderName + "/background");
            Ball0 = Resources.Load<Sprite>(RootFolderName + "/0");
            Ball1 = Resources.Load<Sprite>(RootFolderName + "/1");
            Ball2 = Resources.Load<Sprite>(RootFolderName + "/2");
            Ball3 = Resources.Load<Sprite>(RootFolderName + "/3");
            Ball4 = Resources.Load<Sprite>(RootFolderName + "/4");
            Ball5 = Resources.Load<Sprite>(RootFolderName + "/5");
            Ball6 = Resources.Load<Sprite>(RootFolderName + "/6");
            Ball7 = Resources.Load<Sprite>(RootFolderName + "/7");
            Ball8 = Resources.Load<Sprite>(RootFolderName + "/8");
            Ball9 = Resources.Load<Sprite>(RootFolderName + "/9");
            Ball10 = Resources.Load<Sprite>(RootFolderName + "/10");
            Ball11 = Resources.Load<Sprite>(RootFolderName + "/11");
            Ball12 = Resources.Load<Sprite>(RootFolderName + "/12");
            BallCrown = Resources.Load<Sprite>(RootFolderName + "/crown");
        }
        
    }
}