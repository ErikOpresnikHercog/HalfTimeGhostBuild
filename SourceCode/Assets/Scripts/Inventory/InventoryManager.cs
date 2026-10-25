using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public Item[] AllItems;
    public int[] itemsInInventory;
    public int[] itemAmount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(int itemID, int amount)
    {
        if (itemsInInventory.Contains(itemID))
        {
            for (int i = 0; i < itemsInInventory.Length; i++)
            {
                if (itemsInInventory[i] == itemID)
                {
                    itemAmount[i] += amount;
                    return;
                }
            }
        }
        else
        {
            for(int i = 0; i < itemsInInventory.Length; i++)
            {
                if(itemsInInventory[i] == 0)
                {
                    itemsInInventory[i] = itemID;
                    itemAmount[i] = amount;
                    return;
                }
            }
           
            
        }
    }

    public void RemoveItem(int itemID, int amount)
    {
        for (int i = 0; i < itemsInInventory.Length; i++)
        {
            if (itemsInInventory[i] == itemID)
            {
                itemAmount[i] -= amount;
                if(itemAmount[i] <= 0)
                {
                    itemsInInventory[i] = 0;
                    itemAmount[i] = 0;
                }
                return;
            }
        }
    }

    public void CheckForItem(int itemID, int amount)
    {

    }
}
