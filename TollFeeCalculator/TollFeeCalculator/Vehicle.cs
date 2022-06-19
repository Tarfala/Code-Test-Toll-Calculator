using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TollFeeCalculator
{
    public class Vehicle
    {
        public VehicleType VehicleType { get; }
        public Vehicle(VehicleType vehicleType)
        {
            VehicleType = vehicleType;
        }

        private readonly List<VehicleType> TollFreeVehicles = new List<VehicleType>
        {
            VehicleType.Motorbike,
            VehicleType.Tractor,
            VehicleType.Emergency,
            VehicleType.Diplomat,
            VehicleType.Foreign,
            VehicleType.Military
        };
        public bool VehicleIsTollFree
        {
            get
            {
                return TollFreeVehicles.Contains(this.VehicleType);
            }
        }        
    }
    public enum VehicleType
    {
        Car,
        Motorbike,
        Tractor,
        Emergency,
        Diplomat,
        Foreign,
        Military
    }
}