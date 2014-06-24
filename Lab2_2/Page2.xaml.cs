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
			/*CurrentProject = new Project();
			CurrentPaper = new Paper();*/
			InitializeComponent();
		}

		public Researcher CurrentResearcher {
			get { return _currentResearcher; }
			set {
				if (Equals(value, _currentResearcher)) return;
				_currentResearcher = value;
				OnPropertyChanged();
				OnPropertyChanged("FirstName");
				OnPropertyChanged("LastName");
				OnPropertyChanged("BirthDate");
				OnPropertyChanged("IsDoc");
				//OnPropertyChanged("Projects");
				//OnPropertyChanged("Papers");
			}
		}

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

		public Project CurrentProject {
			get { return _currentProject; }
			set {
				if ((object)value == null)
					_currentProject = new Project();
				else
					_currentProject = value;
				RS = _currentProject.ResearchSet;
				TF = _currentProject.TimeFrame;
				_pc = _currentProject.ParticipantsCount;
				OnPropertyChanged("RS");
				OnPropertyChanged("TF");
				OnPropertyChanged("ParticipantsCount");
			}
		}

		public Paper CurrentPaper {
			get { return _currentPaper; }
			set {
				
				if (Equals(value, _currentPaper)) 
					return;
				if ((object)value == null)
					_currentPaper = new Paper();
				else
					AuthorsCount = value.authors.ToString();
				PublicationName = value.name;
				OnPropertyChanged("AuthorsCount");
				OnPropertyChanged("PublicationName");
				OnPropertyChanged();
			}
		}

		public string AuthorsCount {
			get { return _ac.ToString(); }
			set { _ac = Convert.ToInt32(value); }
		}
		public string PublicationName { get; set; }

		public ResearchSet RS { get; set; }

		public TimeFrame TF { get; set; }

		public string ParticipantsCount {
			get { return _pc.ToString(); }
			set { _pc = Convert.ToInt32(value); }
		}

		private int _pc = 0;
		private int _ac = 0;
		private Paper _currentPaper;
		private Researcher _currentResearcher;

		#endregion
		#region Handlers
		private void AddResearcherHandler
			(object sender, ExecutedRoutedEventArgs e) {
			((ResearcherObservableCollection)Application.Current.Resources
				["KeyResearcherCollection"]).AddResearcher(CurrentResearcher);
			CurrentResearcher = new Researcher();
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
		protected virtual void OnPropertyChanged(
			[CallerMemberName] string propertyName = null) 
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) 
				handler(this, new PropertyChangedEventArgs(propertyName));
		}


		private void AddPaper(object sender, RoutedEventArgs e) {
			CurrentResearcher.Papers.Add(
				new Paper(PublicationName, _ac));
		}

		private void DeleteItem(object sender, ExecutedRoutedEventArgs e) {
			if (e.Parameter.ToString() == "Project") {
				Project p = CurrentProject;
				CurrentProject = null;
				CurrentResearcher.Projects.Remove(p);
			}
			else if (e.Parameter.ToString() == "Paper") {
				Paper p = CurrentPaper;
				CurrentPaper = null;
				CurrentResearcher.Papers.Remove(p);
			}
			else
				MessageBox.Show(e.Parameter.ToString());
		}

		private void CanDeleteItem(object sender, CanExecuteRoutedEventArgs e) {
			if (e.Parameter == null)
				e.CanExecute = false;
			else if (e.Parameter.ToString() == "Project")
				e.CanExecute = CurrentResearcher.Projects.Contains(CurrentProject);
			else if (e.Parameter.ToString() == "Paper")
				e.CanExecute = CurrentResearcher.Papers.Contains(CurrentPaper);
		}
	}
}
