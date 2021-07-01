using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    public GameObject explosion;
    public static bool isLose = false;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && !isLose)
        {

            isLose = true;
            other.gameObject.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(1, 1, 1) * 200f);
            GameObject vfx = Instantiate(explosion, other.contacts[0].point, Quaternion.identity);
            Destroy(vfx, 5f);

            if (PlayerPrefs.GetString("music") != "No")
                GetComponent<AudioSource>().Play();
        }
    }

}
