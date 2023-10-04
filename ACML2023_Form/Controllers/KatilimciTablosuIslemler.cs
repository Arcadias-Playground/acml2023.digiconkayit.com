using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
	public partial class KatilimciTablosuIslemler : KatilimciTablosuIslemlerBase
	{
		public KatilimciTablosuIslemler() : base() { }

		public KatilimciTablosuIslemler(OleDbTransaction tran) : base (tran) { }
	}
}
