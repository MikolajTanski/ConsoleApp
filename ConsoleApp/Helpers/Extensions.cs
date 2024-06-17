namespace ConsoleApp
{
    using System;

    public static class Extensions
    {
        public static string Clear(this string input)
        {
            return input.Trim().Replace(" ", "").Replace(Environment.NewLine, "");
        }
        public static bool ClearEquals(this string input, string comparison)
        {
            return input.Clear().Equals(comparison.Clear(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
