using BookWorm.SharedKernel.ActivityScope;

namespace BookWorm.Ordering.Infrastructure.EventStore;

public static class TelemetryTags
{
    public const string ActivityName = "Marten";
    public const string Stream = $"{ActivitySourceProvider.DefaultSourceName}.stream";
}
