using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homewokr12
{
    public abstract class Car
    {
        public string Model { get; }
        public int Speed { get; protected set; }
        public int Position { get; protected set; }

        public event Action<Car> Finish;

        protected Car(string model, int speed)
        {
            Model = model;
            Speed = speed;
            Position = 0;
        }

        public void Move()
        {
            Position += Speed;

            if (Position >= 100)
            {
                Position = 100;
                OnFinish();
            }
        }

        protected virtual void OnFinish()
        {
            Finish?.Invoke(this);
        }
    }

    public class SportsCar : Car
    {
        public SportsCar(string model) : base(model, 5) { }
    }

    public class PassengerCar : Car
    {
        public PassengerCar(string model) : base(model, 3) { }
    }

    public class Truck : Car
    {
        public Truck(string model) : base(model, 2) { }
    }

    public class Bus : Car
    {
        public Bus(string model) : base(model, 4) { }
    }

    public class RaceGame
    {
        public delegate void RaceHandler(string message);

        public event RaceHandler RaceInfo;

        public void StartRace()
        {
            var sportsCar = new SportsCar("SportsCar");
            var passengerCar = new PassengerCar("PassengerCar");
            var truck = new Truck("Truck");
            var bus = new Bus("Bus");

            sportsCar.Finish += HandleFinish;
            passengerCar.Finish += HandleFinish;
            truck.Finish += HandleFinish;
            bus.Finish += HandleFinish;

            while (true)
            {
                sportsCar.Move();
                passengerCar.Move();
                truck.Move();
                bus.Move();

                DisplayRaceInfo(sportsCar, passengerCar, truck, bus);

                if (sportsCar.Position == 100 || passengerCar.Position == 100 || truck.Position == 100 || bus.Position == 100)
                    break;
            }
        }

        private void DisplayRaceInfo(params Car[] cars)
        {
            foreach (var car in cars)
            {
                RaceInfo?.Invoke($"{car.Model} is at position {car.Position}");
            }

            RaceInfo?.Invoke("----------------------");
        }

        private void HandleFinish(Car car)
        {
            RaceInfo?.Invoke($"{car.Model} has finished the race!");
        }
    }

    class Program
    {
        static void Main()
        {
            var raceGame = new RaceGame();
            raceGame.RaceInfo += Console.WriteLine; 
            raceGame.StartRace();

            Console.ReadKey();
        }
    }

}
