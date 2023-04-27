using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class NetworkPort
{
    public delegate void PacketDelegate(Packet packet, int port);
    public PacketDelegate packetSent;
    public PacketDelegate packetReceived;
    public int index;
    public PhysicalAddress parentMAC;

    public NetworkPort(int index, PhysicalAddress parentMAC) {
        this.index = index;
        this.parentMAC = parentMAC;
    }

    public void LinkPort(NetworkPort networkPort) {
        networkPort.packetSent += RecievePacket;
        //UnityEngine.Debug.Log(string.Format("Linked: {0} : {1} <-> {2} : {3}", this.parentMAC, this.index, networkPort.parentMAC, networkPort.index));
    }

    public void RecievePacket(Packet packet, int port) {
        UnityEngine.Debug.Log(this.parentMAC + " received packet on port " + index);
        packetReceived?.Invoke(packet, index);
    }

    public void SendPacket(Packet packet, int port) {
        UnityEngine.Debug.Log(this.parentMAC + " sent packet on port " + index);
        packetSent?.Invoke(packet, index);
    }
}
