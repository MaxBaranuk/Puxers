using UnityEngine;

namespace ResourcesControl
{
    public class OriginStyle : Style
    {
        protected override string RootFolderName => "Origin";

        public override Type StyleType => Type.Origin;
      
        public OriginStyle()
        {
            Init();
        }      
     
    }
}