using UnityEngine;

public class ObjectCrack : MonoBehaviour
{

    ItemObject IO;
    public GameObject CrackedObject;

    Vector3 scale;

    public bool resize;
    // Start is called before the first frame update
    void Awake()
    {
        IO = GetComponent<ItemObject>();

        
    }

    private void OnDestroy()
    {
        GameObject obj = Instantiate(CrackedObject , this.transform.position , this.transform.rotation );
        
        if (resize)
        {
            obj.transform.localScale = new Vector3(this.transform.lossyScale.x * 0.01f, this.transform.lossyScale.y * 0.01f, this.transform.lossyScale.z * 0.01f);
        }
        else
        {
            obj.transform.localScale = this.transform.lossyScale;
        }
        
        Destroy( obj , 10f );


    }
    

}
