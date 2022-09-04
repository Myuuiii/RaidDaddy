using YamlDotNet.Serialization;

namespace RaidDaddy.Domain.Repositories;

public class GuildRepository : IGuildRepository
{
	private const string fileName = "./resource/guilds.yaml";
	private List<Guild> _guilds;

	public GuildRepository()
	{
		if (File.Exists(fileName))
			_guilds = new Deserializer().Deserialize<List<Guild>>(File.ReadAllText(fileName));
		else
			_guilds = new();
	}

	public void AddGuild(Guild guild)
	{
		_guilds.Add(guild);
		Save();
	}

	public Guild GetGuild(ulong guildId)
	{
		return _guilds.First(g => g.Id == guildId);
	}

	public List<Guild> GetGuilds()
	{
		return _guilds;
	}

	public bool GuildExists(ulong guildId)
	{
		return _guilds.Any(g => g.Id == guildId);
	}

	public void RemoveGuild(ulong guild)
	{
		_guilds.Remove(_guilds.First(g => g.Id == guild));
		Save();
	}

	public void Save()
	{
		File.WriteAllText(fileName, new Serializer().Serialize(_guilds));
	}

	public void SetRaiderRole(ulong guildId, ulong roleId)
	{
		Guild guild = GetGuild(guildId);
		guild.RaiderRoleId = roleId;
		Save();
	}

	public void SetRaidUpdateChannel(ulong guildId, ulong channelId)
	{
		Guild guild = GetGuild(guildId);
		guild.UpdateChannelId = channelId;
		Save();
	}

	public void UpdateGuild(Guild guild)
	{
		_guilds.Remove(_guilds.First(g => g.Id == guild.Id));
		_guilds.Add(guild);
		Save();
	}
}