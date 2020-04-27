using System;
using System.Collections.Generic;
using System.Text;

namespace barsTest.CustomServer
{
    interface IServer
    {
        public float getDataBaseSizeInGb();
        public float getServerSizeInGb();
    }
}
