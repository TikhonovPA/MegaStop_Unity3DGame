using System;
using UnityEngine;
using UnityEngine.UI;

public class WhichCarSelected : MonoBehaviour
{

    public Image[] checkButtons;
    public Sprite closeImage, checkImage;

    private void Start()
    {
        CheckButtons();
    }

    public void CheckButtons()
    {
        foreach (Image img in checkButtons)
        {
            img.sprite = closeImage;
        }

        if (!PlayerPrefs.HasKey("NowCar") || PlayerPrefs.GetString("NowCar") == "PizzaCar")
        {
            checkButtons[0].sprite = checkImage;
        }
        else if (PlayerPrefs.GetString("NowCar") == "HotDog")
        {
            checkButtons[1].sprite = checkImage;
        }
    }
}
