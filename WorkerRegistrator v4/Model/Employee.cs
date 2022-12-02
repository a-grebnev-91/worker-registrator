using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WorkerRegistrator_v4.Model
{
	[DataContract]
	class Employee : IComparable
	{
		[DataMember]
		public string LastName { get; private set; }
		[DataMember]
		public string FirstName { get; private set; }
		[DataMember]
		public string MiddleName { get; private set; }
		public string FirstNameShort
		{
			get
			{
				if (LastName != "Булыгин")
					return FirstName.Substring(0, 1) + ".";
				else
					return FirstName.Substring(0, 4) + ".";
			}
			private set { FirstNameShort = value; }
		}
		public string MiddleNameShort
		{
			get { return MiddleName.Substring(0, 1) + "."; }
			private set { MiddleNameShort = value; }
		}
		[DataMember]
		public string Position { get; private set; }
		[DataMember]
		public string Department { get; private set; }
		[DataMember]
		public bool IsDriver { get; private set; }
		public string ArrivalString { get { return ArrivalTime.ToString("HH:mm"); } }
		public bool IsSelected { get; set; }
		public string DepartureString { get { return DepartureTime.ToString("HH:mm"); } }
		[DataMember]
		public DateTime ArrivalTime { get; set; }
		[DataMember]
		public DateTime DepartureTime { get; set; }

		public Employee(string firstName, string lastName, string middleName, string position, string department)
		{
			FirstName = firstName;
			LastName = lastName;
			MiddleName = MiddleName;
			Position = position;
			Department = department;
			IsSelected = false;
		}

		public Employee(string name, string position, string department, bool isDriver)
		{
			Position = position;
			Department = department;
			name = name.Trim();
			string[] names = name.Split(new Char[] { ' ' });
			FirstName = names[1];
			LastName = names[0];
			MiddleName = names[2];
			IsDriver = isDriver;
		}

		//del me
		public override string ToString()
		{
			StringBuilder result = new StringBuilder(LastName);
			result.Append(" ");
			result.Append(FirstName);
			result.Append(" ");
			result.Append(MiddleName);
			return result.ToString();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (obj == this)
				return true;
			if (obj is Employee)
			{
				Employee empl = (Employee)obj;
				if (this.FirstName == empl.FirstName && this.LastName == empl.LastName && this.MiddleName == empl.MiddleName)
					return true;
				else
					return false;
			}
			else return false;
		}

		public override int GetHashCode()
		{
			return FirstName.GetHashCode() + LastName.GetHashCode() + MiddleName.GetHashCode();
		}

		public int CompareTo(object obj)
		{
			Employee emp = obj as Employee;
			if (emp != null)
				if (this.LastName == emp.LastName)
					if (this.FirstName == emp.FirstName)
						return this.MiddleName.CompareTo(emp.MiddleName);
					else
						return this.FirstName.CompareTo(emp.FirstName);
				else
					return this.LastName.CompareTo(emp.LastName);
			else
				throw new Exception("Невозможно сравнить два объекта");
		}
	}
}
