using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Ionic.Zlib;

namespace SMPCTool
{
	// Token: 0x02000007 RID: 7
	public class DAG
	{
		// Token: 0x06000025 RID: 37 RVA: 0x000038E0 File Offset: 0x00001AE0
		public DAG(string fileName)
		{
			this.Filename = fileName;
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000038FC File Offset: 0x00001AFC
		public void DecompressDAG(string fileName)
		{
			BinaryReader binaryReader = new BinaryReader(File.OpenRead(this.Filename));
			uint magic = binaryReader.ReadUInt32();

			// Validate magic for both PC and PS4
			bool validMagic = (magic == headerMagicPC || magic == headerMagicPS4);
			if (!validMagic)
			{
				MessageBox.Show("Invalid DAG file (magic: 0x" + magic.ToString("X8") + ")",
					"", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				binaryReader.Close();
				binaryReader.Dispose();
				return;
			}

			if (Globals.Platform == PlatformMode.PS4)
			{
				// PS4 DAG: magic(4) + decompressed_size(4) + unknown(4) + zlib_data
				// The zlib stream starts at offset 12 (after 4 extra bytes)
				int expectedSize = binaryReader.ReadInt32();
				binaryReader.ReadInt32(); // skip 4 unknown bytes before zlib
				byte[] compressedData = binaryReader.ReadBytes(
					(int)(binaryReader.BaseStream.Length - binaryReader.BaseStream.Position));
				binaryReader.Close();
				binaryReader.Dispose();

				byte[] bytes;
				try
				{
					bytes = ZlibStream.UncompressBuffer(compressedData);
				}
				catch
				{
					// Fallback: streaming decompression
					using (var ms = new MemoryStream(compressedData))
					using (var zs = new ZlibStream(ms, Ionic.Zlib.CompressionMode.Decompress))
					using (var output = new MemoryStream())
					{
						zs.CopyTo(output);
						bytes = output.ToArray();
					}
				}

				File.WriteAllBytes(fileName, bytes);
				this.decompressedFileName = fileName;
			}
			else
			{
				// PC DAG: magic(4) + decompressed_size(4) + skip(4) + zlib_data
				int count = binaryReader.ReadInt32();
				binaryReader.ReadInt32(); // skip 4 bytes
				byte[] bytes = ZlibStream.UncompressBuffer(binaryReader.ReadBytes(count));
				File.WriteAllBytes(fileName, bytes);
				this.decompressedFileName = fileName;
				binaryReader.Close();
				binaryReader.Dispose();
			}
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003984 File Offset: 0x00001B84
		private ulong StringToHash(string text)
		{
			Process process = new Process();
			process.StartInfo.FileName = "SMPS4HashTool.exe";
			process.StartInfo.Arguments = "\"" + text + "\"";
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.CreateNoWindow = true;
			process.StartInfo.RedirectStandardOutput = true;
			process.Start();
			ulong hash = 0UL;
			process.OutputDataReceived += delegate(object sender2, DataReceivedEventArgs e2)
			{
				string data = e2.Data;
				bool flag = string.IsNullOrWhiteSpace(data);
				if (!flag)
				{
					bool flag2 = data.Contains(" ");
					if (!flag2)
					{
						hash = ulong.Parse(data);
					}
				}
			};
			process.BeginOutputReadLine();
			process.WaitForExit();
			return hash;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00003A2C File Offset: 0x00001C2C
		public void ParseDecompressedDAG()
		{
			bool flag = !File.Exists(this.decompressedFileName);
			if (!flag)
			{
				this.Dependencies.Clear();
				BinaryReader binaryReader = new BinaryReader(File.OpenRead(this.decompressedFileName));
				binaryReader.BaseStream.Position = 102L;
				long position = binaryReader.BaseStream.Position;
				for (;;)
				{
					bool flag2 = binaryReader.ReadByte() == 0;
					if (flag2)
					{
						long num = binaryReader.BaseStream.Position - 1L;
						binaryReader.BaseStream.Position = position;
						byte[] bytes = binaryReader.ReadBytes((int)(num - position));
						string @string = Encoding.ASCII.GetString(bytes);
						Console.WriteLine(@string);
						this.Dependencies.Add(@string);
						binaryReader.BaseStream.Position += 1L;
						position = binaryReader.BaseStream.Position;
						bool flag3 = binaryReader.ReadByte() == 0;
						if (flag3)
						{
							break;
						}
					}
				}
				foreach (string text in this.Dependencies)
				{
					MessageBox.Show(this.StringToHash(text).ToString("X2"));
				}
				File.WriteAllLines(Globals.TemporaryDirectory + "DependencyDag.txt", this.Dependencies);
				binaryReader.Close();
				binaryReader.Dispose();
			}
		}

		// Token: 0x0400001E RID: 30
		public string Filename;

		// Token: 0x0400001F RID: 31
		public List<string> Dependencies = new List<string>();

		// Token: 0x04000020 RID: 32
		private const uint stringStartOffset = 102U;

		// Token: 0x04000021 RID: 33
		// PC DAG magic: 0x891FAF77 (bytes: 77 AF 1F 89)
		private const uint headerMagicPC = 0x891FAF77U;
		// PS4 DAG magic: 0x891F77AF (bytes: AF 77 1F 89) - confirmed from actual PS4 file
		private const uint headerMagicPS4 = 0x891F77AFU;

		// Token: 0x04000022 RID: 34
		private string decompressedFileName;
	}
}
