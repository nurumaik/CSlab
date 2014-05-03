using System;
using System.Text;


namespace Lab
{
	[Serializable]
	public class Person : IDeepCopy
	{
		protected string mFirstName;
		protected string mLastName;
		protected System.DateTime mBirthDate;

        public string firstName
        {
          get
          {
            return mFirstName;
          }

          set
          {
            mFirstName = value;
          }
        }

        public string lastName
        {
          get
          {
            return mLastName;
          }

          set
          {
            mLastName = value;
          }
        }

        public DateTime birthDate
        {
          get
          {
            return mBirthDate;
          }

          set
          {
            mBirthDate = value;
          }
        }

		    public Person (string fName, string lName, DateTime bDate)
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

        public override bool Equals(object obj)
        {
          if (!(obj is Person))
            return false;
          Person p = obj as Person;
          return (p.firstName == firstName) &&
                 (p.lastName == lastName) &&
                 (p.birthDate == birthDate);
        }

        public static bool operator==(Person p, Person obj)
        {
          return p.Equals(obj);
        }

        public static bool operator!=(Person p, Person obj)
        {
          return !(p.Equals(obj));
        }

        public override int GetHashCode()
        {
          return ToString().GetHashCode();
        }

        public object DeepCopy()
        {
          return new Person(firstName, lastName, birthDate);
        }


	}
}

