using System;
using System.Xml.Linq;

namespace Cake.NUnitRetry.Model
{
    public class TestCase
    {
        public string FullName { get; }

        public Result Result { get; }

        public DateTime StartTime { get; }

        public DateTime EndTime { get; }

        public TimeSpan Duration { get; }

        public XElement Element { get; }

        public TestCase(XElement element)
        {
            FullName = element.Attribute("fullname").Value;
            Result = (Result)Enum.Parse(typeof(Result), element.Attribute("result").Value);
            StartTime = DateTime.Parse(element.Attribute("start-time").Value).ToUniversalTime();
            EndTime = DateTime.Parse(element.Attribute("end-time").Value).ToUniversalTime();
            Duration = TimeSpan.FromSeconds(Convert.ToDouble(element.Attribute("duration").Value));

            Element = element;
        }
    }
}
