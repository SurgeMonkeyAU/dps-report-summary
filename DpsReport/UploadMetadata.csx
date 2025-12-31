#load "EncounterMetaData.csx"
#load "EvtcMetaData.csx"
#load "PlayerMetaData.csx"
#load "ReportMetaData.csx"
#load "ReportSummary.csx"

using System;

public class UploadMetadata
{
    public string Id { get; set; }
    public string Permalink { get; set; }
    public long UploadTime { get; set; }
    public long EncounterTime { get; set; }
    public string Generator { get; set; }
    public int GeneratorId { get; set; }
    public int GeneratorVersion { get; set; }
    public string Language { get; set; }
    public int LanguageId { get; set; }
    public EvtcMetaData Evtc { get; set; }
    public Dictionary<string, PlayerMetaData> Players { get; set; }
    public EncounterMetaData Encounter { get; set; }
    public ReportMetaData Report { get; set; }

    public override string ToString() => $"UploadMetadata(Id:{Id}, Permalink:{Permalink}, UploadTime:{UploadTime}, EncounterTime:{EncounterTime}, Generator:{Generator}, GeneratorId:{GeneratorId}, GeneratorVersion:{GeneratorVersion}, Language:{Language}, LanguageId:{LanguageId})";

    public DateTimeOffset GetUploadTime()
    {
        DateTimeOffset unixEpoch = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        return unixEpoch.AddSeconds(UploadTime);
    }

    public DateTimeOffset GetEncounterTime()
    {
        DateTimeOffset unixEpoch = new(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);
        return unixEpoch.AddSeconds(EncounterTime);
    }

    public static DateTime ConvertToLocalTimeZone(DateTimeOffset utcTime)
    {
        TimeZoneInfo localTimeZone = TimeZoneInfo.Local;
        return TimeZoneInfo.ConvertTimeFromUtc(utcTime.UtcDateTime, localTimeZone);
    }
}





