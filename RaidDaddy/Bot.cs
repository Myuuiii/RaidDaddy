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
	public static BotConfiguration Config;
	private CommandService _commandService;
	private CommandHandler _handler;

	public static IRaidRepository RaidRepository;
	public static IGuildRepository GuildRepository;
	private static System.Timers.Timer _weeklyTimerCheck;
	public static int CurrentWeek;
	public static WeeklyRaid CurrentWeeklyRaid;
	private static Logger _logger;

	public Bot()
	{
		RaidRepository = new RaidRepository();
		GuildRepository = new GuildRepository();
		_weeklyTimerCheck = new System.Timers.Timer(3600);
		_weeklyTimerCheck.Elapsed += _weeklyTimerCheck_Elapsed;
		_logger = new LoggerConfiguration()
			.MinimumLevel.Debug()
			.WriteTo.Console()
			.CreateLogger();
		CurrentWeek = GetWeekNr();
	}

	private void _weeklyTimerCheck_Elapsed(object sender, ElapsedEventArgs e)
	{
		if (CurrentWeek == GetWeekNr()) return;
		CurrentWeek = GetWeekNr();
		CurrentWeeklyRaid = (WeeklyRaid)(CurrentWeek % Enum.GetValues(typeof(WeeklyRaid)).Length);

		foreach (Guild guild in GuildRepository.GetGuilds().Where(guild => guild.UpdateChannelId != 0))
			_client.GetGuild(guild.Id).GetTextChannel(guild.UpdateChannelId)
				.SendMessageAsync($"Weekly raid is now: {CurrentWeeklyRaid}");
		_weeklyTimerCheck.Stop();
		_weeklyTimerCheck.Start();
	}


	public static void Main(string[] args)
	{
		new Bot().MainAsync().GetAwaiter().GetResult();
	}

	private async Task MainAsync()
	{
		if (!File.Exists(BotConfiguration._fileName))
		{
			await File.WriteAllTextAsync(BotConfiguration._fileName, new Serializer().Serialize(new BotConfiguration()));
			_logger.Information("Created a new configuration file");
		}
		Config = new Deserializer().Deserialize<BotConfiguration>(await File.ReadAllTextAsync(BotConfiguration._fileName));
		_logger.Information("Loaded configuration");

		_commandService = new CommandService();
		_logger.Information("Created command service");

		_client = new DiscordSocketClient(new DiscordSocketConfig
		{
			AlwaysDownloadUsers = true,
			GatewayIntents = GatewayIntents.All
		});
		_logger.Information("Created discord client");

		_handler = new CommandHandler(_client, Config);
		_logger.Information("Created command handler");

		if (string.IsNullOrEmpty(Config.Token))
		{
			_logger.Fatal("Token is empty, please fill it in the config file");
			throw new Exception("Token is not set!");
		}

		_logger.Information("Logging in...");
		await _client.LoginAsync(TokenType.Bot, Config.Token);
		_logger.Information("Logged in");
		_logger.Information("Starting...");
		await _client.StartAsync();
		_logger.Information("Started");

		_logger.Information("Bot is online");
		await Task.Delay(-1);
	}

	private static int GetWeekNr()
	{
		DateTime dt = DateTime.Now;
		Calendar cal = new CultureInfo("en-US").Calendar;
		int week = cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
		return week;
	}
}