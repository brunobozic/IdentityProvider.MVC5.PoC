using Newtonsoft.Json;
using System.Globalization;

namespace Module.CrossCutting.LatestAdditions
{
    public class ByteArrayConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            byte[] numArray;

            switch (reader.TokenType)
            {
                case JsonToken.StartArray:
                    numArray = ReadByteArray(reader);
                    break;
                case JsonToken.String:
                    numArray = Convert.FromBase64String(reader.Value.ToString());
                    break;
                default:
                    throw new Exception(
                        $"Unexpected token parsing binary. Expected String or StartArray, got {reader.TokenType}.");
            }

            return numArray;
        }

        private byte[] GetByteArray(object value)
        {
            return value as byte[];
        }

        private byte[] ReadByteArray(JsonReader reader)
        {
            var list = new List<byte>();

            while (reader.Read())
                switch (reader.TokenType)
                {
                    case JsonToken.Comment:

                        continue;
                    case JsonToken.Integer:
                        list.Add(Convert.ToByte(reader.Value, CultureInfo.InvariantCulture));

                        continue;
                    case JsonToken.EndArray:

                        return list.ToArray();
                    default:
                        throw new Exception($"Unexpected token when reading bytes: {reader.TokenType}");
                }

            throw new Exception("Unexpected end when reading bytes.");
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
            }
            else
            {
                var arr = (byte[])value;
                var y = arr.Select(Convert.ToInt32).ToArray();
                var z = string.Join(", ", y);
                var val = @"[" + z.Replace("\"", string.Empty) + "]";

                writer.WriteRawValue(val);
            }
        }

        private int[] GetIntArrayFromByteArray(byte[] byteArray)
        {
            var intArray = new int[byteArray.Length / 4];

            for (var i = 0; i < byteArray.Length; i += 4) intArray[i / 4] = BitConverter.ToInt32(byteArray, i);

            return intArray;
        }

        private byte[] GetByteArrayFromIntArray(int[] intArray)
        {
            var data = new byte[intArray.Length * 4];

            for (var i = 0; i < intArray.Length; i++) Array.Copy(BitConverter.GetBytes(intArray[i]), 0, data, i * 4, 4);

            return data;
        }
    }
}