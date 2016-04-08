using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;


namespace Web.Core
{
    public class HistoryManager
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        private static HistoryManager _instance = new HistoryManager();

        public static HistoryManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private HistoryManager()
        {

        }


        #region Add
        public async Task<IAsyncResult<History>> Add(History model)
        {
            return await this.Add(model, true);
        }

        public async Task<IAsyncResult<History>> Add(History model, bool saveChanges)
        {
            return await this.InternalAdd(this._context, model, saveChanges);
        }

        internal async Task<IAsyncResult<History>> InternalAdd(ApplicationDbContext context, History model, bool saveChanges)
        {
            model.Guid = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;

            var result = context.History.Add(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<History>(result, error);
        }
        #endregion


        #region Update
        public async Task<IAsyncResult<History>> Update(History model)
        {
            return await this.Update(model, true);
        }

        public async Task<IAsyncResult<History>> Update(History model, bool saveChanges)
        {
            return await this.InternalUpdate(this._context, model, saveChanges);
        }

        internal async Task<IAsyncResult<History>> InternalUpdate(ApplicationDbContext context, History model, bool saveChanges)
        {
            model.LastUpdateTime = DateTime.Now;
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<History>(model, error);
        }
        #endregion


        #region Remove
        public async Task<IAsyncResult<History>> Remove(int id)
        {
            return await this.Remove(id, true);
        }

        public async Task<IAsyncResult<History>> Remove(int id, bool saveChanges)
        {
            return await this.InternalRemove(this._context, id, saveChanges);
        }

        public async Task<IAsyncResult<History>> InternalRemove(ApplicationDbContext context, int id, bool saveChanges)
        {
            var model = context.History.FirstOrDefault(a => a.Id == id);
            var result = context.History.Remove(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<History>(result, error);
        }
        #endregion


        #region Find
        public async Task<History> Find(int id)
        {
            return await _context.History.FindAsync(id);
        }

        public async Task<History> FirstOrDefault(Expression<Func<History, bool>> predicate)
        {
            return await _context.History.FirstOrDefaultAsync(predicate);
        }

        public async Task<History[]> All()
        {
            return await _context.History.ToArrayAsync();
        }


        public async Task<History[]> FindByPresentationId(int presentationId)
        {
            return await (
                from h in _context.History
                where h.PresentationId == presentationId
                select h
            ).ToArrayAsync();
        }
      
        #endregion
    }
}