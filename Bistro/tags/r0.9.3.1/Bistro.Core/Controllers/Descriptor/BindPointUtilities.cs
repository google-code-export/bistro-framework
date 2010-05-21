using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Bistro.Controllers.Descriptor
{

    /// <summary>
    /// Collection of utility methods for dealing with bind points.
    /// </summary>
    public static class BindPointUtilities
    {
        /// <summary>
        /// Regular expression for splitting a bind expression into its components. This experssion
        /// will match a slash (/), an ampersand (&), or a question mark when it is not followed by
        /// a slash, the EOF, or another question mark. This effectively splits the bind expression
        /// into path components, and query string components.
        /// </summary>
        private static Regex bindExpr = new Regex(@"/|\?(?!$|/|\?)|&", RegexOptions.Compiled);

        /// <summary>
        /// Regular expression for the path-part of a bind point. The structure is any character, except for a question mark.
        /// For question marks, only question marks not followed by either EOF, a slash (/) or another question mark (?) 
        /// are considered not part of the bind expression.
        /// </summary>
        private static Regex bindPathExpr = new Regex(@"\?(?!$|/|\?).*", RegexOptions.Compiled);

        /// <summary>
        /// A list of accepted REST verbs
        /// </summary>
        public static ICollection<string> BistroVerbs = new List<string>(new string[] { "GET", "POST", "PUT", "DELETE", "HEAD", "EVENT" });

        /// <summary>
        /// A list of accepted HTTP verbs
        /// </summary>
        public static ICollection<string> HttpVerbs = new List<string>(new string[] { "GET", "POST", "PUT", "DELETE", "HEAD" });

        /// <summary>
        /// Makes sure the url is [VERB/url], not [VERB url].
        /// Note that the url must be verb normalized, as this 
        /// method works off relative indices, and not actual verbs.
        /// </summary>
        /// <param name="url">The verb-qualified URL.</param>
        /// <returns></returns>
        public static string VerbNormalize(string url)
        {
            foreach (string verb in BistroVerbs)
            {
                if (!url.StartsWith(verb, StringComparison.OrdinalIgnoreCase))
                    continue;

                var remainder = url.Substring(verb.Length);
                return verb + "/" + remainder.Trim(' ', '/');
            }

            throw new ApplicationException(String.Format("\"{0}\" is not verb-qualified", url));
        }

        /// <summary>
        /// Makes sure tha the url is verb-qualified and normalized. If not qualified,
        /// the value of defaultVerb will be used to qualify the url.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="defaultVerb">The default verb.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">If the default verb is not a valid or supported http verb.</exception>
        public static string VerbQualify(string url, string defaultVerb)
        {
            if (IsVerbQualified(url))
                return VerbNormalize(url);

            var cleanedVerb = defaultVerb.ToUpper().Trim();
            if (!BistroVerbs.Contains(cleanedVerb))
                throw new ArgumentException(String.Format("\"{0}\" is not a valid HTTP verb", cleanedVerb));

            return Combine(cleanedVerb, url);
        }

        /// <summary>
        /// Determines whether the target bind site is prefixed with an HTTP verb.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>
        /// 	<c>true</c> if the url is verb-qualified; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsVerbQualified(string target)
        {
            // the verb can be specified as either "VERB url" or "VERB/url"
            var index = target.IndexOfAny(new char[] { ' ', '/' });

            // we don't want stuff that starts with a leading slash either.
            // that implies a url starting with a verb (e.g. - something.com/get/something)
            return (index > 0) && BistroVerbs.Contains(target.Substring(0, index).ToUpper());
        }

        /// <summary>
        /// Combines the specified uri1.
        /// </summary>
        /// <param name="uri1">The uri1.</param>
        /// <param name="uri2">The uri2.</param>
        /// <returns></returns>
        public static string Combine(string uri1, string uri2)
        {
            return uri1.TrimEnd('/', ' ') + '/' + uri2.TrimStart('/', ' ');
        }

        /// <summary>
        /// Gets the individual components of a Bind point
        /// </summary>
        /// <param name="bindPoint">The bind point.</param>
        /// <returns></returns>
        public static string[] GetBindComponents(string bindPoint)
        {
            return bindExpr.Split(bindPoint);
        }

        /// <summary>
        /// Trims off the query string part of a bind point, if any
        /// </summary>
        /// <param name="bindPoint">The bind point.</param>
        /// <returns></returns>
        public static string GetBindPath(string bindPoint)
        {
            return bindPathExpr.Replace(bindPoint, String.Empty);
        }
    }

}
