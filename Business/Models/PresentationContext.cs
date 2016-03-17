using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace Business.Models
{
    public class PresentationContext : DbContext
    {
        public PresentationContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Presentation> Presentation { get; set; }
        public DbSet<History> History { get; set; }
    }
}
