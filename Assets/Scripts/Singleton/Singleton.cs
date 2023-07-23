public class Singleton<T> where T : class , new()
{
    private static T _Instance;

    public static T Get()
    {
        if (_Instance == null)
        {
            _Instance = new T();
        }

        return _Instance;
    }
}