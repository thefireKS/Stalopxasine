using System.Collections.Generic;
using UnityEngine;

namespace Memory_Slots
{
    public class UpgradeInventory : MonoBehaviour
    {
        #if UNITY_EDITOR

        [SerializeField] private List<Upgrade> customUpgrades;

        [SerializeField] private bool useCustomData;

        private void Start()
        {
            if(useCustomData) _upgrades = customUpgrades;
        }

        #endif
        
        private List<Upgrade> _upgrades = new List<Upgrade>();

        [SerializeField] private int maxInventorySlots = 5;
        private int _currentSlots;
        
        public static UpgradeInventory Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }

        public void ApplyAllUpgrades()
        {
            foreach (var upgrade in _upgrades)
            {
                upgrade.ApplyUpgrade();
            }
        }

        public bool AddUpgrade(Upgrade upgrade)
        {
            if (CanAddUpgrade(upgrade))
            {
                _upgrades.Add(upgrade);
                _currentSlots += upgrade.RequiredInventorySpace;
                return true;
            }

            return false;
        }

        public void RemoveUpgrade(Upgrade upgrade)
        {
            _upgrades.Remove(upgrade);
            _currentSlots -= upgrade.RequiredInventorySpace;
        }

        private bool CanAddUpgrade(Upgrade upgrade)
        {
            return _currentSlots + upgrade.RequiredInventorySpace <= maxInventorySlots;
        }

        public List<Upgrade> GetUpgrades()
        {
            return _upgrades;
        }
        
    }
}
