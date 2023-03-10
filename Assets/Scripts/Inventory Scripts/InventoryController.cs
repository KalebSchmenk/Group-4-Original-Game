using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static int _mushroomCount = 0;
    public static int _cherryCount = 0;
    public static int _leafCount = 0;
    public static int _pebbleCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Mushroom count: " + _mushroomCount);
        Debug.Log("Cherry count: " + _cherryCount);
        Debug.Log("Leaf count: " + _leafCount);
        Debug.Log("Pebble count: " + _pebbleCount);
    }
}
