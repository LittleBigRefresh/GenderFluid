using System.Text.Json.Serialization;

namespace GenderFluid;

public struct Entry
{
    public uint Key;
    [JsonIgnore] internal uint StringOffset;
    public string Contents;
}