using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mvc4.WebAPI.SPA.Web.FakeData
{
    public class FakeData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static List<FakeData> GetList()
        {
            var result = new List<FakeData>
                {
                    new FakeData
                        {
                            FirstName = "Hoang Gia",
                            LastName = "Le Huu"
                        },
                    new FakeData
                        {
                            FirstName = "Bao Duy",
                            LastName = "Truong Nguyen"
                        },
                    new FakeData
                        {
                            FirstName = "Hai",
                            LastName = "Nguyen Tuan"

                        }
                };
            return result;
        }

        public static FakeData GetItem()
        {
            return new FakeData
                {
                    FirstName = "Khoa",
                    LastName = "Tran Viet"
                };
        }
    }
}