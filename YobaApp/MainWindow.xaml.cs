using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModel;

namespace YobaApp {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private VM viewModel;
		public MainWindow() {
			InitializeComponent();
			viewModel = new VM();
			DataContext = viewModel;
		}

		private void AddProjectHandler(object sender, RoutedEventArgs e) {
			viewModel.AddProject();
		}

		private void RemoveProjectHandler(object sender, RoutedEventArgs e) {
			viewModel.RemoveProject();
		}

		private void AddPaperHandler(object sender, RoutedEventArgs e) {
			viewModel.AddPaper();
		}

		private void RemovePaperHandler(object sender, RoutedEventArgs e) {
			viewModel.RemovePaper();
		}

		private void NewHandler(object sender, RoutedEventArgs e) {
			viewModel.NewResearcher();
		}

		private void OpenHandler(object sender, RoutedEventArgs e) {
			viewModel.LoadResearcher();
		}

		private void SaveHandler(object sender, RoutedEventArgs e) {
			viewModel.SaveResearcher();
		}

		private void OnClosing(object sender, CancelEventArgs e) {
			e.Cancel = !viewModel.CheckClose();
		}
	}
}
