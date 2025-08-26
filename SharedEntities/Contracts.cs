using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SharedEntities.Enums;

namespace SharedEntities;

public record VideoJobCreated(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)]
    ObjectId Id,

    [property: BsonRepresentation(BsonType.ObjectId)]
    ObjectId JobId,

    IFormFile Video,
    DateTime CreatedAtUtc);


public record VideoJobStatusChanged(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)]
    ObjectId Id,

    [property: BsonRepresentation(BsonType.ObjectId)]
    ObjectId JobId,

    StatusEnum Status,
    string? Message,
    DateTime ChangedAtUtc);

public record QrCodeFound(
    [property: BsonId, BsonRepresentation(BsonType.ObjectId)]
    ObjectId Id,

    [property: BsonRepresentation(BsonType.ObjectId)]
    ObjectId JobId,

    double TimestampSeconds,
    string Content,
    DateTime FoundAtUtc);