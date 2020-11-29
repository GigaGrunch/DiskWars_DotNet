using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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

        public async Task HostServer(int port)
        {
            Log("creating TCP listener");
            TcpListener listener = new TcpListener(IPAddress.Any, port: port);
            listener.Start();

            TcpClient client = await listener.AcceptTcpClientAsync();
            Log("a client connected");

            networkStream = client.GetStream();

            HandleIncomingMessages();
        }

        public async Task ConnectClient(string host, int port)
        {
            Log("creating TCP client");
            TcpClient tcpClient = new TcpClient();

            await tcpClient.ConnectAsync(host: host, port: port);
            Log("connected to server");

            networkStream = tcpClient.GetStream();

            HandleIncomingMessages();
        }

        public void SendMessage(Message.Payload payload)
        {
            Message message = new Message();

            switch (payload)
            {
                case Message.Chat chat:
                {
                    message.type = Message.Type.Chat;
                    message.payload = Message.Serialize(chat, 8192);
                } break;
                case Message.DiskPlacement diskPlacement:
                {
                    message.type = Message.Type.DiskPlacement;
                    message.payload = Message.Serialize(diskPlacement, 8192);
                } break;
            }

            byte[] serialized = Message.Serialize(message);
            networkStream.WriteAsync(serialized, 0, serialized.Length);
        }

        public async void HandleIncomingMessages()
        {
            Network.Message message = default;
            byte[] buffer = new byte[Marshal.SizeOf(message)];

            async Task<bool> ReadNext(NetworkStream stream)
            {
                int readBytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                message = Message.Deserialize(buffer);
                return readBytes == buffer.Length;
            }

            while (await ReadNext(networkStream))
            {
                ChatReceived("TODO!"); // TODO!
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct Message
        {
            public enum Type
            {
                Chat,
                DiskPlacement
            }

            public Type type;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst=8192)]
            public byte[] payload;

            public interface Payload { }

            [StructLayout(LayoutKind.Sequential, Pack = 0)]
            public struct Chat : Payload
            {
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1024)]
                public string message;
            }

            [StructLayout(LayoutKind.Sequential, Pack = 0)]
            public struct DiskPlacement : Payload
            {

            }

            public static byte[] Serialize<T>(T structure)
            {
                return Serialize(structure, Marshal.SizeOf(structure));
            }

            public static byte[] Serialize<T>(T structure, int size)
            {
                byte[] bytes = new byte[size];

                IntPtr pointer = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(structure, pointer, false);
                Marshal.Copy(pointer, bytes, 0, size);
                Marshal.FreeHGlobal(pointer);

                return bytes;
            }

            public static Message Deserialize(byte[] serialized)
            {
                return new Message(); // TODO!
            }
        }
    }
}
