#r "nuget: System.Text.Json, 8.0.0"
#r "nuget: Newtonsoft.Json, 13.0.4"
#r "nuget: System.Net.Http, 4.3.4"
using System;
using System.Net.Http;
using System.Text.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class JsonFetcher
{
    private readonly HttpClient _httpClient;
    private const string HttpEndpoint = "https://dps.report/getUploadMetadata";
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public JsonFetcher()
    {
        _httpClient = new HttpClient();
    }

    public async Task<T> FetchUploadMetadataById<T>(string id)
    {
        var url = $"{HttpEndpoint}?id={id}";
        return await FetchUploadMetadata<T>(url);
    }

    public async Task<T> FetchUploadMetadataByPermalink<T>(string permalink)
    {
        var url = $"{HttpEndpoint}?permalink={permalink}";
        return await FetchUploadMetadata<T>(url);
    }

    private async Task<T> FetchUploadMetadata<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode(); // Throws an exception if the request failed

        var json = await response.Content.ReadAsStringAsync();
        var metadata = JsonConvert.DeserializeObject<T>(json);
        return metadata;
    }

}