using System;
using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;

namespace RaidDaddy.Models
{
	public class RaidData
	{
		public RaidData(Raid raid, string notes)
		{
			Raid = raid;
			Notes = notes;
		}

		public Raid Raid { get; set; } = Raid.LW;
		public string Notes { get; set; } = "";
		public List<ulong> UserIds { get; set; } = new List<ulong>();
		public List<string> UserNames { get; set; } = new List<string>();

		public bool Join(SocketGuildUser user)
		{
			if (!UserIds.Contains(user.Id))
			{
				UserIds.Add(user.Id);
				UserNames.Add(user.Username);
				return true;
			}
			return false;
		}

		public bool Leave(SocketGuildUser user)
		{
			if (UserIds.Contains(user.Id))
			{
				UserIds.Remove(user.Id);
				UserNames.Remove(user.Username);
				return true;
			}
			return false;
		}
	}
}