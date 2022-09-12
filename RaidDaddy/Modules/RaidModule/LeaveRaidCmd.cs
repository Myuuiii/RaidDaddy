using Discord.Commands;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.RaidModule;

public class LeaveRaidCmd : ModuleBase<SocketCommandContext>
{
    private readonly IRaidRepository _raidRepository;
    private readonly IGuildRepository _guildRepository;

    public LeaveRaidCmd()
    {
        this._raidRepository = Bot.RaidRepository;
        this._guildRepository = Bot.GuildRepository;
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
}