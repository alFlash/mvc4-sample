using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace MVP.Base.BaseUserControl
{
    public static class BaseControlCollection
    {
        public static Dictionary<string, Dictionary<string, BaseUserControl>> Controls
        {
            get
            {
                if (HttpContext.Current.Session["SS_BaseControlCollection_Key"] == null)
                {
                    HttpContext.Current.Session["SS_BaseControlCollection_Key"] = new Dictionary<string, Dictionary<string, BaseUserControl>>();
                }
                return (Dictionary<string, Dictionary<string, BaseUserControl>>)HttpContext.Current.Session["SS_BaseControlCollection_Key"];
            }
        }

        public static void Register(string parentKey, string key, BaseUserControl control)
        {
            if (!string.IsNullOrEmpty(parentKey))
            {
                lock (Controls)
                {
                    if (Controls.ContainsKey(parentKey))
                    {
                        if (Controls[parentKey] == null)
                        {
                            Controls[parentKey] = new Dictionary<string, BaseUserControl>();
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
                        Controls.Add(parentKey, new Dictionary<string, BaseUserControl>());
                        Controls[parentKey].Add(key, control);
                    }
                }
            }
        }
        
    }
}
