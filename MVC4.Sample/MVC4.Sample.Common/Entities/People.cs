using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC4.Sample.Common.Entities
{
    public class People
    {
        public Guid Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName {get { return string.Format("{0} {1}", FirstName, LastName); }}
        public int Age { get; set; }

        public static List<People> GetLists()
        {
            var result = new List<People>();
            for (var i = 0; i < 10; i++)
            {
                var people = new People
                                 {
                                     Age = i,
                                     FirstName = string.Format("FirstName: {0}", i),
                                     LastName = string.Format("LastName: {0}", i),
                                     Id = Guid.NewGuid()
                                 };
                result.Add(people);
            }
            return result;
        } 
    }
}
