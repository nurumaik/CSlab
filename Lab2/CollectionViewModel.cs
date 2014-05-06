using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Lab;
using Lab.Annotations;


namespace Lab2 {
	public class CollectionViewModel : INotifyPropertyChanged {
		public enum ResearcherDataTemplateEnum
		{
			Default,
			Short,
			Custom
		}

		public ResearcherDataTemplateEnum ResearcherDataTemplate { get; set; }

		//public ICommand RemoveCommand;
		//public ICommand AddDefaultResearcherCommand;

		//public ICommand SetResearcher;

		public ObservableCollection<Project> Projects
		{
			get
			{
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
			get { return mCurrentResearcher.FirstName; }
			set { mCurrentResearcher.FirstName = value; }
		}

		public string LastName {
			get { return mCurrentResearcher.FirstName; }
			set { mCurrentResearcher.FirstName = value; }
		}

		public string BirthDate {
			get { return mCurrentResearcher.BirthDate.ToShortDateString(); }
			set {
				DateTime result;
				bool success = DateTime.TryParse(value, out result);
				if (success) {
					mCurrentResearcher.BirthDate = result;
				}

			}
		}

		public bool Degree {
			get { return mCurrentResearcher.IsDoc; }
			set { mCurrentResearcher.IsDoc = value; }
		}

		public string AverageTeamSize {
			get {
				return mCurrentResearcher.Projects.
					Average(p => p.ParticipantsCount).ToString();
			}
		}

		public void NewCommandHandler
			(object source, ExecutedRoutedEventArgs args) {

		}
		public void OpenCommandHandler
			(object source, ExecutedRoutedEventArgs args) {

		}
		public void SaveCommandHandler
			(object source, ExecutedRoutedEventArgs args) {

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
			set { mCurrentResearcher = value; }
		}

		public Paper SelectedPaper
		{
			get { return mCurrentPaper; }
			set { mCurrentPaper = value; }
		}

		public ResearcherObservableCollection Researchers
		{
			get { return mData; }
		}

		private ResearcherObservableCollection mData;
		private Researcher mCurrentResearcher;
		private Project mCurrentProject;
		private Paper mCurrentPaper;
		private bool mUpdated;


		static private readonly Dictionary<ResearchSet, string> ResearchSetNames =
			new Dictionary<ResearchSet, string>
			{
				{ResearchSet.Theme1, "First theme"},
				{ResearchSet.Theme2, "Second theme"},
				{ResearchSet.Theme3, "Third theme"}
			};

		private static readonly Dictionary<TimeFrame, string> TimeFrameNames =
			new Dictionary<TimeFrame, string>
			{
				{TimeFrame.Long, "Long"},
				{TimeFrame.TwoYears, "Two years"},
				{TimeFrame.Year, "Year"}
			};

		public CollectionViewModel() {
			mData = new ResearcherObservableCollection();
			mCurrentResearcher = null;
			mCurrentProject = null;
			mCurrentPaper = null;
			mUpdated = false;
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
