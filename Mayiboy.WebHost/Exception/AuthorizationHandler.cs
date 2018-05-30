using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mayiboy.WebHost.Exception
{
    public class AuthorizationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
          
            return base.SendAsync(request, cancellationToken);
        }


        /// <summary>
        /// 非法访问
        /// </summary>
        /// <returns></returns>
        private Task<HttpResponseMessage> Unauthorized()
        {
            return null;
        }
    }
}