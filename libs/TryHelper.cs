namespace DotForge;

public static class TryHelper
{
    public static bool TryExec(Action action)
    {
        try
        {
            action();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
