using Newtonsoft.Json;
using System;
using ModelRelation;

namespace Model
{
	public partial class KatilimciTipiTablosuModel : KatilimciTipiTablosuModelRelation
	{
		public decimal FormUcret { get { return Convert.ToDateTime("19.10.2023 15:00:00") > DateTime.Now ? ErkenUcret : NormalUcret; } }

		public virtual string FullJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}