using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.IO.Ports;
public class SerialReadButtons : MonoBehaviour
{
    private SerialPort _port;
    public string[] ports = SerialPort.GetPortNames();
    public int portIndex = 0;
    public int baud = 115200;
    public Parity parity;
    private bool open;
    private string _buffer;
    public bool[] buttons;
    private void Awake()
    {
        buttons = new bool[8];
        ports = SerialPort.GetPortNames();
        _port = new SerialPort(ports[portIndex], baud, parity);
        _port.Open();

        byte[] buffer = new byte[8*11];
        Action kickoffRead = null;
        kickoffRead = delegate
        {
            _port.BaseStream.BeginRead(buffer, 0, buffer.Length, delegate(IAsyncResult ar)
            {
                try
                {
                    int actualLength = _port.BaseStream.EndRead(ar);
                    byte[] received = new byte[actualLength];
                    Buffer.BlockCopy(buffer, 0, received, 0, actualLength);
                    PortOnDataReceived(received);
                }
                catch (IOException exc)
                {
                    Debug.LogError(exc);
                   // handleAppSerialError(exc);
                }

                kickoffRead();
            }, null);
        };
        kickoffRead();
    }


    void Start()
    {
        open = false;
    }

    private void PortOnDataReceived(byte[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            string c = char.ConvertFromUtf32(data[i]);
            if (c == "\n")
            {
                if (_buffer != "")
                {
                    OnMessage(_buffer.Trim());//gets rid of the \r
                    _buffer = "";
                }
            }
            else
            {
                _buffer += c;
            }
        }

    }

    private void OnMessage(string message)
    {
        if(int.TryParse(message, out int i))
        {
            for (int j = 0; j < buttons.Length; j++)
            {
                buttons[j] = ((i>>j) & 1) == 1;
            }
        }
    }

    private void Update()
    {
        if (_port.IsOpen != open)
        {
            open = _port.IsOpen;
            Debug.Log("Port is open: "+open);
        }
    }

    private void OnDestroy()
    {
        _port.Dispose();
    }
    
    
}
