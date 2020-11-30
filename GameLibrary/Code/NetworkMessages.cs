using System.Text.RegularExpressions;

namespace DiskWars
{
    public struct NetworkMessage
    {
        public enum Type
        {
            Chat,
            DiskPlacement
        }

        public Type type;

        public Chat chat;
        public DiskPlacement diskPlacement;

        public string Serialize()
        {
            string typeString = null;
            string payload = null;

            switch (type)
            {
                case Type.Chat:
                {
                    typeString = nameof(chat);
                    payload = chat.Serialize();
                } break;
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
                case nameof(chat):
                {
                    message.type = Type.Chat;
                    message.chat = Chat.Deserialize(payload);
                } break;
                case nameof(diskPlacement):
                {
                    message.type = Type.DiskPlacement;
                    message.diskPlacement = DiskPlacement.Deserialize(payload);
                } break;
            }

            return message;
        }

        public struct Chat
        {
            public string message;

            public string Serialize()
            {
                return
                    "{" +
                        $"\"{nameof(message)}\":\"{message}\"" +
                    "}";
            }

            public static Chat Deserialize(string json)
            {
                string pattern =
                    "{.*" +
                        $"\"{nameof(message)}\".*:.*\"(?<{nameof(message)}>.*)\"" +
                    ".*}";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(json);

                string messageString = match.Groups[nameof(message)].Value;

                return new Chat
                {
                    message = messageString
                };
            }
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

