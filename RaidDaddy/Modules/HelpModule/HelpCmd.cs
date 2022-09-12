using System.Text;
using Discord;
using Discord.Commands;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.HelpModule;

public class HelpCmd : ModuleBase<SocketCommandContext>
{
    public HelpCmd()
    {
    }
    
    [Command("help")]
    public async Task HelpCommand()
    {
        EmbedBuilder embed = new EmbedBuilder();
        embed.WithTitle("RaidDaddy Help");
        embed.WithColor(Color.Blue);

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("**The ones you guys know**");
        sb.AppendLine($"`{Bot.Config.Prefix}create <raid name> [encounter name]` - Creates a raid");
        sb.AppendLine($"`{Bot.Config.Prefix}join` - Joins the raid");
        sb.AppendLine($"`{Bot.Config.Prefix}leave` - Leaves the raid");
        sb.AppendLine($"`{Bot.Config.Prefix}list` - Lists all raids and encounters");
        sb.AppendLine($"`{Bot.Config.Prefix}disband` - Disbands the raid");
        sb.AppendLine($"`{Bot.Config.Prefix}help` - Shows this help message");
        sb.AppendLine(Environment.NewLine);
        sb.AppendLine("**The ones you guys don't know**");
        sb.AppendLine($"`{Bot.Config.Prefix}info` - Lists all raids and encounters");
        sb.AppendLine($"`{Bot.Config.Prefix}reserve <user>` - Adds the given user to the raid");
        sb.AppendLine($"`{Bot.Config.Prefix}remove <user>` - Removes the given user from the raid");
        sb.AppendLine($"`{Bot.Config.Prefix}set <encounter/raid> <value>`  - Sets the raid/encounter for the currently running raid. All translations are 1 word, no spaces.");

        embed.WithDescription(sb.ToString());
        await ReplyAsync("", false, embed.Build());
    }
}