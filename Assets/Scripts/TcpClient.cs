using System;
using System.Net.Sockets;
using System.Net;

public sealed class TcpClient
{
    public bool isConnected { get; private set; }
    
    private Socket socket;

    public TcpClient()
    {
        isConnected = false;
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public bool ConnectServer(string ip, int port)
    {
        IPAddress ipAddress = IPAddress.Parse(ip);
        IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
        try
        {
            socket.Connect(ipEndPoint);
            isConnected = true;
            return true;
        }
        catch
        {
            isConnected = false;
            return false;
        }
    }

    public bool SendMessage(string message)
    {
        if (!isConnected)
        {
            return false;
        }

        try
        {
            socket.Send(System.Text.Encoding.ASCII.GetBytes(message));
            return true;
        }
        catch
        {
            isConnected = false;
            return false;
        }
    }

    public void Disconnect()
    {
        socket.Disconnect(true);
    }
}
