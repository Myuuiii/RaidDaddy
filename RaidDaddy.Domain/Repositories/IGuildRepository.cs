namespace RaidDaddy.Domain.Repositories;

public interface IGuildRepository
{
	List<Guild> GetGuilds();
	Guild GetGuild(ulong guildId);
	void AddGuild(Guild guild);
	void UpdateGuild(Guild guild);
	void RemoveGuild(ulong guild);
	bool GuildExists(ulong guildId);
	void Save();

	void SetRaiderRole(ulong guildId, ulong roleId);
}