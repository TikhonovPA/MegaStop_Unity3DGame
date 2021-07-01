using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text coinText, nowScore, topScore;
    public GameObject[] canvasButtons;
    public Transform mainCam;
    public GameObject[] roads;
    public GameObject barrier, blinker;
    public GameObject[] cars;
    public float cameraMoveSpeed = 10f;
    public List<GameObject> roadOnScreen = new List<GameObject>();
    private float _prevXCamPos, nextRoadXPos = 146.8f, _distanceForNewRoad = 60f, barrierXPos = -23f, rotateSpeed;
    private int CountRoads = 3, lastBarrierRoad, getListSideElement, parkedCars;
    private Rigidbody createdCar;
    private List<bool> turnSides = new List<bool>();
    private bool needToTurnCarLeft;
    private GameObject playerCar;
    public bool needToTurnCarRight = true;
    public AudioClip carNoise, breaks, correctPark;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();

        if (!PlayerPrefs.HasKey("NowCar"))
        {
            playerCar = cars[0];
            return;
        }
        else
        {
            foreach (GameObject car in cars)
            {
                if (car.name == PlayerPrefs.GetString("NowCar"))
                {
                    playerCar = car;
                    break;
                }
            }
        }

        PlayCarNoise();

        topScore.text = "TOP: " + PlayerPrefs.GetInt("Score");
        
        Barrier.isLose = false;
        rotateSpeed = cameraMoveSpeed * 5f;

        createdCar = Instantiate(playerCar,
        new Vector3(-30f, -0.24f, -9.8f), 
        Quaternion.Euler(0,-90,0)).GetComponent<Rigidbody>();
        createdCar.transform.SetParent(transform);
        _prevXCamPos = mainCam.position.x;

        CreateBarrier();
    }

    private void Update()
    {
        mainCam.Translate(Vector3.right * cameraMoveSpeed * Time.deltaTime, Space.World);
        transform.Translate(Vector3.right * cameraMoveSpeed * Time.deltaTime, Space.World);

        if (createdCar.transform.localPosition.x < 15)
        {
            Vector3 pos = createdCar.transform.localPosition;
            createdCar.transform.localPosition = Vector3.MoveTowards(
                pos,
                new Vector3(-15f, pos.y, pos.z),
                10f * Time.deltaTime);
        }

        if (mainCam.position.x > _prevXCamPos + _distanceForNewRoad) 
        {
            _prevXCamPos = mainCam.position.x;
            GameObject nextRoad = Instantiate(roads[Random.Range(0,roads.Length)], new Vector3(nextRoadXPos,0,0), Quaternion.identity);
            nextRoadXPos += 73.4f;

            nextRoad.name = "Road - " + CountRoads;
            CountRoads++;

            bool? leftSide = null;
            int whichBarrier = -2;

            if (nextRoad.name != "Road - " + lastBarrierRoad)
            {
                leftSide = Random.Range(0, 2) == 0;
                whichBarrier = Random.Range(1, 13);
            }


            bool isAddSideToList = false;
            for (short i = 0; i < 15; i++)
            {
                if (leftSide == true && (i == whichBarrier || i == whichBarrier + 1))
                {
                    lastBarrierRoad = CountRoads;
                    if (!isAddSideToList)
                    {
                        turnSides.Add(true);
                        isAddSideToList = true;
                    }

                    GameObject newBlinker = Instantiate(blinker, new Vector3(barrierXPos, -0.2f, -3.05f), Quaternion.identity);
                    newBlinker.transform.SetParent(nextRoad.transform);
                }
                else
                    InstantiateBarrier(barrierXPos, nextRoad.transform);

                if (leftSide == false && (i == whichBarrier || i == whichBarrier + 1))
                {
                    lastBarrierRoad = CountRoads;

                    if (!isAddSideToList)
                    {
                        turnSides.Add(false);
                        isAddSideToList = true;
                    }

                    GameObject newBlinker = Instantiate(blinker, new Vector3(barrierXPos, -0.2f, -16.99f), Quaternion.identity);
                    newBlinker.transform.SetParent(nextRoad.transform);
                }
                else
                    InstantiateBarrier(barrierXPos, nextRoad.transform, true);

                barrierXPos += 5f;
            }

            roadOnScreen.Add(nextRoad);

            foreach (GameObject road in roadOnScreen)
            {
                if (road.transform.position.x < mainCam.position.x - 73.4f)
                {
                    Destroy(road);
                    roadOnScreen.Remove(road);
                    break;
                }
            }
        }

        if (Barrier.isLose && !canvasButtons[0].activeSelf)
        {
            foreach (GameObject btn in canvasButtons)
            {
                btn.SetActive(true);
            }
        }

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
#if !UNITY_EDITOR

            if (Input.GetTouch(0).phase != TouchPhase.Began)
                return;
#endif
            if (createdCar.transform.localPosition.x < -15f)
                return;

            if (needToTurnCarLeft || needToTurnCarRight)
                return;

            if (PlayerPrefs.GetString("music") != "No" && _audioSource.clip != breaks)
            {
                _audioSource.clip = breaks;
                _audioSource.loop = false;
                _audioSource.Play();
            }

            if (turnSides.Count == 0 || turnSides.Count - 1 < getListSideElement)
                return;

            if (turnSides[getListSideElement])
                needToTurnCarLeft = true;
            else
                needToTurnCarRight = true;
            getListSideElement++;
        }
    }

    private void FixedUpdate()
    {
        if ((needToTurnCarLeft || needToTurnCarRight) && !Barrier.isLose)
            TurnCar(rotateSpeed);

        if (Barrier.isLose && cameraMoveSpeed > 0)
            cameraMoveSpeed = 0;
    }

    void TurnCar(float tSpeed)
    {
        if (needToTurnCarLeft)
            tSpeed *= -1;

        cameraMoveSpeed -= cameraMoveSpeed * 1.5f * Time.deltaTime;
        if (cameraMoveSpeed < 3f)
            cameraMoveSpeed = 0f;

        if (createdCar.transform.eulerAngles.y > 90f && needToTurnCarLeft
            || needToTurnCarRight && createdCar.transform.eulerAngles.y < 88f || createdCar.transform.eulerAngles.y > 93f)
        {
            createdCar.MoveRotation(
                createdCar.rotation * Quaternion.Euler(0, tSpeed * Time.fixedDeltaTime, 0));

            float moveSpeed = 6f * Time.deltaTime;
            if (needToTurnCarLeft)
                moveSpeed = 5f * Time.deltaTime * -1;

            createdCar.transform.localPosition -= new Vector3(0, 0, moveSpeed);
        }
        else
        {
            cameraMoveSpeed = 0;
            createdCar.transform.localEulerAngles = new Vector3(0, 90, 0);

            Invoke("createNewCar", 1f);

            if (PlayerPrefs.GetString("music") != "No" && _audioSource.clip != correctPark)
            {
                _audioSource.clip = correctPark;
                _audioSource.loop = false;
                _audioSource.Play();
            }
        }
    }
    void createNewCar()
    {
        if (Barrier.isLose || (!needToTurnCarLeft && !needToTurnCarRight))
            return;

        Destroy(createdCar);

        createdCar.transform.SetParent(null);
        Destroy(createdCar.gameObject, 3f);

        needToTurnCarLeft = false;
        needToTurnCarRight = false;
        cameraMoveSpeed = Random.Range(40, 55);

        createdCar = Instantiate(playerCar,
        new Vector3(mainCam.transform.position.x - 2f, -0.24f, -9.8f),
        Quaternion.Euler(0, -90, 0)).GetComponent<Rigidbody>();
        createdCar.transform.SetParent(transform);

        PlayCarNoise();


        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 1);
        coinText.text = PlayerPrefs.GetInt("Coins").ToString();
        parkedCars++;
        nowScore.text = "<color=#FF6464>NOW:</color> " + parkedCars;

        if (PlayerPrefs.GetInt("Score") < parkedCars)
        {
            PlayerPrefs.SetInt("Score",parkedCars);
            topScore.text = "TOP: " + parkedCars;
        }
    }

    private void PlayCarNoise()
    {
        if (PlayerPrefs.GetString("music") != "No")
        {
            _audioSource.clip = carNoise;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    private void CreateBarrier()
    {
        for (short i = 0; i < 15; i++)
        {
            InstantiateBarrier(barrierXPos, roadOnScreen[0].transform);
            InstantiateBarrier(barrierXPos, roadOnScreen[0].transform, true);
            barrierXPos += 5f;
        }

        for (short i = 0; i < 15; i++)
        {
            InstantiateBarrier(barrierXPos, roadOnScreen[1].transform);
            InstantiateBarrier(barrierXPos, roadOnScreen[1].transform, true);
            barrierXPos += 5f;
        }

    }

    private void InstantiateBarrier(float xPos, Transform roadParent, bool rightPosition = false)
    {
        float zPos = rightPosition ? -16.5f : -3.5f;
        GameObject newObj = Instantiate(barrier, new Vector3(xPos, 0.3f, zPos), Quaternion.identity);
        newObj.transform.SetParent(roadParent);
    }

}
