using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker.Http;


namespace Bodorrio.Lista;

public static class HttpRequestExtensions
{
    public static async Task<(T? Data, string? Error)> ParseJsonBodyAsync<T>(this HttpRequestData req)
    {
        try
        {
            var body = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<T>(body, new JsonSerializerOptions{ PropertyNameCaseInsensitive = true});
            return (data, null);
        }
        catch (Exception)
        {
            return (default, "Invalid JSON format.");
        }
    }

    public static async Task<HttpResponseData> CreateResponseWithMessage(this HttpRequestData req, HttpStatusCode code, string message)
    {
        var response = req.CreateResponse(code);
        await response.WriteStringAsync(message);
        return response;
    }
        
    public static async Task<HttpResponseData> CreateJsonResponse<T>(this HttpRequestData req, HttpStatusCode status, T data)
    {
        var response = req.CreateResponse(status);
        await response.WriteAsJsonAsync(data);
        return response;
    }
}