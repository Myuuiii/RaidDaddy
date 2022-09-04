namespace RaidDaddy.Domain;

public enum Destiny2Encounter
{
	INVALID,
	CLEAN,

	// Garden of Salvation
	GOS_EvadeConsecrated,
	GOS_SummonConsecrated,
	GOS_DefeatConsecrated,
	GOS_ConquerSanctified,

	// Last Wish
	LW_Kalli,
	LW_ShuroChi,
	LW_Morgeth,
	LW_Vault,
	LW_Riven,
	LW_Queenswalk,

	// Deep Stone Crypt
	DSC_Blizzard,
	DSC_SecurityBreach,
	DSC_Atraks,
	DSC_SpaceWalk,
	DSC_Descent,
	DSC_Taniks,

	// Vow of the Disciple
	VOTD_Payload,
	VOTD_Obelisk,
	VOTD_Caretaker,
	VOTD_Artifacts,
	VOTD_Rhulk,

	// Vault of Glass
	VOG_Entrance,
	VOG_Conflux,
	VOG_Oracles,
	VOG_Templar,
	VOG_GorgonLabyrinth,
	VOG_JumpingPuzzle,
	VOG_Gatekeeper,
	VOG_Atheon,

	// Kings Fall
	KF_Entrance,
	KF_Relics,
	KF_Totems,
	KF_Warpriest,
	KF_Golgoroth,
	KF_JumpingPuzzle,
	KF_DaughtersOfOryx,
	KF_Oryx
}