public class EncounterMetaData
{
    public int? UniqueId { get; set; }
    public bool Success { get; set; }
    public float Duration { get; set; }
    public int CompDps { get; set; }
    public int NumberOfPlayers { get; set; }
    public int NumberOfGroups { get; set; }
    public int BossId { get; set; }
    public string Boss { get; set; }
    public bool IsCm { get; set; }
    public bool IsLegendaryCm { get; set; }
    public int Emboldened { get; set; }
    public int Gw2Build { get; set; }
    public bool JsonAvailable { get; set; }
}