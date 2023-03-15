using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovableObjects
{
    void MovementDirection();
}

public interface IDamageable
{
    void GiveDamage(float damage_value);
}

public interface IDestroyable
{
    void DestroyObject();
}