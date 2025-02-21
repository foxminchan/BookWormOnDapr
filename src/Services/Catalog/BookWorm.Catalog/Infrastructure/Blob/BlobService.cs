﻿namespace BookWorm.Catalog.Infrastructure.Blob;

public sealed class BlobService(BlobServiceClient client) : IBlobService
{
    public async Task DeleteFileAsync(
        string fileName,
        CancellationToken cancellationToken = default
    )
    {
        var blobContainerClient = client.GetBlobContainerClient(nameof(BookWorm));

        var blobClient = blobContainerClient.GetBlobClient(fileName);

        await blobClient.DeleteIfExistsAsync(
            DeleteSnapshotsOption.IncludeSnapshots,
            cancellationToken: cancellationToken
        );
    }

    public string GetFileUrl(string fileName)
    {
        var blobContainerClient = client.GetBlobContainerClient(nameof(BookWorm));

        var blobClient = blobContainerClient.GetBlobClient(fileName);

        return blobClient.Uri.AbsoluteUri;
    }

    public async Task<string> UploadFileAsync(
        IFormFile file,
        CancellationToken cancellationToken = default
    )
    {
        var blobContainerClient = client.GetBlobContainerClient(nameof(BookWorm));

        await blobContainerClient.CreateIfNotExistsAsync(
            PublicAccessType.BlobContainer,
            cancellationToken: cancellationToken
        );

        var uniqueFileName = $"{Guid.NewGuid()}-{file.FileName}";

        var blobClient = blobContainerClient.GetBlobClient(uniqueFileName);

        await blobClient.UploadAsync(file.OpenReadStream(), cancellationToken: cancellationToken);

        return uniqueFileName;
    }
}
