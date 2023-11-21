using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemAndAmount
{
    public int ID;
    public int amount;
}


[CreateAssetMenu(fileName = "new Recipe" , menuName = "Recipe/Create new Recipe")]
public class CreateRecipe : ScriptableObject    
{
    public List<ItemAndAmount> materials;   //materials ¬O­Ó°}¦C!
    public int outputAmount;
    public GameObject output;
    public Item Itemoutput;

    public GameObject craftresult;

    public bool forsmelting;
    public bool forcooking;

}
