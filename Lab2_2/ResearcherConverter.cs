using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using Lab;

namespace Lab2_2
{
	[ValueConversion(typeof(Researcher), typeof(string))]
	public class ResearcherConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, 
			                    CultureInfo culture)
		{
			Researcher res = value as Researcher;
			if (res == null)
				return "###FAIL";

			return String.Format("{0} {1} with {2} maximum team size", 
				                   res.FirstName, res.LastName,
				                   res.Projects.Count == 0 ? 0 :
													 res.Projects.Max(p => p.ParticipantsCount));
		}

		public object ConvertBack(object value, Type targetType, object parameter, 
			                        CultureInfo culture)
		{
			throw new NotImplementedException(); //THERE IS NO WAY BACK
		}
	}
}
