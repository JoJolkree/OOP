using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation
{

    public class Common
    {
        public static int Earlier(DateTime v, int day, int month, int year)
        {
            if (v.Year < year) return 1;
            if (v.Year > year) return 0;
            if (v.Month < month) return 1;
            if (v.Month > month) return 0;
            if (v.Day < day) return 1;
            return 0;
        }
    }

    public class ReportMaker
    {
        /// <summary>
        /// </summary>
        /// <param name="day"></param>
        /// <param name="failureTypes">0 for unexpected shutdown, 1 for short non-responding, 2 for hardware failures,
        /// 3 for connection problems</param>
        /// <param name="deviceId"></param>
        /// <param name="times"></param>
        /// <param name="devices"></param>
        /// <returns></returns>
        public static List<string> FindDevicesFailedBeforeDateObsolete(
            int day,
            int month,
            int year,
            int[] failureTypes, 
            int[] deviceId, 
            object[][] times,
            List<Dictionary<string, object>> devices)
        {
            var date = new DateTime(year, month, day);
            var newFailures = failureTypes.Select(x => new Failure((FailureType) x)).ToArray();
            var newDevices = devices.Select(x => new Device((int)x["DeviceId"], x["Name"] as string)).ToList();
            var newTimes = times.Select(x => new DateTime((int) x[2], (int) x[1], (int) x[0])).ToArray();
            return NewFindDevicesFailedBeforeDateObsolete(date, newFailures, deviceId, newTimes, newDevices);
        }

        public static List<string> NewFindDevicesFailedBeforeDateObsolete(DateTime date,
            Failure[] failures,
            int[] deviceId,
            DateTime[] times,
            List<Device> devices)
        {
            var problematicDevices = new HashSet<int>();
            for (var i = 0; i < failures.Length; i++)
                if (failures[i].IsSerious && Common.Earlier(times[i], date.Day, date.Month, date.Year)==1)
                    problematicDevices.Add(deviceId[i]);

            var result = new List<string>();
            foreach (var device in devices)
                if (problematicDevices.Contains(device.DeviceId))
                    result.Add(device.Name);

            return result;
        }
    }

    public class Device
    {
        public int DeviceId { get; }
        public string Name { get; }

        public Device(int deviceId, string name)
        {
            DeviceId = deviceId;
            Name = name;
        }
    }

    public enum FailureType
    {
        UnexpectedShutdown = 0,
        ShortNonResponding,
        HardwareFailure,
        ConnectionProblem
    }

    public class Failure
    {
        public FailureType Type { get; }
        public bool IsSerious { get; }

        public Failure(FailureType type)
        {
            Type = type;
            IsSerious = (int) Type % 2 == 0;
        }
    }
}
