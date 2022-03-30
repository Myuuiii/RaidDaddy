namespace RaidDaddy.Domain;

public class EncounterTranslation
{
	public EncounterTranslation()
	{
		this.Translations = new();
	}
	public EncounterTranslation(Destiny2Raid raid, Destiny2Encounter encounter, params string[] translations)
	{
		this.Raid = raid;
		this.Encounter = encounter;
		this.Translations = translations.ToList();
	}

	public Destiny2Encounter Encounter { get; set; }
	public Destiny2Raid Raid { get; set; }
	public List<string> Translations { get; set; }
}