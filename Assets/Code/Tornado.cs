using UnityEngine;

public class Tornado : MonoBehaviour
{
    private float speed = 8;
    private float direction = 1f;

    void Update()
    {
        Vector3 move = new Vector3(direction, 0, 0f);
        transform.Translate(move * speed * Time.deltaTime);
    }
}
