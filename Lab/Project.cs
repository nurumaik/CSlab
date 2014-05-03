using System;
using System.Text;

namespace Lab
{
	[Serializable]
	public class Project : IDeepCopy
	{
    public ResearchSet researchSet {get; set;}
    public int participantsCount {get; set;}
    public TimeFrame timeFrame {get; set;}

		public Project (ResearchSet rs = ResearchSet.Theme1,
        int pc = 1, TimeFrame tf = TimeFrame.Long)
		{
      researchSet = rs;
      participantsCount = pc;
      timeFrame = tf;
		}

    public object DeepCopy()
    {
      return new Project(researchSet, participantsCount, timeFrame);
    }

    public override string ToString()
    {
        return new StringBuilder()
            .Append("Research set: ")
            .AppendLine(researchSet.ToString())
            .Append("Participants count: ")
            .AppendLine(participantsCount.ToString())
            .Append("Time frame: ")
            .AppendLine(timeFrame.ToString())
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
      return (p.researchSet == researchSet) &&
             (p.participantsCount == participantsCount) &&
             (p.timeFrame == timeFrame);
    }

    public static bool operator==(Project p, Project o)
    {
      return p.Equals(o);
    }

    public static bool operator!=(Project p, Project o)
    {
      return !(p.Equals(o));
    }

    public override int GetHashCode()
    {
      return ToString().GetHashCode();
    }
	}
}

