/****************************************************************************
 * 
 *  Bistro Framework Copyright © 2003-2009 Hill30 Inc
 *
 *  This file is part of Bistro Framework.
 *
 *  Bistro Framework is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  Bistro Framework is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with Bistro Framework.  If not, see <http://www.gnu.org/licenses/>.
 *  
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.MethodsEngine.Reflection;
using Bistro.Controllers.Descriptor;
using Bistro.Configuration.Logging;

namespace Bistro.MethodsEngine
{
    /// <summary>
    /// Class, implementing topological sorting algorithm for Bind Points.
    /// </summary>
    internal class DependencyGraph
    {

        /// <summary>
        /// Class, which allows sorting of Bind points by priority, before/payload/after/teardown and bind length
        /// </summary>
        public class VertexKey : IComparable<VertexKey>
        {

            enum Errors 
            {
                [DefaultMessage("Same controller name in graph: {0}")]
                SameControllerName
            }


            /// <summary>
            /// Initializes a new instance of the <see cref="VertexKey"/> class.
            /// </summary>
            /// <param name="_vertex">The vertex.</param>
            public VertexKey(Vertex _vertex)
            {
                bindPoint = _vertex.BindPoint;
                vertex = _vertex;
            }

            /// <summary>
            /// corresponding bindpoint.
            /// </summary>
            private IMethodsBindPointDesc bindPoint;

            /// <summary>
            /// corresponding vertex
            /// </summary>
            private Vertex vertex;

            /// <summary>
            /// Gets the controller type name.
            /// </summary>
            /// <value>The controller type name.</value>
            private string ctrTypeName
            {
                get { return bindPoint.Controller.ControllerTypeName; }
            }

            /// <summary>
            /// Gets the bindpoint priority.
            /// </summary>
            /// <value>The bindpoint priority.</value>
            private int priority
            {
                get { return bindPoint.Priority; }
            }

            /// <summary>
            /// Gets the bind type.
            /// </summary>
            /// <value>The bind type.</value>
            private BindType bindType
            {
                get { return bindPoint.ControllerBindType; }
            }

            /// <summary>
            /// Gets the length of the bind in facets.
            /// </summary>
            /// <value>The length of the bind in facets.</value>
            private int bindLength
            {
                get { return bindPoint.BindLength; }
            }

        
            #region IComparable<VertexKey> Members

            /// <summary>
            /// Compares the current object with another object of the same type.
            /// </summary>
            /// <param name="other">An object to compare with this object.</param>
            /// <returns>
            /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings:
            /// Value
            /// Meaning
            /// Less than zero
            /// This object is less than the <paramref name="other"/> parameter.
            /// Zero
            /// This object is equal to <paramref name="other"/>.
            /// Greater than zero
            /// This object is greater than <paramref name="other"/>.
            /// </returns>
            public int CompareTo(VertexKey other)
            {
                if (this.vertex == other.vertex)
                    return 0;// vertexes are unique.

                Func<int, int, int> nonZero = (a, b) => a == 0 ? b : a;

                if (ctrTypeName == other.ctrTypeName)
                {
                    vertex.Graph.Engine.Logger.Report(Errors.SameControllerName, ctrTypeName);
                    throw new ApplicationException(String.Format("Same controller type name found in graph: {0}", ctrTypeName));
                }

                return nonZero(bindType.CompareTo(other.bindType),
                               nonZero(other.priority.CompareTo(this.priority),
                                    nonZero(bindLength.CompareTo(other.bindLength), this.ctrTypeName.CompareTo(other.ctrTypeName))));
            }

            #endregion
        }


        #region Vertex
        public class Vertex
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="graph"></param>
            /// <param name="bindPoint"></param>
            public Vertex(DependencyGraph graph, IMethodsBindPointDesc bindPoint)
            {
                this.Graph = graph;
                this.bindPoint = bindPoint;
                children = new List<Vertex>();
                isRoot = true;
                index = -1;
                visited = false;
            }

            public List<Vertex> Children { get { return children; } }
            internal IMethodsBindPointDesc BindPoint { get { return bindPoint; } }

            private IMethodsBindPointDesc bindPoint;
            private List<Vertex> children;
            private bool visited;
            public bool isRoot;
            public int index;
            public int ParentCount = 0;
            internal DependencyGraph Graph;


            /// <summary>
            /// 
            /// </summary>
            /// <param name="index"></param>
            /// <returns></returns>
            internal int Traverse(int index)
            {
                if (visited)
                    return -1;  // we ran into a loop
                visited = true;
                if (this.index == -1)
                    Graph.vertexCount++;
//                this.seqNumber = this.index = ++index;
                this.index = ++index;
                foreach (Vertex child in children)
                    if ((index = child.Traverse(index)) == -1)
                        return -1;
                visited = false;
                return index;
            }

            internal void IncreaseParents()
            {
                foreach (Vertex child in children)
                {
                    child.ParentCount++;
                }
            }
        }
        #endregion


        /// <summary>
        /// Dictionary to link Bind points to vertices
        /// </summary>
        Dictionary<IMethodsBindPointDesc, Vertex> vertices = new Dictionary<IMethodsBindPointDesc, Vertex>();

        /// <summary>
        /// initial list of bind points.
        /// </summary>
        List<IMethodsBindPointDesc> listToSort;


        /// <summary>
        /// total vertices count.
        /// </summary>
        int vertexCount = 0;

        /// <summary>
        /// Bistro Engine
        /// </summary>
        internal EngineControllerDispatcher Engine;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyGraph"/> class.
        /// </summary>
        /// <param name="vertices">List of the bind points.</param>
        public DependencyGraph(EngineControllerDispatcher _engine, List<IMethodsBindPointDesc> vertices)
        {
            Engine = _engine;
            listToSort = vertices;
            foreach (IMethodsBindPointDesc bindPoint in vertices)
                this.vertices.Add(bindPoint, new Vertex(this, bindPoint));
        } 

        /// <summary>
        /// Adds the edge to the graph.
        /// </summary>
        /// <param name="providingBindPoint">The providing bind point.</param>
        /// <param name="consumingBindPoint">The consuming bind point.</param>
        internal void AddEdge(IMethodsBindPointDesc providingBindPoint, IMethodsBindPointDesc consumingBindPoint)
        {
            Vertex endpoint = vertices[consumingBindPoint];
            Vertex startpoint = vertices[providingBindPoint];
            if (!startpoint.Children.Contains(endpoint))
            {
                startpoint.Children.Add(endpoint);
                endpoint.isRoot = false;
            }
        }


        /// <summary>
        /// Sorts listToSort if that's possible
        /// </summary>
        /// <returns>true if sort succeeded, otherwise - false</returns>
        internal bool TopologicalSort(out List<IMethodsBindPointDesc> listSorted)
        {
            listSorted = listToSort;

            int index = 0;
            foreach (Vertex origin in vertices.Values)
                if (origin.isRoot)
                    if ((index = origin.Traverse(index)) == -1)
                        return false;
            if (vertexCount == vertices.Count)
            {
                foreach (Vertex vrt in vertices.Values)
                    vrt.IncreaseParents();


                listSorted = new List<IMethodsBindPointDesc>();

                SortedList<VertexKey, Vertex> nextChildrenToSort =
                    vertices.Values.Where(a => a.isRoot).Aggregate(new SortedList<VertexKey, Vertex>(), (acc, vert) => { acc.Add(new VertexKey(vert), vert); return acc; });

                while (nextChildrenToSort.Count != 0)
                {
                    listSorted.Add(nextChildrenToSort.Values[0].BindPoint);

                    Vertex vrt = nextChildrenToSort.Values[0];

                    nextChildrenToSort.RemoveAt(0);

                    foreach (Vertex vrtNew in vrt.Children)
                    {
                        vrtNew.ParentCount--;
                        if (vrtNew.ParentCount == 0)
                        {
                            VertexKey vrtNewKey = new VertexKey(vrtNew);
                            if (!nextChildrenToSort.ContainsKey(vrtNewKey))
                                nextChildrenToSort.Add(vrtNewKey, vrtNew);
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}
