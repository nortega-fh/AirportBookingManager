using System.Reflection;
using Xunit.Sdk;

namespace AiportBooking.Tests.Serializers.Attributes;

public class FlightDeserializeValuesAttribute : DataAttribute
{
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        yield return new object[] { "null", "120", "130", new decimal[] { 120m, 130m } };
        yield return new object[] { "140", "null", "180", new decimal[] { 140m, 180m } };
        yield return new object[] { "140", "160", "null", new decimal[] { 140m, 160m } };
        yield return new object[] { "140", "160", "250", new decimal[] { 140m, 160m, 250m } };
    }
}
