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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lab2
{
	/// <summary>
	/// Interaction logic for CollectionView.xaml
	/// </summary>
	public partial class CollectionView : Page
	{
// ReSharper disable once InconsistentNaming
		public CollectionViewModel VM;
		public CollectionView()
		{
			InitializeComponent();
			VM = new CollectionViewModel();
		}

		private void NewCommandHandler(object sender, ExecutedRoutedEventArgs e) {
			VM.NewCommandHandler();
		}

		private void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e) {
			throw new NotImplementedException();
		}

		private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e) {
			throw new NotImplementedException();
		}

		private void RemoveCommandHandler(object sender, ExecutedRoutedEventArgs e) {
			throw new NotImplementedException();
		}

		private void SaveCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
		}

		private void AddResearcherCommandHandler(object sender, ExecutedRoutedEventArgs e) {
			throw new NotImplementedException();
		}
	}
}
