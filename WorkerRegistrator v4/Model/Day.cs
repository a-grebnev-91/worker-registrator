using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace WorkerRegistrator_v4.Model
{
	[DataContract]
	class Day
	{
		[DataMember]
		private List<Employee> _employees;
		public List<Employee> Employees { get { return _employees; } set { _employees = value; } }
		public DateTime CurrentDate { get; private set; }
		public Day(List<Employee> employees, DateTime currentDate)
		{
			CurrentDate = currentDate;
			_employees = employees;
		}
	}
}
