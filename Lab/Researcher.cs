using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.ComponentModel;

namespace Lab
{
	[Serializable]
	public class Researcher : 
		Person, 
		IDeepCopy, 
		IEnumerable<Project>, 
		IComparable<Researcher>, 
		IComparer<Researcher>, 
		IDataErrorInfo
	{
		private List<Paper> mPapers;
		private List<Project> mProjects;
		private bool mIsDoc;

		public static void Serialize(string fileName, Researcher res)
		{
			FileStream stream = new FileStream(fileName, 
				                                 FileMode.Create, 
																				 FileAccess.Write);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, res);
			stream.Close();
		}

		public static Researcher Deserialize(string fileName)
		{
			FileStream stream = new FileStream(fileName, 
				                                 FileMode.Open, 
				                                 FileAccess.Read);
			BinaryFormatter formatter = new BinaryFormatter();
			Researcher result = formatter.Deserialize(stream) as Researcher;
			stream.Close();
			return result;
		}
		public bool IsDoc
		{
			get { return mIsDoc; }
			set
			{
				mIsDoc = value; 
				OnPropertyChanged("IsDoc");
			}
		}

		public double Mean
		{
			get
			{
				double tmp = 0;
				foreach (Project p in mProjects)
					tmp += p.ParticipantsCount;
				tmp /= mProjects.Count;
				return tmp;
			}
		}

		public Person Data
		{
			get
			{
				return new Person(mFirstName, mLastName, mBirthDate);
			}

			set
			{
				FirstName = value.FirstName;
				LastName = value.LastName;
				BirthDate = value.BirthDate;
			}
		}

		public List<Paper> Papers //TODO: notifypropertychanged?
		{
			get
			{
				return mPapers;
			}

			set
			{
				mPapers = value;
			}
		}

		public List<Project> Projects //TODO: notifypropertychanged?
		{
			get
			{
				return mProjects;
			}

			set
			{
				mProjects = value;
			}
		}

		public Researcher(string firstName = "NULL NAME",
											 string lastName = "NULL LASTNAME",
											 DateTime birthDate = new DateTime(),
											 bool d = false)
			: base(firstName, lastName, birthDate)
		{
			mPapers = new List<Paper>();
			mProjects = new List<Project>();
			IsDoc = d;
		}

		public Researcher(Person data, bool d)
			: this(data.FirstName, data.LastName, data.BirthDate, d)
		{
		}

		public Researcher AddPapers(params Paper[] newPapers)
		{
			if (newPapers.Length == 0)
				return this;
			mPapers.AddRange(newPapers);
			OnPropertyChanged("Papers");
			return this;
		}

		public Researcher AddProjects(params Project[] newProjects)
		{
			if (newProjects.Length == 0)
				return this;
			mProjects.AddRange(newProjects);
			OnPropertyChanged("Projects");
			return this;
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb
				.AppendLine(base.ToString())
				.Append("Is doc: ")
				.AppendLine(IsDoc.ToString())
				.AppendLine("Papers: ");
			foreach (Paper p in mPapers)
				sb.AppendLine(p.ToString());
			sb.AppendLine("Projects: ");
			foreach (Project p in mProjects)
				sb.AppendLine(p.ToString());
			return sb.ToString();
		}

		public override string ToShortString()
		{
			return new StringBuilder()
					.AppendLine(base.ToString())
					.Append("Is doc: ")
					.AppendLine(IsDoc.ToString())
					.ToString();
		}

		public new object DeepCopy()
		{
			Researcher result = new Researcher(base.DeepCopy() as Person, IsDoc);
			foreach (Paper p in mPapers)
				result.mPapers.Add(p.DeepCopy() as Paper);
			foreach (Project p in mProjects)
				result.mProjects.Add(p.DeepCopy() as Project);
			return result;
		}

		public IEnumerator<Project> GetEnumerator()
		{
			foreach (Project p in mProjects)
				yield return p;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			foreach (Project p in mProjects)
				yield return p;
		}

		public IEnumerable<Paper> GetPapersEnumerator()
		{
			foreach (Paper p in mPapers)
				yield return p;
		}

		public IEnumerable<Project> GetProjectsEnumerator(int count)
		{
			foreach (Project p in mProjects)
				if (p.ParticipantsCount == count)
					yield return p;
		}

		public int CompareTo(Researcher other)
		{
			return mPapers.Count.CompareTo(other.mPapers.Count);
		}

		public int Compare(Researcher a, Researcher b)
		{
			return a.Mean.CompareTo(b.Mean);
		}

		public string this[string columnName]
		{
			get
			{
				string errormessage = String.Empty;
				switch (columnName)
				{
					case "birthDate":
						if (BirthDate.Year < 1930 || BirthDate.Year > 1990)
							errormessage = "Bad birth date. Year must be between 1930 and 1990";
						break;
					case "projects":
						if (Projects.Count == 0)
							errormessage = "No projects";
						break;
				}
				return errormessage;
			}
		}

		public string Error {get { return String.Empty; }}
	}

}

