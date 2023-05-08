
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using lambda_aot;

public class CustomerRepository
{

    private const string TableName = "customers";
    private readonly IAmazonDynamoDB _dynamoDbClient;

    public CustomerRepository(IAmazonDynamoDB dynamoDbClient)
    {
        _dynamoDbClient = dynamoDbClient;
    }
    
        
    
    public async Task<Customer?> GetAsync(Guid id)
    {
        var request = new GetItemRequest
        {
            TableName = TableName,
            Key = new Dictionary<string, AttributeValue>
            {
                { "pk", new AttributeValue { S = id.ToString() } },
                { "sk", new AttributeValue { S = id.ToString() } }
            }
        };

        var response = await _dynamoDbClient.GetItemAsync(request);

        if(response.Item.Count == 0) return null;

        var itemAsDocument = Document.FromAttributeMap(response.Item);

        return JsonSerializer.Deserialize(itemAsDocument.ToJson(), LambdaFunctionJsonSerializerContext.Default.Customer);
    }


       
    }
   


public class Customer
{
    [JsonPropertyName("pk")]
    public string Pk { get; set; } = default!;

    [JsonPropertyName("sk")]
    public string Sk { get; set; } = default!;

   
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("fullName")]
    public string FullName { get; set; } = default!;

     [JsonPropertyName("email")]
    public string Email { get; set; } = default!;

}