using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
	public partial class KatilimciTipiTablosuIslemler : KatilimciTipiTablosuIslemlerBase
	{
		public KatilimciTipiTablosuIslemler() : base() { }

		public KatilimciTipiTablosuIslemler(OleDbTransaction tran) : base (tran) { }
	}
}
