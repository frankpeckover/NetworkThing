using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;

public class Host : NetworkDevice
{
    private IPAddress defaultGateway;
    private IPAddress subnetMask;
    public Dictionary<IPAddress, PhysicalAddress> arpTable;

    public Host(int maxConnections=1) : base(maxConnections) {
        this.arpTable = new Dictionary<IPAddress, PhysicalAddress>();
        this.defaultGateway = IPAddress.Parse("10.0.0.1");
        this.subnetMask = IPAddress.Parse("255.255.255.0");
    }

    public Packet CreatePacket(IPAddress destinationIP) {
        Packet packet;
        bool isSameNetwork = this.isSameNetwork(destinationIP, this.subnetMask);
        if (isSameNetwork) {
            packet = new Packet(this.ipAddress, destinationIP, this.macAddress, this.getMACAddress(destinationIP));
        } else {
            packet = new Packet(this.ipAddress, destinationIP, this.macAddress, this.getMACAddress(this.defaultGateway));
        }
        //UnityEngine.Debug.Log("Created Packet: " + packet.payload);
        return packet;
    }

    public override void HandlePacket(Packet packet, int port) {
        this.UpdateARPTable(packet.sourceIP, packet.sourceMAC);
        if (packet.destinationMAC == this.macAddress) {
            // Do something with packet
            if (packet.payload == PAYLOAD.ARP) {
                Packet arpReply = new Packet(this.ipAddress, packet.sourceIP, this.macAddress, packet.sourceMAC, PAYLOAD.ARPREPLY);
                this.networkPorts[0].SendPacket(arpReply, 0);
            }
        } else {
            // Drop this shit
        }
    }

    private void UpdateARPTable(IPAddress ipAddress, PhysicalAddress macAddress) {
        if (this.arpTable.ContainsKey(ipAddress))
        {
            this.arpTable[ipAddress] = macAddress;
        } else {
            this.arpTable.Add(ipAddress, macAddress);
        }
        //UnityEngine.Debug.Log("Updated MAC address table: " + sourceMac + " -> " + port);
        return;
    }

    private PhysicalAddress getMACAddress(IPAddress ipAddress) {
        if (this.arpTable.ContainsKey(ipAddress)) {
            return this.arpTable[ipAddress];
        } else {
            Packet arpPacket = new Packet(this.ipAddress, ipAddress, this.macAddress, NetworkDevice.BROADCASTADDRESS);
            this.BroadcastPacket(arpPacket);
            
             // Wait for a response to the ARP request
            float startTime = Time.time;
            while (!arpTable.ContainsKey(ipAddress))
            {
                if (Time.time - startTime > 5f) // Timeout after 5 seconds
                {
                    return PhysicalAddress.Parse("00.00.00.00.00.00");
                }
                // Wait for a short time before checking again
                Thread.Sleep(100); // Use System.Threading.Thread.Sleep() to avoid blocking the main Unity thread
            }

            return this.arpTable[ipAddress];
        }
    }
}
