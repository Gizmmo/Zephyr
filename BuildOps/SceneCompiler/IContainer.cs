using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zephyr.BuildOps.SceneCompiler
{
    public interface IContainer
    {
        void Serialize(string path);
    }
}
