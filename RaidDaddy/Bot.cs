using System.Globalization;
using System.Timers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;
using Serilog;
using Serilog.Core;
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
	public static System.Timers.Timer _weeklyTimerCheck;
	public static int _currentWeek;
	public static WeeklyRaid _currentWeeklyRaid;
	public static Logger _logger;

	public Bot()
	{
		_raidRepository = new RaidRepository();
		_guildRepository = new GuildRepository();
		_weeklyTimerCheck = new System.Timers.Timer(3600);
		_weeklyTimerCheck.Elapsed += _weeklyTimerCheck_Elapsed;
		_logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.Console()
			.CreateLogger();
		_currentWeek = GetWeekNr();
	}

	private void _weeklyTimerCheck_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (_currentWeek != GetWeekNr())
		{
			_currentWeek = GetWeekNr();
			_currentWeeklyRaid = (WeeklyRaid)(_currentWeek % Enum.GetValues(typeof(WeeklyRaid)).Length);

			foreach (Guild guild in _guildRepository.GetGuilds())
			{
				if (guild.UpdateChannelId != 0)
				{
					_client.GetGuild(guild.Id).GetTextChannel(guild.UpdateChannelId).SendMessageAsync($"Weekly raid is now: {_currentWeeklyRaid}");
				}
			}
			_weeklyTimerCheck.Stop();
			_weeklyTimerCheck.Start();
		}
	}


	public static void Main(string[] args)
	{
		new Bot().MainAsync().GetAwaiter().GetResult();
	}

	public async Task MainAsync()
	{
		if (!File.Exists(BotConfiguration._fileName))
		{
			File.WriteAllText(BotConfiguration._fileName, new Serializer().Serialize(new BotConfiguration()));
			_logger.Information("Created a new configuration file");
		}
		_config = new Deserializer().Deserialize<BotConfiguration>((string)File.ReadAllText(BotConfiguration._fileName));
		_logger.Information("Loaded configuration");

		_commandService = new CommandService();
		_logger.Information("Created command service");

		_client = new DiscordSocketClient(new DiscordSocketConfig
		{
			AlwaysDownloadUsers = true,
			GatewayIntents = GatewayIntents.All
		});
		_logger.Information("Created discord client");

		_handler = new CommandHandler(_client, _config);
		_logger.Information("Created command handler");

		if (string.IsNullOrEmpty(_config.Token))
		{
			_logger.Fatal("Token is empty, please fill it in the config file");
			throw new Exception("Token is not set!");
		}

		_logger.Information("Logging in...");
		await _client.LoginAsync(TokenType.Bot, _config.Token);
		_logger.Information("Logged in");
		_logger.Information("Starting...");
		await _client.StartAsync();
		_logger.Information("Started");

		_logger.Information("Bot is online");
		await Task.Delay(-1);
	}

	private int GetWeekNr()
	{
		DateTime dt = DateTime.Now;
		Calendar cal = new CultureInfo("en-US").Calendar;
		int week = cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
		return week;
	}
}