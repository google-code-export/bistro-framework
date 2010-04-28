using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bistro.Controllers.Dispatch;
using NUnit.Framework;
using Bistro.Controllers;

namespace Bistro.UnitTests.Tests.Compatibility
{
	internal class CtrGroupCommon
	{
		internal CtrGroupCommon(params object[] controllersAndGroups)
		{
			Processed = false;
		}

		internal virtual bool ValidateNext(string controllerName)
		{
			return false;
		}

		protected List<object> groupsList;

		internal int GetCount()
		{
			return groupsList.OfType<String>().Count() + groupsList.OfType<CtrGroupCommon>().Sum(grp => grp.GetCount());
		}

		internal protected bool Processed { get; protected set; }

	}


	internal class CtrGroupUnordered : CtrGroupCommon
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="controllers"></param>
		internal CtrGroupUnordered(params object[] controllers)
		{
			groupsList = controllers.ToList();
			if (groupsList.Count ==0)
				throw new Exception("UrlControllersTest has invalid definition");

			processedGroups = new List<object>();
		}

		private List<object> processedGroups;



		internal override bool ValidateNext(string controllerName)
		{
			bool retVal = false;
			if (groupsList.OfType<String>().Contains(controllerName) && (!processedGroups.OfType<String>().Contains(controllerName)))
			{
				processedGroups.Add(controllerName);


				retVal = true;
			}
			else 
			{
				foreach (var group in groupsList.OfType<CtrGroupOrdered>())
				{
					if (processedGroups.Contains(group))
						continue;

					if (group.ValidateNext(controllerName))
					{
						if (group.Processed)
							processedGroups.Add(group);
						retVal = true;
						break;
					}
				}
			}
			Processed = (processedGroups.Count == groupsList.Count);
			return retVal;
		}



	}

	internal class CtrGroupOrdered : CtrGroupCommon
	{
		internal CtrGroupOrdered(params object[] controllers)
		{
			nextItem = 0;
			groupsList = controllers.ToList();
			if (groupsList == null)
				throw new Exception("UrlControllersTest has invalid definition");
		}

		private int nextItem;

		internal override bool ValidateNext(string controllerName)
		{
			bool retVal = true;
			object obj = groupsList[nextItem];
			if (obj is CtrGroupUnordered)
			{
				var ctrUnordered = obj as CtrGroupUnordered;
				if (!ctrUnordered.ValidateNext(controllerName))
				{
					retVal = false;
				}
				else
				{
					if (ctrUnordered.Processed)
						nextItem++;
				}
			} else 
			{
				String str = obj as String;
				if (str != controllerName)
				{
					retVal = false;
				}
				else
				{
					nextItem++;
				}

			}


			Processed = (nextItem >= groupsList.Count);
			return retVal;
		}



	}



    internal class UrlControllersTest
    {
        internal UrlControllersTest(string name, string url, params object[] controllers)
        {
            testUrl = url;

			rootGroup = new CtrGroupOrdered(controllers);
        }

        string testUrl;
		CtrGroupOrdered rootGroup;

        public void Validate(IControllerDispatcher dispatcher)
        {
			Func<String, ControllerInvocationInfo, String> sumStr = (oldStr, invInfo) => oldStr += "+" + invInfo.BindPoint.Controller.ControllerTypeName;

            var ctrlrs = dispatcher.GetControllers(testUrl);
			Assert.AreEqual(rootGroup.GetCount(), ctrlrs.Length, "Controller queues lengths are different. URL:{0}; Return Controllers:{1} ", testUrl, ctrlrs.Aggregate(String.Empty, sumStr));

			int i = 0;
			foreach (var controllerInfo in ctrlrs)
			{
				Assert.IsTrue(rootGroup.ValidateNext(controllerInfo.BindPoint.Controller.ControllerTypeName), "Controller names are different at position: {0}; Controllers:{1}", i,ctrlrs.Aggregate(String.Empty,sumStr));
				i++;
			}

            
        }
    }

}
