using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using ModelBase;
using Model;

namespace ModelRelation
{
	public abstract class KatilimciTablosuModelRelation : KatilimciTablosuModelBase
	{
		public virtual OdemeTablosuModel OdemeBilgisi { get; set; }
		public virtual KatilimciTipiTablosuModel KatilimciTipiBilgisi { get; set; }
		public virtual UlkeTablosuModel UlkeBilgisi { get; set; }

		public virtual string RelationJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}
	}
}