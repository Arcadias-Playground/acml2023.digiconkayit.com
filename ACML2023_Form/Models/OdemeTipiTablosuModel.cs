using Newtonsoft.Json;
using System;
using ModelRelation;

namespace Model
{
	public partial class OdemeTipiTablosuModel : OdemeTipiTablosuModelRelation
	{

		public virtual string FullJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}