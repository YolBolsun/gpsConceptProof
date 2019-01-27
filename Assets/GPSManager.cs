using UnityEngine;
using System.Collections;

public class GPSManager : MonoBehaviour {

    public GUIStyle style;
    float oldLat = 0;
    float oldLong = 0;
    float totalDistance = 0f;
    float earthRadius = 6371000f;
    float totalDeltaLat = 0;
    float totalDeltaLong = 0;
    bool started = false;
	// Update is called once per frame
	void Update () {
        //Mathf.Deg2Rad
        //Mathf.Cos
        if (started)
        {
            float newLong = Input.location.lastData.longitude;
            float newLat = Input.location.lastData.latitude;


            float deltaLong = newLong - oldLong;
            totalDeltaLong += Mathf.Abs(deltaLong);
            float deltaLat = newLat - oldLat;
            totalDeltaLat += Mathf.Abs(deltaLat);
            //float temp =Mathf.Sqrt(Mathf.Pow((deltaLat / 360 * earthRadius * 2 * Mathf.PI), 2) + Mathf.Pow((deltaLong / 360 * earthRadius * 2 * Mathf.PI), 2));
            float temp = Mathf.Pow(Mathf.Sin(Mathf.Deg2Rad * deltaLat / 2), 2)
                + Mathf.Cos(Mathf.Deg2Rad * oldLat) * Mathf.Cos(Mathf.Deg2Rad * newLat)
                * Mathf.Pow(Mathf.Sin(Mathf.Deg2Rad * deltaLong / 2), 2);
            temp = 2 * Mathf.Atan2(Mathf.Sqrt(temp), Mathf.Sqrt(1 - temp));
            temp *= earthRadius;
            Debug.Log(temp);
            if (!float.IsNaN(temp))
            {
                totalDistance += Mathf.Abs(temp);
            }
            //totalDistance += temp;
            oldLong = newLong;
            oldLat = newLat;
        }
	}
    /*IEnumerator Start()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield break;

        // Start service before querying location
        Input.location.Start();

        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }
        oldLong = Input.location.lastData.longitude;
        oldLat = Input.location.lastData.latitude;

        // Stop service if there is no need to query location updates continuously
        //Input.location.Stop();
    }*/
    void OnGUI()
    {
        GUI.Box(new Rect(10, 10, Screen.width - 20, Screen.height - 20), "latitude: " + Input.location.lastData.latitude +
            "\n" + "longitude: " + Input.location.lastData.longitude + 
            "\n" + "Total Distance: " + totalDistance +
            "\n" + "Total Delta Lat: " + totalDeltaLat +
            "\n" + "Total Delta Long: " + totalDeltaLong, style);
        if (GUI.Button(new Rect(Screen.width-160, 140, 160, 40), "Stop Tracking"))
        {
            Input.location.Stop();
        }
        if (GUI.Button(new Rect(Screen.width-160, 240, 160, 40), "Start Tracking"))
        {
            Input.location.Start();
            oldLong = Input.location.lastData.longitude;
            oldLat = Input.location.lastData.latitude;
            started = true;
        }
    }
}
