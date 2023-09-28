using Memory_Slots;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Upgrade upgrade;

    private GameObject _upgradeHUD;
    private GameObject _selectUpgrade;

    private UpgradeInventory _upgradeInventory;

    private bool _isActive;
    
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();

        _upgradeInventory = FindObjectOfType<UpgradeInventory>();

        _selectUpgrade = GameObject.Find("SelectUpgrade");
        _upgradeHUD = GameObject.Find("UpgradeHUD");
        
        gameObject.AddComponent<Button>().onClick.AddListener(ChangeState);
        
        if(upgrade) Initialize(upgrade);
    }

    public void Initialize(Upgrade _upgrade)
    {
        upgrade = _upgrade;
        _image.sprite = upgrade.Icon;
    }

    public void SetActive()
    {
        _isActive = true;
    }

    private void ChangeState()
    {
        if (!_isActive)
        {
            if (_upgradeInventory.AddUpgrade(upgrade))
            {
                gameObject.transform.SetParent(_upgradeHUD.transform);
                _isActive = !_isActive;
            }
        }
        else
        {
            _upgradeInventory.RemoveUpgrade(upgrade);
            gameObject.transform.SetParent(_selectUpgrade.transform);
            _isActive = !_isActive;
        }
    }
}
