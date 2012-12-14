using FluentHibernate.DAO.Models;
using FluentNHibernate.Mapping;

namespace FluentHibernate.DAO.ClassMap
{
    public class PeopleMap : ClassMap<People>
    {
        public PeopleMap()
        {
            Id(x => x.Id);
            Map(x => x.FirstName).Not.Nullable();
            Map(x => x.LastName).Not.Nullable();
            HasMany(x => x.Children);
        }
    }
}