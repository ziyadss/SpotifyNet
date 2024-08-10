using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyNet.Core.Utilities;

public static class ExtensionMethods
{
    public static async Task<IReadOnlyList<TResult>> ChunkedSelect<TSource, TResult>(
        this ICollection<TSource> source,
        int chunkSize,
        Func<IEnumerable<TSource>, Task<IReadOnlyList<TResult>>> selector)
    {
        var chunks = source.Count / chunkSize;
        var result = new List<TResult>(source.Count);
        for (var i = 0; i < chunks - 1; i++)
        {
            var chunk = source.Skip(i * chunkSize).Take(chunkSize);
            var chunkResult = await selector(chunk).ConfigureAwait(false);
            result.AddRange(chunkResult);
        }

        var lastChunk = source.Skip((chunks - 1) * chunkSize);
        var lastChunkResult = await selector(lastChunk).ConfigureAwait(false);
        result.AddRange(lastChunkResult);

        return result;
    }
}
