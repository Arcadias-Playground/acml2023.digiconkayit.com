using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
	public partial class UlkeTablosuIslemler : UlkeTablosuIslemlerBase
	{
		public UlkeTablosuIslemler() : base() { }

		public UlkeTablosuIslemler(OleDbTransaction tran) : base (tran) { }
	}
}
