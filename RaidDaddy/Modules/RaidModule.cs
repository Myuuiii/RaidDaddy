using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules;

public class RaidModule : ModuleBase<SocketCommandContext>
{
	private IRaidRepository _raidRepository;
	private IGuildRepository _guildRepository;

	public RaidModule()
	{
		this._raidRepository = Bot._raidRepository;
		this._guildRepository = Bot._guildRepository;
	}

	/// <summary>
	/// Creates a raid
	/// </summary>
	/// <param name="raidName"></param>
	/// <param name="encounterName"></param>
	/// <returns></returns>
	[Command("create"), Alias("c", "new")]
	public async Task CreateRaid(string raidName, string encounterName = "")
	{
		Guild guild = _guildRepository.GetGuild(Context.Guild.Id);
		if (_raidRepository.RaidExists(guild.Id))
		{
			await ReplyAsync(StaticValues.RaidAlreadyInProgress);
			return;
		}

		Raid raid = new Raid();

		raid.GuildId = Context.Guild.Id;

		if (RaidNameTranslations.GetRaid(raidName) == Destiny2Raid.INVALID)
		{
			await ReplyAsync(StaticValues.InvalidRaidName);
			return;
		}

		raid.SelectedRaid = RaidNameTranslations.GetRaid(raidName);

		if (encounterName != "")
		{
			if (RaidEncounterTranslations.GetEncounter(raid.SelectedRaid, encounterName) == Destiny2Encounter.INVALID)
			{
				await ReplyAsync(StaticValues.InvalidEncounterName);
				return;
			}
			raid.SelectedEncounter = RaidEncounterTranslations.GetEncounter(raid.SelectedRaid, encounterName);
		}
		else
			raid.SelectedEncounter = Destiny2Encounter.CLEAN;

		raid.Creator = Context.User.Id;
		_raidRepository.CreateRaid(raid);
		_raidRepository.JoinRaid(guild.Id, Context.User.Id);
		await ReplyAsync($"{guild.GetMention()} A new {raid.SelectedRaid.ToString()} raid has been created by {Context.User.Mention}. Type `{Bot._config.Prefix}join` to join it.");
	}

	/// <summary>
	/// Disbands the raid
	/// </summary>
	/// <returns></returns>
	[Command("disband"), Alias("d", "end")]
	public async Task DisbandRaid()
	{
		Guild guild = _guildRepository.GetGuild(Context.Guild.Id);

		if (!_raidRepository.RaidExists(guild.Id))
		{
			await ReplyAsync(StaticValues.NoActiveRaid);
			return;
		}

		Raid raid = _raidRepository.GetRaid(guild.Id);

		// if (raid.Creator != Context.User.Id)
		// {
		// 	await ReplyAsync(StaticValues.NotCreator);
		// 	return;
		// }

		_raidRepository.DeleteRaid(raid.GuildId);
		await ReplyAsync($"{guild.GetMention()} " + StaticValues.RaidDisbanded);
	}

	/// <summary>
	/// Join the raid
	/// </summary>
	/// <returns></returns>
	[Command("join")]
	public async Task JoinRaid()
	{
		Guild guild = _guildRepository.GetGuild(Context.Guild.Id);

		if (!_raidRepository.RaidExists(guild.Id))
		{
			await ReplyAsync(StaticValues.NoActiveRaid);
			return;
		}

		Raid raid = _raidRepository.GetRaid(guild.Id);

		if (raid.IsMember(Context.User.Id))
		{
			await ReplyAsync(StaticValues.AlreadyInRaid);
			return;
		}

		if (!raid.HasSlotAvailable())
		{
			await ReplyAsync(StaticValues.NoSlotsAvailable);
			return;
		}

		_raidRepository.JoinRaid(guild.Id, Context.User.Id);
		await ReplyAsync($"{Context.User.Mention}" + StaticValues.HasJoined + " " + raid.GetMemberCountString());

		if (raid.IsFull())
			await ReplyAsync(StaticValues.RaidFull, embed: raid.GetEmbed());
	}

	/// <summary>
	/// Leave the raid
	/// </summary>
	/// <returns></returns>
	[Command("leave")]
	public async Task LeaveRaid()
	{
		Guild guild = _guildRepository.GetGuild(Context.Guild.Id);

		if (!_raidRepository.RaidExists(guild.Id))
		{
			await ReplyAsync(StaticValues.NoActiveRaid);
			return;
		}

		Raid raid = _raidRepository.GetRaid(guild.Id);

		if (!raid.IsMember(Context.User.Id))
		{
			await ReplyAsync(StaticValues.NotInRaid);
			return;
		}

		_raidRepository.LeaveRaid(guild.Id, Context.User.Id);
		await ReplyAsync($"{Context.User.Mention}" + StaticValues.HasLeft + " " + raid.GetMemberCountString());
	}

	/// <summary>
	/// Reserve a spot in the raid for someone
	/// </summary>
	/// <param name="targetUser"></param>
	/// <returns></returns>
	[Command("reserve")]
	public async Task ReserveSpotCommand(SocketUser targetUser)
	{
		Guild guild = _guildRepository.GetGuild(Context.Guild.Id);

		if (!_raidRepository.RaidExists(guild.Id))
		{
			await ReplyAsync(StaticValues.NoActiveRaid);
			return;
		}

		Raid raid = _raidRepository.GetRaid(guild.Id);

		// if (!raid.IsCreator(Context.User.Id))
		// {
		// 	await ReplyAsync(StaticValues.NotCreator);
		// 	return;
		// }

		if (!raid.IsMember(Context.User.Id))
		{
			await ReplyAsync(StaticValues.MembershipRequired);
			return;
		}

		if (raid.IsMember(targetUser.Id))
		{
			await ReplyAsync(StaticValues.TargetUserAlreadyInRaid);
			return;
		}

		if (!raid.HasSlotAvailable())
		{
			await ReplyAsync(StaticValues.NoSlotsAvailable);
			return;
		}

		_raidRepository.JoinRaid(guild.Id, targetUser.Id);
		await ReplyAsync($"{targetUser.Mention}" + StaticValues.Reserved + " " + raid.GetMemberCountString());

		if (raid.IsFull())
			await ReplyAsync(StaticValues.RaidFull, embed: raid.GetEmbed());
	}

	/// <summary>
	/// Remove a member from the raid
	/// </summary>
	/// <param name="targetUser"></param>
	/// <returns></returns>
	[Command("remove")]
	public async Task RemoveReservationCommand(SocketUser targetUser)
	{
		Guild guild = _guildRepository.GetGuild(Context.Guild.Id);

		if (!_raidRepository.RaidExists(guild.Id))
		{
			await ReplyAsync(StaticValues.NoActiveRaid);
			return;
		}

		Raid raid = _raidRepository.GetRaid(guild.Id);

		// if (!raid.IsCreator(Context.User.Id))
		// {
		// 	await ReplyAsync(StaticValues.NotCreator);
		// 	return;
		// }

		if (!raid.IsMember(Context.User.Id))
		{
			await ReplyAsync(StaticValues.MembershipRequired);
			return;
		}

		if (!raid.IsMember(targetUser.Id))
		{
			await ReplyAsync(StaticValues.TargetUserNotInRaid);
			return;
		}

		_raidRepository.LeaveRaid(guild.Id, targetUser.Id);
		await ReplyAsync($"{targetUser.Mention}" + StaticValues.Removed + " " + raid.GetMemberCountString());
	}

	/// <summary>
	/// List the raid information
	/// </summary>
	/// <returns></returns>
	[Command("info")]
	public async Task RaidInfo()
	{
		Guild guild = _guildRepository.GetGuild(Context.Guild.Id);
		bool raidExists = _raidRepository.RaidExists(guild.Id);

		if (!raidExists)
		{
			await ReplyAsync(StaticValues.NoActiveRaid);
			return;
		}

		Raid raid = _raidRepository.GetRaid(guild.Id);
		await ReplyAsync(embed: raid.GetEmbed());
	}

	/// <summary>
	/// Will list all the raids and or encounters
	/// </summary>
	[Command("list")]
	public async Task ListCommand()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine("__**Raids and Encounters**__");
		sb.AppendLine(Environment.NewLine);

		foreach (RaidTranslation translation in RaidNameTranslations.Translations)
		{
			sb.AppendLine($"- **{translation.Raid.Humanize()}** [`{String.Join(", ", translation.Translations)}`]");
			foreach (EncounterTranslation encounterTranslation in RaidEncounterTranslations.Translations.Where(r => r.Raid == translation.Raid))
			{
				sb.AppendLine($"\t\t - *{encounterTranslation.Encounter.Humanize()}* [`{String.Join(", ", encounterTranslation.Translations)}`]");
			}
			sb.AppendLine(Environment.NewLine);
		}
		await ReplyAsync(sb.ToString());
	}
}