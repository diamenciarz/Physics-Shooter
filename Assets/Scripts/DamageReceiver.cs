using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] int maxHP;
    public int currentHP;
    [SerializeField] int team;
    [Header("Optional")]
    [SerializeField] GameObject healthBarPrefab;
    [SerializeField] Vector3 healthBarDeltaPositionFromObject;

    private GameObject healthBarGO;
    private bool isHealthBarOn;
    private bool isDestroyed;
    ProgressionBarController healthBarScript;
    PhotonView healthBarView;
    PhotonView photonView;
    void Start()
    {
        photonView = PhotonView.Get(this); 

        currentHP = maxHP;

        photonView.RPC("CreateHealthBar", RpcTarget.AllBuffered);
        //CreateHealthBar();
    }

    [PunRPC]
    private void CreateHealthBar()
    {
        if (!isHealthBarOn)
        {
            if (healthBarPrefab != null)
            {
                healthBarGO = PhotonNetwork.Instantiate(healthBarPrefab.name, transform.position, transform.rotation);
                healthBarScript = healthBarGO.GetComponent<ProgressionBarController>();
                healthBarScript.SetObjectToFollow(gameObject);

                healthBarView = healthBarGO.GetComponent<PhotonView>();
                if (healthBarDeltaPositionFromObject.magnitude != 0)
                {
                    healthBarScript.SetDeltaPositionToObject(healthBarDeltaPositionFromObject);
                }

                isHealthBarOn = true;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;

        CollisionDamageDealer collisionDamageDealer;
        if (collisionObject.TryGetComponent<CollisionDamageDealer>(out collisionDamageDealer))
        {
            if (collisionDamageDealer.GetTeam()!= team)
            {
                float damage = collisionDamageDealer.GetDamage();
                Debug.Log("damage: " + damage);
                photonView.RPC("ReceiveDamage", RpcTarget.All, damage);
            }
        }
    }*/

    /*
    public const byte destroyOtherBulletCode = 1;
    private void SendDestroyOtherBullet(GameObject objectToDestroy)
    {
        object content = objectToDestroy;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All};
        PhotonNetwork.RaiseEvent(destroyOtherBulletCode,content,raiseEventOptions,SendOptions.SendReliable);
    }*/

    [PunRPC]
    public void ReceiveDamage(int damage)
    {
        currentHP -= damage;
        if (isHealthBarOn)
        {
            healthBarView.RPC("UpdateProgressionBar", RpcTarget.AllBuffered, currentHP, maxHP);
        }
        CheckHP();
    }
    private void CheckHP()
    {
        if (!isDestroyed)
        {
            if (currentHP <= 0)
            {
                RespawnOnDeath respawnOnDeath;
                if (TryGetComponent<RespawnOnDeath>(out respawnOnDeath))
                {
                    respawnOnDeath.ActWhenDestroyed();
                }

                photonView.RPC("DestroyProperly", RpcTarget.AllBuffered);
            }
        }
    }
    [PunRPC]
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
    public int GetTeam()
    {
        return team;
    }
}
