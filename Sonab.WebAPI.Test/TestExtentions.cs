using Sonab.WebAPI.Models;

namespace Sonab.WebAPI.Test;

public static class TestExtentions
{
    public static bool TryGetData<T>(this ServiceResponse response, out T data)
    {
        if (response.Data is T obj)
        {
            data = obj;
            return true;
        }
        else
        {
            data = default;
            return false;
        }
    }
}
