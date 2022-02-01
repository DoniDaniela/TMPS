using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private ProxyVehicle SelectedItem;

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            Singleton.Singleton.Instance.Clear();
            VehicleBuilder builder;
            // Create shop with vehicle builders
            Factory facory = new Factory();

            builder = new ScooterBuilder();
            facory.Construct(builder);
            Singleton.Singleton.Instance.AddModel(builder);

            builder = new CarBuilder();
            facory.Construct(builder);
            Singleton.Singleton.Instance.AddModel(builder);

            builder = new MotorCycleBuilder();
            facory.Construct(builder);
            Singleton.Singleton.Instance.AddModel(builder);

            string[] list = Singleton.Singleton.Instance.InitList();
            for (int i = 0; i < list.Length; i++)
            {
                listBox1.Items.Add(list[i].ToString());
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                MessageBox.Show("Select vehicle!");
            }
            else
            {
                SelectedItem = Singleton.Singleton.Instance.GetVehicle(listBox1.SelectedIndex) as ProxyVehicle;

                var newItem = SelectedItem.Clone(int.Parse(edtAge.Text));
                if (newItem != null)
                {
                    string ss = newItem.Vehicle.GetModel();

                    listBox2.Items.Clear();
                    listBox2.Items.Add(ss);
                }
                else MessageBox.Show("You are less then 18 years old!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (SelectedItem != null)
            {
                VehicleBuilder itm = SelectedItem as VehicleBuilder;

                DecoratorEngine elm = new DecoratorEngine(SelectedItem as VehicleBuilder);

                if (checkBox1.Checked)
                    elm.BuildEngine(new ForcedEngine());
                else elm.BuildEngine(new EchoEngine());

                string ss = elm.Clone().Vehicle.GetModel();

                listBox3.Items.Clear();
                listBox3.Items.Add(ss);
            }
        }
    }

    class Factory
    {
        // Builder uses a complex series of steps
        public void Construct(VehicleBuilder vehicleBuilder)
        {
            vehicleBuilder.BuildFrame();
            vehicleBuilder.BuildEngine();
            vehicleBuilder.BuildWheels();
            vehicleBuilder.BuildDoors();
        }
    }

    abstract class VehicleBuilder
    {
        public Vehicle vehicle;
        // Gets vehicle instance
        public Vehicle Vehicle
        {
            get { return vehicle; }
        }
        // Abstract build methods
        public abstract void BuildFrame();
        public abstract void BuildEngine();
        public abstract void BuildWheels();
        public abstract void BuildDoors();

        public abstract VehicleBuilder Clone();
    }

    class ProxyVehicle : VehicleBuilder
    {
        public override void BuildFrame()
        {
        }
        public override void BuildEngine()
        {
        }
        public override void BuildWheels()
        {
        }
        public override void BuildDoors()
        {
        }
        public override VehicleBuilder Clone()
        {
            return (VehicleBuilder)this.MemberwiseClone();
        }
        public VehicleBuilder Clone(int age)
        {
            if (age > 17)
            {
                return (VehicleBuilder)this.MemberwiseClone();
            }
            else return null;
        }
    }

    class MotorCycleBuilder : ProxyVehicle
    {
        public MotorCycleBuilder()
        {
            vehicle = new Vehicle("MotorCycle");
        }
        public override void BuildFrame()
        {
            vehicle["frame"] = "MotorCycle Frame";
        }
        public override void BuildEngine()
        {
            vehicle["engine"] = "500 cc";
        }
        public override void BuildWheels()
        {
            vehicle["wheels"] = "2";
        }
        public override void BuildDoors()
        {
            vehicle["doors"] = "0";
        }

    }
    /// <summary>
    /// The 'ConcreteBuilder2' class
    /// </summary>
    class CarBuilder : ProxyVehicle
    {
        public CarBuilder()
        {
            vehicle = new Vehicle("Car");
        }
        public override void BuildFrame()
        {
            vehicle["frame"] = "Car Frame";
        }
        public override void BuildEngine()
        {
            vehicle["engine"] = "2500 cc";
        }
        public override void BuildWheels()
        {
            vehicle["wheels"] = "4";
        }
        public override void BuildDoors()
        {
            vehicle["doors"] = "4";
        }

    }
    /// <summary>
    /// The 'ConcreteBuilder3' class
    /// </summary>
    class ScooterBuilder : ProxyVehicle
    {
        public ScooterBuilder()
        {
            vehicle = new Vehicle("Scooter");
        }
        public override void BuildFrame()
        {
            vehicle["frame"] = "Scooter Frame";
        }
        public override void BuildEngine()
        {
            vehicle["engine"] = "50 cc";
        }
        public override void BuildWheels()
        {
            vehicle["wheels"] = "2";
        }
        public override void BuildDoors()
        {
            vehicle["doors"] = "0";
        }

    }
    /// <summary>
    /// The 'Product' class
    /// </summary>
    class Vehicle
    {
        private string _vehicleType;
        private Dictionary<string, string> _parts =
          new Dictionary<string, string>();
        // Constructor
        public Vehicle(string vehicleType)
        {
            this._vehicleType = vehicleType;
        }
        // Indexer
        public string this[string key]
        {
            get { return _parts[key]; }
            set { _parts[key] = value; }
        }

        public string GetModel()
        {
            return $"Vehicle Type: {_vehicleType},\n Frame : {_parts["frame"]}, Engine : { _parts["engine"]}, #Wheels: {_parts["wheels"]}, #Doors : {_parts["doors"]}";
        }
    }

    class DecoratorEngine 
    {
        protected VehicleBuilder vehicleBuilder;

        public EngineType engineType;

        public DecoratorEngine(VehicleBuilder vehicleBuilder)
        {
            this.vehicleBuilder = vehicleBuilder;
        }

        public void BuildEngine(EngineType type)
        {
            engineType = type;
           
        }

        public VehicleBuilder Clone()
        {
            if (vehicleBuilder != null)
            {
                if (engineType.GetType().Name == "ForcedEngine")
                {
                    vehicleBuilder.vehicle["engine"] = vehicleBuilder.vehicle["engine"] + " forced";
                }
                else
                    if (engineType.GetType().Name == "EchoEngine")
                        vehicleBuilder.vehicle["engine"] = vehicleBuilder.vehicle["engine"] + " echo";

                return vehicleBuilder;
            }
            else return null;
        }

    }

    //state

    abstract class EngineType
    {
        public abstract void Handle(DecoratorEngine context);
    }
    /// <summary>
    /// A 'ConcreteState' class
    /// </summary>
    class ForcedEngine : EngineType
    {
        public override void Handle(DecoratorEngine context)
        {
            context.engineType = new ForcedEngine();
        }
    }
    /// <summary>
    /// A 'ConcreteState' class
    /// </summary>
    class EchoEngine : EngineType
    {
        public override void Handle(DecoratorEngine context)
        {
            context.engineType = new EchoEngine();
        }
    }

}
