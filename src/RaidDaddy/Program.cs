using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using RaidDaddy.Handlers;
using RaidDaddy.Models;
using RaidDaddy.Models.Configuration;

namespace RaidDaddy
{
	class Program
	{
		public static DiscordSocketClient _client;
		public static RaidDaddyConfiguration _config;
		public static RaidDaddyData _data;
		public static CommandHandler _commandHandler;
		static void Main(string[] args) => Program.MainAsync().GetAwaiter().GetResult();

		public static async Task MainAsync()
		{
			_client = new DiscordSocketClient();
			_config = Functions.ConfigLoader.LoadBotConfig("./config.json");
			_data = Functions.DataLoader.LoadData("./data.json");

			await _client.LoginAsync(Discord.TokenType.Bot, _config.BotToken);
			await _client.StartAsync();

			await _client.SetGameAsync($"{_config.Prefix}", null, ActivityType.Listening);
			_commandHandler = new CommandHandler(_client, _config);

			await Task.Delay(-1);
		}
	}
}
