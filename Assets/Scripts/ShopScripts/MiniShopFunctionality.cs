using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MiniShopFunctionality : MonoBehaviour
{
    private const string default_upgrade_text = "Upgrade cost: ";
    [SerializeField] private Upgrades mini_shop_upgrades;
    [SerializeField] private GameObject damage_upgrade_obj;
    [SerializeField] private GameObject health_upgrade_obj;
    [SerializeField] private GameObject currency_upgrade_obj;
    [SerializeField] private TextMeshProUGUI upgrade_text;

    [SerializeField] private int base_line_price = 100;
    [SerializeField] private bool upgrade_done = false;

    public bool IsUpgradeDone() => upgrade_done;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    private void SetUp()
    {
        damage_upgrade_obj.SetActive(false);
        health_upgrade_obj.SetActive(false);
        currency_upgrade_obj.SetActive(false);

        DetermineActiveUpgradeObject();
    }

   

    private void DetermineActiveUpgradeObject()
    {
        switch(mini_shop_upgrades.GetUpgradeType())
        {
            case UpgradeType.Health:
                    health_upgrade_obj.SetActive(true);
                    break;
            case UpgradeType.Damage:
                    damage_upgrade_obj.SetActive(true);
                    break;
            case UpgradeType.CurrencyMultiplier:
                    currency_upgrade_obj.SetActive(true);
                    break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        upgrade_text.text = default_upgrade_text + (base_line_price * mini_shop_upgrades.GetUpgrade()).ToString();
    }


    private void OnCollisionStay(Collision collision)
    {
        if (Input.GetKeyDown(KeyCode.E) && !upgrade_done)
        {
            mini_shop_upgrades.IncreaseUpgradeValue();
            upgrade_done = true;
            upgrade_text.text = "";
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (upgrade_done)
        {
            this.gameObject.SetActive(false);
        }
        upgrade_text.text = "";
    }
}
