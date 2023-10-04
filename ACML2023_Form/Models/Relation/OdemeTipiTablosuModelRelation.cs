using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class OdemeTipiTablosuModelRelation : OdemeTipiTablosuModelBase
	{
		public virtual IList<OdemeTablosuModel> OdemeBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}