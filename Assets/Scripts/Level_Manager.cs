using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Manager : MonoBehaviour
{
    [SerializeField] private Tank player_position_reference;

    [SerializeField] private GameObject ai_prefab;
    [SerializeField] private int allowed_ais;
    private Vector3 ai_offset;
    [SerializeField] private List<GameObject> ai_references = new List<GameObject>();
    private float ai_x_space_taken = 20.0f;
    private float ai_z_space_taken = 50.0f;

    [SerializeField] private GameObject shops_prefab;
    private Vector3 shops_offset;
    [SerializeField] private int allowed_shops;
    [SerializeField] private List<GameObject> shop_references = new List<GameObject>();
    private float shop_x_space_taken = 40.0f;
    private float shop_z_space_taken = 80.0f;

    [SerializeField] private Transform player_start_point;
    [SerializeField] private Transform player_finish_point;
    [SerializeField] private GameObject finish_point;

    private List<float[,]> taken_spaces = new List<float[,]>();

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
            PlaceAI(ai_clone, ai_x_space_taken ,ai_z_space_taken);
            ai_references.Add(ai_clone);
        }

        for (int index = 0; index < allowed_shops; index++)
        {
            GameObject shop_clone = Instantiate(shops_prefab);
            //shop_clone.AddComponent<MiniShopFunctionality>();
            PlaceShop(shop_clone, shop_x_space_taken, shop_z_space_taken);
            shop_references.Add(shop_clone);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlaceAI(GameObject ai_clone, float x_taken_space, float z_taken_space)
    {
        int x_offset = Random.Range(200, 401) * Randomizer(called_randomizer);
        int z_offset = Random.Range(200, 401) * Randomizer(called_randomizer);
        int y_offset = 10;

        float[,] taken_space = new float[(int)x_taken_space, (int)z_taken_space];
        ai_clone.GetComponent<AI_UI_Manager>().SetPlayerReference(player_position_reference);

        ai_offset = player_position_reference.transform.position + FindSpace(x_offset, y_offset, z_offset, taken_space);
        ai_clone.transform.position = ai_offset;

    }

    private void PlaceShop(GameObject shop_clone, float x_taken_space, float z_taken_space)
    {
        int x_offset = Random.Range(350, 401) * Randomizer(called_randomizer);
        int z_offset = Random.Range(350, 401) * Randomizer(called_randomizer);
        int y_offset = 10;


        float[,] taken_space = new float[(int)x_taken_space, (int)z_taken_space];
        shops_offset = player_position_reference.transform.position + FindSpace(x_offset, y_offset, z_offset, taken_space);
        shop_clone.transform.position = shops_offset;

        shop_clone.GetComponent<MiniShopFunctionality>().SetUp();
    }

    private Vector3 FindSpace(int x_offset, int y_offset, int z_offset, float[,] taken_area)
    {
        float[,] ai_position = new float[(int)player_position_reference.transform.position.x + x_offset, (int)player_position_reference.transform.position.z + z_offset];
        bool space_available = true;
        for (int index = 0; index < taken_spaces.Count; index++)
        {
            space_available = AreaAroundPositionCheck(taken_area, ai_position, index);
            if (!space_available)
            {
                x_offset += (int)taken_area.GetLength(0) / 2;
                z_offset += (int)taken_area.GetLength(1) / 2;
                index = 0;
            }
        }
        taken_spaces.Add(ai_position);
        return new Vector3(ai_position.GetLength(0), y_offset, ai_position.GetLength(1));

    }

    private bool AreaAroundPositionCheck(float[,] taken_area, float[,] ai_position, int index)
    {
        ///    VVV the x value in taken spaces list:: VVV the lower side end of the object on the x axis::     VVV the x value in taken spaces list::   VVV the higher side end of the object on the x axis::   VVV the z value in taken spaces list::   VVV the lower side end of the object on the z axis::   VVV the z value in taken spaces list::  VVV the higher side end of the object on the z axis
        return taken_spaces[index].GetLength(0) < ai_position.GetLength(0) - taken_area.GetLength(0) / 2.0f && taken_spaces[index].GetLength(0) > ai_position.GetLength(0) + taken_area.GetLength(0) / 2.0f && taken_spaces[index].GetLength(1) < ai_position.GetLength(1) - taken_area.GetLength(1) / 2.0f && taken_spaces[index].GetLength(1) > ai_position.GetLength(1) + taken_area.GetLength(1) / 2.0f;
    }

  

    private int Randomizer(int call_number)
    {
        int copy = call_number;
        call_number++;
        return (copy % 2 == 0) ? -1 : 1;
    }
}
