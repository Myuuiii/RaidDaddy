using Discord.Commands;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.RaidModule;

public class InfoCmd : ModuleBase<SocketCommandContext>
{
    private readonly IRaidRepository _raidRepository;
    private readonly IGuildRepository _guildRepository;

    public InfoCmd()
    {
        this._raidRepository = Bot.RaidRepository;
        this._guildRepository = Bot.GuildRepository;
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
}