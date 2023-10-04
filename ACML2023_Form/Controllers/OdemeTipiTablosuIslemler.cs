using Model;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using VeritabaniIslemMerkeziBase;

namespace VeritabaniIslemMerkezi
{
	public partial class OdemeTipiTablosuIslemler : OdemeTipiTablosuIslemlerBase
	{
		public OdemeTipiTablosuIslemler() : base() { }

		public OdemeTipiTablosuIslemler(OleDbTransaction tran) : base (tran) { }
	}
}
