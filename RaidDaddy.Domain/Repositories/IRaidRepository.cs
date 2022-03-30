namespace RaidDaddy.Domain.Repositories;

public interface IRaidRepository
{
	List<Raid> GetRaids();
	Raid GetRaid(ulong guildId);
	void UpdateRaid(Raid raid);
	void DeleteRaid(ulong guildId);
	void CreateRaid(Raid raid);
	void Save();
	bool RaidExists(ulong guildId);

	bool MemberIsInRaid(ulong guildId, ulong userId);
	void JoinRaid(ulong guildId, ulong userId);
	void LeaveRaid(ulong guildId, ulong userId);

	void SetRaid(ulong guildId, Destiny2Raid newRaid);
	void SetEncounter(ulong guildId, Destiny2Encounter newEncounter);
}