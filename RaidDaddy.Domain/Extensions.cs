using Discord;

namespace RaidDaddy.Domain;

public static class Extensions
{
	public static string Humanize(this Destiny2Raid raid)
	{
		switch (raid)
		{
			case Destiny2Raid.GardenOfSalvation: return "Garden of Salvation";
			case Destiny2Raid.LastWish: return "Last Wish";
			case Destiny2Raid.DeepStoneCrypt: return "Deep Stone Crypt";
			case Destiny2Raid.VowOfTheDisciple: return "Vow of the Disciple";
			case Destiny2Raid.VaultOfGlass: return "Vault of Glass";
			case Destiny2Raid.KingsFall: return "King's Fall";
			default: return "Unknown";
		}
	}

	public static Color GetColor(this Destiny2Raid raid)
	{
		switch (raid)
		{
			case Destiny2Raid.GardenOfSalvation: return new Color(0x6faf54);
			case Destiny2Raid.LastWish: return new Color(0xb766b4);
			case Destiny2Raid.DeepStoneCrypt: return new Color(0x6c87c9);
			case Destiny2Raid.VowOfTheDisciple: return new Color(0x364730);
			case Destiny2Raid.VaultOfGlass: return new Color(0x6eC3ff);
			default: return Color.Default;
		}
	}

	public static string Humanize(this Destiny2Encounter encounter)
	{
		switch (encounter)
		{
			case Destiny2Encounter.GOS_EvadeConsecrated: return "Evade Consecrated";
			case Destiny2Encounter.GOS_SummonConsecrated: return "Summon Consecrated";
			case Destiny2Encounter.GOS_DefeatConsecrated: return "Defeat Consecrated";
			case Destiny2Encounter.GOS_ConquerSanctified: return "Conquer Sanctified";

			case Destiny2Encounter.LW_Kalli: return "Kalli";
			case Destiny2Encounter.LW_ShuroChi: return "Shuro Chi";
			case Destiny2Encounter.LW_Morgeth: return "Morgeth";
			case Destiny2Encounter.LW_Vault: return "Vault";
			case Destiny2Encounter.LW_Riven: return "Riven";
			case Destiny2Encounter.LW_Queenswalk: return "Queenswalk";

			case Destiny2Encounter.DSC_Blizzard: return "Blizzard";
			case Destiny2Encounter.DSC_SecurityBreach: return "Security Breach";
			case Destiny2Encounter.DSC_Atraks: return "Atraks";
			case Destiny2Encounter.DSC_SpaceWalk: return "Space Walk";
			case Destiny2Encounter.DSC_Descent: return "Descent";
			case Destiny2Encounter.DSC_Taniks: return "Taniks";

			case Destiny2Encounter.VOTD_Payload: return "Payload";
			case Destiny2Encounter.VOTD_Obelisk: return "Obelisk";
			case Destiny2Encounter.VOTD_Caretaker: return "Caretaker";
			case Destiny2Encounter.VOTD_Artifacts: return "Artifacts";
			case Destiny2Encounter.VOTD_Rhulk: return "Rhulk";

			case Destiny2Encounter.VOG_Entrance: return "Entrance";
			case Destiny2Encounter.VOG_Conflux: return "Conflux";
			case Destiny2Encounter.VOG_Oracles: return "Oracles";
			case Destiny2Encounter.VOG_Templar: return "Templar";
			case Destiny2Encounter.VOG_GorgonLabyrinth: return "Gorgon Labyrinth";
			case Destiny2Encounter.VOG_JumpingPuzzle: return "Jumping Puzzle";
			case Destiny2Encounter.VOG_Gatekeeper: return "Gatekeeper";
			case Destiny2Encounter.VOG_Atheon: return "Atheon";

			case Destiny2Encounter.KF_DaughtersOfOryx: return "Daughters of Oryx";
			case Destiny2Encounter.KF_Entrance: return "Entrance";
			case Destiny2Encounter.KF_Golgoroth: return "Golgoroth";
			case Destiny2Encounter.KF_JumpingPuzzle: return "Jumping Puzzle";
			case Destiny2Encounter.KF_Oryx: return "Oryx";
			case Destiny2Encounter.KF_Relics: return "Relics";
			case Destiny2Encounter.KF_Totems: return "Totems";
			case Destiny2Encounter.KF_Warpriest: return "Warpriest";

			case Destiny2Encounter.CLEAN: return "From Start";
			default: return "Unknown";
		}
	}
}