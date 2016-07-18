using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangentIdeas.Core.Entities;

namespace TangentIdeas.MailWindowsService.Model
{
    public class MailServiceModel:DbContext
    {
        public MailServiceModel() : base("name=DefaultConnection")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Mail> Mail { get; set; }
    }
}
