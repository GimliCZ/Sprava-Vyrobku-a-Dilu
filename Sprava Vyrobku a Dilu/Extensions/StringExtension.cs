namespace Sprava_Vyrobku_a_Dilu.Extensions
{
    public static class StringExtension
    {
        public static string ReplaceAt(this string original, char replacement, int index)
        {
            // Split the original string and replace the substring
            return original.Substring(0, index) + replacement + original.Substring(index + 1);
        }
    }
}
