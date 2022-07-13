using System.Text;
using Discord;

namespace RaidDaddy.Domain;

public class Raid
{
	public Raid()
	{
		this.Raiders = new();
	}

	public ulong GuildId { get; set; }
	public Destiny2Raid SelectedRaid { get; set; }
	public Destiny2Encounter SelectedEncounter { get; set; } = Destiny2Encounter.CLEAN;
	public List<ulong> Raiders { get; set; }
	public ulong Creator { get; set; }

	public bool IsFull()
	{
		return Raiders.Count >= 6;
	}

	public int GetMemberCount()
	{
		return Raiders.Count;
	}

	public string GetMemberCountString()
	{
		return $"({GetMemberCount()}/6)";
	}

	public bool IsMember(ulong userId)
	{
		return Raiders.Contains(userId);
	}

	public bool IsCreator(ulong userId)
	{
		return Creator == userId;
	}

	public void Join(ulong userId)
	{
		Raiders.Add(userId);
	}

	public void Leave(ulong userId)
	{
		Raiders.Remove(userId);
	}

	public Embed GetEmbed()
	{
		StringBuilder sb = new StringBuilder();

		var embed = new EmbedBuilder();
		embed.WithTitle($"{this.SelectedRaid.Humanize()} Raid");
		embed.WithColor(this.SelectedRaid.GetColor());
		embed.WithTimestamp(DateTime.Now);
		embed.AddField("Raid", this.SelectedRaid.Humanize(), true);
		embed.AddField("Checkpoint", this.SelectedEncounter.Humanize(), true);
		embed.WithThumbnailUrl("https://cdn.myuuiii.com/projects/raiddaddy/newRaidIcon.png");
		embed.WithImageUrl("https://cdn.myuuiii.com/projects/raiddaddy/clantext.png");

		sb.AppendLine($"**Creator**: <@{this.Creator}>" + Environment.NewLine);
		sb.AppendLine("**Raiders**: " + this.GetMemberCountString());
		foreach (var raider in this.Raiders)
			sb.AppendLine($"- <@{raider}>");

		embed.WithDescription(sb.ToString());
		return embed.Build();
	}

	public bool HasSlotAvailable()
	{
		return this.Raiders.Count < 6;
	}
}