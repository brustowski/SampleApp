using System;
using System.Globalization;
using System.Threading;
using AutoMapper;
using FilingPortal.Domain.Common;
using FilingPortal.Infrastructure.Common;
using FilingPortal.Infrastructure.Mapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilingPortal.Infrastructure.Tests.Mapping
{
    [TestClass]
    public class DateMappingTests
    {
        class SimpleDateContainerSource
        {
            public string Date { get; set; }
            public string NullableDate { get; set; }
        }

        class SimpleDateContainerDestination
        {
            public DateTime Date { get; set; }
            public DateTime? NullableDate { get; set; }
        }

        public class TestProfile : Profile
        {
            public TestProfile()
            {
                CreateMap<SimpleDateContainerSource, SimpleDateContainerDestination>();
                CreateMap<SimpleDateContainerDestination, SimpleDateContainerSource>();
            }
        }

        int _month;
        int _day;
        int _year;
        private int _shortYear;

        private IMapper _mapper;


        [TestInitialize]
        public void TestInitialize()
        {
            var config = new MapperConfiguration(x =>
                {
                    x.AddProfile<TestProfile>();
                    x.CreateMap<string, DateTime?>().ConvertUsing(new StringToNulableDateTimeConverter());
                    x.CreateMap<DateTime?, string>().ConvertUsing(new NullableDateTimeToStringConverter());
                    x.CreateMap<string, DateTime>().ConvertUsing(new StringToDateTimeConverter());
                    x.CreateMap<DateTime, string>().ConvertUsing(new DateTimeToStringConverter());
                }
            );

            _mapper = config.CreateMapper();

            _month = 10;
            _day = 10;
            _year = 2016;
            _shortYear = 16;
        }

        [TestMethod]
        public void StringToDate_WhenWellFormed_CanBeParsed()
        {
            var source = new SimpleDateContainerSource
            {
                Date = $"{_day}/{_month}/{_year}",
                NullableDate = $"{_day}/{_month}/{_year}"
            };

            var destination = _mapper.Map<SimpleDateContainerDestination>(source);

            Assert.AreEqual(
                destination.Date.ToString(FormatsContainer.UI_DATETIME_FORMAT), 
                new DateTime(_year, _month, _day).ToString(FormatsContainer.UI_DATETIME_FORMAT)
                );
        }

        [TestMethod]
        public void StringToNullableDate_WhenWellFormed_CanBeParsed()
        {
            var source = new SimpleDateContainerSource
            {
                Date = $"{_day}/{_month}/{_year}",
                NullableDate = $"{_day}/{_month}/{_year}"
            };

            var destination = _mapper.Map<SimpleDateContainerDestination>(source);

            Assert.AreEqual(
                ((DateTime)destination.NullableDate).ToString(FormatsContainer.UI_DATETIME_FORMAT), 
                new DateTime(_year, _month, _day).ToString(FormatsContainer.UI_DATETIME_FORMAT)
                );
        }

        [TestMethod]
        public void StringToNullableDate_WhenNullableSourceIsNull_NullableIsNull()
        {
            var source = new SimpleDateContainerSource
            {
                Date = $"{_day}/{_month}/{_year}"
            };

            var destination = _mapper.Map<SimpleDateContainerDestination>(source);

            Assert.IsNull(destination.NullableDate);
        }

        [TestMethod]
        public void StringToDate_WhenCultureIsWierd_DateIsParsed()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CHS");
            var source = new SimpleDateContainerSource
            {
                Date = $"{_day}/{_month}/{_year}"
            };

            var destination = _mapper.Map<SimpleDateContainerDestination>(source);

            Assert.IsNull(destination.NullableDate);
        }

        [TestMethod]
        public void DateToString_WhenCultureIsWierd_DateIsSerialized()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("zh-CHS");
            var source = new SimpleDateContainerDestination
            {
                Date = new DateTime(_year, _month, _day)
            };

            var destination = _mapper.Map<SimpleDateContainerSource>(source);

            Assert.AreEqual($"{_day}/{_month}/{_year}", destination.Date);
        }

        [TestMethod]
        public void NullableDateToString_WhenWellFormed_DateIsSerialized()
        {
            var source = new SimpleDateContainerDestination()
            {
                Date = new DateTime(_year, _month, _day),
                NullableDate = new DateTime(_year, _month, _day)
            };

            var destination = _mapper.Map<SimpleDateContainerSource>(source);

            Assert.AreEqual($"{_day}/{_month}/{_year}", destination.NullableDate);
        }

        [TestMethod]
        public void StringToDate_WhenWellFormed_WithShortYear_CanBeParsed()
        {
            var source = new SimpleDateContainerSource
            {
                Date = $"{_day}/{_month}/{_shortYear}",
                NullableDate = $"{_day}/{_month}/{_shortYear}"
            };

            var destination = _mapper.Map<SimpleDateContainerDestination>(source);

            Assert.AreEqual(
                destination.Date.ToString(FormatsContainer.UI_DATETIME_FORMAT_SHORT_YEAR),
                new DateTime(_year, _month, _day).ToString(FormatsContainer.UI_DATETIME_FORMAT_SHORT_YEAR)
            );
        }

        [TestMethod]
        public void StringToDate_WrongFormat_ThrowsFormatException()
        {
            var source = new SimpleDateContainerSource
            {
                Date = "WrongFormat",
                NullableDate = "WrongFormat"
            };

            try
            {
                var destination = _mapper.Map<SimpleDateContainerDestination>(source);
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(AutoMapperMappingException));
                if (e is AutoMapperMappingException autoMapperException)
                {
                    Assert.IsInstanceOfType(autoMapperException.InnerException, typeof(FormatException));
                    Assert.AreEqual(autoMapperException.InnerException.Message, "Wrong date format. Please provide date in M/d/yyyy or M/d/yy format");
                }
                else
                {
                    Assert.Fail();
                }
            }
        }
    }
}
