using Google.Maps;
using Google.Maps.Examples;
using Google.Maps.Loading;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
    //52.20925
    //20.97479
    public TextMeshProUGUI coordinatesText;
    [HideInInspector]
    public static GPS instance;

    public float latitude = 0f;
    //= 52.20925f;
    public float longitude = 0f;
    //= 20.97479f;
    private float updateDelay = 1f;
    private float lastUpdate;

    [HideInInspector]
    public bool coordinatesReady = false;
    [HideInInspector]
    public bool isInit = false;

    public bool HardCodeCoordinates;

    //---for direction------
    Vector2 oldPos = Vector2.zero;
    Vector2 currPos = Vector2.zero;
    Vector2 diraction = Vector2.zero;
    //---------------


    [HideInInspector]
    public Vector3 iniRef = Vector3.zero;


    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
            DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(StartLocationService()); 
    }

    private IEnumerator StartLocationService()
    {
        
        if (!HardCodeCoordinates)
        {
            yield return new WaitUntil(() => Input.location.isEnabledByUser);

            if (!Input.location.isEnabledByUser)
            {
                Debug.Log("GPS is disabled");
                yield break;
            }
            else { Debug.Log("GPS is enabled"); }


            Input.location.Start(3, 3);
            yield return new WaitForSeconds(1);
            int maxWait = 20;
            while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
            {
                yield return new WaitForSeconds(1);
                maxWait--;
            }
            if (maxWait <= 0)
            {
                Debug.Log("Time out");
                yield break;
            }
            if (Input.location.status == LocationServiceStatus.Failed)
            {
                Debug.Log("Unable to determin device location");
                yield break;
            }

            UpdateCoordinates();
        }

        coordinatesReady = true;

        Init();

        yield break;
    }

    private void Update()
    {
        if (!HardCodeCoordinates && coordinatesReady && Time.time > (updateDelay + lastUpdate))
        {
            UpdateCoordinates();

            coordinatesText.text = "lat: " + latitude + "\n" + "lon: " + longitude;

            lastUpdate = Time.time;
        }

    }

    private void UpdateCoordinates()
    {
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
    }

    void Init()
    {
        currPos = new Vector2(latitude, longitude);
        print(latitude + " " + longitude + "Init latlan");

        iniRef.x = (float)((longitude * 20037508.34 / 180) / 100);
        iniRef.z = (float)(Math.Log(Math.Tan((90 + latitude) * Math.PI / 360)) / (Math.PI / 180));
        iniRef.z = (float)((iniRef.z * 20037508.34 / 180) / 100);
        isInit = true;
    }


    public void Test()
    {
        print(latitude + " " + longitude);
        var test1 = CoordinateRecounter.Recount(latitude,longitude);
        print(test1);
        var test = CoordinateRecounter.RecountReverse(test1.x, test1.z);
        print(test.x+"|||"+ test.y);
        test = CoordinateRecounter.RecountReverse(0,0);
        print(test.x+"|||"+ test.y);
        
    }
}



