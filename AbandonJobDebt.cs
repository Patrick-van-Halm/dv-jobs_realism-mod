using DV.Logic.Job;
using DV.ServicePenalty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVJobsRealism
{
    class AbandonJobDebt : DisplayableDebt
    {
        public const DebtType AbandonedDebt = (DebtType)101;

        Job job;
        public AbandonJobDebt(Job job)
        {
            this.job = job;
        }

        public override string ID { 
            get {
                if (job != null)
                    return job.ID;
                else
                    return "Unknown";
            }
        }

        public override CarDebtData[] GetCarDebts()
        {
            return new CarDebtData[] { };
        }

        public override DebtType GetDebtType()
        {
            return AbandonedDebt;
        }

        public override float GetTotalPrice()
        {
            return job.GetBasePaymentForTheJob() / 2;
        }

        public override void Pay()
        {
            base.Pay();
            SingletonBehaviour<CareerManagerDebtController>.Instance.UnregisterDebt(this);
        }
    }
}
