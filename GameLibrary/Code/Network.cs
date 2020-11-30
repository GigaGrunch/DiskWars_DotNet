using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiskWars
{
    public class Network
    {
        public delegate void LogCallback(string message);
        public LogCallback Log;

        public delegate void ChatCallback(string message);
        public ChatCallback ChatReceived;

        public NetworkStream networkStream;
        public StreamWriter networkWriter;
        public StreamReader networkReader;

        public async Task HostServer(int port)
        {
            Log("creating TCP listener");
            TcpListener listener = new TcpListener(IPAddress.Any, port: port);
            listener.Start();

            TcpClient client = await listener.AcceptTcpClientAsync();
            Log("a client connected");

            networkStream = client.GetStream();
            networkWriter = new StreamWriter(networkStream);
            networkReader = new StreamReader(networkStream);

            HandleIncomingMessages();
        }

        public async Task ConnectClient(string host, int port)
        {
            Log("creating TCP client");
            TcpClient tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(host: host, port: port);
            Log("connected to server");

            networkStream = tcpClient.GetStream();
            networkWriter = new StreamWriter(networkStream);
            networkReader = new StreamReader(networkStream);

            HandleIncomingMessages();
        }

        public void SendMessage(Message message)
        {
            string serialized = message.Serialize();
            networkWriter.WriteLine(serialized);
            networkWriter.Flush();
        }

        public async void HandleIncomingMessages()
        {
            Network.Message message = default;

            async Task<bool> ReadNext(StreamReader reader)
            {
                string serialized = await reader.ReadLineAsync();
                if (serialized == null)
                {
                    return false;
                }

                message = Message.Deserialize(serialized);
                return true;
            }

            while (await ReadNext(networkReader))
            {
                ChatReceived("TODO!"); // TODO!
            }
        }

        public struct Message
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

            public static Message Deserialize(string json)
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

                Message message = new Message();

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
}
