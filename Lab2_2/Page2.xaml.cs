using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using Lab;
using Lab2_2.Annotations;

namespace Lab2_2 {
	/// <summary>
	/// Interaction logic for Page2.xaml
	/// </summary>
	public partial class Page2 : Page, INotifyPropertyChanged {
		private Project _currentProject;

		public Page2() {
			CurrentResearcher = new Researcher();
			CurrentProject = new Project();
			InitializeComponent();
		}

		public Researcher CurrentResearcher { get; set; }

		static public Dictionary<TimeFrame, string> TimeFrameItems {
			get {
				return new Dictionary<TimeFrame, string>() {
					{TimeFrame.Long, "Long"},
					{TimeFrame.TwoYears, "Two years"},
					{TimeFrame.Year, "Year"}
				};
			}
		}

		static public Dictionary<ResearchSet, string> ResearchSetItems {
			get {
				return new Dictionary<ResearchSet, string>
				{
					{ResearchSet.Theme1, "First theme"},
					{ResearchSet.Theme2, "Second theme"},
					{ResearchSet.Theme3, "Third theme"}
				};
			}
		}

		#region Properties

		public string FirstName {
			get { return CurrentResearcher.FirstName; }
			set { CurrentResearcher.FirstName = value; }
		}

		public string LastName {
			get { return CurrentResearcher.LastName; }
			set { CurrentResearcher.LastName = value; }
		}

		public DateTime BirthDate {
			get { return CurrentResearcher.BirthDate; }
			set { CurrentResearcher.BirthDate = value; }
		}

		public bool IsDoc {
			get { return CurrentResearcher.IsDoc; }
			set { CurrentResearcher.IsDoc = value; }
		}

		public ObservableCollection<Project> Projects {
			get {
				return new ObservableCollection<Project>(CurrentResearcher.Projects);
			}
		}

		public Project CurrentProject {
			get { return _currentProject; }
			set {
				_currentProject = value;
				RS = value.ResearchSet;
				TF = value.TimeFrame;
				_pc = value.ParticipantsCount;
				OnPropertyChanged("RS");
				OnPropertyChanged("TF");
				OnPropertyChanged("ParticipantsCount");
			}
		}

		public ResearchSet RS { get; set; }

		public TimeFrame TF { get; set; }

		public string ParticipantsCount {
			get { return _pc.ToString(); }
			set { _pc = Convert.ToInt32(value); }
		}

		private int _pc;

	#endregion
		#region Handlers
		private void AddResearcherHandler
			(object sender, ExecutedRoutedEventArgs e) {
			((ResearcherObservableCollection)Application.Current.Resources
				["KeyResearcherCollection"]).AddResearcher(CurrentResearcher);
			((TabControl) Application.Current.MainWindow.FindName("MainTabControl"))
				.SelectedIndex = 0;
		}

		private void AddResearcherCanExecute
			(object sender, CanExecuteRoutedEventArgs e) {
			foreach (FrameworkElement i in ((StackPanel)FindName("ResearcherData")).Children)
				if (Validation.GetHasError(i)) {
					e.CanExecute = false;
					return;
				}
			e.CanExecute = true;
		
		}

		private void Cancel(object sender, RoutedEventArgs e) {
			((TabControl)Application.Current.MainWindow.FindName("MainTabControl"))
				.SelectedIndex = 0;
		}

		private void AddProject(object sender, RoutedEventArgs e) {
			CurrentResearcher.Projects.Add(new Project(RS, _pc, TF));
			OnPropertyChanged("Projects");
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}


	}
}
