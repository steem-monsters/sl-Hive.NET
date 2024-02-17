namespace sl_Hive;

public static class MemoryStreamExtensions
{
    public static ReadOnlySpan<byte> AsReadOnlySpan(this MemoryStream stream) =>
        stream.TryGetBuffer(out var result)
            ? result.AsSpan()
            : stream.ToArray().AsSpan();
}
