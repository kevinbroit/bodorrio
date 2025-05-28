using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.OpenApi.Models;

namespace Bodorrio.Lista;

public class InviteFunction
{
    private readonly ILogger<InviteFunction> _logger;

    public InviteFunction(ILogger<InviteFunction> logger)
    {
        _logger = logger;
    }

    [Function("GetInvites")]
    [OpenApiParameter(name: "code", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Function access key")]
    [OpenApiOperation("GetInvites", tags: new[] { "Invites" }, Summary = "Get all invites")]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<Dictionary<string, string>>), Description = "List of invites")]
    public async Task<HttpResponseData> GetInvites(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "invites")] HttpRequestData req)
    {
        var invites = await InviteRepository.GetInvites();

        return await req.CreateJsonResponse(HttpStatusCode.OK, invites);
    }


    [Function("AddInvite")]
    [OpenApiOperation("AddInvite", tags: new[] { "Invites" }, Summary = "Insert an invite entity into Azure Table Storage")]
    [OpenApiParameter(name: "code", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Function access key")]
    [OpenApiRequestBody("application/json", typeof(InviteModel), Required = true, Description = "Invite entity")]
    [OpenApiResponseWithBody(HttpStatusCode.Created, "text/plain", typeof(string), Description = "Entity successfully inserted")]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "Validation or insert failed")]
    public async Task<HttpResponseData> AddInvite(
    [HttpTrigger(AuthorizationLevel.Function, "post", Route = "invites")] HttpRequestData req)
    {
        var (invite, error) = await req.ParseJsonBodyAsync<InviteModel>();
        if (error != null) return await req.CreateResponseWithMessage(HttpStatusCode.BadRequest, error);

        var validationError = invite.Validate();
        if (validationError != null) return await req.CreateResponseWithMessage(HttpStatusCode.BadRequest, validationError);

        var success = await InviteRepository.AddInvite(invite);
        if (!success)
        {
            return await req.CreateResponseWithMessage(HttpStatusCode.BadRequest, "Invite could not be inserted.");
        }

        return await req.CreateResponseWithMessage(HttpStatusCode.Created, "Entity inserted.");
    }

    [Function("DeleteInvite")]
    [OpenApiOperation("DeleteInvite", tags: new[] { "Invites" }, Summary = "Delete an invite by PartitionKey and RowKey")]
    [OpenApiParameter(name: "code", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "Function access key")]
    [OpenApiRequestBody("application/json", typeof(InviteModel), Required = true, Description = "Invite to delete")]
    [OpenApiResponseWithoutBody(HttpStatusCode.OK, Description = "Invite deleted successfully")]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Description = "Missing or invalid input")]
    [OpenApiResponseWithoutBody(HttpStatusCode.InternalServerError, Description = "Invite could not be deleted")]
    public async Task<HttpResponseData> DeleteInvite(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "invites")] HttpRequestData req)
    {
        var (invite, error) = await req.ParseJsonBodyAsync<InviteModel>();
        if (error != null) return await req.CreateResponseWithMessage(HttpStatusCode.BadRequest,error);

        var validationError = invite.Validate();
        if (validationError != null) return await req.CreateResponseWithMessage(HttpStatusCode.BadRequest, validationError);
        
        var success = await InviteRepository.DeleteInvite(invite);
        if (!success) return await req.CreateResponseWithMessage(HttpStatusCode.InternalServerError, "Invite could not be deleted.");

        return await req.CreateResponseWithMessage(HttpStatusCode.OK, "Entity deleted.");
    }
}
