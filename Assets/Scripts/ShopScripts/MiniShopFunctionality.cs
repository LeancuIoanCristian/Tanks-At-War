using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniShopFunctionality : MonoBehaviour
{
    private const string default_upgrade_text = "Upgrade cost: ";
    [SerializeField] private UpgradeType upgrade_type;
    [SerializeField] private GameObject damage_upgrade_obj;
    [SerializeField] private GameObject health_upgrade_obj;
    [SerializeField] private GameObject currency_upgrade_obj;
    [SerializeField] private Level_Manager level_manager_reference;
   // [SerializeField] private TextMeshProUGUI upgrade_text;

    [SerializeField] private int base_line_price = 100;
    [SerializeField] private bool upgrade_done = false;

    public bool IsUpgradeDone() => upgrade_done;
    public void SetUpgradeDone(bool value) => upgrade_done = value;
    public UpgradeType GetMinishopUpgrade() =>upgrade_type; 
    public void SetLevelManagerReference(Level_Manager reference) => level_manager_reference = reference;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    public void SetUp()
    {       
        damage_upgrade_obj.SetActive(false);
        health_upgrade_obj.SetActive(false);
        currency_upgrade_obj.SetActive(false);

        DetermineActiveUpgradeObject((int)Random.Range(0,3));
    }

   

    private void DetermineActiveUpgradeObject(int option)
    {
        switch(option)
        {
            case 0:
                upgrade_type = UpgradeType.Health;
                health_upgrade_obj.SetActive(true);
                break;
            case 1:
                upgrade_type = UpgradeType.Damage;
                damage_upgrade_obj.SetActive(true);
                    break;
            case 2:
                upgrade_type = UpgradeType.CurrencyMultiplier;
                currency_upgrade_obj.SetActive(true);
                    break;
        }
    }

    public string ShowText()
    {
        return default_upgrade_text + (base_line_price * (level_manager_reference.GetUpgradesDone(upgrade_type) + 1)).ToString();
    }

    public void MakeUpgrade(UpgradeType upgrade_type_reference)
    {
        level_manager_reference.UpgradeValue(upgrade_type_reference);
    }
}
