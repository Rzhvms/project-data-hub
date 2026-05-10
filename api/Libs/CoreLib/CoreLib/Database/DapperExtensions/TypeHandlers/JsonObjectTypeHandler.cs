using System.Data;
using System.Text.Json.Nodes;
using Dapper;
using Npgsql;

namespace CoreLib.Database.DapperExtensions.TypeHandlers;

public class JsonObjectTypeHandler : SqlMapper.TypeHandler<JsonObject?>
{
    public override void SetValue(IDbDataParameter parameter, JsonObject? value)
    {
        parameter.Value = value?.ToJsonString() ?? (object)DBNull.Value;
        if (parameter is NpgsqlParameter npgsqlParameter)
        {
            npgsqlParameter.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb;
        }
    }

    public override JsonObject? Parse(object? value)
    {
        if (value == null || value is DBNull)
            return null;
            
        var jsonString = value.ToString();
        return JsonNode.Parse(jsonString!) as JsonObject;
    }
}