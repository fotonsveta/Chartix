using System;
using Chartix.Core.Entities;
using Chartix.Infrastructure.Telegram.Models;
using Chartix.Infrastructure.Telegram.Services;
using Xunit;

namespace Chartix.Tests.Core.Services
{
    public class ValueParserTest
    {
        private static readonly int _currentYear = DateTime.Now.Year;
        private static readonly DateTime _someDay = new DateTime(_currentYear, 5, 19, 9, 12, 0);

        private readonly ValueParser _parser;

        public ValueParserTest()
        {
            _parser = new ValueParser();
        }

        public static object[][] CorrectData => new object[][]
        {
            new object[] { LangCode.Ru, _someDay, "10", 10, new DateTime(_currentYear, 5, 19, 9, 12, 0) },
            new object[] { LangCode.Ru, _someDay, " 10.1 12.03  ", 10.1, new DateTime(_currentYear, 3, 12, 9, 12, 0) },
            new object[] { LangCode.Ru, _someDay, " 10.1 9.8  ", 10.1, new DateTime(_currentYear, 8, 9, 9, 12, 0) },
            new object[] { LangCode.Ru, _someDay, "10 12.3.2000", 10, new DateTime(2000, 3, 12, 9, 12, 0) },
            new object[] { LangCode.Ru, _someDay, " 10.1 4.03  12:00  ", 10.1, new DateTime(_currentYear, 3, 4, 12, 0, 0) },
            new object[] { LangCode.Ru, _someDay, "0.54 12.03.2008  23:59  ", 0.54, new DateTime(2008, 3, 12, 23, 59, 0) },
            new object[] { LangCode.Ru, _someDay, "10 1.1 0:1", 10, new DateTime(_currentYear, 1, 1, 0, 1, 0) },

            new object[] { LangCode.En, _someDay, "10", 10, new DateTime(_currentYear, 5, 19, 9, 12, 0) },
            new object[] { LangCode.En, _someDay, " 10.1 3/4  ", 10.1, new DateTime(_currentYear, 3, 4, 9, 12, 0) },
            new object[] { LangCode.En, _someDay, "10 2000/03/12", 10, new DateTime(2000, 3, 12, 9, 12, 0) },
            new object[] { LangCode.En, _someDay, " 10.18 03/12  12:00  ", 10.18, new DateTime(_currentYear, 3, 12, 12, 0, 0) },
            new object[] { LangCode.En, _someDay, "0.54 2008/3/12  23:59  ", 0.54, new DateTime(2008, 3, 12, 23, 59, 0) },

            new object[] { LangCode.En, _someDay, " 10.1 3.12  ", 10.1, new DateTime(_currentYear, 3, 12, 9, 12, 0) },
            new object[] { LangCode.En, _someDay, "10 2000.03.12", 10, new DateTime(2000, 3, 12, 9, 12, 0) },
            new object[] { LangCode.En, _someDay, " 10.1 03.12  12:00  ", 10.1, new DateTime(_currentYear, 3, 12, 12, 0, 0) },
            new object[] { LangCode.En, _someDay, "0.54 2008.3.12  23:59  ", 0.54, new DateTime(2008, 3, 12, 23, 59, 0) },
        };

        public static object[][] NotCorrectData => new object[][]
        {
            new object[] { LangCode.Ru, _someDay, "10,0" },
            new object[] { LangCode.Ru, _someDay, "10,0 12" },
            new object[] { LangCode.Ru, _someDay, "ten" },
            new object[] { LangCode.Ru, _someDay, "100 12.12.12" },
            new object[] { LangCode.Ru, _someDay, "10 1.1 25:00" },
            new object[] { LangCode.Ru, _someDay, "10 1.12 21:89" },

            new object[] { LangCode.En, _someDay, "10 20/03/12" },
            new object[] { LangCode.En, _someDay, " 10.1 03/12  12:99  " },
            new object[] { LangCode.En, _someDay, "0.54  23:59  " },
            new object[] { LangCode.En, _someDay, " 10.1 3.126  " },
            new object[] { LangCode.En, _someDay, "10 20.03.12" },
        };

        [Theory]
        [MemberData(nameof(CorrectData))]
        public void Parse_Success(LangCode code, DateTime updateDate, string content,
            double expectedNumber, DateTime expectedDate)
        {
            var message = new TextUpdateMessage()
            {
                LanguageCode = code,
                Content = content,
                UpdateDatetime = updateDate,
            };

            (Value value, string error) = _parser.Parse(message);

            Assert.NotNull(value);
            Assert.Null(error);
            Assert.Equal(value.Content, expectedNumber);
            EqualDateTimeToMinutes(value.ValueDate, expectedDate);
        }

        [Theory]
        [MemberData(nameof(NotCorrectData))]
        public void Parse_Fail(LangCode code, DateTime updateDate, string content)
        {
            var message = new TextUpdateMessage()
            {
                LanguageCode = code,
                Content = content,
                UpdateDatetime = updateDate,
            };

            (Value value, string error) = _parser.Parse(message);

            Assert.Null(value);
            Assert.NotNull(error);
        }

        private void EqualDateTimeToMinutes(DateTime realDate, DateTime expectedDate)
        {
            Assert.Equal(realDate.Year, expectedDate.Year);
            Assert.Equal(realDate.Month, expectedDate.Month);
            Assert.Equal(realDate.Day, expectedDate.Day);
            Assert.Equal(realDate.Hour, expectedDate.Hour);
            Assert.Equal(realDate.Minute, expectedDate.Minute);
        }
    }
}
