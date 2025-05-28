using Azure.Data.Tables;

namespace Bodorrio.Lista;

internal class InviteRepository
{
    private const string StorageParameter = "InviteStorageTable";
    private const string TableName = "invites";

    internal static async Task<List<TableEntity>> GetInvites()
    {
        return await GetTable().QueryAsync<TableEntity>().ToListAsync();
    }

    internal static async Task<bool> AddInvite(InviteModel invite)
    {
        var entity = new TableEntity(invite.PartitionKey, invite.RowKey);
        var response = await GetTable().AddEntityAsync(entity);

        return response.IsError;
    }

    internal static async Task<bool> DeleteInvite(InviteModel invite)
    {
        var response = await GetTable().DeleteEntityAsync(invite.PartitionKey, invite.RowKey);
        return response.IsError;
    }
    
    private static TableClient GetTable()
    {
        return new TableClient(Environment.GetEnvironmentVariable(StorageParameter), TableName);
    }
}
