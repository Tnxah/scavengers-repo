using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class DebugMap : MonoBehaviour
{
    public static DebugMap instance;

    private const string KEY = "lbGCj5a0FVmQjIjwuubeNrdyvn0D0TtN";
    private const string LINK = "https://www.mapquestapi.com/staticmap/v5/map?";

    // Start is called before the first frame update
    void Start()
    {
        if( instance != null)
        {
            Destroy(instance.gameObject);
            instance = null;
        }
        if (instance == null)
        {
            instance = this;
        }

        StartCoroutine(LoadMap());
    }

    private string GetURL()
    {
        return $"{LINK}key={KEY}" +
            $"&center={Regex.Replace(GPSController.latitude.ToString(), ",", ".")},{Regex.Replace(GPSController.longitude.ToString(), ",", ".")}" +
            $"&zoom=19&type=map" +
            $"&size=1920,1920@2x";
    }

    private IEnumerator LoadMap()
    {
        yield return new WaitUntil(() => GPSController.isLocationServiceEnabled);
        var gameposition = CoordinateConverter.GPSToGamePosition(GPSController.latitude, GPSController.longitude);
        transform.position = new Vector3(gameposition.x, 0.01f, gameposition.z);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(GetURL());
        yield return request.SendWebRequest();

        yield return new WaitUntil(() => request.result == UnityWebRequest.Result.Success);
        GetComponent<Renderer>().material.mainTexture = null;
        Texture texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        GetComponent<Renderer>().material.mainTexture = texture;
    }
}
