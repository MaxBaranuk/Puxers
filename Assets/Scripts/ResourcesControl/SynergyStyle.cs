using UnityEngine;

namespace ResourcesControl
{
    public class SynergyStyle : Style
    {
        protected override string RootFolderName => "Synergy";
        public override Type StyleType => Type.Synergy;

        public SynergyStyle()
        {
            Init();
        }     
    }
}