using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ModelBase
{
	[Table("OdemeTablosu")]
	public abstract class OdemeTablosuModelBase
	{
		[Key]
		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(255, ErrorMessage = "UzunlukUyari")]
		[Column("OdemeID", Order = 0)]
		public virtual string OdemeID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[ForeignKey("OdemeTipiTablosu")]
		[Column("OdemeTipiID", Order = 1)]
		public virtual int OdemeTipiID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[ForeignKey("KatilimciTablosu")]
		[Column("KatilimciID", Order = 2)]
		public virtual int KatilimciID { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[Column("Durum", Order = 3)]
		public virtual bool Durum { get; set; }

		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("OdemeTarihi", Order = 4)]
		public virtual DateTime? OdemeTarihi { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(536870910, ErrorMessage = "UzunlukUyari")]
		[Column("OdemeParametreleri", Order = 5)]
		public virtual string OdemeParametreleri { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[MaxLength(536870910, ErrorMessage = "UzunlukUyari")]
		[Column("SecureHash", Order = 6)]
		public virtual string SecureHash { get; set; }

		[Required(ErrorMessage = "BosUyari")]
		[DataType(DataType.DateTime, ErrorMessage = "GecersizUyari")]
		[Column("EklenmeTarihi", Order = 7)]
		public virtual DateTime EklenmeTarihi { get; set; }


		public static int OzellikSayisi { get { return typeof(OdemeTablosuModelBase).GetProperties().Count(x => !x.GetAccessors()[0].IsStatic); }}

		public static string SQLSutunSorgusu { get { return string.Join(", ", typeof(OdemeTablosuModelBase).GetProperties().Where(x => !x.GetAccessors()[0].IsStatic).OrderBy(x => (x.GetCustomAttributes(typeof(ColumnAttribute), true).First() as ColumnAttribute).Order).Select(x => $"[OdemeTablosu].[{x.Name}]")); }}

		public virtual string BaseJsonModel()
		{
			return JsonConvert.SerializeObject(this);
		}

	}
}