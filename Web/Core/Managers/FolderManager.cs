using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Web.Core
{
    public class FolderManager
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        private static FolderManager _instance = new FolderManager();

        public static FolderManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private FolderManager()
        {
        }


        #region Add
        public async Task<IAsyncResult<Folder>> Add(Folder model)
        {
            return await this.Add(model, true);
        }

        public async Task<IAsyncResult<Folder>> Add(Folder model, bool saveChanges)
        {
            return await this.InternalAdd(this._context, model, saveChanges);
        }

        internal async Task<IAsyncResult<Folder>> InternalAdd(ApplicationDbContext context, Folder model, bool saveChanges)
        {
            model.Guid = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;

            var result = context.Folder.Add(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<Folder>(result, error);
        }
        #endregion


        #region Update
        public async Task<IAsyncResult<Folder>> Update(Folder model)
        {
            return await this.Update(model, true);
        }

        public async Task<IAsyncResult<Folder>> Update(Folder model, bool saveChanges)
        {
            return await this.InternalUpdate(this._context, model, saveChanges);
        }

        internal async Task<IAsyncResult<Folder>> InternalUpdate(ApplicationDbContext context, Folder model, bool saveChanges)
        {
            model.LastUpdateTime = DateTime.Now;
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<Folder>(model, error);
        }
        #endregion


        #region Remove
        public async Task<IAsyncResult<Folder>> Remove(int id)
        {
            return await this.Remove(id, true);
        }

        public async Task<IAsyncResult<Folder>> Remove(int id, bool saveChanges)
        {
            var model = _context.Folder.FirstOrDefault(a => a.Id == id);
            var result = _context.Folder.Remove(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await _context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<Folder>(result, error);
        }
        #endregion

        
        #region Find
        public async Task<Folder> Find(int id)
        {
            return await _context.Folder.FindAsync(id);
        }

        public async Task<Folder> FirstOrDefault(Expression<Func<Folder, bool>> predicate)
        {
            return await _context.Folder.FirstOrDefaultAsync(predicate);
        }

        public async Task<Folder[]> All()
        {
            return await _context.Folder.ToArrayAsync();
        }
        #endregion
    }
}