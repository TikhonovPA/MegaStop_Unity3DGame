using UnityEngine;

public class TimeChange : MonoBehaviour
{
    public float scrollspeed = 0.1f;
    public Material material;

    private void Start()
    {
        float rand = Random.Range(0f, 0.2f);
        material.mainTextureOffset = new Vector2(rand, 0);
    }

    private void Update()
    {
        float offset = scrollspeed * Time.deltaTime;
        material.mainTextureOffset += new Vector2(offset, 0);
    }
}
