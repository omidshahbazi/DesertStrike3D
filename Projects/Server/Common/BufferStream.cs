//Rambo Team
using System;

namespace RamboTeam.Common
{
	public class BufferStream
	{
		public byte[] Buffer
		{
			get;
			private set;
		}

		public int Index
		{
			get;
			private set;
		}

		public BufferStream(byte[] Buffer)
		{
			this.Buffer = Buffer;
		}

		public void Reset()
		{
			Index = 0;
		}

		public int ReadInt32()
		{
			int value = BitConverter.ToInt32(Buffer, Index);
			Index += 4;
			return value;
		}

		public float ReadFloat32()
		{
			float value = BitConverter.ToSingle(Buffer, Index);
			Index += 4;
			return value;
		}

		public byte ReadByte()
		{
			return Buffer[Index++];
		}

		public void ReadBytes(ref byte[] Data, int Length)
		{
			for (int i = 0; i < Length; ++i)
				Data[i] = Buffer[Index++];
		}

		public void WriteInt32(int Value)
		{
			WriteBytes(BitConverter.GetBytes(Value));
		}

		public void WriteFloat32(float Value)
		{
			WriteBytes(BitConverter.GetBytes(Value));
		}

		public void WriteBytes(params byte[] Data)
		{
			for (int i = 0; i < Data.Length; ++i)
				Buffer[Index++] = Data[i];
		}

		public void Print(int BytesPerLine = 8)
		{
			Console.Write("Size: ");
			Console.Write(Buffer.Length);
			Console.WriteLine();

			int rowCount = (int)Math.Ceiling(Buffer.Length / (float)BytesPerLine);

			for (int i = 0; i < rowCount; ++i)
			{
				for (int j = 0; j < BytesPerLine; ++j)
				{
					int index = (i * BytesPerLine) + j;

					string hexValue = "  ";

					if (index < Buffer.Length)
						hexValue = Buffer[index].ToString("X2");

					Console.Write(hexValue);
					Console.Write(' ');
				}

				Console.Write('\t');

				for (int j = 0; j < BytesPerLine; ++j)
				{
					int index = (i * BytesPerLine) + j;

					if (index >= Buffer.Length)
						break;

					byte b = Buffer[index];

					if (b == 0)
						b = (byte)'.';

					Console.Write((char)b);
				}

				Console.WriteLine();
			}

			Console.WriteLine();
		}
	}
}
