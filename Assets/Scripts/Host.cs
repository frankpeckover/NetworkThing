using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;

public class Host : NetworkDevice
{
    private IPAddress defaultGateway;

    public Dictionary<IPAddress, PhysicalAddress> arpTable;

    public Host(int maxConnections=1) : base(maxConnections) {
        this.arpTable = new Dictionary<IPAddress, PhysicalAddress>();
    }

    public Packet CreatePacket(IPAddress destinationIP) {
        Packet packet;
        if (arpTable.ContainsKey(destinationIP)) {
            packet = new Packet(this.ipAddress, destinationIP, this.macAddress, arpTable[destinationIP]);
        } else {
            packet = new Packet(this.ipAddress, destinationIP, this.macAddress);
        }

        //UnityEngine.Debug.Log("Created Packet: " + packet.payload);
        return packet;
    }

    public override void HandlePacket(Packet packet, int port)
    {
        if (packet.sourceMAC == this.macAddress)
        {
            this.networkPorts[port].SendPacket(packet, port);
        }
        // if Mac address matches mine then MY PACKET
        // else drop packet?
    }

}
