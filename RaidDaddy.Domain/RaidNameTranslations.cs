namespace RaidDaddy.Domain;

public static class RaidNameTranslations
{
	public static List<RaidTranslation> Translations { get; set; } = new()
	{
		new RaidTranslation(Destiny2Raid.GardenOfSalvation, "gos", "garden", "gardenofsalvation"),
		new RaidTranslation(Destiny2Raid.LastWish, "lw", "wish", "lastwish"),
		new RaidTranslation(Destiny2Raid.DeepStoneCrypt, "dsc", "crypt", "deepstonecrypt"),
		new RaidTranslation(Destiny2Raid.VowOfTheDisciple, "votd", "vow", "disciple", "vowofthedisciple"),
		new RaidTranslation(Destiny2Raid.VaultOfGlass, "vog", "vault", "vaultofglass")
	};

	public static Destiny2Raid GetRaid(string translation)
	{
		foreach (var raid in Translations)
			if (raid.Translations.Contains(translation))
				return raid.Raid;
		return Destiny2Raid.INVALID;
	}
}