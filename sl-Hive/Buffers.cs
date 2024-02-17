namespace sl_Hive;

// Generated
public static class Buffers
{
    public static ReadOnlySpan<byte> From(in ReadOnlySpan<byte> a, in ReadOnlySpan<byte> b) {
        Span<byte> result = new byte[a.Length + b.Length];
        a.CopyTo(result);
        b.CopyTo(result[a.Length..]);
        return result;
    }

    public static ReadOnlySpan<byte> From(in ReadOnlySpan<byte> a, in ReadOnlySpan<byte> b, in ReadOnlySpan<byte> c) {
        Span<byte> result = new byte[a.Length + b.Length + c.Length];
        a.CopyTo(result);
        b.CopyTo(result[a.Length..]);
        c.CopyTo(result[(a.Length + b.Length)..]);
        return result;
    }

    public static ReadOnlySpan<byte> From(in ReadOnlySpan<byte> a, in ReadOnlySpan<byte> b, in ReadOnlySpan<byte> c, in ReadOnlySpan<byte> d) {
        Span<byte> result = new byte[a.Length + b.Length + c.Length + d.Length];
        a.CopyTo(result);
        b.CopyTo(result[a.Length..]);
        c.CopyTo(result[(a.Length + b.Length)..]);
        d.CopyTo(result[(a.Length + b.Length + c.Length)..]);
        return result;
    }

    public static ReadOnlySpan<byte> From(in ReadOnlySpan<byte> a, in ReadOnlySpan<byte> b, in ReadOnlySpan<byte> c, in ReadOnlySpan<byte> d, in ReadOnlySpan<byte> e) {
        Span<byte> result = new byte[a.Length + b.Length + c.Length + d.Length + e.Length];
        a.CopyTo(result);
        b.CopyTo(result[a.Length..]);
        c.CopyTo(result[(a.Length + b.Length)..]);
        d.CopyTo(result[(a.Length + b.Length + c.Length)..]);
        e.CopyTo(result[(a.Length + b.Length + c.Length + d.Length)..]);
        return result;
    }

    public static ReadOnlySpan<byte> From(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b, ReadOnlySpan<byte> c, ReadOnlySpan<byte> d, ReadOnlySpan<byte> e, ReadOnlySpan<byte> f) {
        Span<byte> result = new byte[a.Length + b.Length + c.Length + d.Length + e.Length + f.Length];
        a.CopyTo(result);
        b.CopyTo(result[a.Length..]);
        c.CopyTo(result[(a.Length + b.Length)..]);
        d.CopyTo(result[(a.Length + b.Length + c.Length)..]);
        e.CopyTo(result[(a.Length + b.Length + c.Length + d.Length)..]);
        f.CopyTo(result[(a.Length + b.Length + c.Length + d.Length + e.Length)..]);
        return result;
    }
}
