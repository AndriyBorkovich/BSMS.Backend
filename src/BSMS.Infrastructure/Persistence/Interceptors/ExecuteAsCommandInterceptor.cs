using System.Data;
using System.Data.Common;
using BSMS.Application.Helpers;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BSMS.Infrastructure.Persistence.Interceptors;

public class ExecuteAsCommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result)
    {
        AddExecuteAsUser(command);
        
        return result;
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default)
    {
        AddExecuteAsUser(command);
       
        return new ValueTask<InterceptionResult<DbDataReader>>(result);
    }

    private void AddExecuteAsUser(IDbCommand command)
    {
        const string defaultName = "AndriiAdmin";
        var username = GlobalStore.CurrentUser ?? defaultName;
        command.CommandText = $"""
                               EXECUTE AS USER = '{username}';
                               {command.CommandText};
                               REVERT;
                               """;
    }
}