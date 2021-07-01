using UnityEngine;

public class Clouds : MonoBehaviour
{
    public float speed = 0.1f;
    private float yPos;

    private void Start()
    {
        yPos = transform.position.y;

        int rand = Random.Range(0, 2);
        if (rand == 1)
            speed *= -1;
    }

    private void Update()
    {
        if (transform.position.y > yPos + 3f || transform.position.y < yPos - 3f)
            speed *= -1;
        transform.position += new Vector3(0, speed * Time.deltaTime, 0);
    }
}
