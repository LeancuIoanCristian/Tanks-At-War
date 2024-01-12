using UnityEngine;
public class Hull : MonoBehaviour, IDamageable
{
    private const int base_health = 3000;
    [SerializeField] private int tank_health;
    public int GetHealth() => tank_health;
    [SerializeField] private Tracks tank_tracks;
    public Tracks GetTracks() => tank_tracks;
    public void SetTracks(Tracks tracks_reference) => tank_tracks = tracks_reference;
    [SerializeField] private Engine tank_engine;
    //armor
    [SerializeField] private int armor_front;
    [SerializeField] private int armor_side;
    [SerializeField] private int armor_rear;
    [SerializeField] private int weight;


    public int GetBaseHealth() => base_health;

    public void SetHealth(int value) => tank_health = value;

    public int GetHullHealth() => tank_health;

    public void GiveDamage()
    {
        Debug.Log("Hull Component GiveDamage called");
    }

    public void TakeDamage(int damage_value)
    {
        tank_health -= damage_value;
        if (tank_health <= 0)
        {
            GetComponentInParent<Tank>()?.IDestroy();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Ammo ammo = collision.gameObject.GetComponent<Ammo>();
        if (ammo == null) return;
        Vector3 velocity = ammo.last_velocity;
        Vector3 collision_face_normal = collision.contacts[0].normal;
       

        float impact_angle = Vector3.Angle(collision_face_normal, -velocity.normalized);

        Debug.DrawRay(collision.contacts[0].point, collision_face_normal, Color.red, 10f);
        Debug.DrawRay(collision.contacts[0].point, -velocity.normalized, Color.green, 10f);

        print(impact_angle);       
    }
}
