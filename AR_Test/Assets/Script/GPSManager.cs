using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSManager : MonoBehaviour
{
    bool gpsInit = false;

    LocationInfo currentGPSPosition;
    double detailed_num =1.0;//기존 좌표는 float형으로 소수점 자리가 비교적 자세히 출력되는 double을 곱하여 자세한 값을 구합니다.

    // Use this for initialization

    private static GPSManager _instance = null;
    public static GPSManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GPSManager)) as GPSManager;

                if (_instance == null)
                {
                    Debug.LogError("There's no active GPSManager object");
                }
            }
            return _instance;
        }
    }

    void Start()

    {
        Input.location.Start(0.5f);

        int wait = 1000; // 기본 값

        // Checks if the GPS is enabled by the user (-> Allow location ) 
        if (Input.location.isEnabledByUser)//사용자에 의하여 좌표값을 실행 할 수 있을 경우
        {
            while (Input.location.status == LocationServiceStatus.Initializing && wait > 0)//초기화 진행중이면
            {
                wait--; // 기다리는 시간을 뺀다
            }

            //GPS를 잡는 대기시간
            if (Input.location.status != LocationServiceStatus.Failed)//GPS가 실행중이라면
            {
                gpsInit = true;
                // We start the timer to check each tick (every 3 sec) the current gps position
                InvokeRepeating("RetrieveGPSData", 0.0001f, 1.0f);//0.0001초에 실행하고 1초마다 해당 함수를 실행합니다.
            }

        }
        else//GPS가 없는 경우 (GPS가 없는 기기거나 안드로이드 GPS를 설정 하지 않았을 경우
        {
            TestConsoleManager.Instance.currentLocationText.text = "GPS not available";
        }
    }

    void RetrieveGPSData()
    {
        currentGPSPosition = Input.location.lastData;//gps를 데이터를 받습니다.

        TestConsoleManager.Instance.currentLocationText.text = 
        "위도 : " + (currentGPSPosition.latitude * detailed_num).ToString() 
        + "\n" + "경도 : " + (currentGPSPosition.longitude * detailed_num).ToString();
    }

    public Vector2 GetLocation()
    {
        Vector2 location = new Vector2((float)(currentGPSPosition.latitude * detailed_num), (float)(currentGPSPosition.longitude * detailed_num));
        
        return location;
    }
}
