using System.Text;
using Discord.Commands;
using RaidDaddy.Domain;
using RaidDaddy.Domain.Repositories;

namespace RaidDaddy.Modules.RaidModule;

public class ListCmd : ModuleBase<SocketCommandContext>
{
    public ListCmd()
    {
    }
    
    /// <summary>
    /// Will list all the raids and or encounters
    /// </summary>
    [Command("list")]
    public async Task ListCommand()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("__**Raids and Encounters**__");
        sb.AppendLine(Environment.NewLine);

        foreach (RaidTranslation translation in RaidNameTranslations.Translations)
        {
            sb.AppendLine($"- **{translation.Raid.Humanize()}** [`{String.Join(", ", translation.Translations)}`]");
            foreach (EncounterTranslation encounterTranslation in RaidEncounterTranslations.Translations.Where(r => r.Raid == translation.Raid))
            {
                sb.AppendLine($"\t\t - *{encounterTranslation.Encounter.Humanize()}* [`{String.Join(", ", encounterTranslation.Translations)}`]");
            }
            sb.AppendLine(Environment.NewLine);
        }
        await ReplyAsync(sb.ToString());
    }
}