using System.Data;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}