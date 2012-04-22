using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoClient
{
    public interface IAutoObserver
    {
        void update(string message);
    }
}
