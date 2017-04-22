using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using Ddd.Infrastructure;

namespace Ddd.Taxi.Domain
{
	public class TaxiApi : ITaxiApi<TaxiOrder>
	{
		private int idCounter;
		private readonly Func<DateTime> currentTime;

		public TaxiApi(Func<DateTime> currentTime)
		{
			this.currentTime = currentTime;
		}

		public TaxiOrder CreateOrderWithoutDestination(string firstName, string lastName, string street, string building)
		{
		    return
		        new TaxiOrder(idCounter++, firstName, lastName, street, building, currentTime());
		    //{
		    //	Id = idCounter++,
		    //                ClientName = new PersonName(clientFirstName, clientLastName),
		    //                Start = new Address(street, building),
		    //	CreationCreationTime = currentTime()
		    //};
		}

		public void UpdateDestination(TaxiOrder order, string street, string building)
		{
            order.UpdateDestination(street, building);
		}

		public void AssignDriver(TaxiOrder order, int driverId)
		{
            order.AssignDriver(driverId, currentTime());
		}

		public void UnassignDriver(TaxiOrder order)
		{
		    order.UnassignDriver();
		}

		public string GetDriverFullInfo(TaxiOrder order)
		{
			if (order.Status == TaxiOrderStatus.WaitingForDriver) return null;
			return string.Join(" ",
				"Id: " + order.Driver.Id,
				"DriverName: " + FormatName(order.Driver),
				"Color: " + order.Car.Color,
				"CarModel: " + order.Car.Model,
				"PlateNumber: " + order.Car.PlateNumber);
		}

		public string GetShortOrderInfo(TaxiOrder order)
		{
			return string.Join(" ",
				"OrderId: " + order.Id,
				"Status: " + order.Status,
				"Client: " + FormatName(order.ClientName),
				"Driver: " + FormatName(order.Driver),
				"From: " + FormatAddress(order.Start),
				"To: " + FormatAddress(order.Destination),
				"LastProgressTime: " + GetLastProgressTime(order).ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		}

		private DateTime GetLastProgressTime(TaxiOrder order)
		{
			if (order.Status == TaxiOrderStatus.WaitingForDriver) return order.CreationCreationTime;
			if (order.Status == TaxiOrderStatus.WaitingCarArrival) return order.DriverAssignmentTime;
			if (order.Status == TaxiOrderStatus.InProgress) return order.StartRideTime;
			if (order.Status == TaxiOrderStatus.Finished) return order.FinishRideTime;
			if (order.Status == TaxiOrderStatus.Canceled) return order.CancelTime;
			throw new NotSupportedException(order.Status.ToString());
		}

		private string FormatName(PersonName person)
		{
		    return person == null ? "" : string.Join(" ", person.FirstName, person.LastName);
		}

	    private string FormatName(Driver driver)
	    {
	        return driver == null ? "" : string.Join(" ", driver.Name.FirstName, driver.Name.LastName);
	    }

		private string FormatAddress(Address address)
		{
		    return address == null ? "" : string.Join(" ", address.Street, address.Building);
		}

		public void Cancel(TaxiOrder order)
		{
		    order.Cancel(currentTime());
		}

		public void StartRide(TaxiOrder order)
		{
			order.StartRide(currentTime());
		}

		public void FinishRide(TaxiOrder order)
		{
			order.FinishRide(currentTime());
		}
	}

	public class TaxiOrder : Entity<int>
	{
		//public int Id { get; }
	    public PersonName ClientName { get; private set; }
	    public Address Start { get; private set; }
	    public Address Destination { get; private set; }
	    public Driver Driver { get; private set; }
        public Car Car { get; private set; }
		public TaxiOrderStatus Status { get; private set; }
	    public DateTime CreationCreationTime { get; }
	    public DateTime DriverAssignmentTime { get; private set; }
		public DateTime CancelTime { get; private set; }
		public DateTime StartRideTime { get; private set; }
		public DateTime FinishRideTime { get; private set; }

	    public TaxiOrder(int id, 
            string clientFirstName, 
            string clientLastName, 
            string street, 
            string building, 
            DateTime creationTime) 
            : base(id)
	    {
	        ClientName = new PersonName(clientFirstName, clientLastName);
            Start = new Address(street, building);
	        CreationCreationTime = creationTime;
	    }

	    public void UpdateDestination(string street, string building)
	    {
            if(Status == TaxiOrderStatus.Canceled || Status == TaxiOrderStatus.Finished)
                throw new InvalidOperationException();
	        Destination = new Address(street, building);
	    }

	    public void AssignDriver(int driverId, DateTime time)
	    {
            if(Status != TaxiOrderStatus.WaitingForDriver) throw new InvalidOperationException();
	        if (driverId == 15)
	        {
	            Driver = new Driver(driverId, "Drive", "Driverson");
	            Car = new Car("Baklazhan", "Lada sedan", "A123BT 66");
	        }
	        else
	            throw new Exception("Unknown driver id " + driverId);
	        DriverAssignmentTime = time;
	        Status = TaxiOrderStatus.WaitingCarArrival;
        }

	    public void UnassignDriver()
	    {
            if(Status != TaxiOrderStatus.WaitingCarArrival)
                throw new InvalidOperationException();
	        Driver = null;
	        Car = null;
	        Status = TaxiOrderStatus.WaitingForDriver;
        }

	    public void Cancel(DateTime time)
	    {
            if(Status == TaxiOrderStatus.InProgress) throw new InvalidOperationException();
	        Status = TaxiOrderStatus.Canceled;
	        CancelTime = time;
	    }

	    public void StartRide(DateTime time)
	    {
            if(Status != TaxiOrderStatus.WaitingCarArrival) throw new InvalidOperationException();
	        Status = TaxiOrderStatus.InProgress;
	        StartRideTime = time;
	    }

	    public void FinishRide(DateTime time)
	    {
            if(Status != TaxiOrderStatus.InProgress) throw new InvalidOperationException();
	        Status = TaxiOrderStatus.Finished;
	        FinishRideTime = time;
	    }
    }

    public class Driver : Entity<int>
    {
        public Driver(int id, string firstName, string lastName) : base(id)
        {
            Name = new PersonName(firstName, lastName);
        }

        public PersonName Name { get; }
    }

    public class Car : ValueType<Car>
    {
        public Car(string color, string model, string plateNumber)
        {
            Color = color;
            Model = model;
            PlateNumber = plateNumber;
        }


        public string Color { get; }
        public string Model { get; }
        public string PlateNumber { get; }
    }

}