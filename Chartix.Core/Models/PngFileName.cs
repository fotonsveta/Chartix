using System;
using Chartix.Core.Validation;

namespace Chartix.Core.Models
{
    public class PngFileName
    {
        public PngFileName(string filename)
        {
            filename = Check.NullIfEmpty(filename) ?? throw new ArgumentException(filename);
            Name = filename.Trim().ToLower().EndsWith(".png")
                ? filename
                : string.Format($"{filename}.png");
        }

        public string Name { get; }
    }
}
