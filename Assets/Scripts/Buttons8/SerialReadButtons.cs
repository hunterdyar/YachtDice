using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.IO.Ports;
using System.Linq;
using DefaultNamespace.Buttons8;

public class SerialReadButtons : MonoBehaviour
{
    public string[] ports = SerialPort.GetPortNames();
    private SerialReader _serialReader;
    public int portIndex = 0;
    public int baud = 115200;
    public Parity parity;
   
    // void 

    private void OnDestroy()
    {
        // _port.Dispose();
    }
    
}
