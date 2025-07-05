using PdfSharpCore.Fonts;
using System.Collections.Concurrent;

namespace ReflexCoreAgent.Helpers
{
    public class CustomFontResolver : IFontResolver
    {
        private static readonly ConcurrentDictionary<string, byte[]> _fontData = new();

        public string DefaultFontName => "Sarabun-Regular";

        public byte[] GetFont(string faceName)
        {
            if (_fontData.TryGetValue(faceName, out var data))
                return data;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Fonts", $"{faceName}.ttf");
            var bytes = File.ReadAllBytes(path);
            _fontData[faceName] = bytes;
            return bytes;
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            var faceName = "Sarabun-Regular";
            return new FontResolverInfo(faceName);
        }
    }
}
