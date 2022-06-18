using System.Text.Json;

namespace PalletStorage.Tests
{
    public class ObjectComparison
    {
        public static bool EqualsByJson(object firstObject, object secondObject)
        {
            var firstData = JsonSerializer.Serialize(firstObject);
            var secondData = JsonSerializer.Serialize(secondObject);

            return firstData.Equals(secondData);
        }
    }
}
