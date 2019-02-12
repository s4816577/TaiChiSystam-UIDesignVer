#if !UNITY_EDITOR
using System;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
#endif

public class UdpNetworkServer
{
    static private UdpNetworkServer instance = null;

#if !UNITY_EDITOR
    private DatagramSocket pressureSocket = null;
    private DatagramSocket imageSocket = null;
#endif

    // Pressure Received Observer.
    public delegate void OnPressureReceivedHandler(byte[] data);
    public event OnPressureReceivedHandler OnPressureReceivedEvent;

    private readonly string pressureReceivePort = "7000";

    private PressurePreProcessor pressurePreProcessor;
    public PressurePreProcessor PressurePreProcessor { get { return pressurePreProcessor; } }


    // Image Received Observer.
    public delegate void OnImageReceivedHandler(byte[] data);
    public event OnImageReceivedHandler OnImageReceivedEvent;

    private readonly string imageReceivePort = "12345";


    static public UdpNetworkServer GetInstance()
    {
        if (instance == null)
            instance = new UdpNetworkServer();

        return instance;
    }
    
    private UdpNetworkServer()
    {
        pressurePreProcessor = new PressurePreProcessor(this);
#if !UNITY_EDITOR
        InitialServer();
#endif
    }

#if !UNITY_EDITOR
    private async void InitialServer()
    {
        pressureSocket = new DatagramSocket();
        imageSocket = new DatagramSocket();

        pressureSocket.MessageReceived += OnPressureReceived;
        imageSocket.MessageReceived += OnImageReceived;

        try
        {
            //await pressureSocket.BindEndpointAsync(null, "7000");
            await pressureSocket.BindServiceNameAsync(pressureReceivePort);
        }
        catch (Exception e)
        {
            string log = e.ToString();
            log = SocketError.GetStatus(e.HResult).ToString();
            return;
        }

        try
        {
            await imageSocket.BindServiceNameAsync(imageReceivePort);
        }
        catch (Exception e)
        {
            string log = e.ToString();
            log = SocketError.GetStatus(e.HResult).ToString();
            return;
        }
        //await pressureSocket.BindServiceNameAsync(pressureReceivePort);
        //await imageSocket.BindServiceNameAsync(imageReceivePort);
    }
#endif

#if !UNITY_EDITOR
    private async void OnPressureReceived(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs arguments)
    {
        DataReader reader = arguments.GetDataReader();

        uint length = reader.UnconsumedBufferLength;
        byte[] receiveBytes = new byte[length];
        reader.ReadBytes(receiveBytes);

        if (OnPressureReceivedEvent != null)
            OnPressureReceivedEvent.Invoke(receiveBytes);
    }
#endif

#if !UNITY_EDITOR
    private async void OnImageReceived(DatagramSocket socket, DatagramSocketMessageReceivedEventArgs arguments)
    {
        DataReader reader = arguments.GetDataReader();

        uint length = reader.UnconsumedBufferLength;
        byte[] receiveBytes = new byte[length];
        reader.ReadBytes(receiveBytes);

        if (OnImageReceivedEvent != null)
            OnImageReceivedEvent.Invoke(receiveBytes);
    }
#endif

}
