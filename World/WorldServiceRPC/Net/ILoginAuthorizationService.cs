
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WorldServiceDataContracts.Net;

namespace WorldServiceContracts.Net
{
    [ServiceContract(Name = "WorldServiceContracts.Net.LoginStatusService")]
    public interface ILoginAuthorizationService
    {
        Task<SetLoginAuthorizationResponse> SetLoginAuthorization(SetLoginAuthorizationRequest req);
    }
}
