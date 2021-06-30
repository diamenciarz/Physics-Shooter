using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

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
    PhotonView photonView;
    void Start()
    {
        photonView = PhotonView.Get(this);

        currentHP = maxHP;

        CheckForHealthBar();
    }
    private void CheckForHealthBar()
    {
        if (healthBarPrefab != null)
        {
            healthBarGO = PhotonNetwork.Instantiate(healthBarPrefab.name, transform.position, transform.rotation);
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
    }

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
                photonView.RPC("DestroyProperly", RpcTarget.All);
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
