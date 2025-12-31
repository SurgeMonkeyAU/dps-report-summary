#load "Common/JsonFetcher.csx"
#load "DpsReport/UploadMetadata.csx"
using System;
using System.Runtime.CompilerServices;

var jsonFetcher = new JsonFetcher();
var reports = new List<string>(){};
var fileListOfReports = "./reports.txt";
var summaryFile = "./summary-report.txt";

if( reports.Count <= 0 )
{
    if( File.Exists(fileListOfReports) )
    {
        reports = File.ReadAllLines(fileListOfReports).ToList();
    }
}

if( reports.Count <= 0 )
{
    Console.WriteLine("");
    Console.WriteLine($" No reports found in: {fileListOfReports}");
    Console.WriteLine("");
    System.Environment.Exit(1);
}

FileStream reportStream;
if( !File.Exists(summaryFile) )
    reportStream = File.Create(summaryFile);
else
{
    reportStream = new(summaryFile, FileMode.Truncate, FileAccess.ReadWrite);
}
var writer = new StreamWriter(reportStream);

var summary = new ReportSummary();
foreach( var permalink in reports )
{
    var metadata = await jsonFetcher.FetchUploadMetadataByPermalink<UploadMetadata>(permalink);
    var localTime = UploadMetadata.ConvertToLocalTimeZone(metadata.GetEncounterTime());
    Console.WriteLine($"Processing Encounter: {metadata.Id}, Date: {localTime:yyyy-MM-dd HH:mm:sszzz}");
    writer.WriteLine($"Processing Encounter: {metadata.Id}, Date: {localTime:yyyy-MM-dd HH:mm:sszzz}");

    var uniquePlayers = new HashSet<PlayerMetaData>();
    foreach (var ( _, player) in metadata.Players)
    {
        uniquePlayers.Add(player);
    }

    var day = $"{localTime:yyyy-MM-dd}";

    // Add the players in attendance to the current date.
    if( !summary.PlayersAttendance.TryAdd(day, uniquePlayers) )
    {
        // If a key exists already, merge with the existing hashset.
        summary.PlayersAttendance[day].UnionWith(uniquePlayers);
    }
}

var playerCounts = summary.SummarizePlayerCounts();
var orderedPlayerCounts = playerCounts.OrderByDescending(pair => pair.Value);
var (firstDate, lastDate) = summary.GetFirstAndLastDate();
var headers = new List<string>(){"  == Account Name ==", "== Days Attended =="};

Console.WriteLine(string.Concat(Enumerable.Repeat('-',80)));
Console.WriteLine($"Summary of Encounters");
Console.WriteLine($"  From {firstDate:yyyy-MM-dd}");
Console.WriteLine($"    To {lastDate:yyyy-MM-dd}");
Console.WriteLine($"{headers[0].PadRight(28)} {headers[1]}");

writer.WriteLine(string.Concat(Enumerable.Repeat('-',80)));
writer.WriteLine($"Summary of Encounters");
writer.WriteLine($"  From {firstDate:yyyy-MM-dd}");
writer.WriteLine($"    To {lastDate:yyyy-MM-dd}");
writer.WriteLine($"{headers[0].PadRight(28)} {headers[1]}");

foreach (var (playerName, count) in orderedPlayerCounts)
{
    Console.WriteLine($"  - {playerName.PadRight(24)} : {count}");
    writer.WriteLine($"  - {playerName.PadRight(24)} : {count}");
}

writer.Close();
writer.Dispose();
reportStream.Close();
reportStream.Dispose();

if (!Console.IsInputRedirected)
{
    Console.WriteLine("Press any key to continue...");
    Console.ReadKey();
}
System.Environment.Exit(0);