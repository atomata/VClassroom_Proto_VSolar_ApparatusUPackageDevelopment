namespace HexUN.Framework.Debugging
{
    /// <summary>
    /// Useful functions that operate on ILog
    /// </summary>
    public static class UtILog
    {
        public static void Error_NullArgument<TType>(this ILog log, string category, string argName) where TType : class
        {
            log.Error(category, $"{nameof(TType)} {argName} is null");
        }

        public static void Error_PrefabLoad(this ILog log, string category, string prefabName)
        {
            log.Error(category, $"Failed to load prefab {prefabName}");
        }
    }
}