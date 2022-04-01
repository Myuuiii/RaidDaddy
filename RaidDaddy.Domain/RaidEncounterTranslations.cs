namespace RaidDaddy.Domain;


public static class RaidEncounterTranslations
{
	public static List<EncounterTranslation> Translations { get; set; } = new()
	{
		// Garden of Salvation
		new EncounterTranslation(Destiny2Raid.GardenOfSalvation, Destiny2Encounter.GOS_EvadeConsecrated, "gos_evade", "evade", "opening", "start", "entrance"),
		new EncounterTranslation(Destiny2Raid.GardenOfSalvation, Destiny2Encounter.GOS_SummonConsecrated, "gos_summon", "summon"),
		new EncounterTranslation(Destiny2Raid.GardenOfSalvation, Destiny2Encounter.GOS_DefeatConsecrated, "gos_defeat", "defeat", "consecrated"),
		new EncounterTranslation(Destiny2Raid.GardenOfSalvation, Destiny2Encounter.GOS_ConquerSanctified, "gos_conquer", "conquer", "sanctified", "boss", "final"),

		// Last Wish
		new EncounterTranslation(Destiny2Raid.LastWish, Destiny2Encounter.LW_Kalli, "lw_kalli", "kalli", "opening", "start", "entrance"),
		new EncounterTranslation(Destiny2Raid.LastWish, Destiny2Encounter.LW_ShuroChi, "lw_shurochi", "shurochi"),
		new EncounterTranslation(Destiny2Raid.LastWish, Destiny2Encounter.LW_Morgeth, "lw_morgeth", "morgeth"),
		new EncounterTranslation(Destiny2Raid.LastWish, Destiny2Encounter.LW_Vault, "lw_vault", "vault"),
		new EncounterTranslation(Destiny2Raid.LastWish, Destiny2Encounter.LW_Riven, "lw_riven", "riven", "boss"),
		new EncounterTranslation(Destiny2Raid.LastWish, Destiny2Encounter.LW_Queenswalk, "lw_queenswalk", "queenswalk", "final"),

		// Deep Stone Crypt
		new EncounterTranslation(Destiny2Raid.DeepStoneCrypt, Destiny2Encounter.DSC_Blizzard, "dsc_blizzard", "blizzard", "opening", "start", "entrance"),
		new EncounterTranslation(Destiny2Raid.DeepStoneCrypt, Destiny2Encounter.DSC_SecurityBreach, "dsc_security", "security", "breach"),
		new EncounterTranslation(Destiny2Raid.DeepStoneCrypt, Destiny2Encounter.DSC_Atraks, "dsc_atraks", "atraks"),
		new EncounterTranslation(Destiny2Raid.DeepStoneCrypt, Destiny2Encounter.DSC_SpaceWalk, "dsc_spacewalk", "spacewalk"),
		new EncounterTranslation(Destiny2Raid.DeepStoneCrypt, Destiny2Encounter.DSC_Descent, "dsc_descent", "descent", "descend", "nuclear"),
		new EncounterTranslation(Destiny2Raid.DeepStoneCrypt, Destiny2Encounter.DSC_Taniks, "dsc_taniks", "taniks", "boss", "final"),

		// Vow of the Disciple
		new EncounterTranslation(Destiny2Raid.VowOfTheDisciple, Destiny2Encounter.VOTD_Payload, "votd_payload", "payload", "opening", "start", "entrance"),
		new EncounterTranslation(Destiny2Raid.VowOfTheDisciple, Destiny2Encounter.VOTD_Obelisk, "votd_obelisk", "obelisk"),
		new EncounterTranslation(Destiny2Raid.VowOfTheDisciple, Destiny2Encounter.VOTD_Caretaker, "votd_caretaker", "caretaker"),
		new EncounterTranslation(Destiny2Raid.VowOfTheDisciple, Destiny2Encounter.VOTD_Artifacts, "votd_artifacts", "artifacts"),
		new EncounterTranslation(Destiny2Raid.VowOfTheDisciple, Destiny2Encounter.VOTD_Rhulk, "votd_rhulk", "rhulk", "boss", "final"),

		// Vault of Glass
		new EncounterTranslation(Destiny2Raid.VaultOfGlass, Destiny2Encounter.VOG_Entrance, "vog_entrance", "entrance", "opening", "start", "entrance"),
		new EncounterTranslation(Destiny2Raid.VaultOfGlass, Destiny2Encounter.VOG_Conflux, "vog_conflux", "conflux"),
		new EncounterTranslation(Destiny2Raid.VaultOfGlass, Destiny2Encounter.VOG_Oracles, "vog_oracles", "oracles"),
		new EncounterTranslation(Destiny2Raid.VaultOfGlass, Destiny2Encounter.VOG_Templar, "vog_templar", "templar"),
		new EncounterTranslation(Destiny2Raid.VaultOfGlass, Destiny2Encounter.VOG_GorgonLabyrinth, "vog_gorgons", "gorgons"),
		new EncounterTranslation(Destiny2Raid.VaultOfGlass, Destiny2Encounter.VOG_JumpingPuzzle, "vog_jumpingpuzzle", "jumpingpuzzle", "jump"),
		new EncounterTranslation(Destiny2Raid.VaultOfGlass, Destiny2Encounter.VOG_Gatekeeper, "vog_gatekeeper", "gatekeeper"),
		new EncounterTranslation(Destiny2Raid.VaultOfGlass, Destiny2Encounter.VOG_Atheon, "vog_atheon", "atheon", "boss", "final"),
	};

	public static Destiny2Encounter GetEncounter(Destiny2Raid raid, string translation)
	{
		if (Translations.Any(t => t.Raid == raid && t.Translations.Contains(translation.ToLower())))
			return Translations.Single(t => t.Raid == raid && t.Translations.Contains(translation.ToLower())).Encounter;
		return Destiny2Encounter.INVALID;
	}

	public static EncounterTranslation GetTranslation(Destiny2Encounter encounter)
	{
		return Translations.Single(t => t.Encounter == encounter);
	}
}