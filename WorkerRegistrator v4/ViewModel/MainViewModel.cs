using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkerRegistrator_v4.Model;
using System.ComponentModel;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace WorkerRegistrator_v4.ViewModel
{
	class MainViewModel : INotifyPropertyChanged
	{
		private MainModel _model = new MainModel();
		private string _employeeName = string.Empty;
		private string _empInBuildingSearch = string.Empty;
		private string _empDepartureSearch = string.Empty;
		public Employee EmployeeToEdit { get { return _model.EmployeeToEdit; } set { _model.EmployeeToEdit = value; } }
		public ICollectionView Employees { get; set; }
		public ICollectionView Drivers { get; set; }
		public ICollectionView EmpInBuilding { get; set; }
		public ICollectionView EmpDeparted { get; set; }
		public string EmpInBuildingSearch { get { return _empInBuildingSearch; } set { _empInBuildingSearch = value; EmpInBuilding.Refresh(); } }
		public string EmpDeparutreSearch { get { return _empDepartureSearch; } set { _empDepartureSearch = value; EmpDeparted.Refresh(); } }
		public string EmployeeNameSearch { get { return _employeeName; } set { _employeeName = value; Employees.Refresh(); } }
		public DateTime CurrentDate { get { return _model.CurrentDate; } set { _model.CurrentDate = value; } }
		public string Today { get { return CurrentDate.ToString("dd MMMM yyyyг. (dddd)"); } }
		public int TotalCount { get { return _model.TotalCount; } }
		public int CountInBuilding { get { return _model.CountInBuilding; } }
		public int SelectedIndexInComboBox { get { return _model.IndexOfSort; } set { _model.IndexOfSort = value; } }
		public Employee EmpInBuildingToEdit { get; set; }
		public Employee EmpDepartedToEdit { get; set; }



		public event PropertyChangedEventHandler PropertyChanged;

		public MainViewModel()
		{
			Employees = CollectionViewSource.GetDefaultView(_model.Employees);
			Drivers = CollectionViewSource.GetDefaultView(_model.Drivers);
			EmpInBuilding = CollectionViewSource.GetDefaultView(_model.EmpInBuilding);
			EmpDeparted = CollectionViewSource.GetDefaultView(_model.EmpDeparted);
			Employees.Filter = FilterForEmployeesList;
			EmpInBuilding.Filter = FilterForInBuildingList;
		}

		private bool FilterForEmployeesList(object obj)
		{
			Employee emp = obj as Employee;
			return emp.LastName.ToLower().StartsWith(_employeeName.ToLower());
		}

		private bool FilterForInBuildingList(object obj)
		{
			Employee emp = obj as Employee;
			return emp.LastName.ToLower().StartsWith(_empInBuildingSearch.ToLower());
		}

		private void OnPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		public void EmployeeArrived(object obj)
		{
			Employee emp = obj as Employee;
			if (emp.IsDriver)
			{
				_model.DriverArrived(emp);
				Drivers.Refresh();
			}
			else
			{
				_model.EmpArrived(emp);
				Employees.Refresh();
			}
			OnPropertyChanged("TotalCount");
			OnPropertyChanged("CountInBuilding");
			EmpInBuilding.Refresh();
			_model.Save();
		}

		public void EmployeeDeparted(object obj)
		{
			Employee emp = obj as Employee;
			_model.EmpDepart(emp);
			OnPropertyChanged("TotalCount");
			OnPropertyChanged("CountInBuilding");
			EmpInBuilding.Refresh();
			EmpDeparted.Refresh();
			_model.Save();
		}

		internal void DelFromArrived(object selectedItem)
		{
			Employee emp = selectedItem as Employee;
			_model.DelFromArrived(emp);
			OnPropertyChanged("TotalCount");
			OnPropertyChanged("CountInBuilding");
			if (emp.IsDriver)
				Drivers.Refresh();
			else
				Employees.Refresh();
			EmpInBuilding.Refresh();
			_model.Save();
		}

		internal void DelFromDeparted(object selectedItem)
		{
			Employee emp = selectedItem as Employee;
			_model.DelFromDeparted(emp);
			OnPropertyChanged("TotalCount");
			OnPropertyChanged("CountInBuilding");
			EmpInBuilding.Refresh();
			EmpDeparted.Refresh();
			_model.Save();
		}

		internal void SortDepartureList()
		{
			_model.SortDeparture();
			EmpDeparted.Refresh();
		}

		public void EditEmployee(object obj)
		{
			this.EmployeeToEdit = obj as Employee;
		}

		public void CreateReportFile()
		{
			_model.CreateTodayReport();
		}

		internal bool TryToEditArrivalTime(string newArrivalTime)
		{
			TimeSpan time;
			if (!String.IsNullOrEmpty(newArrivalTime))
			{
				if (TimeSpan.TryParse(newArrivalTime, out time))
				{
					_model.EditArrivalTime(time);
					EmpInBuilding.Refresh();
					EmpDeparted.Refresh();
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}
		internal bool TryToEditArrivalTime(object employee, string newArrivalTime, out string timeToDisplay)
		{
			EmployeeToEdit = employee as Employee;
			TimeSpan time;
			timeToDisplay = EmployeeToEdit.ArrivalString;
			if (!String.IsNullOrEmpty(newArrivalTime))
			{
				if (newArrivalTime.Equals(timeToDisplay))
					return true;
				if (TimeSpan.TryParse(newArrivalTime, out time))
				{
					_model.EditArrivalTime(time);
					EmpInBuilding.Refresh();
					EmpDeparted.Refresh();
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}

		internal bool TryToEditDepartureTime(string newDepartureTime)
		{
			TimeSpan time;
			if (!String.IsNullOrEmpty(newDepartureTime))
			{
				if (TimeSpan.TryParse(newDepartureTime, out time))
				{
					_model.EditDepartureTime(time);
					EmpDeparted.Refresh();
					return true;
				}
				else
					return false;
			}
			else
				return false;
		}

		public void changeEmpInBuilding(object emp)
		{
			if (EmpInBuildingToEdit != null)
				EmpInBuildingToEdit.IsSelected = false;
			EmpInBuildingToEdit = emp as Employee;
			if (EmpInBuildingToEdit != null)
				EmpInBuildingToEdit.IsSelected = true;
			EmpInBuilding.Refresh();
		}
	}
}
