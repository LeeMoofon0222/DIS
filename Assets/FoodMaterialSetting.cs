using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMaterialSetting : MonoBehaviour
{

    
    // Start is called before the first frame update
    public void changeMats(FoodObject _item , int step)
    {
        Material[] mats = this.gameObject.GetComponent<Renderer>().materials;
        mats[0] = _item.roastStep[step];
        GetComponent<Renderer>().materials = mats;

    }

}
