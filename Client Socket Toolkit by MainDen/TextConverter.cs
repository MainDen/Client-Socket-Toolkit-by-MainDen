using System.Text.RegularExpressions;

namespace MainDen.ClientSocketToolkit
{
    public static class TextConverter
    {
        static TextConverter()
        {
            MultiLineRegex = new Regex(MultiLinePattern);
            SingleLineRegex = new Regex(SingleLinePattern);
        }

        private static readonly Regex MultiLineRegex;

        private static readonly Regex SingleLineRegex;

        private static readonly string MultiLinePattern = @"\\|\r\n|\r|\n";

        private static readonly string SingleLinePattern = @"\\\\|\\r\\n|\\r|\\n";

        private static string ToSingleLineMatchEvaluator(Match match)
        {
            switch (match.Value)
            {
                case "\\":
                    return @"\\";
                case "\r\n":
                    return @"\r\n";
                case "\r":
                    return @"\r";
                case "\n":
                    return @"\n";
                default:
                    return match.Value;
            }
        }

        private static string ToMultiLineMatchEvaluator(Match match)
        {
            switch (match.Value)
            {
                case @"\\":
                    return "\\";
                case @"\r\n":
                    return "\r\n";
                case @"\r":
                    return "\r";
                case @"\n":
                    return "\n";
                default:
                    return match.Value;
            }
        }

        public static string ToSingleLine(string text)
        {
            return MultiLineRegex.Replace(text, ToSingleLineMatchEvaluator);
        }

        public static string ToMultiLine(string text)
        {
            return SingleLineRegex.Replace(text, ToMultiLineMatchEvaluator);
        }
    }
}
