using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Runtime.Serialization;
using System.IO;

namespace WorkerRegistrator_v4.Model
{
	class Saver
	{
		private string _savePath = ConfigurationManager.AppSettings["savePath"];

		public void Save(Day day)
		{
			string fileName = ConstructFileName(day.CurrentDate);
			if (File.Exists(fileName))
				File.Delete(fileName);
			using (Stream output = File.OpenWrite(fileName))
			{
				DataContractSerializer ser = new DataContractSerializer(typeof(Day));
				ser.WriteObject(output, day);
			}
		}

		private string ConstructFileName(DateTime currentDate)
		{
			StringBuilder builder = new StringBuilder(_savePath);
			builder.Append(@"\");
			builder.Append(currentDate.Year);
			builder.Append("_");
			builder.Append(currentDate.ToString("MM"));
			builder.Append("_");
			builder.Append(currentDate.ToString("dd"));
			builder.Append(".xml");
			return builder.ToString();
		}
	}
}
