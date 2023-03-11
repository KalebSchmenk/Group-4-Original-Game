using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleController : MonoBehaviour
{
    private enum CollectibleObject
    {
        Mushroom,
        Cherry,
        Pebble,
        Leaf
    }

    [SerializeField] private CollectibleObject _collectibleObject;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") == false) return;

        if (_collectibleObject == CollectibleObject.Mushroom)
        {
            InventoryController._mushroomCount += 1;
        }
        else if (_collectibleObject == CollectibleObject.Cherry)
        {
            InventoryController._cherryCount += 1;
        }
        else if (_collectibleObject == CollectibleObject.Leaf)
        {
            InventoryController._leafCount += 1;
        }
        else if (_collectibleObject == CollectibleObject.Pebble)
        {
            InventoryController._pebbleCount += 1;
        }

        Destroy(this.gameObject);
    }
}
