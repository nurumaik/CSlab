using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Lab
{
	[Serializable]
	public sealed class ResearcherObservableCollection :
		System.Collections.ObjectModel.ObservableCollection<Researcher>
	{
		public ResearcherObservableCollection()
		{
			Subscribe();
		}

		private void Subscribe()
		{
			CollectionChanged += (sender, args) => Updated = true;
		}

		public static void Serialize(ResearcherObservableCollection col, string fileName)
		{
			FileStream stream = new FileStream(fileName, 
				                                 FileMode.Create, 
																				 FileAccess.Write);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.Serialize(stream, col);
			col.Updated = false;
			stream.Close();
		}

		public static ResearcherObservableCollection Deserialize(string fileName)
		{
			FileStream stream = new FileStream(fileName, 
				                                 FileMode.Open, 
				                                 FileAccess.Read);
			BinaryFormatter formatter = new BinaryFormatter();
			ResearcherObservableCollection result = formatter.Deserialize(stream) as ResearcherObservableCollection;
			result.Updated = false;
			result.Subscribe();
			foreach (var res in result)
				res.PropertyChanged += result.OnItemPropertyChanged;
			stream.Close();
			return result;
		}

		public bool Updated { get; set; }
		public void AddResearcher(Researcher item)
		{
			item.PropertyChanged += OnItemPropertyChanged;
			Add(item);
		}

		private void OnItemPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			Updated = true;
		}

		public void RemoveResearcherAt(int index)
		{
			this[index].PropertyChanged -= OnItemPropertyChanged;
			RemoveAt(index);
		}

		public void AddDefaultResearcher()
		{
			AddResearcher(new Researcher());
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
