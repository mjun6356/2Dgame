using UnityEngine;

public class CubeMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,0, -moveSpeed *  Time.deltaTime);

        if (transform.position.z < -20)
        {
            Destroy(gameObject);
        }
    }
}
