using UnityEngine;
public interface IDamageable
{
    void GiveDamage(int damage_value, Camera player_camera);

    void TakeDamage(int damage_value);
}


