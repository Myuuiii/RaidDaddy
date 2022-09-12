using Discord.Commands;
using Discord.WebSocket;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.RaidModule;

public class RemoveCmd : ModuleBase<SocketCommandContext>
{
    private readonly IRaidRepository _raidRepository;
    private readonly IGuildRepository _guildRepository;

    public RemoveCmd()
    {
        this._raidRepository = Bot.RaidRepository;
        this._guildRepository = Bot.GuildRepository;
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

        Domain.Raid raid = _raidRepository.GetRaid(guild.Id);

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
}