using SharedEntities.Enums;

namespace SharedEntities;

public record VideoJobCreated(
    string JobId,
    string OriginalFileName,
    string StoragePath,
    DateTime CreatedAtUtc);

public record VideoJobStatusChanged(
    string JobId,
    StatusEnum Status,
    int? ProgressPercent,
    string? Message,
    DateTime ChangedAtUtc);

public record QrCodeFound(
    string JobId,
    double TimestampSeconds,
    string Content,
    DateTime FoundAtUtc);