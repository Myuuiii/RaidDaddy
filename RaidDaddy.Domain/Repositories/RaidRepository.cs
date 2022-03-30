using YamlDotNet.Serialization;

namespace RaidDaddy.Domain.Repositories;

public class RaidRepository : IRaidRepository
{
	private const string fileName = "./raids.yml";
	private List<Raid> raids = new();

	public RaidRepository()
	{
		if (File.Exists(fileName))
			raids = new Deserializer().Deserialize<List<Raid>>(File.ReadAllText(fileName));
		else
			raids = new();
	}

	public void CreateRaid(Raid raid)
	{
		this.raids.Add(raid);
		Save();
	}

	public void DeleteRaid(ulong guildId)
	{
		this.raids.Remove(this.raids.Single(r => r.GuildId == guildId));
		Save();
	}

	public Raid GetRaid(ulong guildId)
	{
		return this.raids.Single(r => r.GuildId == guildId);
	}

	public List<Raid> GetRaids()
	{
		return this.raids;
	}

	public void UpdateRaid(Raid raid)
	{
		Raid existingRaid = GetRaid(raid.GuildId);
		existingRaid = raid;
		Save();
	}

	public void Save()
	{
		File.WriteAllText(fileName, new Serializer().Serialize(this.raids));
	}

	public bool RaidExists(ulong guildId)
	{
		return this.raids.Any(r => r.GuildId == guildId);
	}

	public bool MemberIsInRaid(ulong guildId, ulong userId)
	{
		return this.raids.Any(r => r.GuildId == guildId && r.IsMember(userId));
	}

	public void JoinRaid(ulong guildId, ulong userId)
	{
		Raid raid = GetRaid(guildId);
		raid.Join(userId);
		Save();
	}

	public void LeaveRaid(ulong guildId, ulong userId)
	{
		Raid raid = GetRaid(guildId);
		raid.Leave(userId);
		Save();
	}

	public void SetRaid(ulong guildId, Destiny2Raid newRaid)
	{
		Raid raid = GetRaid(guildId);
		raid.SelectedRaid = newRaid;
		Save();
	}

	public void SetEncounter(ulong guildId, Destiny2Encounter newEncounter)
	{
		Raid raid = GetRaid(guildId);
		raid.SelectedEncounter = newEncounter;
		Save();
	}
}