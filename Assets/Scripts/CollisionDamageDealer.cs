using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamageDealer : MonoBehaviour
{
    [Header("Damage Stats")]
    [SerializeField] int damage;
    [SerializeField] int team;
    [SerializeField] bool destroyOnCollision = true;
    public int GetDamage()
    {
        return damage;
    }
    public int GetTeam()
    {
        return team;
    }
    public bool GetDestroyOnCollision()
    {
        return destroyOnCollision;
    }
}
