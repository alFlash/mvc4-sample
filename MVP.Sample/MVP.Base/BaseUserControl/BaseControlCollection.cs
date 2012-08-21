using System.Collections.Generic;
using System.Web;
using MVP.Base.BasePresenter;

namespace MVP.Base.BaseUserControl
{
    public static class BaseControlCollection
    {
        /// <summary>
        /// Gets the controls.
        /// </summary>
        public static Dictionary<string, Dictionary<string, IBaseUserControl>> Controls
        {
            get
            {
                if (HttpContext.Current.Session["SS_BaseControlCollection_Key"] == null)
                {
                    HttpContext.Current.Session["SS_BaseControlCollection_Key"] = new Dictionary<string, Dictionary<string, IBaseUserControl>>();
                }
                return (Dictionary<string, Dictionary<string, IBaseUserControl>>)HttpContext.Current.Session["SS_BaseControlCollection_Key"];
            }
        }

        /// <summary>
        /// Registers the specified parent key.
        /// </summary>
        /// <param name="parentKey">The parent key.</param>
        /// <param name="key">The key.</param>
        /// <param name="control">The control.</param>
        public static void Register(string parentKey, string key, IBaseUserControl control)
        {
            if (!string.IsNullOrEmpty(parentKey))
            {
                lock (Controls)
                {
                    if (Controls.ContainsKey(parentKey))
                    {
                        if (Controls[parentKey] == null)
                        {
                            Controls[parentKey] = new Dictionary<string, IBaseUserControl>();
                        }
                        if (Controls[parentKey].ContainsKey(key))
                        {
                            Controls[parentKey][key] = control;
                        }
                        else
                        {
                            Controls[parentKey].Add(key, control);
                        }
                    }
                    else
                    {
                        Controls.Add(parentKey, new Dictionary<string, IBaseUserControl>());
                        Controls[parentKey].Add(key, control);
                    }
                }
            }
        }
        
    }
}
