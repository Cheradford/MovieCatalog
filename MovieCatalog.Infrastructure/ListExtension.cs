using MovieCatalog.Domain;

namespace MovieCatalog.Infrastructure;

public static class ListExtension
{
    public static T AddWithId<T>(this IList<T> list, T objectId) where T : ObjectId
    {
        var nextId = list.Any() ? list.Max(o => o.Id) : 0;
        objectId.Id = nextId;
        list.Add(objectId);
        return objectId;
    }
}