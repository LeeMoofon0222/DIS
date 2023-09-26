using UnityEngine;

public class Chaser : MonoBehaviour
{
    public float speed = 5.0f;

    public int health;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(h, 0.0f, v);

        transform.position += moveDirection * speed * Time.deltaTime;
    }
}