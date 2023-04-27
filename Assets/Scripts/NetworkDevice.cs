using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System;


public abstract class NetworkDevice
{

    public IPAddress ipAddress { get; private set;}
    public PhysicalAddress macAddress { get; private set;}
    public NetworkPort[] networkPorts { get; private set; }

    public NetworkDevice(int maxConnections) {

        this.macAddress = PhysicalAddress.Parse(this.GenerateRandomMACAddress());
        this.ipAddress = IPAddress.Parse(this.GenerateRandomIPAddress());

        this.networkPorts = new NetworkPort[maxConnections];

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

        string ipAddress = "";

        int octet = random.Next(255);
        ipAddress += octet.ToString();

        for (int octets = 0; octets < 3; octets++)
        {
            octet = random.Next(255);
            ipAddress += ("." + octet);
        }

        //UnityEngine.Debug.Log("Generated IP address: " + ipAddress);
        return ipAddress;
    }

    public abstract void HandlePacket(Packet packet, int port);
}