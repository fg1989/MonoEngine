using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Engine
{
    /// <summary>
    /// Ceci concerne les events qui s'appele à chaque update
    /// </summary>
    /// <param name="deltaTime">temps depuis la dernière update</param>
    public delegate void Update(float deltaTime);

}
