using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RaidDaddy.Models;

namespace RaidDaddy.Modules
{
	public class RaidModule : ModuleBase
	{
		[Command("create")]
		public async Task CreateRaid(Raid raid, [Remainder] string notes = "No Notes")
		{
			Program._data.CurrentRaid = new RaidData(raid, notes);
			Program._data.Save("./data.json");
			await ReplyAsync($"A new {raid} raid has been created. You can join it by executing `{Program._config.Prefix}join`");
		}

		[Command("end")]
		public async Task EndRaid()
		{
			if (Program._data.CurrentRaid != null)
			{
				Program._data.CurrentRaid = null;
				Program._data.Save("./data.json");
				await ReplyAsync("Raid has been ended.");
			}
		}

		[Command("join")]
		public async Task JoinRaid()
		{
			if (Program._data.CurrentRaid == null)
			{
				await ReplyAsync("There is no raid currently running.");
			}
			else
			{
				RaidData raid = Program._data.CurrentRaid;
				if (raid.UserIds.Contains(Context.User.Id))
				{
					await ReplyAsync("You are already in the raid.");
				}
				else
				{
					if (raid.UserIds.Count < 6)
					{
						Program._data.CurrentRaid.Join(Context.User as SocketGuildUser, Program._config.RoleId);
						Program._data.Save("./data.json");
						await ReplyAsync($"{Context.User.Mention} has joined the raid.");
					}
					else
					{
						await ReplyAsync("The raid is full.");
					}
				}
			}
		}

		[Command("leave")]
		public async Task LeaveRaid()
		{
			if (Program._data.CurrentRaid == null)
			{
				await ReplyAsync("There is no raid currently running.");
			}
			else
			{
				RaidData raid = Program._data.CurrentRaid;
				if (!raid.UserIds.Contains(Context.User.Id))
				{
					await ReplyAsync("You are not in the raid.");
				}
				else
				{
					Program._data.CurrentRaid.Leave(Context.User as SocketGuildUser, Program._config.RoleId);
					Program._data.Save("./data.json");
					await ReplyAsync($"{Context.User.Mention} has left the raid.");
				}
			}
		}

		[Command("info")]
		public async Task RaidInfo()
		{
			if (Program._data.CurrentRaid == null)
			{
				await ReplyAsync("There is no raid currently running.");
			}
			else
			{
				RaidData raid = Program._data.CurrentRaid;

				EmbedBuilder builder = new EmbedBuilder();

				switch (raid.Raid)
				{
					case Raid.LW:
						builder.WithTitle("Last Wish Raid");
						builder.WithColor(new Color(248, 185, 255));
						break;
					case Raid.GOS:
						builder.WithTitle("Garden of Salvation Raid");
						builder.WithColor(new Color(81, 170, 100));
						break;
					case Raid.DSC:
						builder.WithTitle("Deep Stone Crypt Raid");
						builder.WithColor(new Color(44, 41, 111));
						break;
					case Raid.VOG:
						builder.WithTitle("Vault of Glass Raid");
						builder.WithColor(new Color(110, 195, 255));
						break;
				}

				StringBuilder sbDescription = new StringBuilder();
				sbDescription.AppendLine($"**Notes:** {raid.Notes}");
				sbDescription.AppendLine($"**Players:** {raid.UserIds.Count}/6");
				sbDescription.AppendLine("");
				sbDescription.AppendLine("**Possible Commands**");
				sbDescription.AppendLine($"Join Raid: `{Program._config.Prefix}join`");
				sbDescription.AppendLine($"Leave Raid: `{Program._config.Prefix}leave`");
				sbDescription.AppendLine($"End Raid: `{Program._config.Prefix}end`");
				sbDescription.AppendLine($"Change Raid: `{Program._config.Prefix}setraid <raidName (VOG/DSC/LW/GOS)>`");
				sbDescription.AppendLine($"Change Notes: `{Program._config.Prefix}setnotes <new notes>`");

				builder.WithDescription(sbDescription.ToString());
				builder.WithThumbnailUrl("http://cdn.mutedevs.nl/d2raid.png");
				builder.WithImageUrl("http://cdn.mutedevs.nl/PerfectedEntropy.png");

				StringBuilder sb = new StringBuilder();
				foreach (string userName in raid.UserNames)
					sb.AppendLine($"- {userName}");

				if (!string.IsNullOrWhiteSpace(sb.ToString()))
				{
					builder.AddField("Fireteam", sb.ToString());
				}
				else
				{
					builder.AddField("Fireteam", "-- Empty --");
				}
				await ReplyAsync("", false, builder.Build());
			}
		}

		[Command("setnotes")]
		public async Task SetNotes([Remainder] string notes)
		{
			if (Program._data.CurrentRaid == null)
			{
				await ReplyAsync("There is no raid currently running.");
			}
			else
			{
				Program._data.CurrentRaid.Notes = notes;
				Program._data.Save("./data.json");
				await ReplyAsync($"The notes have been set to `{notes}`");
			}
		}

		[Command("setraid")]
		public async Task SetRaid(Raid raid)
		{
			if (Program._data.CurrentRaid == null)
			{
				await ReplyAsync("There is no raid currently running.");
			}
			else
			{
				Program._data.CurrentRaid.Raid = raid;
				Program._data.Save("./data.json");
				await ReplyAsync($"The raid has been set to `{raid}`");
			}
		}

		[Command("setrole")]
		public async Task SetRole([Remainder] SocketRole role)
		{
			Program._config.RoleId = role.Id;
			Program._config.Save("./config.json");
			await ReplyAsync($"The role has been set to `{role.Name}`");
		}

		[Command("help")]
		public async Task Help()
		{
			EmbedBuilder builder = new EmbedBuilder();
			builder.WithTitle("Help");
			builder.WithColor(Color.Blue);

			StringBuilder sb = new StringBuilder();
			sb.AppendLine("**Commands** <> = required, () = optional");
			sb.AppendLine($"`{Program._config.Prefix}help` - Shows this message");
			sb.AppendLine($"`{Program._config.Prefix}create <raidName> (notes)` - Creates a new raid");
			sb.AppendLine($"`{Program._config.Prefix}join` - Joins the raid");
			sb.AppendLine($"`{Program._config.Prefix}leave` - Leaves the raid");
			sb.AppendLine($"`{Program._config.Prefix}end` - Ends the raid");
			sb.AppendLine($"`{Program._config.Prefix}info` - Shows the raid info");
			sb.AppendLine($"`{Program._config.Prefix}setnotes <new notes>` - Sets the notes");
			sb.AppendLine($"`{Program._config.Prefix}setraid <raidName (VOG/DSC/LW/GOS)>` - Sets the raid");
			sb.AppendLine($"`{Program._config.Prefix}setrole <role name/id>` - Sets the role");
			builder.WithDescription(sb.ToString());
			builder.WithImageUrl("http://cdn.mutedevs.nl/PerfectedEntropy.png");
			await ReplyAsync(null, false, builder.Build());
		}
	}
}