using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class CollisionDamageDealer : MonoBehaviour
{
    [Header("Damage Stats")]
    [SerializeField] int damage;
    [SerializeField] int team;
    [SerializeField] bool destroyMyselfOnCollision = true;

    PhotonView photonView;
    DataCollector dataCollector;
    private void Start()
    {
        dataCollector = FindObjectOfType<DataCollector>();
        photonView = PhotonView.Get(this);
    }
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
        return destroyMyselfOnCollision;
    }
    /*
    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        byte destroyThisBulletEventCode = 1;

        if (eventCode == destroyThisBulletEventCode)
        {

        }
    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        PhotonView targetView = collision.gameObject.GetComponent<PhotonView>();

        DamageReceiver damageReceiver;
        if (collisionObject.TryGetComponent<DamageReceiver>(out damageReceiver))
        {
            if (damageReceiver.GetTeam() != team)
            {
                targetView.RPC("ReceiveDamage", RpcTarget.AllBuffered, damage);

                if (destroyMyselfOnCollision)
                {
                    photonView.RPC("DestroyThisObject", RpcTarget.AllBuffered);
                }
            }
        }
    }
    [PunRPC]
    public void DestroyThisObject()
    {
        dataCollector.RemoveBulletFromList(gameObject);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
