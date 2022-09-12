using System.Reflection;
using Discord.Commands;
using Discord.WebSocket;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy
{
	internal class CommandHandler
	{
		private DiscordSocketClient _client;
		private BotConfiguration _config;
		private CommandService _service;

		private IGuildRepository _guildRepository;
		private IRaidRepository _raidRepository;

		public CommandHandler(DiscordSocketClient client, BotConfiguration config)
		{
			this._guildRepository = Bot.GuildRepository;
			this._raidRepository = Bot.RaidRepository;

			this._client = client;
			this._config = config;
			this._service = new CommandService();
			this._service.AddModulesAsync(Assembly.GetEntryAssembly(), null);

			client.MessageReceived += StartHandle;
		}

		private async Task StartHandle(SocketMessage sm)
		{
			if (!_guildRepository.GuildExists((sm.Author as SocketGuildUser).Guild.Id))
			{
				_guildRepository.AddGuild(new Guild
				{
					Id = (sm.Author as SocketGuildUser).Guild.Id
				});
			}

			try
			{
				var message = sm as SocketUserMessage;
				if (message == null) return;
				int argPos = 0;

				if (!(message.HasStringPrefix(_config.Prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)) || message.Author.IsBot)
					return;

				var context = new SocketCommandContext(_client, message);

				// Command Handler
				await _service.ExecuteAsync(context: context, argPos: argPos, services: null);
			}
			catch { }
		}
	}
}