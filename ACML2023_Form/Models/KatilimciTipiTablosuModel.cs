using Newtonsoft.Json;
using System;
using ModelRelation;

namespace Model
{
	public partial class KatilimciTipiTablosuModel : KatilimciTipiTablosuModelRelation
	{
		public decimal FormUcret { get { return Convert.ToDateTime("13.10.2023 23:59:59") > DateTime.Now ? ErkenUcret : NormalUcret; } }


		public virtual string FullJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}