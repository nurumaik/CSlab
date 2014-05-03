using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Lab;
using Microsoft.Win32;
using ViewModel.Annotations;

namespace ViewModel {
	// ReSharper disable once InconsistentNaming
	public class VM : INotifyPropertyChanged  {
		private Researcher mRes;
		private Project mCurrentProject;
		private Paper mCurrentPaper;
		private bool mUpdated;
		//private RoutedCommand mNewResearcherCommand = ;
		//private RoutedCommand mLoadResearcherCommand;
		//private RoutedCommand mSaveResearcherCommand;
		static public RoutedCommand AddProjectCommand = new RoutedCommand();
		static public RoutedCommand RemoveProjectCommand = new RoutedCommand();
		static public RoutedCommand AddPaperCommand = new RoutedCommand();
		static public RoutedCommand RemovePaperCommand = new RoutedCommand();


		public VM() {
			SelectedProject = null;
			SelectedPaper = null;
			NewResearcher();
			//mNewResearcherCommand = new RoutedCommand();
			//mLoadResearcherCommand = new RoutedCommand();
			//mSaveResearcherCommand = new RoutedCommand();
			//mAddProjectCommand = new RoutedCommand();
			//mRemoveProjectCommand = new RoutedCommand();
			//mAddPaperCommand = new RoutedCommand();
			//mRemovePaperCommand = new RoutedCommand();
		}

		public Dictionary<TimeFrame, string> TimeFrameItems {
			get {
				return new Dictionary<TimeFrame, string>() {
					{TimeFrame.Long, "Long"},
					{TimeFrame.TwoYears, "Two years"},
					{TimeFrame.Year, "Year"}
				};
			}
		}

		public Dictionary<ResearchSet, string> ResearchSetItems {
			get {
				return new Dictionary<ResearchSet, string>() {
					{ResearchSet.Theme1, "First theme"},
					{ResearchSet.Theme2, "Second theme"},
					{ResearchSet.Theme3, "Third theme"}
				};
			}
		}

		public string Name {
			get { return mRes.firstName; }
			set {
				mUpdated = true; 
				mRes.firstName = value; 
			}
		}

		public string LastName {
			get { return mRes.lastName; }
			set {
				mUpdated = true; 
				mRes.lastName = value; 
			}
		}

		public DateTime BirthDate {
			get { return mRes.birthDate; }
			set {
				mUpdated = true;
				mRes.birthDate = value;
			}
		}

		public bool Degree {
			get { return mRes.isDoc; }
			set {
				mUpdated = true;
				mRes.isDoc = value;
			}
		}

		public string ParticipantsCount {
			get {
				return mCurrentProject.participantsCount.ToString(CultureInfo.InvariantCulture);
			}

			set {
				int tmp;
				bool result = Int32.TryParse(value, out tmp);
				if (result)
					mCurrentProject.participantsCount = tmp;
			}
		}
		// ReSharper disable InconsistentNaming
		public ResearchSet RS {
			get { return mCurrentProject.researchSet; }
			set {
				mCurrentProject.researchSet = value; 
			}
		}

		public TimeFrame TF {
			get { return mCurrentProject.timeFrame;  }
			set { mCurrentProject.timeFrame = value; }
		}
		// ReSharper restore InconsistentNaming
		public string PublicationName {
			get { return mCurrentPaper.name; }
			set { mCurrentPaper.name = value; }
		}

		public string AuthorsCount {
			get { return mCurrentPaper.authors.ToString(CultureInfo.InvariantCulture); }
			set {
				int tmp;
				bool result = Int32.TryParse(value, out tmp);
				if (result)
					mCurrentPaper.authors = tmp;
			}
		}

		public ObservableCollection<Project> Projects {
			get { return new ObservableCollection<Project>(mRes.projects); }
		}

		public ObservableCollection<Paper> Papers {
			get { return new ObservableCollection<Paper>(mRes.papers); }
		}

		public Project SelectedProject { get; set; }
		public Paper SelectedPaper { get; set; }

		public void NewResearcher() {
			mRes = new Researcher("<Place your ad here>", "<Here can be ad too>");
			mCurrentProject = new Project();
			mCurrentPaper = new Paper();
			mUpdated = false;
			OnPropertyChanged("");
		}

		public void LoadResearcher() {
			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == true)
				try {
					mRes = Researcher.Deserialize(dialog.FileName);
				}
				catch {
					MessageBox.Show("An error occured while loading researcher from file.");
				}
			mCurrentProject = new Project();
			mCurrentPaper = new Paper();
			mUpdated = false;
			OnPropertyChanged("");
		}

		public void SaveResearcher() {
			SaveFileDialog dialog = new SaveFileDialog();
			if (dialog.ShowDialog() != true) return;
			try {
				Researcher.Serialize(dialog.FileName, mRes); 
			}
			catch {
				MessageBox.Show("An error occured while saving researcher to file.");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}

		public void AddProject() {
			mUpdated = true; 
			mRes.addProjects(new Project(RS, Int32.Parse(ParticipantsCount), TF));
			OnPropertyChanged("Projects");
		}

		public void RemoveProject() {
			mUpdated = true; 
			mRes.projects.Remove(SelectedProject);
			OnPropertyChanged("Projects");
		}

		public void AddPaper() {
			mUpdated = true; 
			mRes.addPapers(new Paper(PublicationName, Int32.Parse(AuthorsCount)));
			OnPropertyChanged("Papers");
		}

		public void RemovePaper() {
			mUpdated = true; 
			mRes.papers.Remove(SelectedPaper);
			OnPropertyChanged("Papers");
		}

		public bool CheckClose() {
			if (!mUpdated) 
				return true;
			switch (MessageBox.Show("Researcher was updated. Save changes?", "Save changes?", MessageBoxButton.YesNoCancel))
			{
				case MessageBoxResult.Yes:
					SaveResearcher();
					return true;
				case MessageBoxResult.No:
					return true;
				case MessageBoxResult.Cancel:
					return false;
				default:
					return true;
			}
		}
	}
}
