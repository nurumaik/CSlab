using System;
using System.Text;

namespace Lab
{
	[Serializable]
	public class Paper : IDeepCopy
	{
    public string name {get; set;}
    public int authors {get; set;}

		public Paper (string n = "NULL PAPER", int a = 0)
		{
      name = n;
      authors = a;
		}

    public override bool Equals(object o)
    {
      if (!(o is Paper))
        return false;
      Paper p = o as Paper;
      return (p.name == name) && (p.authors == authors);
    }

    public static bool operator==(Paper p, Paper obj)
    {
      return p.Equals(obj);
    }

    public static bool operator!=(Paper p, Paper obj)
    {
      return !(p.Equals(obj));
    }

    public override int GetHashCode()
    {
      return ToString().GetHashCode();
    }

    public override string ToString()
    {
        return new StringBuilder()
            .Append("Paper name: ")
            .AppendLine(name)
            .Append("Authors count: ")
            .AppendLine(authors.ToString())
            .ToString();
      //return new StringBuilder(name).Append(authors).ToString();
    }

    public object DeepCopy() {
      return new Paper(name, authors);
    }
	}
}

