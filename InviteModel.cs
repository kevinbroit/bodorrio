namespace Bodorrio.Lista;

public class InviteModel
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
}

public static class InviteModelExtensions
{
    public static string? Validate(this InviteModel? model)
    {
        if (model == null)
            return "Request body is missing.";

        if (string.IsNullOrWhiteSpace(model.PartitionKey))
            return "PartitionKey is required.";

        if (string.IsNullOrWhiteSpace(model.RowKey))
            return "RowKey is required.";

        return null;
    }
}