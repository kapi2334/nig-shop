using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;

namespace clientApp.Models
{
    public class User
    {
        public int id { get; set; }

        public string login { get; set; } = string.Empty;

        public string passwordHash { get; set; } = string.Empty;

        public int addressID { get; set; }

        public ICollection<AddressDto>? Addresses { get; set; }

        public long? nip { get; set; }

        private char _type = 'u';

        [JsonConverter(typeof(TypeConverter))]
        public char type
        {
            get => _type;
            set
            {
                if (value == 'u' || value == 'c')
                {
                    _type = value;
                }
                else
                {
                    _type = 'u';
                    throw new ArgumentException("The type values can only be set to \"u\" and \"c\". The default value \"u\" is used");
                }
            }
        }

        public string name { get; set; } = string.Empty;

        public string? surname { get; set; }

        [JsonIgnore]
        public bool IsCompany => type == 'c';

        public override string ToString()
        {
            return $"User[id={id}, login={login}, name={name}, surname={surname}, type={type}, nip={nip?.ToString() ?? "null"}]";
        }
    }

    public class TypeConverter : JsonConverter<char>
    {
        public override char Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                var value = reader.GetString();
                if (string.IsNullOrEmpty(value)) return 'u';
                char type = value[0];
                return type == 'c' ? 'c' : 'u';
            }
            return 'u';
        }

        public override void Write(Utf8JsonWriter writer, char value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
} 