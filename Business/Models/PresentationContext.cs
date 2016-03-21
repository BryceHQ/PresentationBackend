using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using Core;
using System.Data.Entity.Validation;
using System.Data;

namespace Business.Models
{
    public class PresentationContext : DbContext
    {
        public PresentationContext()
            : base("DefaultConnection")
        {
        }

        public DbSet<User> User { get; set; }

        public DbSet<Presentation> Presentation { get; set; }
        public DbSet<History> History { get; set; }

        public DbSet<Folder> Folder { get; set; }

        public ErrorCode SafeSaveChanges()
        {
            try
            {
                this.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return new ErrorCode("数据检验失败");
            }
            catch (DataException dataException)
            {
                return new ErrorCode(string.Format("保存数据库出错，详细原因：{0}", dataException.Message));
            }

            return null;
        }
    }
}
