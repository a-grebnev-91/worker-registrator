using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRegistrator_v4.Model
{
	class MainModel
	{
		public Employee EmployeeToEdit { get; set; }
		public int TotalCount { get { return EmpInBuilding.Count + EmpDeparted.Count; } }
		public int CountInBuilding { get { return EmpInBuilding.Count; } }
		public List<Employee> Employees { get; set; }
		public List<Employee> Drivers { get; set; }
		public List<Employee> EmpInBuilding { get; set; }
		public List<Employee> EmpDeparted { get; set; }
		public DateTime CurrentDate { get; set; }
		public int IndexOfSort { get; set; }

		private ArrivalTimeComparer _arrivalComparer = new ArrivalTimeComparer();
		private DepartureTimeComparer _departureComparer = new DepartureTimeComparer();

		public MainModel()
		{
			Loader loader = new Loader();
			Employees = loader.Employees;
			Drivers = loader.Drivers;
			EmpInBuilding = loader.EmpInBuilding;
			EmpDeparted = loader.EmpDeparted;
			CurrentDate = loader.CurrentDate;
			IndexOfSort = 0;
		}
		public void EmpArrived(Employee emp)
		{
			Employees.Remove(emp);
			emp.ArrivalTime = DateTime.Now;
			EmpInBuilding.Add(emp);
			EmpInBuilding.Sort(_arrivalComparer);
		}

		public void DriverArrived(Employee driver)
		{
			Drivers.Remove(driver);
			driver.ArrivalTime = DateTime.Now;
			EmpInBuilding.Add(driver);
			EmpInBuilding.Sort(_arrivalComparer);
		}

		public void EmpDepart(Employee emp)
		{
			EmpInBuilding.Remove(emp);
			emp.DepartureTime = DateTime.Now;
			EmpDeparted.Add(emp);
			SortDeparture();
		}

		public void Save()
		{
			Saver saver = new Saver();
			Day day = new Day(MakeListToSave(), CurrentDate);
			saver.Save(day);
		}

		internal void DelFromDeparted(Employee emp)
		{
			EmpDeparted.Remove(emp);
			emp.DepartureTime = new DateTime();
			EmpInBuilding.Add(emp);
			EmpInBuilding.Sort(_arrivalComparer);
			Save();
		}

		internal void DelFromArrived(Employee emp)
		{
			EmpInBuilding.Remove(emp);
			emp.ArrivalTime = new DateTime();
			if (emp.IsDriver)
			{
				Drivers.Add(emp);
				Drivers.Sort();
			}
			else
			{
				Employees.Add(emp);
				Employees.Sort();
			}
			Save();
		}

		public void CreateTodayReport()
		{
			ReportCreator creator = new ReportCreator();
			creator.CreateReport(MakeListToSave(), CurrentDate);
		}

		public void EditArrivalTime(TimeSpan time)
		{
			EmployeeToEdit.ArrivalTime = EmployeeToEdit.ArrivalTime.Date + time;
			EmpInBuilding.Sort(_arrivalComparer);
			SortDeparture();
			Save();
		}

		public void EditDepartureTime(TimeSpan time)
		{
			EmployeeToEdit.DepartureTime = EmployeeToEdit.DepartureTime.Date + time;
			SortDeparture();
			Save();
		}

		private List<Employee> MakeListToSave()
		{
			List<Employee> employeesToSave = new List<Employee>();
			foreach (Employee emp in EmpInBuilding)
				employeesToSave.Add(emp);
			foreach (Employee emp in EmpDeparted)
				employeesToSave.Add(emp);
			employeesToSave.Sort();
			return employeesToSave;
		}

		internal void SortDeparture()
		{
			switch (IndexOfSort)
			{
				case 0:
					EmpDeparted.Sort(_departureComparer);
					break;
				case 1:
					EmpDeparted.Sort(_arrivalComparer);
					break;
				case 2:
					EmpDeparted.Sort();
					break;
			}
		}
	}
}
