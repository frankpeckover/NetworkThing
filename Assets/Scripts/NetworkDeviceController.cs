using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using UnityEngine;

public class NetworkDeviceController : MonoBehaviour
{
    public Host me { get; private set; }
    public Switch sw { get; private set; }

    public Host you { get; private set; }

    void Start()
    {
        this.me = new Host(1);
        UnityEngine.Debug.Log("me: " + this.me.macAddress + " : " + this.me.ipAddress);
        this.sw = new Switch(6);
        UnityEngine.Debug.Log("sw: " + this.sw.macAddress + " : " + this.sw.ipAddress);
        this.you = new Host(1);
        UnityEngine.Debug.Log("you: " + this.you.macAddress + " : " + this.you.ipAddress);

        this.me.arpTable.Add(you.ipAddress, you.macAddress);

        connectDevices(me, 0, sw, 0);
        connectDevices(you, 0, sw, 2);

        Packet packet = this.me.CreatePacket(you.ipAddress);
        this.me.HandlePacket(packet, 0);

    }   

    private void connectDevices(NetworkDevice deviceA, int portA, NetworkDevice deviceB, int portB) {
        deviceA.networkPorts[portA].LinkPort(deviceB.networkPorts[portB]);
        deviceB.networkPorts[portB].LinkPort(deviceA.networkPorts[portA]);
    }
}
    
