using System;
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
using Microsoft.Win32;

namespace Lab2_2 {
	/// <summary>
	/// Interaction logic for Page1.xaml
	/// </summary>
	public partial class Page1 : Page, INotifyPropertyChanged {
#region Enum dictionaries

		private static readonly Dictionary<ResearchSet, string> ResearchSetNames =
			new Dictionary<ResearchSet, string> {
				{ResearchSet.Theme1, "First theme"},
				{ResearchSet.Theme2, "Second theme"},
				{ResearchSet.Theme3, "Third theme"}
			};

		static private readonly Dictionary<TimeFrame, string> TimeFrameNames =
			new Dictionary<TimeFrame, string>
			{
				{TimeFrame.Long, "Long"},
				{TimeFrame.TwoYears, "Two years"},
				{TimeFrame.Year, "Year"}
			};
#endregion
		public enum ResearcherDataTemplateEnum {
			Default,
			Short,
			Custom
		};

		public ResearcherDataTemplateEnum ResearcherDataTemplate {
			get { return _researcherDataTemplate; }
			set {
				if (value == _researcherDataTemplate) return;
				_researcherDataTemplate = value;
				OnPropertyChanged();
				OnPropertyChanged("RCollTemplate");
			}
		}

		public ResearcherObservableCollection RColl {
			get {
				return (ResearcherObservableCollection)
					Application.Current.Resources["KeyResearcherCollection"];
			}

			set {
				ResearcherObservableCollection tmp = (ResearcherObservableCollection)
					Application.Current.Resources["KeyResearcherCollection"];
				tmp.Clear();
				foreach (var r in value) {
					tmp.AddResearcher(r);
				}
			}
		}

		/*public string DummyProperty {
			get { return _dummyProperty; }
			set { _dummyProperty = value; }
		}*/

		

		public Page1() {
			CurrentResearcher = new Researcher();
			CurrentProject = new Project();
			RColl.CollectionChanged += (sender, args) => { Updated = true; };
			//RColl.Count();
			Updated = false;
			//Reset();
			InitializeComponent();
		}

		#region Command handlers

		private void NewCommandHandler(object sender, ExecutedRoutedEventArgs e) {
			if (Updated) {
				switch (MessageBox.Show(
					"Save changes?",
					"Researcher collection changed. Save changes to file?",
					MessageBoxButton.YesNoCancel)) {
						case MessageBoxResult.Cancel:
							return;
						case MessageBoxResult.Yes:
							PerformSave();
							break;
						case MessageBoxResult.No:
							break;
						default:
							throw new ArgumentOutOfRangeException();
				}
			}
			RColl = new ResearcherObservableCollection();
			Updated = false;
		}

		private void OpenCommandHandler(object sender, ExecutedRoutedEventArgs e) {
			var fd = new OpenFileDialog();
			if (fd.ShowDialog() == true) {
				try {
					RColl = ResearcherObservableCollection.Deserialize(fd.FileName);
				}
				catch {
					MessageBox.Show("Something failed while loading from file", "FAIL");
					return;
				}
			}
			Updated = false;
		}

		private void SaveCommandHandler(object sender, ExecutedRoutedEventArgs e) {
			PerformSave();
		}

		private void PerformSave() {
			var fd = new SaveFileDialog();
			if (fd.ShowDialog() == true) {
				try {
					ResearcherObservableCollection.Serialize(RColl, fd.FileName);
				}
				catch {
					MessageBox.Show("Something failed while saving to file", "FAIL");
					return;
				}
			}
			//Updated = false;
		}

		private bool Updated {
			get { return RColl.Updated; } 
			set { RColl.Updated = value; }
		}
		private Researcher _currentResearcher;
		private Project _currentProject;
		private ResearcherDataTemplateEnum _researcherDataTemplate 
			= ResearcherDataTemplateEnum.Short;
		//private string _dummyProperty = "FUCK OFF";

		private void SaveCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) {
			e.CanExecute = Updated;
		}

		private void DeleteCommandHandler(object sender, ExecutedRoutedEventArgs e) {
			RColl.RemoveResearcher(CurrentResearcher);
			CurrentResearcher = new Researcher();
		}

		private void AddDefaultResearcher(object sender, RoutedEventArgs e) {
			RColl.AddDefaultResearcher();
		}

		private void DeleteCommandCanExecute(object sender,
			CanExecuteRoutedEventArgs e) {
			e.CanExecute = RColl.Contains(CurrentResearcher);
		}

		#endregion

		#region Properties

		public Researcher CurrentResearcher {
			get { return _currentResearcher; }
			set {
				if (Equals(value, _currentResearcher) || value == null) return;
				_currentResearcher = value;
				OnPropertyChanged();
				OnPropertyChanged("AverageTeam");
				OnPropertyChanged("FirstName");
				OnPropertyChanged("LastName");
				OnPropertyChanged("BirthDate");
				OnPropertyChanged("Projects");
				OnPropertyChanged("Papers");
			}
		}

		public Project CurrentProject {
			get { return _currentProject; }
			set {
				if (Equals(value, _currentProject)) return;
				_currentProject = value;
				OnPropertyChanged();
				OnPropertyChanged("RS");
				OnPropertyChanged("TF");
				OnPropertyChanged("ParticipantsCount");
			}
		}

		public double AverageTeam {
			get {
				return (double)
						CurrentResearcher.Projects.Sum(p => p.ParticipantsCount) /
						CurrentResearcher.Projects.Count();
			}
		}

		public DataTemplate RCollTemplate {
			get {
				switch (ResearcherDataTemplate) {
					case ResearcherDataTemplateEnum.Default:
						return null;
					case ResearcherDataTemplateEnum.Short:
						return (DataTemplate)
							Application.Current.FindResource("KeyMinimalDataTemplate");
					case ResearcherDataTemplateEnum.Custom:
						return (DataTemplate)
							Application.Current.FindResource("KeyMaxResDataTemplate");
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

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

		public ObservableCollection<Paper> Papers {
			get {
				return new ObservableCollection<Paper>(CurrentResearcher.Papers);
			}
		}

		// ReSharper disable InconsistentNaming
		public string RS
		{
			get { return ResearchSetNames[CurrentProject.ResearchSet]; }
		}
		public string TF
		{
			get { return TimeFrameNames[CurrentProject.TimeFrame]; }
		}
		// ReSharper restore InconsistentNaming
		public string ParticipantsCount
		{
			get { return CurrentProject.ParticipantsCount.ToString(); }
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged
			([CallerMemberName] string propertyName = null) 
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) 
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
