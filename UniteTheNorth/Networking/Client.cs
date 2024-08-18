﻿using System.Net;
using System.Net.Sockets;
using LiteNetLib;
using MelonLoader;
using UniteTheNorth.Networking.ServerBound;
using UnityEngine;

namespace UniteTheNorth.Networking;

[RegisterTypeInIl2Cpp]
public class Client : MonoBehaviour, INetEventListener
{
    private const string Ip = "127.0.0.1";
    private const int Port = 4657;
    private const string Username = "Test";
    public static Client? Instance;

    public NetManager? NetClient;
    public int ping;

    private void Start()
    {
        Instance = this;
        NetClient = new NetManager(this)
        {
            UnconnectedMessagesEnabled = true,
            UpdateTime = 15,
            ChannelsCount = 4
        };
        NetClient.Connect(new IPEndPoint(IPAddress.Parse(Ip), Port),
            PacketManager.SerializePacket(new UserConnectPacket(Username, "")));
    }

    private void Update()
    {
        NetClient?.PollEvents();
    }

    public bool IsConnected()
    {
        return NetClient != null && NetClient.FirstPeer.ConnectionState == ConnectionState.Connected;
    }

    private void OnDestroy()
    {
        NetClient?.Stop();
    }

    public void OnPeerConnected(NetPeer peer)
    {
        UniteTheNorth.Logger.Msg($"[Client] Connected to {peer.Address}");
    }

    public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
    {
        UniteTheNorth.Logger.Msg($"[Client] Disconnected with reason: {disconnectInfo.Reason}");
    }

    public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
    {
        UniteTheNorth.Logger.Warning($"[Client] Network Error: {socketError}");
    }

    public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
    {
        PacketManager.HandlePacket(reader);
    }

    public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
    {
        // Do absolutely nothing :D
    }

    public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
    {
        ping = latency;
    }

    public void OnConnectionRequest(ConnectionRequest request)
    {
        // Do absolutely nothing again :D
    }
}