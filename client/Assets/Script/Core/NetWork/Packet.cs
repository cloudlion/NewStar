using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameNetWork.Packet
{
    public enum PacketType
    {
        // Handshake represents a handshake: request(client) <====> handshake response(server)
        Handshake = 0x01,

    // HandshakeAck represents a handshake ack from client to server
        HandshakeAck = 0x02,

    // Heartbeat represents a heartbeat
        Heartbeat = 0x03,

    // Data represents a common data packet
        Data = 0x04,

    // Kick represents a kick off packet
        Kick = 0x05 // disconnect message from server
    }

    public enum MessageType
    {
        Request = 0x00,
        Notify = 0x01,
        Response = 0x02,
        Push = 0x03
    }
    public class Packet
    {
        public PacketType type;
        public int length;
        public byte[] data;
    }
}
