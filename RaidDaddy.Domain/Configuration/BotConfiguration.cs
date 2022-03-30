namespace RaidDaddy.Domain;

public class BotConfiguration
{
	public static string _fileName = "./config.yml";
	public BotConfiguration()
	{
	}
	public string Token { get; set; } = "";
	public string Prefix { get; set; } = "rd!";
}