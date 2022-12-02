using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Runtime.Serialization;

namespace WorkerRegistrator_v4.Model
{
	class Loader
	{
		private string _sourceFile = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["sourceFile"];
		private string _driversSourceFile = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["driversSourceFile"];
		private string _savePath = ConfigurationManager.AppSettings["savePath"];
		private ArrivalTimeComparer _arrivalComparer = new ArrivalTimeComparer();
		private DepartureTimeComparer _deparutreComparer = new DepartureTimeComparer();

		public DateTime CurrentDate { get; private set; }
		public List<Employee> Employees { get; private set; }
		public List<Employee> Drivers { get; private set; }
		public List<Employee> EmpInBuilding { get; private set; }
		public List<Employee> EmpDeparted { get; private set; }

		public Loader()
		{
			Employees = new List<Employee>();
			Drivers = new List<Employee>();
			EmpInBuilding = new List<Employee>();
			EmpDeparted = new List<Employee>();
			LoadEmployees();
			LoadDrivers();
			CurrentDate = DateTime.Now;
			if (IsTodaySaveFileExists())
				LoadArrivedAndDepEmpl();
		}

		private void LoadArrivedAndDepEmpl()
		{
			using (Stream input = File.OpenRead(GetCurrentSaveFileName()))
			{
				DataContractSerializer ser = new DataContractSerializer(typeof(Day));
				Day day = ser.ReadObject(input) as Day;
				List<Employee> employees = day.Employees;
				foreach (Employee emp in employees)
				{
					if (emp.DepartureTime == new DateTime())
						EmpInBuilding.Add(emp);
					else
						EmpDeparted.Add(emp);
				}
			}
			if (EmpDeparted.Count > 0)
			{
				foreach (Employee emp in EmpDeparted)
					if (emp.IsDriver)
						Drivers.Remove(emp);
					else
						Employees.Remove(emp);
				EmpDeparted.Sort(_deparutreComparer);
			}
			if (EmpInBuilding.Count > 0)
			{
				foreach (Employee emp in EmpInBuilding)
					if (emp.IsDriver)
						Drivers.Remove(emp);
					else
						Employees.Remove(emp);
				EmpInBuilding.Sort(_arrivalComparer);
			}
		}

		// проверяет, есть ли ранее сохраненный на данную дату файл
		private bool IsTodaySaveFileExists()
		{
			return File.Exists(GetCurrentSaveFileName());
		}

		private string GetCurrentSaveFileName()
		{
			return _savePath + @"\" + CurrentDate.Year + "_" + CurrentDate.ToString("MM") + "_" + CurrentDate.ToString("dd") + ".xml";
		}

		private void LoadEmployees()
		{
			using (StreamReader reader = File.OpenText(_sourceFile))
			{
				string name;
				string position;
				string department;
				string readLine;
				while (!String.IsNullOrEmpty(readLine = reader.ReadLine()))
				{
					name = readLine;
					position = reader.ReadLine();
					department = reader.ReadLine();
					readLine = reader.ReadLine();
					Employee employee = new Employee(name, position.Trim(), department.Trim(), false);
					Employees.Add(employee);
				}
			}
		}

		private void LoadDrivers()
		{
			using (StreamReader reader = File.OpenText(_driversSourceFile))
			{
				string name;
				string position = "Водитель";
				string department = "Транспортный отдел";
				while (!String.IsNullOrEmpty(name = reader.ReadLine()))
				{
					Employee employee = new Employee(name, position.Trim(), department.Trim(), true);
					Drivers.Add(employee);
				}
			}
		}


	}
}
