using Newtonsoft.Json;
using System;
using ModelRelation;

namespace Model
{
	public partial class OdemeTablosuModel : OdemeTablosuModelRelation
	{

		public virtual string FullJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}