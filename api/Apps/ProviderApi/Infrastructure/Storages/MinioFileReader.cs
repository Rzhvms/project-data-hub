using Application.Ports.Storages;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Storages;

/// <inheritdoc />
internal sealed class MinioFileReader(IMinioClient minioClient, IConfiguration configuration) : IMinioFileReader
{
    private readonly string _bucket = configuration["Minio:Bucket"]!;

    /// <inheritdoc />
    public async Task<Stream> OpenReadAsync(string objectPath)
    {
        var memory = new MemoryStream();

        await minioClient.GetObjectAsync(
            new GetObjectArgs()
                .WithBucket(_bucket)
                .WithObject(objectPath)
                .WithCallbackStream(stream =>
                {
                    stream.CopyTo(memory);
                }));

        memory.Position = 0;
        return memory;
    }

    /// <inheritdoc />
    public async Task<byte[]> ReadAllBytesAsync(string objectPath)
    {
        await using var stream = await OpenReadAsync(objectPath);

        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory);

        return memory.ToArray();
    }
}