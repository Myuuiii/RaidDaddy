using System.IO;
using Newtonsoft.Json;
using RaidDaddy.Models;

namespace RaidDaddy.Functions
{
	public class DataLoader
	{
		public static RaidDaddyData LoadData(string filePath)
		{
			if (File.Exists(filePath))
			{
				return JsonConvert.DeserializeObject<RaidDaddyData>(File.ReadAllText(filePath));
			}
			else
			{
				File.WriteAllText(filePath, JsonConvert.SerializeObject(new RaidDaddyData()));
				return new RaidDaddyData();
			}
		}
	}
}