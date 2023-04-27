using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;

public class Packet
{
    public string payload; // Optional

    public IPAddress sourceIP { get; private set; }
    public IPAddress destinationIP { get; private set; }
    public PhysicalAddress sourceMAC { get; private set; }
    public PhysicalAddress destinationMAC { get; private set; }

    public Packet(IPAddress sourceIP, IPAddress destinationIP, PhysicalAddress sourceMAC) {
        this.sourceIP = sourceIP;
        this.destinationIP = destinationIP;
        this.sourceMAC = sourceMAC;

        this.payload = sourceIP + " -> " + destinationIP;
    }

    public Packet(IPAddress sourceIP, IPAddress destinationIP, PhysicalAddress sourceMAC, PhysicalAddress destinationMAC) {
        this.sourceIP = sourceIP;
        this.destinationIP = destinationIP;
        this.sourceMAC = sourceMAC;
        this.destinationMAC = destinationMAC;

        this.payload = sourceIP + " -> " + destinationIP;
    }
}
