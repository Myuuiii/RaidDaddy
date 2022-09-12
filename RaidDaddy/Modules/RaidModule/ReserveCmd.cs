using Discord.Commands;
using Discord.WebSocket;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.RaidModule;

public class ReserveCmd : ModuleBase<SocketCommandContext>
{
    private readonly IRaidRepository _raidRepository;
    private readonly IGuildRepository _guildRepository;

    public ReserveCmd()
    {
        _raidRepository = Bot.RaidRepository;
        _guildRepository = Bot.GuildRepository;
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
}