using Xunit;

namespace DiskWars.Tests
{
    public class NetworkMessageTests
    {
        [Fact]
        public void Chat_Serialize()
        {
            string chatMessage = "this is a message ... lol!";
            var chat = new NetworkMessage.Chat
            {
                message = chatMessage
            };

            string expected =
                "{" +
                    $"\"{nameof(NetworkMessage.Chat.message)}\":\"{chatMessage}\"" +
                "}";
            string actual = chat.Serialize();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Chat_Deserialize()
        {
            string chatMessage = "this is a message ... lol!";
            string json =
                "{" +
                    $"\"{nameof(NetworkMessage.Chat.message)}\":\"{chatMessage}\"" +
                "}";

            var chat = NetworkMessage.Chat.Deserialize(json);

            Assert.Equal(chatMessage, chat.message);
        }

        [Fact]
        public void NetworkMessage_Chat_Serialize()
        {
            string chatMessage = "this is a message ... lol!";
            var message = new NetworkMessage
            {
                type = NetworkMessage.Type.Chat,
                chat = new NetworkMessage.Chat
                {
                    message = chatMessage
                }
            };

            string expected =
                "{" +
                    $"\"{nameof(NetworkMessage.type)}\":\"{nameof(NetworkMessage.chat)}\"," +
                    $"\"{nameof(NetworkMessage.chat)}\":" +
                    "{" +
                        $"\"{nameof(NetworkMessage.Chat.message)}\":\"{chatMessage}\"" +
                    "}" +
                "}";
            string actual = message.Serialize();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NetworkMessage_Chat_Deserialize()
        {
            string chatMessage = "this is a message ... lol!";
            string json =
                "{" +
                    $"\"{nameof(NetworkMessage.type)}\":\"{nameof(NetworkMessage.chat)}\"," +
                    $"\"{nameof(NetworkMessage.chat)}\":" +
                    "{" +
                        $"\"{nameof(NetworkMessage.Chat.message)}\":\"{chatMessage}\"" +
                    "}" +
                "}";

            var message = NetworkMessage.Deserialize(json);

            Assert.Equal(NetworkMessage.Type.Chat, message.type);
            Assert.Equal(chatMessage, message.chat.message);
        }
    }
}