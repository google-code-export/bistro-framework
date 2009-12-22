﻿/****************************************************************************
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
using System.Text.RegularExpressions;
using Bistro.Controllers.Descriptor;
using Bistro.Configuration.Logging;
using Bistro.MethodsEngine.Reflection;

namespace Bistro.MethodsEngine.Subsets
{
    /// <summary>
    /// Class containing two genbindings - with positive and negative match for specific bind url.
    /// </summary>
    internal class GenBindingTuple
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenBindingTuple"/> class.
        /// </summary>
        /// <param name="_verbNormalizedUrl">The bind url with verb in form verb/url (i.e. GET/a/b/c/).</param>
        /// <param name="_engine">The methods engine.</param>
        internal GenBindingTuple(string _verbNormalizedUrl ,Engine _engine)
        {
            PositiveBind = new GenBinding(_verbNormalizedUrl, true, _engine);
            NegativeBind = new GenBinding(_verbNormalizedUrl, false, _engine);
            Processed = false;

            engine = _engine;
        }

        /// <summary>
        /// Tries the match an specific URL to the positive binding.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>GenBinding with the appropriate matchstatus.</returns>
        internal GenBinding TryMatchUrl(string url)
        {
            return PositiveBind.TryMatchUrl(url) ? PositiveBind : NegativeBind;
        }


        /// <summary>
        /// Marks that this tuple has been processed by the algorithm.
        /// </summary>
        internal void MarkProcessed() 
        {
            Processed = true;
        }

        /// <summary>
        /// field to store the link to the engine
        /// </summary>
        private Engine engine;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GenBindingTuple"/> is processed.
        /// </summary>
        /// <value><c>true</c> if processed; otherwise, <c>false</c>.</value>
        internal bool Processed { get; private set; }

        /// <summary>
        /// Gets the positive bind object.
        /// </summary>
        /// <value>The positive bind object.</value>
        internal GenBinding PositiveBind { get; private set; }

        /// <summary>
        /// Gets the negative bind object.
        /// </summary>
        /// <value>The negative bind object.</value>
        internal GenBinding NegativeBind { get; private set; }
    }


    /// <summary>
    /// Class, which represents a half of the url field. It contains a bind url and match status, which sets - 
    /// whether this half contains urls mathching that bind, or not.
    /// </summary>
    internal class GenBinding
    {
        /// <summary>
        /// Error messages
        /// </summary>
        enum Errors
        {
            [DefaultMessage("Error splitting incoming url: {0}")]
            ErrorSplittingUrl
        }

        /// <summary>
        /// Information messages
        /// </summary>
        enum Messages
        {
            [DefaultMessage("Matching url: {0}")]
            MessageMatchingUrl
        }

        /// <summary>
        /// The only constructor, which splits url to the items.
        /// </summary>
        /// <param name="_verbNormalizedUrl">The bind url with verb in form verb/url (i.e. GET/a/b/c/).</param>
        /// <param name="_matchStatus">status - whether target half of the url field must match this bind, or not.</param>
        internal GenBinding(string _verbNormalizedUrl, bool _matchStatus, Engine _engine)
        {
            engine = _engine;
            InitialUrl = _verbNormalizedUrl;
            MatchStatus = _matchStatus;

            items = GetSplitItems(InitialUrl, out totalLength, out lengthWithoutEndParams);

        }


        #region Private members
        /// <summary>
        /// Regex, which splits url by questionmarks placed near / (or surrounded by /)
        /// </summary>
        private static Regex splitRegex = new Regex(@"/\?/|\?/|/\?", RegexOptions.Compiled);

        /// <summary>
        /// Regex, which splits a string by /
        /// </summary>
        private static Regex subSplitRegex = new Regex(@"/", RegexOptions.Compiled);

        /// <summary>
        /// Regex to test for wildcards
        /// </summary>
        private static Regex wildCardRegex = new Regex(@"\A(?:\*|{[^}]+})\z", RegexOptions.Compiled);

        /// <summary>
        /// Regex to test for parameters
        /// </summary>
		private static Regex paramsRegex = new Regex(@"\A(?:{[^}]+})\z", RegexOptions.Compiled);



        /// <summary>
        /// contains url splitted by ? ar first level, and by / on the second.
        /// </summary>
        private List<List<string>> items;


        /// <summary>
        /// Total length in facets.
        /// </summary>
        private int totalLength = 0;

        /// <summary>
        /// Total length in facets, excluding parameters in the end (like /a/b/c/{aaa}/{bbb} - length = 3) 
        /// </summary>
        private int lengthWithoutEndParams = 0;

        /// <summary>
        /// link to the engine stored here.
        /// </summary>
        private Engine engine;


        #endregion

        #region private methods

        /// <summary>
        /// Gets the url split to the facets (at first - by ?, then by /)
        /// </summary>
        /// <param name="splitUrl">The bind (or simple) URL to split.</param>
        /// <param name="totalLengthLocal">The total length local.</param>
        /// <param name="lengthWithoutEnd">The length without end parameters (like /a/b/c/{aaa}/{bbb} - length = 3).</param>
        /// <returns></returns>
        private List<List<string>> GetSplitItems(string splitUrl, out int totalLengthLocal,out int lengthWithoutEnd)
        {
            totalLengthLocal = 0;
            var preSplitItems = splitRegex.Split(splitUrl);
            var tempItems = new List<List<string>>(preSplitItems.Length);
            foreach (string preSplitItem in preSplitItems)
            {
                var splitItems = subSplitRegex.Split(preSplitItem).Where(inputStr => inputStr != string.Empty);
                totalLengthLocal = totalLengthLocal + splitItems.Count();
                tempItems.Add(new List<string>(splitItems));
            }
            lengthWithoutEnd = totalLengthLocal;
            bool brk = false;
            for (int i = (tempItems.Count - 1); i >= 0; i--)
            {
                for (int j = tempItems[i].Count - 1; j >= 0; j--)
                {
					// {params} items should not be included in the length.
                    if (paramsRegex.IsMatch(tempItems[i][j]))
                        lengthWithoutEnd--;
                    else
                    {
                        brk = true;
                        break;
                    }
                }
                if (brk)
                    break;
            }
            return tempItems;
        }


        #endregion




        #region Internal properties

        /// <summary>
        /// initial(not splitted) bind url stored here.
        /// </summary>
        internal string InitialUrl
        {
            get;
            set;
        }

        /// <summary>
        /// Match status (true/false)
        /// </summary>
        internal bool MatchStatus
        {
            get;
            private set;
        }

        #endregion

        #region Internal methods


        /// <summary>
        /// Tries to match URL to bind URL (NOT to the half of the URL field).
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns>result of the match</returns>
        internal bool TryMatchUrl(string requestUrl)
        {
            engine.Logger.Report(Messages.MessageMatchingUrl, requestUrl);

            int a,b;

            string[] splitQueryString = requestUrl.Split('?');
            string[] requestComponents = smartUrlSplit(splitQueryString[0]);

            // if there are more bind components than there are url components, we don't have a match.
            if (requestComponents.Length < lengthWithoutEndParams)
                return false;

            List<string> firstPart = items[0];

            bool firstItemMatchImpossible = false;
            for (int i = 0; i < firstPart.Count; i++)
            {
                if ((wildCardRegex.IsMatch(firstPart[i])) || (firstPart[i] == requestComponents[i]))
                    continue;

                firstItemMatchImpossible = true;
                break;
            }

            if (firstItemMatchImpossible)
                return false;

            int positionInMatchPart = firstPart.Count;

            var currentMatchPartEnum = items.GetEnumerator();
            currentMatchPartEnum.MoveNext();

            List<string> currentMatchPart = (currentMatchPartEnum.MoveNext()) ? currentMatchPartEnum.Current : null;

            while (currentMatchPart != null)
            {
                if ((positionInMatchPart + currentMatchPart.Count) <= requestComponents.Length)
                {
                    bool placed = true;
                    for (int i = 0; i < currentMatchPart.Count; i++)
                    {
                        if (wildCardRegex.IsMatch(currentMatchPart[i]))
                            continue;
                        if (currentMatchPart[i] == requestComponents[i + positionInMatchPart])
                            continue;
                        placed = false;
                        break;
                    }

                    if (placed)
                    {
                        positionInMatchPart = positionInMatchPart + currentMatchPart.Count;
                        currentMatchPartEnum.MoveNext();
                        currentMatchPart = currentMatchPartEnum.Current;
                    }
                    else 
                        positionInMatchPart++;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// Matches this half of the url field with the method subset of bind urls.
        /// </summary>
        /// <param name="methodUrlsSubset">The method subset of bind urls.</param>
        /// <returns></returns>
        internal bool MatchWithSubset(MethodUrlsSubset methodUrlsSubset)
        {
            List<GenBinding> newList = new List<GenBinding>(methodUrlsSubset.BindingsList);
            IEnumerable<GenBinding> matchBindings = newList.Where(binding => binding.MatchStatus);
            IEnumerable<GenBinding> noMatchBindings = newList.Where(binding => !binding.MatchStatus);

            if (this.MatchStatus)
            {
                foreach (GenBinding binding in matchBindings)
                {
                    if (!CompareWithMatch(binding))
                        return false;
                }

                foreach (GenBinding binding in noMatchBindings)
                {
                    if (!CompareMatchAndNoMatch(this, binding))
                        return false;
                }

            }
            else
            {
                foreach (GenBinding binding in matchBindings)
                {
                    if (!CompareMatchAndNoMatch(binding, this))
                        return false;
                }

            }

            return true;


        }


        /// <summary>
        /// Compares this Genbinding with another GenBinding for intersection. Both must have match status set to true 
        /// for this part of algorithm to work properly
        /// </summary>
        /// <param name="matchBind">Another GenBinding object.</param>
        /// <returns>Result of the evaluation. true/false</returns>
        private bool CompareWithMatch(GenBinding matchBind)
        {
            if (!matchBind.MatchStatus || !this.MatchStatus)
                throw new ApplicationException("improper usage of CompareWithMatch");

            List<string> firstPartOfThis = this.items.Count == 0 ? new List<string>() : new List<string>(this.items[0]);

            List<string> firstPart = matchBind.items.Count == 0 ? new List<string>() : new List<string>(matchBind.items[0]);
            #region Check for different beginning

            int smallestSize = firstPartOfThis.Count < firstPart.Count ? firstPartOfThis.Count : firstPart.Count;


            for (int i = 0; i < smallestSize; i++)
            {
                if ((wildCardRegex.IsMatch(firstPart[i])) || (wildCardRegex.IsMatch(firstPartOfThis[i])) || (firstPartOfThis[i] == firstPart[i]))
                    continue;
                return false;
            }

            #endregion
            return true;
        }


        /// <summary>
        /// Compares two GenBindings with different match statuses for intersection
        /// </summary>
        /// <param name="matchBind">The match bind object.</param>
        /// <param name="noMatchBind">The no match bind object.</param>
        /// <returns>Result of the evaluation. true/false</returns>
        private static bool CompareMatchAndNoMatch(GenBinding matchBind, GenBinding noMatchBind)
        {
            if (!matchBind.MatchStatus && noMatchBind.MatchStatus)
                throw new ApplicationException("improper usage of CompareMatchAndNoMatch");

            List<string> firstPart = matchBind.items.Count == 0 ? new List<string>() : new List<string>(matchBind.items[0]);

            List<string> firstPartOfThis = noMatchBind.items.Count == 0 ? new List<string>() : new List<string>(noMatchBind.items[0]);

            #region Get the start point firstPartOfThis - B; firstPart - A
            if (firstPartOfThis.Count > firstPart.Count)
                return true;

            bool firstItemMatchImpossible = false;
            for (int i = 0; i < firstPartOfThis.Count; i++)
            {
                if ((wildCardRegex.IsMatch(firstPartOfThis[i])) || (firstPart[i] == firstPartOfThis[i]))
                    continue;

                firstItemMatchImpossible = true;
                break;
            }

            if (firstItemMatchImpossible)
                return true;

            var currentNoMatchPartEnum = noMatchBind.items.GetEnumerator();
            currentNoMatchPartEnum.MoveNext();
            var currentMatchPartEnum = matchBind.items.GetEnumerator();

            int positionInMatchPart = (currentNoMatchPartEnum.Current == null) ? 0 : currentNoMatchPartEnum.Current.Count;

            List<string> currentNoMatchPart = (currentNoMatchPartEnum.MoveNext()) ? currentNoMatchPartEnum.Current : null;
            List<string> currentMatchPart = (currentMatchPartEnum.MoveNext()) ? currentMatchPartEnum.Current : null;



            #endregion

            while ((currentNoMatchPart != null) && (currentMatchPart != null))
            {
                //try to place it.
                if (positionInMatchPart + currentNoMatchPart.Count <= currentMatchPart.Count)
                {
                    bool placed = true;
                    for (int i = 0; i < currentNoMatchPart.Count; i++)
                    {
                        if (wildCardRegex.IsMatch(currentNoMatchPart[i]))
                            continue;
                        if (currentNoMatchPart[i] == currentMatchPart[i + positionInMatchPart])
                            continue;

                        placed = false;
                        break;
                    }

                    if (placed)
                    {
                        positionInMatchPart = positionInMatchPart + currentNoMatchPart.Count;
                        currentNoMatchPartEnum.MoveNext();
                        currentNoMatchPart = currentNoMatchPartEnum.Current;
                    }
                    else
                        positionInMatchPart++;

                }
                else
                {
                    currentMatchPartEnum.MoveNext();
                    currentMatchPart = currentMatchPartEnum.Current;
                    positionInMatchPart = 0;
                }

            }

            if ((currentNoMatchPart == null) && (currentMatchPart != null))
            {
                // noMatch binding completely matches with one of the match bindings.
                return false;
            }



            return true;

        }


        #region from the old ControllerDispatcher
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
        #endregion


        #endregion



    }
}
