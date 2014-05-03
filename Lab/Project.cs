using System;
using System.Text;

namespace Lab
{
	[Serializable]
	public class Project : IDeepCopy
	{
		public ResearchSet ResearchSet { get; set; }
		public int ParticipantsCount { get; set; }
		public TimeFrame TimeFrame { get; set; }

		public Project(ResearchSet rs = ResearchSet.Theme1,
				int pc = 1, TimeFrame tf = TimeFrame.Long)
		{
			ResearchSet = rs;
			ParticipantsCount = pc;
			TimeFrame = tf;
		}

		public object DeepCopy()
		{
			return new Project(ResearchSet, ParticipantsCount, TimeFrame);
		}

		public override string ToString()
		{
			return new StringBuilder()
					.Append("Research set: ")
					.AppendLine(ResearchSet.ToString())
					.Append("Participants count: ")
					.AppendLine(ParticipantsCount.ToString())
					.Append("Time frame: ")
					.AppendLine(TimeFrame.ToString())
					.ToString();
			//return new StringBuilder().Append(researchSet).AppendLine()
			//                          .Append(participantsCount).AppendLine()
			//                          .Append(timeFrame).AppendLine()
			//                          .ToString();
		}

		public override bool Equals(object o)
		{
			if (!(o is Project))
				return false;
			Project p = o as Project;
			return (p.ResearchSet == ResearchSet) &&
						 (p.ParticipantsCount == ParticipantsCount) &&
						 (p.TimeFrame == TimeFrame);
		}

		public static bool operator ==(Project p, Project o)
		{
			return p != null && p.Equals(o);
		}

		public static bool operator !=(Project p, Project o)
		{
			return p != null && !(p.Equals(o));
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}
	}
}

