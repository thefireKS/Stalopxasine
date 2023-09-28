using Interfaces;
using UnityEngine;

namespace Memory_Slots
{
    public abstract class Upgrade: ScriptableObject, IUpgrade
    {
        public string UpgradeName;
        public Sprite Icon;
        public string Description;
        public int RequiredInventorySpace;
        
        public virtual void ApplyUpgrade()
        {
            Debug.LogWarning("Set upgrade logic!");
        }
    }
}
