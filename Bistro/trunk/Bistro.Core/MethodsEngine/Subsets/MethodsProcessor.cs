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
using Bistro.Controllers.Descriptor;
using Bistro.MethodsEngine.Reflection;
using System.Text.RegularExpressions;
using Bistro.Configuration.Logging;
using Bistro.Controllers;
using System.Diagnostics;

namespace Bistro.MethodsEngine.Subsets
{

    /// <summary>
    /// This class is responsible for handling of all the url subsets and bindings registered in the engine.
    /// </summary>
    public class MethodsProcessor
    {


        enum Errors
        {
            [DefaultMessage("Error processing link - method not found: {0}")]
            ErrorMethodNotFound,
			[DefaultMessage("Bindings have wrong order in the common list and method.")]
			ErrorWrongBindOrder
        }

		enum Messages
		{
			[DefaultMessage("Method comparison completed in {0} ms (TryMatchUrlGetParams) ")]
			MethodTryMatch,
			[DefaultMessage("Method matched and found in {0} ms (DictionarySearch) ")]
			MethodMatchedAndFound,
			[DefaultMessage("Updating bind points! - Long process.")]
			UpdatingBindPoints
		}


        /// <summary>
        /// Initializes a new instance of the <see cref="SubSetsProcessor"/> class.
        /// </summary>
        /// <param name="_engine">The engine.</param>
        public MethodsProcessor(EngineControllerDispatcher _engine)
        {
            engine = _engine;
            allMethods = new List<BistroMethod>();
            allBindings = new List<MethodBindingTuple>();
            // We need to have at least one empty method. Otherwise it will be impossible to create new non-empty methods.
            allMethods.Add(new BistroMethod(_engine));


        }

        #region Private fields


        /// <summary>
        /// list to store all the binding tuples(positive/negative).
        /// </summary>
        private List<MethodBindingTuple> allBindings;

        /// <summary>
        /// List to store all the methods.
        /// </summary>
        private List<BistroMethod> allMethods;
        public List<BistroMethod> AllMethods
        {
            get { return allMethods; }
        }

		private Dictionary<string, BistroMethod> methodsDictionary;

        /// <summary>
        /// Link to the engine.
        /// </summary>
        private EngineControllerDispatcher engine;


        #endregion




        /// <summary>
        /// Creates new binding tuple from the binding URL and executes new level of methods creation to replace old methods list.
        /// </summary>
        /// <param name="verbNormalizedUrl">The verb normalized URL.</param>
        public void AddNewBinding(string verbNormalizedUrl)
        {
            allBindings.Add(new MethodBindingTuple(verbNormalizedUrl,engine));
            allMethods = CreateNewMethodsLevel();

        }


		/// <summary>
		/// Searches and gets the method by an URL.
		/// </summary>
		/// <param name="requestUrl">The request URL.</param>
		/// <param name="getParams">The returned params from the query string.</param>
		/// <returns></returns>
        internal BistroMethod GetMethodByUrl(string requestUrl)//, out Dictionary<IMethodsBindPointDesc,Dictionary<string,string>> getParams)
        {
            // Compare with each Binding
			List<MethodBinding> bindingsToSearch = new List<MethodBinding>();
			Stopwatch sw1 = new Stopwatch();
			sw1.Start();
            foreach (MethodBindingTuple tuple in allBindings)
            {
				MethodBinding tempBind = tuple.TryMatchUrlGetParams(requestUrl);
				bindingsToSearch.Add(tempBind);
            }



			string key = GetKeyFromBindList(bindingsToSearch);

			if (!methodsDictionary.ContainsKey(key))
			{
				engine.Logger.Report(Errors.ErrorMethodNotFound, requestUrl);
				return null;
			}
			engine.Logger.Report(Messages.MethodMatchedAndFound, sw1.ElapsedMilliseconds.ToString());
			return methodsDictionary[key];
        }





        /// <summary>
        /// Updates bindpoints information in methods.
        /// </summary>
        internal void UpdateBindPoints()
        {
			engine.Logger.Report(Messages.UpdatingBindPoints);
			methodsDictionary = new Dictionary<string,BistroMethod>();
            foreach(BistroMethod subset in allMethods)
            {
				methodsDictionary.Add(GetKeyFromBindList(subset.BindingsList), subset);
                subset.UpdateBindPoints();
            }
        }


        #region Private methods

		/// <summary>
		/// Creates the key from bind list.
		/// </summary>
		/// <param name="bindList">The bind list.</param>
		/// <returns></returns>
		private string GetKeyFromBindList(List<MethodBinding> bindList)
		{
			StringBuilder sb = new StringBuilder();
			int i = 0;
			foreach (var binding in bindList)
			{
				if ((binding.MatchStatus ? allBindings[i].PositiveBind : allBindings[i].NegativeBind) != binding)
					engine.Logger.Report(Errors.ErrorWrongBindOrder);
				sb.Append(binding.MatchStatus ? "1" : "0");

				i++;
			}
			return sb.ToString();
		}


        /// <summary>
        /// Normalizes the url and splits it by slashes, not presenting a blank element if the 
        /// url begins with a slash
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        private string[] smartUrlSplit(string url)
        {
            // trim any excess whitespace, and also the leading /
            string workingCopy = url.Trim().TrimStart('/');

            return BindPointUtilities.GetBindComponents(url);
        }



        /// <summary>
        /// Creates a new methods level for each unprocessed GenBindingTuple iteratively.
        /// </summary>
        /// <returns>Methods list after the last iteration.</returns>
        private List<BistroMethod> CreateNewMethodsLevel()
        {
            List<BistroMethod> newBindingMethods = allMethods;
            List<BistroMethod> oldBindingMethods;
            foreach (MethodBindingTuple tuple in allBindings.Where(tpl => !tpl.Processed))
            {
                oldBindingMethods = newBindingMethods;
                newBindingMethods = new List<BistroMethod>();
                
                foreach (BistroMethod group in allMethods)
                {
                    BistroMethod newGroupTrue = group.ApplyBinding(tuple.PositiveBind);
                    BistroMethod newGroupFalse = group.ApplyBinding(tuple.NegativeBind);
                    tuple.MarkProcessed();

                    if (newGroupTrue != null)
                        newBindingMethods.Add(newGroupTrue);
                    if (newGroupFalse != null)
                        newBindingMethods.Add(newGroupFalse);
                }
            }

            return newBindingMethods;
        }


        #endregion

    }
}
