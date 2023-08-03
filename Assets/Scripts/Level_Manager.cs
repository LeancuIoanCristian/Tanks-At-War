using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

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
    [SerializeField] [Range(0,1000) ] private float x_scale = 1000f;
    [SerializeField] [Range(0, 1000)] private float z_scale = 1000f;
    [SerializeField] private GameObject ground;
    [SerializeField] private NavMeshSurface ground_navmesh_surface;

    //Ai Variables
    [SerializeField] private GameObject ai_prefab;
    [SerializeField] private int allowed_ais;
    [SerializeField] private List<GameObject> ai_references = new List<GameObject>();
    private float ai_x_space_taken = 10.0f;
    private float ai_z_space_taken = 30.0f;

    //Shops Variables
    [SerializeField] private GameObject shops_prefab;
    [SerializeField] private int allowed_shops;
    [SerializeField] private List<GameObject> shop_references = new List<GameObject>();
    private float shop_x_space_taken = 50.0f;
    private float shop_z_space_taken = 90.0f;

    //Interest Points
    [SerializeField] private Transform player_start_point;
    [SerializeField] private Transform player_finish_point;
    [SerializeField] private GameObject finish_point;
    [SerializeField] private bool level_finished = false;
    [SerializeField] private Vector3 origin = new Vector3(0f, 0f, 0f);

    //Upgrades Variables
    [SerializeField]
    private TankUpgradesScriptableObjectScipt upgrades_references;

    [SerializeField] GameState game_state_ref;

    public int GetUpgradesDone(UpgradeType type) => upgrades_references.NumberOfUpgradesDone(type);

    void Start()
    {
        SetUpLevel();
    }

    private void SetUpLevel()
    {      
        allowed_ais = scriptable_object_levels_reference.GetAINumber();      
        allowed_shops = scriptable_object_levels_reference.GetPlatformNumber();
        x_scale = scriptable_object_levels_reference.GetXScale() * 2f;
        z_scale = scriptable_object_levels_reference.GetZScale() * 2f;

        ground.transform.localScale = new Vector3(x_scale,z_scale ,0.0f );
        ground_navmesh_surface.BuildNavMesh();

        for (int index = 0; index < allowed_ais; index++)
        {
            GameObject ai_clone = Instantiate(ai_prefab);          
            ai_clone.GetComponent<AI_UI_Manager>().SetPlayerReference(player_position_reference);
            ai_clone.GetComponentInChildren<AIBrain>().SetPlayerReference(player_position_reference.gameObject);
            ai_clone.GetComponentInChildren<AIBrain>().SetGameStateReference(game_state_ref);
            ai_references.Add(ai_clone);
        }
        Placement(ai_references, allowed_ais, ai_x_space_taken, ai_z_space_taken);
        
        for (int index = 0; index < allowed_shops; index++)
        {
            GameObject shop_clone = Instantiate(shops_prefab);
            shop_clone.GetComponent<MiniShopFunctionality>().SetUp();
            shop_clone.GetComponent<MiniShopFunctionality>().SetLevelManagerReference(this);
            if (x_scale > z_scale)
            {
                shop_clone.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }
            shop_references.Add(shop_clone);
        }
        Placement(shop_references, allowed_shops, shop_x_space_taken, shop_z_space_taken);
        
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

    //private void PlaceAI(GameObject ai_clone, float x_taken_space, float z_taken_space)
    //{
    //    int x_offset = Random.Range(-250, 251);
    //    int z_offset = Random.Range(-250, 251);
    //    int y_offset = 10;

    //    Vector3 position_offset = new Vector3(x_offset, y_offset, z_offset);

    //    ai_offset = player_position_reference.transform.position + position_offset;
    //    ai_clone.transform.position = ai_offset;

    //}

   

    //private void PlaceShop(GameObject shop_clone, float x_taken_space, float z_taken_space)
    //{
    //    float x_offset = Random.Range(-9, 10) * x_taken_space + Randomizer() * x_taken_space /4f ;
    //    float z_offset = Random.Range(-5, 6) * z_taken_space + Randomizer() * z_taken_space / 4f;
    //    float y_offset = 0;

    //    Vector3 position = new Vector3(x_offset, y_offset, z_offset);

    //    shops_offset = position;
    //    shop_clone.transform.position = position;


    //}

    private void Placement(List<GameObject> shop_list, int number_of_units, float taken_space_x, float taken_space_z)
    {
        int list_index = 0;
        int spaceings_x = SpacesCreator(NearestSqrRoot(number_of_units), x_scale);
        int spaceings_z = SpacesCreator(NearestSqrRoot(number_of_units), z_scale);
        for (int row_index = 0; row_index < NearestSqrRoot(number_of_units) && list_index < shop_list.Count; row_index++)
        {
            for (int col_index = 0; col_index < NearestSqrRoot(number_of_units) && list_index < shop_list.Count; col_index++)
            {
                if (x_scale > z_scale)
                {
                    float x_offset = (x_scale / 2f) * -1f + col_index * spaceings_x + taken_space_x / 4f;
                    float z_offset = (z_scale / 2f) * -1f + row_index * spaceings_z + taken_space_z / 4f;
                    float y_offset = 0;
                    shop_list[list_index].transform.position = new Vector3(x_offset, y_offset, z_offset);
                }
                else
                {
                    float x_offset = (x_scale / 2f) * -1f + row_index * spaceings_x + taken_space_x / 4f;
                    float z_offset = (z_scale / 2f) * -1f + col_index * spaceings_z + taken_space_z / 4f;
                    float y_offset = 0;
                    shop_list[list_index].transform.position = new Vector3(x_offset, y_offset, z_offset);
                }
                

                
                list_index++;
            }
        }
    }

    private int NearestSqrRoot(int number_of_units)
    {
        int increment = 0;
        for (increment = 1; increment  < number_of_units / 2; increment++)
        {
            if (increment * increment > number_of_units)
            {
                break;
            }
        }
        return increment;
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

    private int SpacesCreator(int number_of_units_per_line, float value_scale)
    {
        return (int)value_scale / number_of_units_per_line;
    }



}
