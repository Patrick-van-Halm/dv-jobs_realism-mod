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

		[Draw(Label = "Max generated jobs")]
		public int jobsCapacity = 30;

		[Draw(Label = "Max pickups/dropoffs amount")]
		public int maxShuntingStorageTracks = 3;

		[Draw(Label = "Min cars per job (make sure you have the licenses when changing this)")]
		public int minCarsPerJob = 3;

		[Draw(Label = "Max cars per job")]
		public int maxCarsPerJob = 20;


		override public void Save(UnityModManager.ModEntry entry)
		{
			Save<Settings>(this, entry);
		}

		public void OnChange() { }
	}
}
