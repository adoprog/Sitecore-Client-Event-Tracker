using System;
using System.Web;
using Sitecore.Analytics;
using Sitecore.Analytics.Data;
using Sitecore.Diagnostics;
using System.Web.SessionState;

namespace Sitecore.SharedSource.ClientEventTracker
{
    /// <summary>
    /// Summary description for ClientEventTracker
    /// </summary>
    public class ClientEventTracker : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/javascript";
            TriggerEvent(context);
        }

        private static void TriggerEvent(HttpContext context)
        {
            try
            {
                var query = context.Request.QueryString;
                var eventName = query["eventName"];
                var text = query["text"];
                var key = query["key"];
                var data = query["data"];
                var integer = query["integer"];

                if (string.IsNullOrEmpty(eventName))
                {
                    return;
                }

                if (!Tracker.Enabled)
                {
                    return;
                }

                if (!Tracker.IsActive || Tracker.Current == null)
                {
                    Tracker.Initialize(); // init tracker without tracking the endpoint as a page
                }

                if (Tracker.Current == null || Tracker.Current.Interaction == null)
                {
                    return;
                }

                if (Tracker.Current.Interaction.CurrentPage == null)
                {
                    return;
                }


                if (string.IsNullOrEmpty(text))
                {
                    Tracker.Current.Interaction.CurrentPage.Register(eventName, string.Empty);
                }
                else
                {
                    if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(data))
                    {
                        Tracker.Current.Interaction.CurrentPage.Register(eventName, text);
                        return;
                    }

                    var eventData = new PageEventData(eventName)
                    {
                        DataKey = key,
                        Data = data,
                        Text = text
                    };
                    Tracker.Current.Interaction.CurrentPage.Register(eventData);
                    return;

                }
            }
            catch (Exception exception)
            {
                Log.Error(string.Concat("Sitecore.SharedSource.ClientEventTracker.TriggerEvent: error in event triggering. requestUrl: ", context.Request.Url.AbsolutePath), exception, typeof(ClientEventTracker));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
