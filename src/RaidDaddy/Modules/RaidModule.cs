using System;
using System.Collections.Generic;
using System.Linq;
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
			await ReplyAsync(Program._data.Quotes[new Random().Next(Program._data.Quotes.Count)] + $"<@&{Program._config.RoleId}>");
			await ReplyAsync($"A new {raid} raid has been created. You can join it by executing `{Program._config.Prefix}join`");
		}

		[Command("end")]
		public async Task EndRaid()
		{
			if (Program._data.CurrentRaid != null)
			{
				Program._data.CurrentRaid = null;
				Program._data.Save("./data.json");
				await ReplyAsync("Raid fireteam has been disbanded.");
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
						Program._data.CurrentRaid.Join(Context.User as SocketGuildUser);
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
					Program._data.CurrentRaid.Leave(Context.User as SocketGuildUser);
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

		[Command("start")]
		public async Task StartRaid()
		{
			if (Program._data.CurrentRaid == null)
			{
				await ReplyAsync("There is no raid currently running.");
			}
			else
			{
				await ReplyAsync($"<@&{Program._config.RoleId}>. The raid is starting!");
			}
		}

		[Command("addquote")]
		public async Task AddQuote([Remainder] string quote)
		{
			Program._data.Quotes.Add(quote);
			Program._data.Save("./data.json");
			await ReplyAsync("The quote has been added");
		}

		[Command("quotes")]
		public async Task Quotes()
		{
			StringBuilder sb = new StringBuilder();
			int i = 0;
			foreach (string quote in Program._data.Quotes)
			{
				sb.AppendLine($"**{i}** - {quote}");
				i++;
			}

			if (!string.IsNullOrWhiteSpace(sb.ToString()))
			{
				await ReplyAsync(sb.ToString());
			}
			else
			{
				await ReplyAsync("There are no quotes.");
			}
		}

		[Command("removequote")]
		public async Task RemoveQuote(int index)
		{
			if (index >= 0 && index < Program._data.Quotes.Count)
			{
				Program._data.Quotes.RemoveAt(index);
				Program._data.Save("./data.json");
				await ReplyAsync("The quote has been removed");
			}
			else
			{
				await ReplyAsync("There is no quote at that index.");
			}
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
			sb.AppendLine($"`{Program._config.Prefix}start` - Pings the raid role notifying the start of the raid");
			sb.AppendLine($"");
			sb.AppendLine($"`{Program._config.Prefix}addquote <quote>` - Add a new quote");
			sb.AppendLine($"`{Program._config.Prefix}quotes` - List all the quites");
			sb.AppendLine($"`{Program._config.Prefix}removequote <quote index>` - Remove a quote");
			builder.WithDescription(sb.ToString());
			builder.WithImageUrl("http://cdn.mutedevs.nl/PerfectedEntropy.png");
			await ReplyAsync(null, false, builder.Build());
		}
	}
}