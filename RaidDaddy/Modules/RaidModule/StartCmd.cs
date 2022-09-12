using System.Text;
using Discord.Commands;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.RaidModule;

public class StartCmd : ModuleBase<SocketCommandContext>
{
    private readonly IRaidRepository _raidRepository;
    private readonly IGuildRepository _guildRepository;

    public StartCmd()
    {
        this._raidRepository = Bot.RaidRepository;
        this._guildRepository = Bot.GuildRepository;
    }

    /// <summary>
    /// Will ping all the members in the raid telling them that the raid is starting
    /// </summary>
    [Command("start")]
    public async Task StartCommand()
    {
        Guild guild = _guildRepository.GetGuild(Context.Guild.Id);
        bool raidExists = _raidRepository.RaidExists(guild.Id);

        if (!raidExists)
        {
            await ReplyAsync(StaticValues.NoActiveRaid);
            return;
        }

        Raid raid = _raidRepository.GetRaid(guild.Id);
        StringBuilder sb = new StringBuilder();
        sb.Append("  ");
        foreach (ulong userId in raid.Raiders)
        {
            sb.Append("<@" + userId + "> ");
        }

        await ReplyAsync(StaticValues.RaidStarted + sb, embed: raid.GetEmbed());
    }
}