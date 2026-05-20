using Microsoft.EntityFrameworkCore;
using SistemaContabil.Infrastructure.Data;

namespace SistemaContabil.Api.Endpoints;

internal static class EndpointSequenceHelper
{
    public static async Task<int> GetNextValueAsync(SistemaContabilDb db, params string[] sequenceNames)
    {
        foreach (var sequenceName in sequenceNames)
        {
            try
            {
                var result = await db.Database
                    .SqlQueryRaw<int>($"SELECT {sequenceName}.NEXTVAL FROM DUAL")
                    .ToListAsync();

                var nextValue = result.FirstOrDefault();
                if (nextValue > 0)
                {
                    return nextValue;
                }
            }
            catch
            {
                // Some scripts in the project use different sequence names.
                // The caller can fall back to max(id) + 1 when none exists.
            }
        }

        return 0;
    }
}
