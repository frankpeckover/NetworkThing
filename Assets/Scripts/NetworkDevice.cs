using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System;


public abstract class NetworkDevice
{
    public IPAddress ipAddress { get; private set;} // Device IP
    public PhysicalAddress macAddress { get; private set;} // Device Physical Address
    public NetworkPort[] networkPorts { get; private set; } // List of network ports/interfaces on device ie. 1 for PC, 6 for switch

    public static PhysicalAddress BROADCASTADDRESS = PhysicalAddress.Parse("FF-FF-FF-FF-FF-FF"); // Broadcast Address for ARP packets typically
    

    public NetworkDevice(int maxConnections) {

        this.macAddress = PhysicalAddress.Parse(this.GenerateRandomMACAddress());
        this.ipAddress = IPAddress.Parse(this.GenerateRandomIPAddress());

        this.networkPorts = new NetworkPort[maxConnections];

        //Initialise network ports on device with identifier and subscribe to port events for receiving packets
        for (int index = 0; index < maxConnections; index++)
        {
            this.networkPorts[index] = new NetworkPort(index, this.macAddress);
            this.networkPorts[index].packetReceived += HandlePacket;
        }
    }

    private string GenerateRandomMACAddress() {

        Random random = new Random();

        string macAddress = "";

        int randomNumber = random.Next(255);
        string hexString = randomNumber.ToString("X2");
        macAddress += hexString;

        for (int hex = 0; hex < 5; hex++)
        {
            randomNumber = random.Next(255);
            hexString = randomNumber.ToString("X2");
            macAddress += ("-" + hexString);
        }
        
        //UnityEngine.Debug.Log("Generated MAC address: " + macAddress);
        return macAddress;
    }

    private string GenerateRandomIPAddress() {

        Random random = new Random();

        string ipAddress = "10.0.0.";

        int octet = random.Next(255);
        ipAddress += octet.ToString();        

        UnityEngine.Debug.Log("Generated IP address: " + ipAddress);
        return ipAddress;
    }

    public IPAddress ApplySubnetMask(IPAddress ip, IPAddress subnetMask) {
        if (ip.AddressFamily != subnetMask.AddressFamily) {
            throw new ArgumentException("IP address and subnet mask must be of the same address family.");
        }

        byte[] ipBytes = ip.GetAddressBytes();
        byte[] maskBytes = subnetMask.GetAddressBytes();
        byte[] networkBytes = new byte[ipBytes.Length];

        for (int i = 0; i < ipBytes.Length; i++) {
            networkBytes[i] = (byte)(ipBytes[i] & maskBytes[i]);
        }

        return new IPAddress(networkBytes);
    }

    public bool isSameNetwork(IPAddress ip, IPAddress subnetMask) {
        IPAddress destinationNetwork = this.ApplySubnetMask(ip, subnetMask);
        IPAddress sourceNetwork = this.ApplySubnetMask(this.ipAddress, subnetMask);

        return destinationNetwork == sourceNetwork ? true : false;
    }

    public void SendPacket(Packet packet, int port) {
        this.networkPorts[port].SendPacket(packet, port);
    }

    public void BroadcastPacket(Packet packet) {
        for (int port = 0; port < this.networkPorts.Length; port++)
        {
            this.networkPorts[port].SendPacket(packet, port);
        }
    }
    
    public abstract void HandlePacket(Packet packet, int port);
}