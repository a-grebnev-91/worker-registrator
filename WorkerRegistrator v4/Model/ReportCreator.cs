using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace WorkerRegistrator_v4.Model
{
	class ReportCreator
	{
		private string _destPath = ConfigurationManager.AppSettings["destPath"];


		public void CreateReport(List<Employee> employees, DateTime currentDate)
		{
			string fileName = ConstructFileName(currentDate);
			using (StreamWriter writer = new StreamWriter(fileName, false))
			{
				writer.WriteLine(currentDate.ToString("dd MMMM yyyy (dddd)"));
				writer.WriteLine("Всего было {0} сотрудников.", employees.Count);
				writer.WriteLine();
				foreach (Employee emp in employees)
				{
					if (emp.IsDriver)
					{
						writer.Write(emp.LastName + " ");
						writer.Write(emp.FirstName + " ");
						writer.WriteLine(emp.MiddleName);
						writer.WriteLine(emp.Position);
						writer.WriteLine(emp.Department);
						writer.WriteLine("Прибыл(а): " + emp.ArrivalTime.ToString("HH:mm"));
						if (emp.DepartureTime == new DateTime())
							writer.WriteLine("Убыл(а): ");
						else
							writer.WriteLine("Убыл(а): " + emp.DepartureTime.ToString("HH:mm"));
						writer.WriteLine();
					}
				}
				foreach (Employee emp in employees)
				{
					if (!emp.IsDriver)
					{
						writer.Write(emp.LastName + " ");
						writer.Write(emp.FirstName + " ");
						writer.WriteLine(emp.MiddleName);
						writer.WriteLine(emp.Position);
						writer.WriteLine(emp.Department);
						writer.WriteLine("Прибыл(а): " + emp.ArrivalTime.ToString("HH:mm"));
						if (emp.DepartureTime == new DateTime())
							writer.WriteLine("Убыл(а): ");
						else
							writer.WriteLine("Убыл(а): " + emp.DepartureTime.ToString("HH:mm"));
						writer.WriteLine();
					}
				}
				writer.WriteLine("Последний раз файл программно изменен {0}", DateTime.Now);
				writer.Flush();
			}
		}
		
		private string ConstructFileName(DateTime currentDate)
		{
			StringBuilder builder = new StringBuilder(_destPath);
			builder.Append(@"\");
			builder.Append(currentDate.Year);
			builder.Append("_");
			builder.Append(currentDate.ToString("MM"));
			builder.Append("_");
			builder.Append(currentDate.ToString("dd"));
			builder.Append(".txt");
			return builder.ToString();
		}
	}
}
