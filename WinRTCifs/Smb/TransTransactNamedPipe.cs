// This code is derived from jcifs smb client library <jcifs at samba dot org>
// Ported by J. Arturo <webmaster at komodosoft dot net>
//  
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;

namespace WinrtCifs.Smb
{
	internal class TransTransactNamedPipe : SmbComTransaction
	{
		private byte[] _pipeData;

		private int _pipeFid;

		private int _pipeDataOff;

		private int _pipeDataLen;

		internal TransTransactNamedPipe(int fid, byte[] data, int off, int len)
		{
			_pipeFid = fid;
			_pipeData = data;
			_pipeDataOff = off;
			_pipeDataLen = len;
			Command = SmbComTransaction;
			SubCommand = TransTransactNamedPipe;
			MaxParameterCount = 0;
			MaxDataCount = unchecked(0xFFFF);
			MaxSetupCount = unchecked(unchecked(0x00));
			SetupCount = 2;
			Name = "\\PIPE\\";
		}

		internal override int WriteSetupWireFormat(byte[] dst, int dstIndex)
		{
			dst[dstIndex++] = SubCommand;
			dst[dstIndex++] = unchecked(unchecked(0x00));
			WriteInt2(_pipeFid, dst, dstIndex);
			dstIndex += 2;
			return 4;
		}

		internal override int ReadSetupWireFormat(byte[] buffer, int bufferIndex, int len
			)
		{
			return 0;
		}

		internal override int WriteParametersWireFormat(byte[] dst, int dstIndex)
		{
			return 0;
		}

		internal override int WriteDataWireFormat(byte[] dst, int dstIndex)
		{
			if ((dst.Length - dstIndex) < _pipeDataLen)
			{
				if (Log.Level >= 3)
				{
					Log.WriteLine("TransTransactNamedPipe data too long for buffer");
				}
				return 0;
			}
			Array.Copy(_pipeData, _pipeDataOff, dst, dstIndex, _pipeDataLen);
			return _pipeDataLen;
		}

		internal override int ReadParametersWireFormat(byte[] buffer, int bufferIndex, int
			 len)
		{
			return 0;
		}

		internal override int ReadDataWireFormat(byte[] buffer, int bufferIndex, int len)
		{
			return 0;
		}

		public override string ToString()
		{
			return "TransTransactNamedPipe[" + base.ToString() + ",pipeFid=" + _pipeFid
				 + "]";
		}
	}
}
