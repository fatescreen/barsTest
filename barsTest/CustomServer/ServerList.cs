using System.Collections.Generic;

namespace barsTest.CustomServer
{
    /// <summary>
    /// Class for creating list of Servers
    /// </summary>
    class ServerList : IServerList
    {
        public List<Server> servers { get; set; }        

        public ServerList()
        {

        }

    }
}
