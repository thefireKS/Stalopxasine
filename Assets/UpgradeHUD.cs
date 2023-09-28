using Memory_Slots;
using UnityEngine;

public class UpgradeHUD : MonoBehaviour
{ 
    private UpgradeInventory _upgradeInventory;

    [SerializeField] private GameObject buttonPrefab;

    private void Awake()
    {
        _upgradeInventory = FindObjectOfType<UpgradeInventory>();

        foreach (var upgrade in _upgradeInventory.GetUpgrades())
        {
            var button = Instantiate(buttonPrefab, transform);
            var upgradeButton = button.GetComponent<UpgradeButton>();
            upgradeButton.Initialize(upgrade);
            upgradeButton.SetActive();
        }
    }
}
