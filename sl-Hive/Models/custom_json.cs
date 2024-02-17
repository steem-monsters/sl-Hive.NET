using sl_Hive.Requests;
using System.Text.Json.Serialization;

namespace sl_Hive.Models;

public class custom_json: IOperationID
{
    public int opid => 18;

    public required string[] required_auths;

    public required string[] required_posting_auths;

    public required string id;

    public required string json;
}
