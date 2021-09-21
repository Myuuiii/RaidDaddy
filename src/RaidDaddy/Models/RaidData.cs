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

		public bool Join(SocketGuildUser user, ulong roleId)
		{
			if (!UserIds.Contains(user.Id))
			{
				UserIds.Add(user.Id);
				UserNames.Add(user.Username);
				if (!user.Roles.Any(r => r.Id == roleId))
				{
					user.AddRoleAsync(roleId);
				}
				return true;
			}
			return false;
		}

		public bool Leave(SocketGuildUser user, ulong roleId)
		{
			if (UserIds.Contains(user.Id))
			{
				UserIds.Remove(user.Id);
				UserNames.Remove(user.Username);
				if (user.Roles.Any(r => r.Id == roleId))
				{
					user.RemoveRoleAsync(roleId);
				}
				return true;
			}
			return false;
		}
	}
}