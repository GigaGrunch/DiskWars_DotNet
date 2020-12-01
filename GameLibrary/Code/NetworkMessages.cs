using System.Text.RegularExpressions;

namespace DiskWars
{
    public struct NetworkMessage
    {
        public enum Type
        {
            DiskPlacement
        }

        public Type type;

        public DiskPlacement diskPlacement;

        public string Serialize()
        {
            string typeString = null;
            string payload = null;

            switch (type)
            {
                case Type.DiskPlacement:
                {
                    typeString = nameof(diskPlacement);
                    payload = diskPlacement.Serialize();
                } break;
            }

            return
                "{" +
                    $"\"{nameof(type)}\":\"{typeString}\"," +
                    $"\"{typeString}\":{payload}" +
                "}";
        }

        public static NetworkMessage Deserialize(string json)
        {
            string payloadKey = "payload";
            string pattern =
                "{.*" +
                    $"\"{nameof(type)}\".*:.*\"(?<{nameof(type)}>.*)\".*,.*" +
                    $"\".*\".*:.*(?<{payloadKey}>{{.*}})" +
                ".*}";

            Regex regex = new Regex(pattern);
            Match match = regex.Match(json);

            string typeString = match.Groups[nameof(type)].Value;
            string payload = match.Groups[payloadKey].Value;

            NetworkMessage message = new NetworkMessage();

            switch (typeString)
            {
                case nameof(diskPlacement):
                {
                    message.type = Type.DiskPlacement;
                    message.diskPlacement = DiskPlacement.Deserialize(payload);
                } break;
            }

            return message;
        }

        public struct DiskPlacement
        {
            public string Serialize()
            {
                return "{}";
            }

            public static DiskPlacement Deserialize(string json)
            {
                return new DiskPlacement();
            }
        }
    }
}

