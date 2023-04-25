using System;
using System.Text.Json.Serialization;

namespace Hiof.DotNetCourse.V2023.Group14.ClassLibrary.Enums.V1
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum V1Currency
	{
        NOK,
        SEK,
        EUR,
        USD
    }
}

