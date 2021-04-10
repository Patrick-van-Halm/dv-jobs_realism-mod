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
        
        internal static bool AreCarsInOrderAndCoupledTogetherForTransportTask(TransportTask task)
        {
#if DEBUG
            Debug.Log($"[{task.Job.ID}] Received check for task containing {task.cars.Count} cars destination track is {task.destinationTrack.ID.FullDisplayID}");
#endif
            SingletonBehaviour<IdGenerator>.Instance.logicCarToTrainCar.TryGetValue(task.cars[0], out TrainCar trainCar);
            if (!trainCar)
            {
                Debug.LogError("Can't find corresponding TrainCar for car[" + task.cars[0].ID + "]");
                return true;
            }
            var trainsetWithoutLocos = trainCar.trainset.cars.Where(c => !c.IsLoco).ToArray();

#if DEBUG
            Debug.Log($"[{task.Job.ID}] Checking coupled state");
#endif
            foreach (Car car in task.cars)
            {
                if(!trainsetWithoutLocos.Any(c => c.CarGUID == car.carGuid))
                {
#if DEBUG
                    Debug.Log($"[{task.Job.ID}] Cars are not coupled as booklet");
#endif
                    return false;
                }
            }
#if DEBUG
            Debug.Log($"[{task.Job.ID}] Coupled state should be correct");
#endif

            for (int i = 0; i < trainsetWithoutLocos.Count(); i++)
            {
                if(trainsetWithoutLocos[i].CarGUID == task.cars[0].carGuid)
                {
                    if(task.cars.Count > 1)
                    {
                        if(i - 1 >= 0 && trainsetWithoutLocos[i - 1].CarGUID == task.cars[1].carGuid)
                        {
#if DEBUG
                            Debug.Log($"[{task.Job.ID}] Checking reversed order");
#endif
                            for (int j = i; i - j < task.cars.Count; j--)
                            {
                                var car = task.cars[i - j];
                                var carInTrainset = trainsetWithoutLocos[j];
                                if (car.carGuid != carInTrainset.CarGUID)
                                {
#if DEBUG
                                    Debug.Log($"[{task.Job.ID}] Order is incorrect");
#endif
                                    return false;
                                }
                            }
#if DEBUG
                            Debug.Log($"[{task.Job.ID}] Order is correct");
#endif
                            return true;
                        }
                        else if(i + 1 < trainsetWithoutLocos.Length && trainsetWithoutLocos[i + 1].CarGUID == task.cars[1].carGuid)
                        {
#if DEBUG
                            Debug.Log($"[{task.Job.ID}] Checking normal order");
#endif
                            for (int j = i; j - i < task.cars.Count; j++)
                            {
                                var car = task.cars[j - i];
                                var carInTrainset = trainsetWithoutLocos[j];
                                if (car.carGuid != carInTrainset.CarGUID)
                                {
#if DEBUG
                                    Debug.Log($"[{task.Job.ID}] Order is incorrect");
#endif
                                    return false;
                                }
                            }
#if DEBUG
                            Debug.Log($"[{task.Job.ID}] Order is correct");
#endif
                            return true;
                        }
#if DEBUG
                        Debug.Log($"[{task.Job.ID}] Order is incorrect");
#endif
                        return false;
                    }
#if DEBUG
                    Debug.Log($"[{task.Job.ID}] Order is correct since its only 1 car");
#endif
                    return true;
                }
            }
#if DEBUG
            Debug.Log($"[{task.Job.ID}] Skipped order check");
#endif
            return true;
        }
    }

    [HarmonyPatch(typeof(TransportTask), "UpdateTaskState")]
    internal class Task_UpdateTaskState_Patch
    {
        private static void Postfix(ref TaskState __result, TransportTask __instance)
        {
            if (!Main.enabled || __instance.state != TaskState.InProgress || __instance.Job.State != JobState.InProgress)
                return;

            if (!CarsOrderChecker.AreCarsInOrderAndCoupledTogetherForTransportTask(__instance))
            {
                __instance.SetState(TaskState.InProgress);
                __result = __instance.state;
            }
        }
    }
}
