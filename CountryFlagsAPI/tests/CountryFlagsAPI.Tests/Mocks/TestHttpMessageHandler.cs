using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountryFlagsAPI.Tests.Mocks
{
    public class TestHttpMessageHandler :HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _sendAsyncFunc;

        public TestHttpMessageHandler(Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> sendAsyncFunc)
        {
            _sendAsyncFunc = sendAsyncFunc ?? throw new ArgumentNullException(nameof(sendAsyncFunc));
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return _sendAsyncFunc(request, cancellationToken);
        }
    }
}
