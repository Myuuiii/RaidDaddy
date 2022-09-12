using Discord.Commands;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.RaidModule;

public class JoinRaidCmd : ModuleBase<SocketCommandContext>
{
    private readonly IRaidRepository _raidRepository;
    private readonly IGuildRepository _guildRepository;

    public JoinRaidCmd()
    {
        this._raidRepository = Bot.RaidRepository;
        this._guildRepository = Bot.GuildRepository;
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
}