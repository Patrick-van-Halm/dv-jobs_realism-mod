using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace DVJobsRealism
{
	public class Settings : UnityModManager.ModSettings, IDrawable
	{
		[Draw(Label = "Bonus Payment")]
		public bool bonusPaymentEnabled = true;

		override public void Save(UnityModManager.ModEntry entry)
		{
			Save<Settings>(this, entry);
		}

		public void OnChange() { }
	}
}
