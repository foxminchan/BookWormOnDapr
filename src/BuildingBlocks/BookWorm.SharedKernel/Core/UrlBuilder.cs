namespace BookWorm.SharedKernel.Models;

public sealed class UrlBuilder
{
    private int? _version;
    private string? _resource;
    private string? _id;

    public UrlBuilder WithVersion(int? version = 1)
    {
        _version = version;
        return this;
    }

    public UrlBuilder WithResource(string resource)
    {
        _resource = resource.ToLowerInvariant();
        return this;
    }

    public UrlBuilder WithId<T>(T id)
    {
        _id = id?.ToString();
        return this;
    }

    public string Build()
    {
        return $"/api/{_version}/{_resource}/{_id}";
    }
}
