using System.Text.Json.Serialization;

namespace Week8.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Categories
    {
        Work=1,
        Personal=2,
        Leisure=3
    }
}