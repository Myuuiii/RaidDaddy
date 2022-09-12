using Discord.Commands;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.RaidModule;

public class CreateRaidCmd : ModuleBase<SocketCommandContext>
{
    private readonly IRaidRepository _raidRepository;
    private readonly IGuildRepository _guildRepository;

    public CreateRaidCmd()
    {
        this._raidRepository = Bot.RaidRepository;
        this._guildRepository = Bot.GuildRepository;
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

        Domain.Raid raid = new Domain.Raid();

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
        await ReplyAsync($"{guild.GetMention()} A new {raid.SelectedRaid.ToString()} raid has been created by {Context.User.Mention}. Type `{Bot.Config.Prefix}join` to join it.");
    }
}