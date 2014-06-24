using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lab;
using Lab.Annotations;
using Microsoft.Win32;


namespace Lab2 {
	public class CollectionViewModel : INotifyPropertyChanged {
		public enum ResearcherDataTemplateEnum
		{
			Default,
			Short,
			Custom
		}

		private ResearcherDataTemplateEnum mTemplate = ResearcherDataTemplateEnum.Default;
		//public ResearcherDataTemplateEnum ResearcherDataTemplate { get { return mTemplate; } set { mTemplate = value; if  } }

		public ObservableCollection<Project> Projects
		{
			get {
				return new ObservableCollection<Project>(mCurrentResearcher.Projects);
			}
		}

		public ObservableCollection<Paper> Papers
		{
			get
			{
				return new ObservableCollection<Paper>(mCurrentResearcher.Papers);
			}
		}

		// ReSharper disable InconsistentNaming
		public string RS
		{
			get { return ResearchSetNames[mCurrentProject.ResearchSet]; }
		}
		public string TF
		{
			get { return TimeFrameNames[mCurrentProject.TimeFrame]; }
		}
		// ReSharper restore InconsistentNaming
		public string ParticipantsCount
		{
			get { return mCurrentProject.ParticipantsCount.ToString(); }
		}

		public string FirstName {
			get { return ProtectedResearcher.FirstName; }
			set { CurrentResearcher.FirstName = value; }
		}

		public string LastName {
			get { return ProtectedResearcher.LastName; }
			set { CurrentResearcher.LastName = value; }
		}

		public string BirthDate {
			get { return ProtectedResearcher.BirthDate.ToShortDateString(); }         
			set {
				DateTime result;
				bool success = DateTime.TryParse(value, out result);
				if (success) {
					CurrentResearcher.BirthDate = result;
				}

			}
		}

		public bool Degree {
			get { return ProtectedResearcher.IsDoc; }
			set { CurrentResearcher.IsDoc = value; }
		}

		public string AverageTeamSize {
			get {
				return mCurrentResearcher.Projects.Count == 0
					? mCurrentResearcher.Projects
					  .Average(p => p.ParticipantsCount).ToString()
					: String.Empty;
			}
		}

		public void NewCommandHandler() {
			AddDefaultResearcher();
			mUpdated = false;
			OnPropertyChanged();
		}
		public void OpenCommandHandler
			(object source, ExecutedRoutedEventArgs args) {
			var fd = new OpenFileDialog();
			if (fd.ShowDialog() == true) {
				Researchers = ResearcherObservableCollection.Deserialize(fd.FileName);
			}
			mUpdated = false;
			OnPropertyChanged();
		}

		public void SaveCommandHandler
			(object source, ExecutedRoutedEventArgs args) {
			var fd = new SaveFileDialog();
			if (fd.ShowDialog() == true) {
				ResearcherObservableCollection.Serialize(Researchers, fd.FileName);
			}
			mUpdated = false;
		}

		public void AddDefaultResearcher() {
			Researcher res = new Researcher();
			res.AddProjects(new Project());
			res.AddProjects(new Project());
			res.AddProjects(new Project());
			Researchers.AddResearcher(res);
			res = new Researcher("OLOLOLO", "FUCK");
			CurrentResearcher = res;
			res.AddProjects(new Project());
			Researchers.AddResearcher(res);
			SelectedProject = res.Projects.First();
		} 

		public bool SaveCommandCanExecuted() {
			return false; //NAHUI IDI SUKA
		}

		public Visibility ResearcherDataVisible {
			get {
				return mCurrentResearcher == null ?
					Visibility.Hidden : Visibility.Visible;
			}
		}

		public Researcher CurrentResearcher
		{
			get { return mCurrentResearcher; }
			set {
				mCurrentResearcher = value;
			}
		}

		public Paper SelectedPaper
		{
			get { return mCurrentPaper; }
			set { mCurrentPaper = value; }
		}

		public Project SelectedProject {
			get { return mCurrentProject; }
			set { mCurrentProject = value; }
		}

		public ResearcherObservableCollection Researchers
		{
			get {
				return (ResearcherObservableCollection) Application.Current.Resources
					["KeyResearcherObservableCollection"];
			}
			set {
				Application.Current.Resources["KeyResearcherObservableCollection"] = 
					value;
			}
		}

		public bool ResearcherSelected {
			get { return CurrentResearcher != null; }
		}

		public bool PaperSelected {
			get { return mCurrentPaper != null; }
		}

		private Researcher mCurrentResearcher = null;
		private Project mCurrentProject;
		private Paper mCurrentPaper = null;
		//private static readonly Paper DummyPaper = new Paper();
		private static readonly Researcher DummyResearcher = new Researcher();

		private Researcher ProtectedResearcher{
			get { return CurrentResearcher ?? DummyResearcher; }
		}

		//private static Project mDummyProject = new Project(); 
		private bool mUpdated;


		static private readonly Dictionary<ResearchSet, string> ResearchSetNames =
			new Dictionary <ResearchSet, string>
			{
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

		public CollectionViewModel() {
			NewCommandHandler();
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged
			([CallerMemberName] string propertyName = null) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
				handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
