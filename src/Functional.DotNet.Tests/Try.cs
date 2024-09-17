using Xunit;
using System;
using static Functional.DotNet.F;
using Functional.DotNet;

namespace Functional.Net.Tests
{
    using static TestUtils;

    public class TryTests
    {
        Try<Uri> CreateUri(string uri) => () => new Uri(uri);

        [Fact]
        public void SuccessfulTry()
        {
            var uriTry = CreateUri("http://github.com");

            uriTry.Run().Match(
                OnSuccess: uri => Assert.NotNull(uri),
                OnError: ex => Fail()
            );
        }

        [Fact]
        public void FailingTry()
        {
            var uriTry = CreateUri("rubbish");

            uriTry.Run().Match(
                OnSuccess: uri => Fail(),
                OnError: ex => Assert.NotNull(ex)
            );
        }

        [Fact]
        public void ItIsLazy()
        {
            bool tried = false;

            Func<string, Try<Uri>> createUri = (uri) => Try(() =>
            {
                tried = true;
                return new Uri(uri);
            });

            var uriTry = createUri("http://github.com");
            Assert.False(tried, "creating a Try should not run it");

            var schemeTry = uriTry.Map(uri => uri.Scheme);
            Assert.False(tried, "mapping onto a try should not run it");

            uriTry.Run().Match(
                OnSuccess: uri => Assert.NotNull(uri),
                OnError: ex => Assert.True(false, "should have succeeded")
            );
            Assert.True(tried, "matching should run the Try");
        }
    }
}
