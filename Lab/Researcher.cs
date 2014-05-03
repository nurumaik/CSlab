using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Lab {
	[Serializable]
	public class Researcher : Person, IDeepCopy, IEnumerable<Project>, IEnumerable, IComparable<Researcher>, IComparer<Researcher> {
		private List<Paper> mPapers;
		private List<Project> mProjects;

		public static void Serialize(string fileName, Researcher res) {
			FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, res);
			stream.Close();
		}

		public static Researcher Deserialize(string fileName) {
			FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
			BinaryFormatter formatter = new BinaryFormatter();
			Researcher result = formatter.Deserialize(stream) as Researcher;
			stream.Close();
			return result;
		}
		public bool isDoc { get; set; }
		public double mean {
			get {
				double tmp = 0;
				foreach (Project p in mProjects)
					tmp += p.participantsCount;
				tmp /= mProjects.Count;
				return tmp;
			}
		}

		public Person data {
			get {
				return new Person(mFirstName, mLastName, mBirthDate);
			}

			set {
				firstName = value.firstName;
				lastName = value.lastName;
				birthDate = value.birthDate;
			}
		}

		public List<Paper> papers {
			get {
				return mPapers;
			}

			set {
				mPapers = value;
			}
		}

		public List<Project> projects {
			get {
				return mProjects;
			}

			set {
				mProjects = value;
			}
		}

		public Researcher(string firstName = "NULL NAME",
											 string lastName = "NULL LASTNAME",
											 DateTime birthDate = new DateTime(),
											 bool d = false)
			: base(firstName, lastName, birthDate) {
			mPapers = new List<Paper>();
			mProjects = new List<Project>();
			isDoc = d;
		}

		public Researcher(Person data, bool d)
			: this(data.firstName, data.lastName, data.birthDate, d) {
		}

		public Researcher addPapers(params Paper[] newPapers) {
			if (newPapers.Length == 0)
				return this;
			mPapers.AddRange(newPapers);
			return this;
		}

		public Researcher addProjects(params Project[] newProjects) {
			if (newProjects.Length == 0)
				return this;
			mProjects.AddRange(newProjects);
			return this;
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			sb
				.AppendLine(base.ToString())
				.Append("Is doc: ")
				.AppendLine(isDoc.ToString())
				.AppendLine("Papers: ");
			foreach (Paper p in mPapers)
				sb.AppendLine(p.ToString());
			sb.AppendLine("Projects: ");
			foreach (Project p in mProjects)
				sb.AppendLine(p.ToString());
			return sb.ToString();
		}

		public override string ToShortString() {
			return new StringBuilder()
					.AppendLine(base.ToString())
					.Append("Is doc: ")
					.AppendLine(isDoc.ToString())
					.ToString();
		}

		public new object DeepCopy() {
			Researcher result = new Researcher(base.DeepCopy() as Person, isDoc);
			foreach (Paper p in mPapers)
				result.mPapers.Add(p.DeepCopy() as Paper);
			foreach (Project p in mProjects)
				result.mProjects.Add(p.DeepCopy() as Project);
			return result;
		}

		public IEnumerator<Project> GetEnumerator() {
			foreach (Project p in mProjects)
				yield return p;
		}

		IEnumerator IEnumerable.GetEnumerator() {
			foreach (Project p in mProjects)
				yield return p;
		}

		public IEnumerable<Paper> GetPapersEnumerator() {
			foreach (Paper p in mPapers)
				yield return p;
		}

		public IEnumerable<Project> GetProjectsEnumerator(int count) {
			foreach (Project p in mProjects)
				if (p.participantsCount == count)
					yield return p;
		}

		public int CompareTo(Researcher other) {
			return mPapers.Count.CompareTo(other.mPapers.Count);
		}

		public int Compare(Researcher a, Researcher b) {
			return a.mean.CompareTo(b.mean);
		}
	}

}

