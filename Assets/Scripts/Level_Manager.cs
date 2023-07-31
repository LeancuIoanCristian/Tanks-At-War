using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    private static Level_Manager instance = null;
    public static Level_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Level_Manager();
            }
            return instance;
        }
    }

    public void Awake()
    {
        instance = this;
    }


    //Player Variables
    [SerializeField] private Levels_Manager_scriptableObject scriptable_object_levels_reference;
    [SerializeField] private Tank player_position_reference;

    //Ai Variables
    [SerializeField] private GameObject ai_prefab;
    [SerializeField] private int allowed_ais;
    private Vector3 ai_offset;
    [SerializeField] private List<GameObject> ai_references = new List<GameObject>();
    private float ai_x_space_taken = 30.0f;
    private float ai_z_space_taken = 50.0f;

    //Shops Variables
    [SerializeField] private GameObject shops_prefab;
    private Vector3 shops_offset;
    [SerializeField] private int allowed_shops;
    [SerializeField] private List<GameObject> shop_references = new List<GameObject>();
    private float shop_x_space_taken = 50.0f;
    private float shop_z_space_taken = 90.0f;

    //Interest Points
    [SerializeField] private Transform player_start_point;
    [SerializeField] private Transform player_finish_point;
    [SerializeField] private GameObject finish_point;
    [SerializeField] private bool level_finished = false;
    [SerializeField] private Vector3 origin = new Vector3(500f, 0f, 500f);

    //Upgrades Variables
    [SerializeField]
    private TankUpgradesScriptableObjectScipt upgrades_references;

    [SerializeField] GameState game_state_ref;

    public int GetUpgradesDone(UpgradeType type) => upgrades_references.NumberOfUpgradesDone(type);

    private int called_randomizer = 0;
  
    void Start()
    {
        SetUpLevel();
    }

    private void SetUpLevel()
    {
        player_position_reference.transform.position = player_start_point.position;
        called_randomizer = 0;
       
       for (int index = 0; index < allowed_ais; index++)
       {
            GameObject ai_clone = Instantiate(ai_prefab);
            PlaceAI(ai_clone, ai_x_space_taken , ai_z_space_taken);
            ai_clone.GetComponent<AI_UI_Manager>().SetPlayerReference(player_position_reference);
            ai_clone.GetComponentInChildren<AIBrain>().SetPlayerReference(player_position_reference.gameObject);
            ai_clone.GetComponentInChildren<AIBrain>().SetGameStateReference(game_state_ref);
            ai_references.Add(ai_clone);
       }

        for (int index = 0; index < allowed_shops; index++)
        {
            GameObject shop_clone = Instantiate(shops_prefab);
            PlaceShop(shop_clone, shop_x_space_taken, shop_z_space_taken);
            shop_references.Add(shop_clone);
        }

        SetUpPlayer();
    }

    public void SetUpPlayer()
    {
        player_position_reference.GetComponentInChildren<Turret>().GetComponentInChildren<Gun>().GetCurrentAmmo().SetDamage(player_position_reference.GetComponentInChildren<Turret>().GetComponentInChildren<Gun>().GetCurrentAmmo().GetDamage() + upgrades_references.NumberOfUpgradesDone(UpgradeType.Damage) * upgrades_references.GetDamageUp());
        player_position_reference.GetComponentInChildren<Hull>().SetHealth(player_position_reference.GetComponentInChildren<Hull>().GetBaseHealth() + upgrades_references.NumberOfUpgradesDone(UpgradeType.Health) * upgrades_references.GetHealthUp());
    }

    public int GetFullHealthPlayer() => player_position_reference.GetComponentInChildren<Hull>().GetBaseHealth() + upgrades_references.NumberOfUpgradesDone(UpgradeType.Health) * upgrades_references.GetHealthUp();

    internal void UpgradeValue(UpgradeType upgrade_type)
    {
        upgrades_references.IncrementUpgradesDone(upgrade_type);
        UpdatePlayerStats(upgrade_type);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForLeftEnemies();
    }

    private void CheckForLeftEnemies()
    {
        for (int index = 0; index < ai_references.Count; index++)
        {
            if (ai_references[index].activeSelf)
            {
                level_finished = false;
                break;
            }
        }
        if (level_finished)
        {
            finish_point.SetActive(true);
        }
    }

    private void PlaceAI(GameObject ai_clone, float x_taken_space, float z_taken_space)
    {
        int x_offset = Random.Range(-250, 251) * Randomizer() + (int) ai_x_space_taken* Randomizer(); ;
        int z_offset = Random.Range(-250, 251) * Randomizer() + (int) ai_z_space_taken* Randomizer(); ;
        int y_offset = 10;

        Vector3 position_offset = new Vector3(x_offset, y_offset, z_offset);       
      
        ai_offset = player_position_reference.transform.position + position_offset;
        ai_clone.transform.position = ai_offset;

    }

    private void PlaceShop(GameObject shop_clone, float x_taken_space, float z_taken_space)
    {
        int x_offset = Random.Range(-300, 501) * Randomizer() + 2 *(int) shop_x_space_taken;
        int z_offset = Random.Range(-300, 501) * Randomizer() + 2 * (int) shop_z_space_taken;
        int y_offset = 10;

        Vector3 position = new Vector3(x_offset, y_offset, z_offset);

        shops_offset =player_position_reference.transform.position + position; 
        shop_clone.transform.position = shops_offset;

        shop_clone.GetComponent<MiniShopFunctionality>().SetUp();
        shop_clone.GetComponent<MiniShopFunctionality>().SetLevelManagerReference(this);
    }
  
    private int Randomizer()
    {
        int copy = called_randomizer;
        called_randomizer+= 2;

        if (copy % 5 == 0)
        {
            called_randomizer = 0;
            return -1;
        }
        else if (copy % 3 == 0)
        {
            return 1;
        }
        else
        {
            if (copy + 3 * Random.Range(-1, 2) > copy - 5 * Random.Range(-1, 2))
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
    }

    public void LoadNextLevel()
    {
        scriptable_object_levels_reference.NextLevel();
    }

    public void UpdatePlayerStats(UpgradeType upgrade_type_reference)
    {
        switch (upgrade_type_reference)
        {
            case UpgradeType.Health:
            {
                player_position_reference.GetComponent<Hull>().SetHealth(player_position_reference.GetComponent<Hull>().GetBaseHealth() + upgrades_references.GetHealthUp() * upgrades_references.NumberOfUpgradesDone(UpgradeType.Health));
                break;
            }

            case UpgradeType.Damage:
            {
                    Debug.Log("Before:" + player_position_reference.GetComponentInChildren<Gun>().GetCurrentAmmo().GetDamage());
                    Debug.Log(upgrades_references.GetDamageUp());
                    Debug.Log(upgrades_references.NumberOfUpgradesDone(UpgradeType.Damage));
                   
                    player_position_reference.GetComponentInChildren<Gun>().GetCurrentAmmo().SetDamage(player_position_reference.GetComponentInChildren<Gun>().GetCurrentAmmo().GetDamage() + upgrades_references.GetDamageUp() * upgrades_references.NumberOfUpgradesDone(UpgradeType.Damage));
                    Debug.Log("After" + player_position_reference.GetComponentInChildren<Gun>().GetCurrentAmmo().GetDamage());
                    break;
            }

            case UpgradeType.CurrencyMultiplier:
            {
                    Debug.Log("Currency Upgraded");
                break;
            }

            default:
            {
                Debug.Log("No upgrade done");
                break;
            }
        }
    }

}
