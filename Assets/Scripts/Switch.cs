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

    public override void HandlePacket(Packet packet, int port) {
        this.UpdateMacAddressTable(packet.sourceMAC, port);

        if (macAddressTable.ContainsKey(packet.destinationMAC)) {
            int sendPort = macAddressTable[packet.destinationMAC];
            this.networkPorts[sendPort].SendPacket(packet, sendPort);
        } else {
            //gotta ARP -> broadcast request
            UnityEngine.Debug.Log("MAC address not found in switch arp table: " + packet.destinationMAC);
        }
        return;
    }

    private void UpdateMacAddressTable(PhysicalAddress sourceMac, int port) {
        if (macAddressTable.ContainsKey(sourceMac))
        {
            macAddressTable[sourceMac] = port;
        } else {
            macAddressTable.Add(sourceMac, port);
        }
        //UnityEngine.Debug.Log("Updated MAC address table: " + sourceMac + " -> " + port);
        return;
    }
}
