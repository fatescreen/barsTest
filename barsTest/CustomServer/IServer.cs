using System;
using System.Collections.Generic;
using System.Text;

namespace barsTest.CustomServer
{
    interface IServer
    {
        public string getDataBaseSizeInGb();
        public string getServerSizeInGb();
    }
}
