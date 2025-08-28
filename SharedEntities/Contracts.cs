using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SharedEntities.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedEntities;

public record VideoJobCreated(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId), JsonConverter(typeof(ObjectIdSystemTextConverter))]
    ObjectId Id,

    [property: BsonRepresentation(BsonType.ObjectId), JsonConverter(typeof(ObjectIdSystemTextConverter))]
    ObjectId JobId,

    DateTime CreatedAtUtc);

public record VideoJobStatusChanged(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId), JsonConverter(typeof(ObjectIdSystemTextConverter))]
    ObjectId Id,

    [property: BsonRepresentation(BsonType.ObjectId), JsonConverter(typeof(ObjectIdSystemTextConverter))]
    ObjectId JobId,

    StatusEnum Status,
    string? Message,
    string VideoName,
    DateTime ChangedAtUtc);

public record QrCodeFound(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId), JsonConverter(typeof(ObjectIdSystemTextConverter))]
    ObjectId Id,

    [property: BsonRepresentation(BsonType.ObjectId), JsonConverter(typeof(ObjectIdSystemTextConverter))]
    ObjectId JobId,

    double TimestampSeconds,
    string Content,
    DateTime FoundAtUtc);

public sealed class ObjectIdSystemTextConverter : JsonConverter<ObjectId>
{
    public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        => ObjectId.Parse(reader.GetString()!);

    public override void Write(Utf8JsonWriter writer, ObjectId value, JsonSerializerOptions options)
        => writer.WriteStringValue(value.ToString());
}