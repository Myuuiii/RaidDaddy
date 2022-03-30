using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;
using YamlDotNet.Serialization;

namespace RaidDaddy;

public class Bot
{
	private static DiscordSocketClient _client;
	public static BotConfiguration _config;
	private CommandService _commandService;
	private CommandHandler _handler;

	public static IRaidRepository _raidRepository;
	public static IGuildRepository _guildRepository;

	public Bot()
	{
		_raidRepository = new RaidRepository();
		_guildRepository = new GuildRepository();
	}

	public static void Main(string[] args)
	{
		new Bot().MainAsync().GetAwaiter().GetResult();
	}

	public async Task MainAsync()
	{
		if (!File.Exists(BotConfiguration._fileName))
			File.WriteAllText(BotConfiguration._fileName, new Serializer().Serialize(new BotConfiguration()));
		_config = new Deserializer().Deserialize<BotConfiguration>((string)File.ReadAllText(BotConfiguration._fileName));

		_commandService = new CommandService();
		_client = new DiscordSocketClient(new DiscordSocketConfig
		{
			AlwaysDownloadUsers = true
		});

		_handler = new CommandHandler(_client, _config);

		if (string.IsNullOrEmpty(_config.Token))
		{
			throw new Exception("Token is not set!");
		}

		await _client.LoginAsync(TokenType.Bot, _config.Token);
		await _client.StartAsync();

		System.Console.WriteLine("Bot online");
		await Task.Delay(-1);
	}
}