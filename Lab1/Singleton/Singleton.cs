using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.Singleton
{
    public sealed class Singleton
    {
        private Singleton() { }

        private static List<VehicleBuilder> VehicleTypes = new List<VehicleBuilder>()
        {
        };

        private static readonly object testlock = new object ();  
        private static Singleton instance = null;
        public static Singleton Instance
        {
            get
            {
                lock (testlock)
                    {
                        if (instance == null)
                        {
                            instance = new Singleton();
                        }
                        return instance;
                    }
            }
        }

        public void Clear()
        {
            VehicleTypes.Clear();
        }

        public void AddModel(object itm)
        {
            VehicleTypes.Add(itm as VehicleBuilder);
        }

        public object GetVehicle(int index)
        {
            return VehicleTypes[index];
        }

        public string[] InitList()
        {
            string[] res = new string[VehicleTypes.Count()];
            for (int i = 0; i < VehicleTypes.Count(); i++)
            {
                res[i] = VehicleTypes[i].Vehicle.GetModel();
            }
            return res;
        }
    }
}
