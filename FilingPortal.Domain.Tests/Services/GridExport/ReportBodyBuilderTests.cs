using System;
using System.Collections.Generic;
using System.Linq;
using FilingPortal.Domain.Services.GridExport;
using FilingPortal.Domain.Services.GridExport.Configuration;
using FilingPortal.Domain.Services.GridExport.Formatters;
using Framework.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FilingPortal.Domain.Tests.Services.GridExport
{
    [TestClass]
    public class ReportBodyBuilderTests
    {
        private Mock<IDefaultFormattersRegistry> _defaultFormattersRegistry;
        private Mock<IReportModelMapContainer> _reportModelMapContainer;
        private Mock<IReportModelMap> _reportModelMap;
        private Mock<IColumnMapInfo> _info;
        private Mock<IValueFormatter> _formatter;

        #region Setup

        private ReportBodyBuilder ReportBodyBuilder { get; set; }

        #endregion

        [TestInitialize]
        public void Init()
        {
            _reportModelMapContainer = new Mock<IReportModelMapContainer>();

            _reportModelMap = new Mock<IReportModelMap>();

            _info = new Mock<IColumnMapInfo>();
            _info.Setup(x => x.FieldType).Returns(typeof(object));
            _info.Setup(x => x.Getter).Returns(new Func<object, object>(x => x));
            _formatter = new Mock<IValueFormatter>();
            _info.Setup(x => x.ValueFormatter).Returns(_formatter.Object);
            _reportModelMap.Setup(x => x.GetColumnInfos()).Returns(new[] { _info.Object });

            _reportModelMapContainer.Setup(x => x.GetMap<object>()).Returns(_reportModelMap.Object);

            _defaultFormattersRegistry = new Mock<IDefaultFormattersRegistry>();
            ReportBodyBuilder = new ReportBodyBuilder(_reportModelMapContainer.Object, _defaultFormattersRegistry.Object);
        }

        [TestMethod]
        public void GetRows_WhenFormatterSet_CalledForEachModel()
        {
            _info.Setup(x => x.IsValueFormatterSet).Returns(true);
            var item1 = new object();
            var item2 = new object();

            var result = ReportBodyBuilder.GetRows(new List<object> { item1, item2 }).ToList();

            _formatter.Verify(x => x.Format(It.IsAny<object>()), Times.Exactly(2));
            _formatter.VerifyOnce(x => x.Format(item1));
            _formatter.VerifyOnce(x => x.Format(item2));
        }

        [TestMethod]
        public void GetRows_WhenValueIsUsedAsEnumerable_CollectionDoesNotEnumerates()
        {
            _info.Setup(x => x.IsValueFormatterSet).Returns(true);
            var item1 = new object();
            var item2 = new object();

            var result = ReportBodyBuilder.GetRows(new List<object> { item1, item2 });

            _formatter.Verify(x => x.Format(It.IsAny<object>()), Times.Never);
        }

        [TestMethod]
        public void GetRows_WhenFormatterNotSet_DefaultFormatterIsSearching()
        {
            _info.Setup(x => x.IsValueFormatterSet).Returns(false);
            _defaultFormattersRegistry.Setup(x => x.Get(typeof(object)));

            var item1 = new object();
            var item2 = new object();

            var result = ReportBodyBuilder.GetRows(new List<object> { item1, item2 }).ToList();

            _defaultFormattersRegistry.Verify(x => x.Get(typeof(object)), Times.Exactly(2));
        }

        [TestMethod]
        public void GetRows_WhenFormatterNotSet_DefaultFormatterIsFound_DefaultFormatterIsUsed()
        {
            _info.Setup(x => x.IsValueFormatterSet).Returns(false);
            var defaultFormatter = new Mock<IValueFormatter>();
            _defaultFormattersRegistry.Setup(x => x.Get(typeof(object))).Returns(defaultFormatter.Object);

            var item1 = new object();
            var item2 = new object();

            var result = ReportBodyBuilder.GetRows(new List<object> { item1, item2 }).ToList();

            defaultFormatter.Verify(x => x.Format(It.IsAny<object>()), Times.Exactly(2));
            defaultFormatter.VerifyOnce(x => x.Format(item1));
            defaultFormatter.VerifyOnce(x => x.Format(item2));
        }

        [TestMethod]
        public void GetRows_WhenFormatterNotSet_DefaultFormatterIsNotFound_GettersAreInvoked()
        {
            _info.Setup(x => x.IsValueFormatterSet).Returns(false);
            _defaultFormattersRegistry.Setup(x => x.Get(typeof(object)));
            var item1 = new object();
            var item2 = new object();

            var result = ReportBodyBuilder.GetRows(new List<object> { item1, item2 }).ToList();

            _info.Verify(x => x.Getter, Times.Exactly(2));
        }

        [TestMethod]
        public void GetRows_WhenFormatterNotSet_DefaultFormatterIsNotFound_ToStringIsCalled()
        {
            _info.Setup(x => x.IsValueFormatterSet).Returns(false);
            _defaultFormattersRegistry.Setup(x => x.Get(typeof(object)));
            object item1 = "value1";
            object item2 = "value2";

            var result = ReportBodyBuilder.GetRows(new List<object> { item1, item2 }).ToList();

            result.First().Cells.First().Content.ShouldBeEqualTo("value1");
            result.Last().Cells.First().Content.ShouldBeEqualTo("value2");
        }
    }
}
