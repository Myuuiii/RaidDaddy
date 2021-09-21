using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using RaidDaddy.Models.Configuration;

namespace RaidDaddy.Handlers
{
	public class CommandHandler
	{
		private DiscordSocketClient _client;
		private CommandService _commandService;
		private RaidDaddyConfiguration _config;

		public CommandHandler(DiscordSocketClient client, RaidDaddyConfiguration config)
		{
			_client = client;
			_config = config;
			_commandService = new CommandService();

			_commandService.AddModulesAsync(Assembly.GetEntryAssembly(), null);

			_client.MessageReceived += HandleMessageReceived;
		}

		private async Task HandleMessageReceived(SocketMessage sm)
		{
			try
			{
				Int32 argPos = 0;

				if (sm.Author.IsBot || sm.Author.IsWebhook) { return; }
				if (sm.Content == null) { return; }

				SocketCommandContext socketContext = new SocketCommandContext(_client, (sm as SocketUserMessage));
				SocketUserMessage socketUserMessage = socketContext.Message as SocketUserMessage;

				if (socketUserMessage.HasStringPrefix(_config.Prefix, ref argPos))
				{
					IResult result = await _commandService.ExecuteAsync(socketContext, argPos, null);
				}
			}
			catch (Exception e)
			{
				// Error handling
			}
		}
	}
}