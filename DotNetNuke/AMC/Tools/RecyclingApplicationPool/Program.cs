using System;
using Microsoft.Web.Administration;

namespace RecyclingApplicationPool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                foreach (string s in args)
                {
                    Run(s);
                }
            }
            else
            {
                Console.WriteLine("Please input application pool name");
            }
        }

        private static void Run(string poolName)
        {
            try
            {
                var serverManager = new ServerManager();
                ApplicationPoolCollection applicationPoolCollection = serverManager.ApplicationPools;
                ApplicationPool arnTestApplicationPool = null;

                foreach (ApplicationPool applicationPool in applicationPoolCollection)
                {
                    if (string.Compare(applicationPool.Name, poolName, true) == 0)
                    {
                        arnTestApplicationPool = applicationPool;
                        break;
                    }
                }

                if (arnTestApplicationPool != null)
                {
                    if (arnTestApplicationPool.State == ObjectState.Stopped)
                    {
                        arnTestApplicationPool.Start();
                    }
                    else
                    {
                        arnTestApplicationPool.Recycle();
                    }
                }
            }
            catch (ServerManagerException ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}