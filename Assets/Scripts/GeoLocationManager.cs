using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GeoLocationManager : IPrepare
{
    public static string country;
    public static string city;

    public IEnumerator Prepare(Action<bool, string> onComplete)
    {
        string uri = "https://ipapi.co/json/";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onComplete?.Invoke(false, "Error fetching location data: " + webRequest.error);
            }
            else
            {
                try
                {
                    GeoLocationData locationData = GeoLocationData.CreateFromJSON(webRequest.downloadHandler.text);

                    country = locationData.country_code.ToLower();
                    city = locationData.city.ToLower();

                    onComplete?.Invoke(true, null);
                }
                catch (Exception e)
                {
                    onComplete?.Invoke(false, "Error parsing location data: " + e.Message);
                }
            }
        }
    }
}
