using System.Collections.Generic;

namespace Domain.Entities
{
    public class BaseRateCode
    {
        public BaseRateCodeEnum Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Agreement> Agreements { get; set; }
    }
}
