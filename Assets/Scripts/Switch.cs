using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;

public class Switch : NetworkDevice
{

    private Dictionary<PhysicalAddress, int> macAddressTable;

    public Switch(int maxConnections) : base(maxConnections) {
        this.macAddressTable = new Dictionary<PhysicalAddress, int>();
        return;
    }

    public override void HandlePacket(Packet packet, int port)
    {
        this.UpdateMacAddressTable(packet.sourceMAC, port);

        if (packet.destinationMAC == NetworkDevice.BROADCASTADDRESS) {
            this.BroadcastPacket(packet);
            return;
        }

        if (macAddressTable.ContainsKey(packet.destinationMAC)) {
            int sendPort = macAddressTable[packet.destinationMAC];
            this.networkPorts[sendPort].SendPacket(packet, sendPort);
            return;
        }
    }

    private void UpdateMacAddressTable(PhysicalAddress sourceMAC, int port) {
        if (macAddressTable.ContainsKey(sourceMAC))
        {
            macAddressTable[sourceMAC] = port;
        } else {
            macAddressTable.Add(sourceMAC, port);
        }
        //UnityEngine.Debug.Log("Updated MAC address table: " + sourceMac + " -> " + port);
        return;
    }
}
