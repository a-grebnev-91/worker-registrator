using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using WorkerRegistrator_v4.ViewModel;
using Xceed.Wpf.Toolkit;

namespace WorkerRegistrator_v4.View
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private MainViewModel _viewModel;
		public MainWindow()
		{
			InitializeComponent();
			_viewModel = FindResource("viewModel") as MainViewModel;
			this.Closing += CreateReportFile;
		}

		private void arrivedButton_Click(object sender, RoutedEventArgs e)
		{
			if (employeesList.SelectedItem != null)
				_viewModel.EmployeeArrived(employeesList.SelectedItem);
			else
				System.Windows.MessageBox.Show("Выберите сотрудника из общего списка!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		private void arrivedDriverButton_Click(object sender, RoutedEventArgs e)
		{
			if (driversList.SelectedItem != null)
				_viewModel.EmployeeArrived(driversList.SelectedItem);
			else
				System.Windows.MessageBox.Show("Выберите водителя из списка водителей!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);

		}

		private void departedButton_Click(object sender, RoutedEventArgs e)
		{
			if (empInBuilding.SelectedItem != null)
				_viewModel.EmployeeDeparted(empInBuilding.SelectedItem);
			else
				System.Windows.MessageBox.Show("Выберите сотрудника, который находтися в Дирекции!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
			searchBoxInBuilding.Clear();
		}

		private void delFromArrived_Click(object sender, RoutedEventArgs e)
		{
			if (empInBuilding.SelectedItem != null)
				_viewModel.DelFromArrived(empInBuilding.SelectedItem);
			else
				System.Windows.MessageBox.Show("Выберите сотрудника, который ошибочно занесен в список прибывших!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		private void delFromDeparted_Click(object sender, RoutedEventArgs e)
		{
			if (departedList.SelectedItem != null)
				_viewModel.DelFromDeparted(departedList.SelectedItem);
			else
				System.Windows.MessageBox.Show("Выберите сотрудника, который ошибочно занесен в список убывших!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		private void CreateReportFile(object sender, System.ComponentModel.CancelEventArgs e)
		{
			_viewModel.CreateReportFile();
		}

		private void EmployeesSearch_Click(object sender, RoutedEventArgs e)
		{
			searchBox.Clear();
		}

		private void SearchInBuilding_Click(object sender, RoutedEventArgs e)
		{
			searchBoxInBuilding.Clear();
		}

		private void EditArrivalTime_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.EditEmployee(empInBuilding.SelectedItem);
			SetArrivalTimeWindow window = new SetArrivalTimeWindow();
			window.ShowDialog();
		}

		private void EditArrivalTimeFromDepartureList_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.EditEmployee(departedList.SelectedItem);
			SetArrivalTimeWindow window = new SetArrivalTimeWindow();
			window.ShowDialog();
		}

		private void EditDepartureTime_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.EditEmployee(departedList.SelectedItem);
			SetDepartureTimeWindow window = new SetDepartureTimeWindow();
			window.ShowDialog();
		}

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			int boxItemIndex = (sender as ComboBox).SelectedIndex;
			if (_viewModel != null)
			{
				_viewModel.SelectedIndexInComboBox = boxItemIndex;
				_viewModel.SortDepartureList();
			}
		}

		private void MaskedTextBox_LostFocus(object sender, RoutedEventArgs e)
		{
			MaskedTextBox box = sender as MaskedTextBox;
			object context = box.DataContext;
			String timeToDisplay;
			if (!_viewModel.TryToEditArrivalTime(context, box.Text, out timeToDisplay))
			{
				System.Windows.MessageBox.Show("Введено некорректное время!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			box.Text = timeToDisplay;
		}

		private void empInBuilding_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			object item = (sender as ListView).SelectedItem;
			_viewModel.changeEmpInBuilding(item);
		}

		private void MaskedTextBox_GotFocus(object sender, RoutedEventArgs e)
		{
			MaskedTextBox textBox = sender as MaskedTextBox;
			textBox.Text = null;
		}
	}
}

