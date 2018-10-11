using System;
using Karpach.Remote.Keep.Command.Models;

namespace Karpach.Remote.Keep.Command.Helpers
{
    public class KeepRequestFactory
    {
        private readonly RandomHelper _randomHelper;

        public KeepRequestFactory()
        {
            _randomHelper = new RandomHelper();
        }

        public KeepRequest Create(Node[] nodes = null, string targetVersion = null)
        {            
            return new KeepRequest
            {
                ClientTimestamp = DateTimeOffset.Now,
                Nodes = nodes ?? new Node[0],
                TargetVersion = targetVersion,
                RequestHeader = new RequestHeader
                {
                    ClientSessionId = _randomHelper.GetSessionId(),
                    ClientVersion = new ClientVersion
                    {
                        Build = 9,
                        Major = 9,
                        Minor = 9,
                        Revision = 9
                    },
                    ClientPlatform = "ANDROID",
                    Capabilities = new[]
                    {
                        new Capability
                        {
                            Type = "NC"
                        },
                        new Capability
                        {
                            Type = "PI"
                        },
                        new Capability
                        {
                            Type = "LB"
                        },
                        new Capability
                        {
                            Type = "AN"
                        },
                        new Capability
                        {
                            Type = "SH"
                        },
                        new Capability
                        {
                            Type = "DR"
                        },
                        new Capability
                        {
                            Type = "TR"
                        },
                        new Capability
                        {
                            Type = "IN"
                        },
                        new Capability
                        {
                            Type = "SNB"
                        },
                        new Capability
                        {
                            Type = "MI"
                        },
                        new Capability
                        {
                            Type = "CO"
                        }
                    }
                }
            };
        }        
    }
}