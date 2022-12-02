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
using Xceed.Wpf.Toolkit;
using WorkerRegistrator_v4.ViewModel;

namespace WorkerRegistrator_v4.View
{
	/// <summary>
	/// Логика взаимодействия для SetArrivalTimeWindow.xaml
	/// </summary>
	public partial class SetDepartureTimeWindow : Window
	{
		private MainViewModel _viewModel;
		public SetDepartureTimeWindow()
		{
			InitializeComponent();
			this.Owner = Application.Current.MainWindow;
			_viewModel = this.Owner.FindResource("viewModel") as MainViewModel;
			this.DataContext = _viewModel;
		}

		private void OkButton_Click(object sender, RoutedEventArgs e)
		{
			if (_viewModel.TryToEditDepartureTime(newDepartureTime.Text))
				DialogResult = true;
			else
				System.Windows.MessageBox.Show("Введено некорректное время!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
	}
}
