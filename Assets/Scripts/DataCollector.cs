using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    [SerializeField] List<GameObject> bulletList;
    [SerializeField] List<GameObject> playerList;

    


    public void AddBulletGameObjectToList(GameObject objectToAdd)
    {
        bulletList.Add(objectToAdd);
    }
    public void RemoveBulletFromList(GameObject objectToRemove)
    {
        if (bulletList.Contains(objectToRemove))
        {
            bulletList.Remove(objectToRemove);
        }
    }
    public bool CheckIfBulletListContains(GameObject objectToCheck)
    {
        if (bulletList.Contains(objectToCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public GameObject CheckForTheNearestOtherTeamBullet(Vector3 positionVector, int myTeam)
    {
        GameObject theNearestBullet = null;
        foreach (var item in bulletList)
        {
            if (item != null)
            {
                theNearestBullet = item;
                break;
            }
        }

        foreach (var item in bulletList)
        {
            if (item != null)
            {
                if (item.GetComponent<CollisionDamageDealer>().GetTeam() != myTeam)
                {
                    //Sprawdza, czy aktualny pocisk jest bli¿szy od poprzednio bli¿szego. Je¿eli tak, to go ustawia na nowy najbli¿szy
                    if ((positionVector - theNearestBullet.transform.position).sqrMagnitude > (positionVector - item.transform.position).sqrMagnitude)
                    {
                        theNearestBullet = item;
                    }
                }
            }

        }
        if (theNearestBullet == null)
        {
            return null;
        }
        else
        if (theNearestBullet.GetComponent<CollisionDamageDealer>().GetTeam() != myTeam)
        {
            return theNearestBullet;
        }
        return null;
    }
    public List<GameObject> CheckForEnemyBulletsInRange(Vector3 positionVector, int myTeam, float rangeToCheck)
    {
        List<GameObject> bulletsInRangeList = new List<GameObject>();

        foreach (var item in bulletList)
        {
            if (item != null)
            {
                CollisionDamageDealer collisionDamageDealer;
                if (item.TryGetComponent<CollisionDamageDealer>(out collisionDamageDealer))
                {
                    if (collisionDamageDealer.GetTeam() != myTeam)
                    {
                        //Sprawdza, czy aktualny pocisk jest w podanej odleg³oœci od podanej pozycji
                        if ((positionVector - item.transform.position).magnitude <= rangeToCheck)
                        {
                            bulletsInRangeList.Add(item);
                        }
                    }
                }

            }

        }
        return bulletsInRangeList;
    }



    public void AddPlayerToList(GameObject objectToAdd)
    {
        playerList.Add(objectToAdd);
    }
    public void RemovePlayerFromList(GameObject objectToRemove)
    {
        if (playerList.Contains(objectToRemove))
        {
            playerList.Remove(objectToRemove);
        }
    }
    public bool CheckIfPlayerListContains(GameObject objectToCheck)
    {
        if (playerList.Contains(objectToCheck))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public GameObject CheckForTheNearestOtherTeamPlayer(Vector3 positionVector, int myTeam)
    {
        GameObject theNearestPlayer = null;
        foreach (var item in playerList)
        {
            if (item != null)
            {
                theNearestPlayer = item;
                break;
            }
        }

        foreach (var item in playerList)
        {
            if (item != null)
            {
                if (item.GetComponent<DamageReceiver>().GetTeam() != myTeam)
                {
                    //Sprawdza, czy aktualny pocisk jest bli¿szy od poprzednio bli¿szego. Je¿eli tak, to go ustawia na nowy najbli¿szy
                    if ((positionVector - theNearestPlayer.transform.position).sqrMagnitude > (positionVector - item.transform.position).sqrMagnitude)
                    {
                        theNearestPlayer = item;
                    }
                }
            }

        }
        if (theNearestPlayer == null)
        {
            return null;
        }
        else
        if (theNearestPlayer.GetComponent<DamageReceiver>().GetTeam() != myTeam)
        {
            return theNearestPlayer;
        }
        return null;
    }
    public List<GameObject> CheckForEnemyPlayersInRange(Vector3 positionVector, int myTeam, float rangeToCheck)
    {
        List<GameObject> bulletsInRangeList = new List<GameObject>();

        foreach (var item in playerList)
        {
            if (item != null)
            {
                DamageReceiver damageReceiver;
                if (item.TryGetComponent<DamageReceiver>(out damageReceiver))
                {
                    if (damageReceiver.GetTeam() != myTeam)
                    {
                        //Sprawdza, czy aktualny pocisk jest w podanej odleg³oœci od podanej pozycji
                        if ((positionVector - item.transform.position).magnitude <= rangeToCheck)
                        {
                            bulletsInRangeList.Add(item);
                        }
                    }
                }

            }

        }
        return bulletsInRangeList;
    }
}
