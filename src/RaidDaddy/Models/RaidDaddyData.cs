using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace RaidDaddy.Models
{
	public class RaidDaddyData
	{
		public RaidData CurrentRaid { get; set; }
		public List<string> Quotes { get; set; } = new List<string>();

		public void Save(string fileName)
		{
			File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
		}
	}
}