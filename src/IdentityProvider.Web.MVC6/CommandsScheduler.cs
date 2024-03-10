using Dapper;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

public class CommandsScheduler : ICommandsScheduler
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public CommandsScheduler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task EnqueueAsync<T>(ICommand<T> command)
    {
        var connection = _sqlConnectionFactory.GetOpenConnection();

        const string sqlInsert =
            "INSERT INTO [InternalCommands] ([Id], [EnqueueDate] , [Type], [Data]) VALUES " +
            "(@Id, @EnqueueDate, @Type, @Data)";

        await connection.ExecuteAsync(sqlInsert, new
        {
            command.Id,
            EnqueueDate = DateTime.UtcNow,
            Type = command.GetType().FullName,
            Data = JsonConvert.SerializeObject(command)
        });
    }
}