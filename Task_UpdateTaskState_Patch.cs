using DV.Logic.Job;
using DV.Printers;
using HarmonyLib;
using System.Linq;
using UnityEngine;

namespace DVJobsRealism
{
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0051 // Remove unused private members
    static class CarsOrderChecker
    {
        internal static bool AreCarsInOrderAndCoupledTogether(Task task)
        {
            if (task is TransportTask)
            {
                return AreCarsInOrderAndCoupledTogetherForTransportTask(task as TransportTask);
            }
            else return true;
        }

        private static bool AreCarsInOrderAndCoupledTogetherForTransportTask(TransportTask task)
        {
            SingletonBehaviour<IdGenerator>.Instance.logicCarToTrainCar.TryGetValue(task.cars[0], out TrainCar trainCar);
            if (!trainCar)
            {
                Debug.LogError("Can't find corresponding TrainCar for car[" + task.cars[0].ID + "]");
                return true;
            }

            if (trainCar.trainset.cars.Where(c => !c.IsLoco).Count() != task.cars.Count)
                return false;

            bool outOfOrder = false;
            for (int i = 0; i < trainCar.trainset.cars.Count; i++)
            {
                TrainCar carInSet = trainCar.trainset.cars[i];
                Car jobCar = task.cars[i];
                if (jobCar.ID != carInSet.ID)
                {
                    outOfOrder = true;
                    break;
                }
            }

            return outOfOrder;
        }
    }

    [HarmonyPatch(typeof(TransportTask), "UpdateTaskState")]
    internal class Task_UpdateTaskState_Patch
    {
        private static void Postfix(ref TaskState __result, TransportTask __instance)
        {
            if (!Main.enabled)
                return;

            if (!CarsOrderChecker.AreCarsInOrderAndCoupledTogether(__instance))
            {
                __instance.SetState(TaskState.InProgress);
                __result = __instance.state;
            }
        }
    }
}
