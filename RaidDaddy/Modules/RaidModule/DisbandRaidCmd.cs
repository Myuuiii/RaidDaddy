using Discord.Commands;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.RaidModule;

public class DisbandRaidCmd : ModuleBase<SocketCommandContext>
{
    private readonly IRaidRepository _raidRepository;
    private readonly IGuildRepository _guildRepository;

    public DisbandRaidCmd()
    {
        this._raidRepository = Bot.RaidRepository;
        this._guildRepository = Bot.GuildRepository;
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
}