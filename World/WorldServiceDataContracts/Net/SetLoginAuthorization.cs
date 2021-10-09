
using AuthenticationServiceModel;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace WorldServiceDataContracts.Net
{
    [DataContract]
    public class SetLoginAuthorizationRequest
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public AuthenticationResponse AuthenticationToken { get; set; }

        [DataMember(Order = 3)]
        public AccountAuthorization AccountAuthorization { get; set; }
    }

    [DataContract]
    public class SetLoginAuthorizationResponse
    {
        [DataMember(Order = 1)]
        public string Message { get; set; }

    }
}
