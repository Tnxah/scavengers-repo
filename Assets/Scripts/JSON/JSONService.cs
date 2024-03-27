using System.Collections.Generic;
using UnityEngine;

public static class JSONService
{
    public static List<T> DeserializeJsonToList<T>(string jsonArray)
    {
        string newJson = "{\"list\":" + jsonArray + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.list;
    }
}
