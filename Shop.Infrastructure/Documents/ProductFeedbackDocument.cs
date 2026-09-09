using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shop.Infrastructure.Documents;

public sealed class ProductFeedbackDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; init; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("productId")]
    public int ProductId { get; init; }

    [BsonElement("type")]
    public string Type { get; init; } = string.Empty;

    [BsonElement("message")]
    public string Message { get; init; } = string.Empty;

    [BsonElement("customerName")]
    [BsonIgnoreIfNull]
    public string? CustomerName { get; init; }

    [BsonElement("customerEmail")]
    [BsonIgnoreIfNull]
    public string? CustomerEmail { get; init; }

    [BsonElement("createdAtUtc")]
    public DateTime CreatedAtUtc { get; init; }
}