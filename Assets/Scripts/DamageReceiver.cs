using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;
    [SerializeField] int team;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        CollisionDamageDealer collisionDamageDealer;
        if (collision.gameObject.TryGetComponent<CollisionDamageDealer>(out collisionDamageDealer))
        {
            if (collisionDamageDealer.GetTeam()!= team)
            {
                ReceiveDamage(collisionDamageDealer.GetDamage());
            }
        }
    }
    public void ReceiveDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            DestroyProperly();
        }
    }
    public void DestroyProperly()
    {

        Destroy(gameObject);
    }

    public int GetCurrentHP()
    {
        return currentHP;
    }
    public int GetMaxHP()
    {
        return maxHP;
    }
}
