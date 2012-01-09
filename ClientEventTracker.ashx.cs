namespace Sitecore.SharedSource.ClientEventTracker
{
    using System.Web;
    using Sitecore.Analytics;

    /// <summary>
    /// Summary description for ClientEventTracker
    /// </summary>
    public class ClientEventTracker : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/javascript";
            TriggerEvent(context);
        }

        private static void TriggerEvent(HttpContext context)
        {
            var query = context.Request.QueryString;
            var eventName = query["eventName"];
            var text = query["text"];
            var key = query["key"];
            var data = query["data"];
            var integer = query["integer"];

            AnalyticsTracker.StartTracking();
            AnalyticsTracker.Current.Cancel();

            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            if (string.IsNullOrEmpty(text))
            {
                AnalyticsTracker.Current.PreviousPage.TriggerEvent(eventName);
            }
            else
            {
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(data))
                {
                    AnalyticsTracker.Current.PreviousPage.TriggerEvent(eventName, text);
                    return;
                }

                if (!string.IsNullOrEmpty(integer))
                {
                    AnalyticsTracker.Current.PreviousPage.TriggerEvent(eventName, text, key, data, MainUtil.GetInt(integer, 0));
                    return;
                }

                AnalyticsTracker.Current.PreviousPage.TriggerEvent(eventName, text, key, data);
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