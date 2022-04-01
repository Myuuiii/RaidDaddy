using System.Text;
using Discord;
using Discord.Commands;

namespace RaidDaddy.Modules;

public class HelpModule : ModuleBase<SocketCommandContext>
{
	[Command("help")]
	public async Task HelpCommand()
	{
		EmbedBuilder embed = new EmbedBuilder();
		embed.WithTitle("RaidDaddy Help");
		embed.WithColor(Color.Blue);

		StringBuilder sb = new StringBuilder();
		sb.AppendLine("**The ones you guys know**");
		sb.AppendLine($"`{Bot._config.Prefix}create <raid name> [encounter name]` - Creates a raid");
		sb.AppendLine($"`{Bot._config.Prefix}join` - Joins the raid");
		sb.AppendLine($"`{Bot._config.Prefix}leave` - Leaves the raid");
		sb.AppendLine($"`{Bot._config.Prefix}list` - Lists all raids and encounters");
		sb.AppendLine($"`{Bot._config.Prefix}disband` - Disbands the raid");
		sb.AppendLine($"`{Bot._config.Prefix}help` - Shows this help message");
		sb.AppendLine(Environment.NewLine);
		sb.AppendLine("**The ones you guys don't know**");
		sb.AppendLine($"`{Bot._config.Prefix}info` - Lists all raids and encounters");
		sb.AppendLine($"`{Bot._config.Prefix}reserve <user>` - Adds the given user to the raid");
		sb.AppendLine($"`{Bot._config.Prefix}remove <user>` - Removes the given user from the raid");
		sb.AppendLine($"`{Bot._config.Prefix}set <encounter/raid> <value>`  - Sets the raid/encounter for the currently running raid.");

		embed.WithDescription(sb.ToString());
		await ReplyAsync("", false, embed.Build());
	}
}