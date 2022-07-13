namespace RaidDaddy.Domain;

public class Guild
{
	public ulong Id { get; set; }
	public ulong RaiderRoleId { get; set; }
	public ulong UpdateChannelId { get; set; }

	public string GetMention()
	{
		if (RaiderRoleId != 0)
			return $"<@&{this.RaiderRoleId}>";
		return $"";
	}
}