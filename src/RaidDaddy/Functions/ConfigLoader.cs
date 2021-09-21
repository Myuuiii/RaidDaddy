using System.IO;
using Newtonsoft.Json;
using RaidDaddy.Models.Configuration;

namespace RaidDaddy.Functions
{
	public class ConfigLoader
	{
		public static RaidDaddyConfiguration LoadBotConfig(string fileName)
		{
			if (File.Exists(fileName))
			{
				return JsonConvert.DeserializeObject<RaidDaddyConfiguration>(File.ReadAllText(fileName));
			}
			else
			{
				File.WriteAllText(fileName, JsonConvert.SerializeObject(new RaidDaddyConfiguration()));
				throw new System.Exception("Confiruation file was not found and a new one has been created, please configure and restart");
			}
		}
	}
}