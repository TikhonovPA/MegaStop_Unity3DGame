using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseNewCar : MonoBehaviour
{
    public string carName;
    public WhichCarSelected checkCars;

    public void ChooseCar()
    {
        if (PlayerPrefs.GetString("music") != "No")
            GetComponent<AudioSource>().Play();
        PlayerPrefs.SetString("NowCar", carName);
        checkCars.CheckButtons();
    }
}
