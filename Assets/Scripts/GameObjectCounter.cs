using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectCounter : MonoBehaviour
{
    public List<GameObject> bulletList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddBulletToList(GameObject bulletToAdd)
    {
        bulletList.Add(bulletToAdd);
    }
    public void TryRemoveBulletFromList(GameObject bulletToRemove)
    {
        if (bulletList.Contains(bulletToRemove))
        {
            bulletList.Remove(bulletToRemove);
        }
    }
}
