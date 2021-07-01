using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyCarCoins : MonoBehaviour
{
    public string carName;
    public int needCoins = 500;
    public GameObject buyButton, openButton;
    public WhichCarSelected checkCars;
    public Text countCoins;
    public GameObject successSound;

    private void Start()
    {
        PlayerPrefs.DeleteKey(carName);
        PlayerPrefs.SetInt("Coins", 514);
            if(PlayerPrefs.GetString(carName) == "open")
        {
            buyButton.SetActive(false);
            openButton.SetActive(true);
        }

    }

    public void BuyCar()
    {
        int coins = PlayerPrefs.GetInt("Coins");
        if(coins >= needCoins)
        {
            if (PlayerPrefs.GetString("music") != "No")
            {
                GetComponent<AudioSource>().Play();
                GameObject sound = Instantiate(successSound, Vector3.zero, Quaternion.identity);
                Destroy(sound, 3f);
            }

            buyButton.SetActive(false);
            openButton.SetActive(true);
            PlayerPrefs.SetString(carName, "open");
            PlayerPrefs.SetInt("Coins", coins - needCoins);
            countCoins.text = PlayerPrefs.GetInt("Coins").ToString();
            PlayerPrefs.SetString("NowCar", carName);
            checkCars.CheckButtons();
        }
        else
        {
            if (PlayerPrefs.GetString("music") != "No")
                GetComponent<AudioSource>().Play();
        }
    }
}
