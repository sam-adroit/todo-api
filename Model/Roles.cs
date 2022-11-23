using System.Text.Json.Serialization;

namespace Week8.Model
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Roles
    {
        Admin = 1,
        User = 2
    }
}