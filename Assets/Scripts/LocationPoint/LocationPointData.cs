using System;

[Serializable]
public class LocationPointData : ILocationPointData
{
    public string id;
    public string name;
    public float latitude;
    public float longitude;
    public string type;
    public string description;

    public string Id => id;

    public string Name => name;

    public float Latitude => latitude;

    public float Longitude => longitude;

    public string Type => type;

    public string Description => description;
}

public interface ILocationPointData
{
    string Id { get; }
    string Name { get; }
    float Latitude { get; }
    float Longitude { get; }
    string Type { get; }
    string Description { get; }
}