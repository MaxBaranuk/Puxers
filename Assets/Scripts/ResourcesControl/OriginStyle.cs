using UnityEngine;

namespace ResourcesControl
{
    public class OriginStyle : Style
    {
        private Sprite _background;
        private Sprite _ball0;
        private Sprite _ball1;
        private Sprite _ball2;
        private Sprite _ball3;
        private Sprite _ball4;
        private Sprite _ball5;
        private Sprite _ball6;
        private Sprite _ball7;
        private Sprite _ball8;
        private Sprite _ball9;
        private Sprite _ball10;
        private Sprite _ball11;
        private Sprite _ball12;
        private Sprite _ballCrown;

        public override string RootFolderName => "Origin";

        public override Type StyleType => Type.Origin;

        public override Sprite Background =>        
            _background ? _background : _background = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/background");

        public override Sprite Ball0 =>
            _ball0 ? _ball0 : _ball0 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/0");

        public override Sprite Ball1 =>
            _ball1 ? _ball1 : _ball1 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/1");

        public override Sprite Ball2 =>
            _ball2 ? _ball2 : _ball2 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/2");

        public override Sprite Ball3 =>
            _ball3 ? _ball3 : _ball3 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/3");

        public override Sprite Ball4 =>
            _ball4 ? _ball4 : _ball4 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/4");

        public override Sprite Ball5 =>
            _ball5 ? _ball5 : _ball5 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/5");

        public override Sprite Ball6 =>
            _ball6 ? _ball6 : _ball6 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/6");

        public override Sprite Ball7 =>
            _ball7 ? _ball7 : _ball7 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/7");

        public override Sprite Ball8 =>
            _ball8 ? _ball8 : _ball8 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/8");

        public override Sprite Ball9 =>
            _ball9 ? _ball9 : _ball9 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/9");

        public override Sprite Ball10 =>
            _ball10 ? _ball10 : _ball10 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/10");

        public override Sprite Ball11 =>
            _ball11 ? _ball11 : _ball11 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/11");

        public override Sprite Ball12 =>
            _ball12 ? _ball12 : _ball12 = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/12");

        public override Sprite BallCrown =>
            _ballCrown ? _ballCrown : _ballCrown = UnityEngine.Resources.Load<Sprite>(RootFolderName + "/crown");
     
    }
}