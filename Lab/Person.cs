using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Lab.Annotations;


namespace Lab
{
	[Serializable]
	public class Person : IDeepCopy, INotifyPropertyChanged
	{
		protected string mFirstName;
		protected string mLastName;
		protected DateTime mBirthDate;

		public string FirstName
		{
			get
			{
				return mFirstName;
			}

			set
			{
				mFirstName = value;
				OnPropertyChanged("FirstName");
			}
		}

		public string LastName
		{
			get
			{
				OnPropertyChanged("LastName");
				return mLastName;
			}

			set
			{
				mLastName = value;
			}
		}

		public DateTime BirthDate
		{
			get
			{
				return mBirthDate;
			}

			set
			{
				OnPropertyChanged("BirthDate");
				mBirthDate = value;
			}
		}

		public Person(string fName, string lName, DateTime bDate)
		{
			mFirstName = fName;
			mLastName = lName;
			mBirthDate = bDate;
		}

		public override string ToString()
		{
			return new StringBuilder()
				.Append("First name: ")
				.AppendLine(mFirstName)
				.Append("Last name: ")
				.AppendLine(mLastName)
				.Append("Birth date: ")
				.AppendLine(mBirthDate.ToString())
				.ToString();
		}

		public virtual string ToShortString()
		{
			return new StringBuilder()
				.Append("First name: ")
				.AppendLine(mFirstName)
				.Append("Last name: ")
				.AppendLine(mLastName)
				.AppendLine(mBirthDate.ToString())
				.ToString();
		}

		//public override bool Equals(object obj)
		//{
		//	if (!(obj is Person))
		//		return false;
		//	Person p = obj as Person;
		//	return (p.FirstName == FirstName) &&
		//				 (p.LastName == LastName) &&
		//				 (p.BirthDate == BirthDate);
		//}

		//public static bool operator ==(Person p, Person obj)
		//{
		//	return p != null && p.Equals(obj);
		//}

		//public static bool operator !=(Person p, Person obj)
		//{
		//	return p as object != null && !(p.Equals(obj));
		//}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		public object DeepCopy()
		{
			return new Person(FirstName, LastName, BirthDate);
		}

		[field: NonSerialized]
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

