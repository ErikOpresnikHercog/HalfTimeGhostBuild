using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public Texture artwork;
    public Sprite Test1;
    [HideInInspector]
    public int BuyCost;
    [HideInInspector]
    public int SellCost;
    [HideInInspector]
    public bool Sellable = true;
    public RarityMenu Rarity;
    [SerializeField]
    public enum RarityMenu
    {
        Basic,
        Common,
        Uncommon,
        Rare,
        Epic,
        Mythic
         
    }






}
