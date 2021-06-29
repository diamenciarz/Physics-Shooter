using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] int maxHP;
    public int currentHP;
    [SerializeField] int team;
    private bool isDestroyed;
    [Header("Optional")]
    [SerializeField] GameObject healthBarPrefab;

    private GameObject healthBarGO;
    private bool isHealthBarOn;
    ProgressionBarController healthBarScript;
    void Start()
    {
        currentHP = maxHP;

        CheckForHealthBar();
    }
    private void CheckForHealthBar()
    {
        if (healthBarPrefab != null)
        {
            healthBarGO = Instantiate(healthBarPrefab, transform.position, transform.rotation);
            healthBarScript = healthBarGO.GetComponent<ProgressionBarController>();
            healthBarScript.SetObjectToFollow(gameObject);
            isHealthBarOn = true;
        }
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
                if (collisionDamageDealer.GetDestroyOnCollision())
                {
                    Destroy(collision.gameObject);
                }
                ReceiveDamage(collisionDamageDealer.GetDamage());
            }
        }
    }
    public void ReceiveDamage(int damage)
    {
        currentHP -= damage;
        if (isHealthBarOn)
        {
            healthBarScript.UpdateProgressionBar(currentHP, maxHP);
        }

        CheckHP();
    }
    private void CheckHP()
    {
        if (!isDestroyed)
        {
            if (currentHP <= 0)
            {
                DestroyProperly();
            }
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
