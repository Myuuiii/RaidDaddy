﻿using Discord.Commands;
using Discord.WebSocket;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.GuildSettingsModule;

public class SetCmd : ModuleBase<SocketCommandContext>
{
    private IRaidRepository _raidRepository;
    private IGuildRepository _guildRepository;

    public SetCmd()
    {
        this._raidRepository = Bot.RaidRepository;
        this._guildRepository = Bot.GuildRepository;
    }

    /// <summary>
    /// Set some specific settings for the server and the raid
    /// </summary>
    /// <param name="option"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    [Command("set")]
    public async Task SetCommand(string option, string value)
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

	    switch (option)
	    {
		    case "raid":
			    if (RaidNameTranslations.GetRaid(value) == Destiny2Raid.INVALID)
			    {
				    await ReplyAsync(StaticValues.InvalidRaidName);
				    return;
			    }

			    _raidRepository.SetRaid(guild.Id, RaidNameTranslations.GetRaid(value));
			    _raidRepository.SetEncounter(guild.Id, Destiny2Encounter.CLEAN);
			    await ReplyAsync(StaticValues.RaidSet + raid.SelectedRaid.Humanize(), embed: raid.GetEmbed());
			    break;
		    case "encounter":
			    if (RaidEncounterTranslations.GetEncounter(raid.SelectedRaid, value) == Destiny2Encounter.INVALID)
			    {
				    await ReplyAsync(StaticValues.InvalidEncounterName);
				    return;
			    }

			    _raidRepository.SetEncounter(guild.Id,
				    RaidEncounterTranslations.GetEncounter(raid.SelectedRaid, value));
			    await ReplyAsync(StaticValues.EncounterSet + raid.SelectedEncounter.Humanize(), embed: raid.GetEmbed());
			    break;
		    case "role":
			    if (ulong.TryParse(value, out ulong roleId))
			    {
				    SocketRole role = Context.Guild.GetRole(roleId);
				    if (role == null)
				    {
					    await ReplyAsync(StaticValues.InvalidRole);
					    return;
				    }

				    _guildRepository.SetRaiderRole(guild.Id, roleId);
				    await ReplyAsync(StaticValues.RoleSet + role.Name);
			    }
			    else
			    {
				    await ReplyAsync(StaticValues.InvalidRole);
				    return;
			    }

			    break;
		    default:
			    await ReplyAsync(StaticValues.InvalidOption);
			    return;
	    }
    }
}