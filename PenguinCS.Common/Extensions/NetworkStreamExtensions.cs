using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PenguinCS.Common.Extensions;

public static class NetworkStreamExtensions
{
    public static string ReadUTF8NullTerminatedString(this NetworkStream stream)
    {
        using MemoryStream ms = new();

        int character;
        while ((character = stream.ReadByte()) != 0)
        {
            if (character == -1)
            {
                return Encoding.UTF8.GetString(ms.ToArray());
            }

            ms.WriteByte((byte)character);
        }

        return Encoding.UTF8.GetString(ms.ToArray());
    }

    public static Task<string> ReadUTF8NullTerminatedStringAsync(this NetworkStream stream, CancellationToken cancellationToken)
    {
        return Task.Run(() => ReadUTF8NullTerminatedString(stream), cancellationToken);
    }

    public static void WriteUTF8NullTerminatedString(this NetworkStream stream, string value)
    {
        byte[] messageBytes = Encoding.UTF8.GetBytes(value + '\0');
        stream.Write(messageBytes, 0, messageBytes.Length);
        stream.Flush();
    }

    public static Task WriteUTF8NullTerminatedStringAsync(this NetworkStream stream, string value, CancellationToken cancellationToken)
    {
        return Task.Run(() => stream.WriteUTF8NullTerminatedString(value), cancellationToken);
    }

}