using UnityEngine;
using UnityEngine.UI;

public class CoinsText : MonoBehaviour
{
    void Start()
    {
        GetComponent<Text>().text = PlayerPrefs.GetInt("Coins").ToString();
    }

}
