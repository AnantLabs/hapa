using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoClient
{
    interface IAutoObserver
    {
        void update(string message);
    }
}
