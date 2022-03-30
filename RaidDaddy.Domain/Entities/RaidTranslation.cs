namespace RaidDaddy.Domain;

public class RaidTranslation
{
	public RaidTranslation()
	{
		this.Translations = new();
	}

	public RaidTranslation(Destiny2Raid raid, params string[] translations)
	{
		this.Raid = raid;
		this.Translations = translations.ToList();
	}

	public Destiny2Raid Raid { get; set; }
	public List<string> Translations { get; set; }
}