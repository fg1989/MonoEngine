using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoEngine.Engine
{
    public class Bridge
    {
        Edge edge1;
        Edge edge2;
        float rigidity = 1;

        public Bridge(Edge e1, Edge e2) 
        {
            edge1 = e1;
            edge2 = e2;
        }
        public Bridge(Edge edge1, Edge edge2, float rigidity): this (edge1,edge2)
        {
            this.rigidity = rigidity;
        }
    }
}
