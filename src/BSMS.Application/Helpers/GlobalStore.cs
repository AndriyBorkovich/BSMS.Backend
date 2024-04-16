namespace BSMS.Application.Helpers;

public static class GlobalStore
{
    private static readonly object LockObject = new();

    private static string? _currentUser;
    
    public static string? CurrentUser
    {
        get
        {
            lock (LockObject)
            {
                return _currentUser;
            }
        }
        set
        {
            lock (LockObject)
            {
                _currentUser = value;
            }
        }
    }
}