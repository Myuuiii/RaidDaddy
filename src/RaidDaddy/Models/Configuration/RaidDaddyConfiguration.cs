using System.IO;
using Newtonsoft.Json;

namespace RaidDaddy.Models.Configuration
{
	public class RaidDaddyConfiguration
	{
		public string BotToken { get; set; } = "";
		public string Prefix { get; set; } = "rd!";
		public ulong RoleId { get; set; } = 0;

		public void Save(string fileName)
		{
			File.WriteAllText(fileName, JsonConvert.SerializeObject(this));
		}
	}
}