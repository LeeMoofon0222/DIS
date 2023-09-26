using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planting_field : MonoBehaviour
{
    public IEnumerator Planting(GameObject plantpoint, int period, List<GameObject> Plantlist, int p)
    {
        GameObject plant = Instantiate(Plantlist[p], plantpoint.transform.position, plantpoint.transform.rotation);
        plant.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(plant, period);
        p += 1;
        if (p < 3)
        {
            print("DD");
            print(period);
            yield return new WaitForSeconds(period);
            print("ZZ");
            yield return StartCoroutine(Planting(plantpoint, period, Plantlist, p));
        }
    }
}
