using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Recipe", menuName = "Food Recipe/Create new Recipe")]
public class FoodRecipe : ScriptableObject
{
    public List<FoodObject> materials;

    public float time;


    public Item output;

    //public List<FoodRecipe> recipes;

}
