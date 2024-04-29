using System;
using System.IO.Ports;
using UnityEngine.Assertions;

namespace DefaultNamespace.Buttons8
{
	public class BitFlagSerialReader : SerialReader
	{
		public Action<bool[]> OnDataChanged;
		public bool[] Data { get; private set; }
		private bool[] _prevData;
		private int bitSize;
		

		/// <param name="size">Number of bits.</param>
		public BitFlagSerialReader(int size, string portName, int baud = 9600, Parity parity = Parity.None) : base(portName, baud, parity)
		{
			Assert.IsTrue(size >= 0,"Number of bits cannot be negative.");
			this.bitSize = size;
			Data = new bool[size];
			_prevData = new bool[size];
		}

		protected override void OnMessage(string message)
		{
			if (int.TryParse(message, out int i))
			{
				bool changed = false;
				for (int j = 0; j < Data.Length; j++)
				{
					Data[j] = ((i >> j) & 1) == 1;
					if (Data[j] != _prevData[j])
					{
						changed = true;
					}
				}

				if (changed)
				{
					OnDataChanged?.Invoke(Data);
					Array.Copy(Data,_prevData,bitSize);
				}
			}
		}
	}
}