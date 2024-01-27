using SQLite;
using System.Text.Json;

namespace BertScout2024.Models;

public class TeamMatch : BaseModel
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed(Name = "TeamMatchUnique", Order = 1, Unique = true)]
    public int TeamNumber { get; set; }

    [Indexed(Name = "TeamMatchUnique", Order = 2, Unique = true)]
    public int MatchNumber { get; set; }

    [Indexed(Unique = true)]
    public string Uuid { get; set; } = "";

    public string AirtableId { get; set; } = "";

    public bool Changed { get; set; }

    public bool Deleted { get; set; }

    public string ScoutName { get; set; } = "";

    // autonomous

    public bool Auto_Leave { get; set; }
    public int Auto_Speaker { get; set; }
    public int Auto_Amp { get; set; }
    public int Auto_Points { get; set; } = 0;

    // teleop

    public int Tele_Speaker { get; set; }
    public int Tele_Amp { get; set; }
    public int Tele_Amped_Speaker { get; set; }
    public bool Tele_Coop { get; set; }
    public int Tele_Points { get; set; } = 0;

    // end game

    public bool Endgame_Parked { get; set; }
    public bool Endgame_OnStage { get; set; }
    public bool Endgame_Harmony { get; set; }
    public bool Endgame_Spotlit { get; set; }
    public bool Endgame_Trap { get; set; }
    public int Endgame_Points { get; set; } = 0;

    // overall

    public string Comments { get; set; } = "";

    public int ScoutScore { get; set; } = 0;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, WriteOptions);
    }
}