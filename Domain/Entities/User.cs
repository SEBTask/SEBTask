using System.Collections.Generic;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Agreement> Agreements { get; set; }
    }
}
