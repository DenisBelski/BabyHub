using System.Text.Json;
using System.Text.Json.Serialization;

namespace BabyHub.Utils
{
    public class CustomDateTimeConverter : JsonConverter<DateTime>
    {
        private const string Format = "yyyy-MM-ddTHH:mm:ss";

        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            var str = reader.GetString()!;
            return DateTime.Parse(str, null, System.Globalization.DateTimeStyles.RoundtripKind);
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime value,
            JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(Format));
    }
}
