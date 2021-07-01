using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : MonoBehaviour
{
    private MeshRenderer _mesh;
    void Start()
    {
        _mesh = GetComponent<MeshRenderer>();
        StartCoroutine(BlinkObject());
    }

    IEnumerator BlinkObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);
            _mesh.enabled = !_mesh.enabled;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.transform.localPosition.x == -15)
        {
            GameObject.Find("Game Controller").GetComponent<GameController>().needToTurnCarRight = true;
        }
    }
}
