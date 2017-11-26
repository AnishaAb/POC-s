using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Final.NHibernateModels
{
    public class Customer
    {
        public virtual int Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual bool Status { get; set; }
    }
}