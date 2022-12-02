using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRegistrator_v4.Model
{
	class DepartureTimeComparer : IComparer<Employee>
	{
		public int Compare(Employee x, Employee y)
		{
			return x.DepartureTime.CompareTo(y.DepartureTime);
		}
	}
}
