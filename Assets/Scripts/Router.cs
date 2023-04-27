using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;

public class Router : NetworkDevice
{
    public Dictionary<PhysicalAddress, IPAddress> arpTable;

    public Dictionary<IPAddress, IPAddress> routingTable;

    public Router(int maxConnections) : base(maxConnections) {
        this.arpTable = new Dictionary<PhysicalAddress, IPAddress>();
        this.routingTable = new Dictionary<IPAddress, IPAddress>();
    }

    public override void HandlePacket(Packet packet, int port)
    {
        throw new System.NotImplementedException();
    }

}
