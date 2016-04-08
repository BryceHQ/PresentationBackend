using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Threading.Tasks;

namespace Web.Core
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Presentation> Presentation { get; set; }
        public DbSet<History> History { get; set; }

        public DbSet<Folder> Folder { get; set; }

        public DbSet<Attachment> Attachment { get; set; }



        public async Task<ErrorCode> SafeSaveChangesAsync()
        {
            try
            {
                await this.SaveChangesAsync();
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