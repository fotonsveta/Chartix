using System.Collections.Generic;

namespace Chartix.Infrastructure.Telegram.Models
{
    public class PagingMenuData
    {
        public static readonly int DefaultPageSize = 7;

        public IDictionary<string, string> ButtonTexts { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public ButtonCode Previous { get; set; }
    }
}
