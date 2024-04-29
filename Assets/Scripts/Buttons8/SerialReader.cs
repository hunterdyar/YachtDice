using System;
using System.IO;
using System.IO.Ports;
using UnityEngine;

namespace DefaultNamespace.Buttons8
{
	/// <summary>
	/// A wrapper for a SerialPort that reads each serial message Line (until line break) as a message.
	/// </summary>
	public class SerialReader
	{
		public string PortName { get; private set; }
		public int Baud { get; private set; }
		public Parity Parity { get; private set; }

		private SerialPort _port;
		private string _buffer;
		public string Data;

		public SerialReader(string portName, int baud = 9600, Parity parity = Parity.None)
		{
			PortName = portName;
			Baud = baud;
			Parity = parity;
			CreatePort();
			InitRead();
		}

		private void CreatePort()
		{
			_port = new SerialPort(PortName, Baud, Parity);
			//try
			_port.Open();
		}

		void InitRead()
		{
			// ports = SerialPort.GetPortNames();
			// _port = new SerialPort(ports[portIndex], baud, parity);
			// _port.Open();
			
			byte[] buffer = new byte[8 * 11];
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

		private void PortOnDataReceived(byte[] data)
		{
			for (int i = 0; i < data.Length; i++)
			{
				string c = char.ConvertFromUtf32(data[i]);
				if (c == "\n")
				{
					if (_buffer != "")
					{
						OnMessage(_buffer.Trim()); //gets rid of the \r
						_buffer = "";
					}
				}
				else
				{
					_buffer += c;
				}
			}

		}

		protected virtual void OnMessage(string message)
		{
			Data = message;
		}
	}
}