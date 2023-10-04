using Newtonsoft.Json;
using System;
using ModelRelation;

namespace Model
{
	public partial class UlkeTablosuModel : UlkeTablosuModelRelation
	{

		public virtual string FullJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}