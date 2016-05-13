using Zephyr.Singletons;

/// <summary>
/// The class that inherits from SingletonAsComponent.  It must pass iteself as a generic type to work.
/// </summary>
public class MySingletonComponent : SingletonAsComponent<MySingletonComponent>
{
    /// <summary>
    /// Allows outside classes to properly access the Singleton aspect of this class.
    /// </summary>
    public static MySingletonComponent Instance { get { return (MySingletonComponent) _Instance; } set { _Instance = value; } }
}
