using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseCarMainScene : MonoBehaviour
{
    public GameObject[] cars;
    public AnimationClip mainCar;

    private void Start()
    {
        if(!PlayerPrefs.HasKey("NowCar"))
        {
            CreateCar(cars[0]);
            return;
        }

        foreach (GameObject car in cars)
        {
            if(car.name == PlayerPrefs.GetString("NowCar"))
            {
                CreateCar(car); 
                break;
            }
        }
    }

    private void CreateCar(GameObject car)
    {
        GameObject newCar = Instantiate(car, new Vector3(-1.65f, -0.18f, -11.81f), Quaternion.Euler(0, -90, 0));
        Animation animCar = newCar.AddComponent<Animation>();
        animCar.AddClip(mainCar, "MainCar");
        animCar.clip = mainCar;
        animCar.playAutomatically = true;
        animCar.Play();
    }

}
