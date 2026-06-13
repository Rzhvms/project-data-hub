namespace Application.Ports.Storages;

/// <summary>
/// Чтение файла из объектного хранилища Minio.
/// Используется для выгрузки изображений в документы
/// </summary>
public interface IMinioFileReader
{
    /// <summary>
    /// Открывает поток чтения объекта
    /// </summary>
    Task<Stream> OpenReadAsync(string objectPath);

    /// <summary>
    /// Считывает объект целиком в массив байт
    /// </summary>
    Task<byte[]> ReadAllBytesAsync(string objectPath);
}