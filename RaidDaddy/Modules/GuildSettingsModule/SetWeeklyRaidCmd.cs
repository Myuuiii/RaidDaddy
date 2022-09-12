using Discord.Commands;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.GuildSettingsModule;

public class SetWeeklyRaidCmd : ModuleBase<SocketCommandContext>
{
    private IGuildRepository _guildRepository;

    public SetWeeklyRaidCmd()
    {
        this._guildRepository = Bot.GuildRepository;
    }
    
    [Command("setweeklyraid")]
    public async Task SetWeeklyRaidCommand()
    {
        _guildRepository.SetRaidUpdateChannel(this.Context.Guild.Id, this.Context.Channel.Id);
        await this.Context.Channel.SendMessageAsync("Raid update channel set.");
        await this.Context.Channel.SendMessageAsync("CURRENT RAID: " + Bot.CurrentWeeklyRaid.ToString() + " FOR WEEK " + Bot.CurrentWeek);
    }
}