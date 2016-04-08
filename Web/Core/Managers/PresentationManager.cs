using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Web.Core
{
    public class PresentationManager
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        private static PresentationManager _instance = new PresentationManager();

        public static PresentationManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public DbSet<Presentation> QueryableCollection
        {
            get
            {
                return _context.Presentation;
            }
        }

        private PresentationManager()
        {
        }


        #region Add
        public async Task<IAsyncResult<Presentation>> Add(Presentation model)
        {
            return await this.Add(model, true);
        }

        public async Task<IAsyncResult<Presentation>> Add(Presentation model, bool saveChanges)
        {
            return await this.InternalAdd(this._context, model, saveChanges);
        }

        internal async Task<IAsyncResult<Presentation>> InternalAdd(ApplicationDbContext context, Presentation model, bool saveChanges)
        {
            model.Guid = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;

            var result = context.Presentation.Add(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<Presentation>(result, error);
        }
        #endregion


        #region Update
        public async Task<IAsyncResult<Presentation>> Update(Presentation model)
        {
            return await this.Update(model, true);
        }

        public async Task<IAsyncResult<Presentation>> Update(Presentation model, bool saveChanges)
        {
            return await this.InternalUpdate(this._context, model, saveChanges);
        }

        internal async Task<IAsyncResult<Presentation>> InternalUpdate(ApplicationDbContext context, Presentation model, bool saveChanges)
        {
            model.LastUpdateTime = DateTime.Now;
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<Presentation>(model, error);
        }
        #endregion


        #region Remove
        public async Task<IAsyncResult<Presentation>> Remove(int id)
        {
            return await this.Remove(id, true);
        }

        public async Task<IAsyncResult<Presentation>> Remove(int id, bool saveChanges)
        {
            var presentation = _context.Presentation.FirstOrDefault(a => a.Id == id);
            var result = _context.Presentation.Remove(presentation);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await _context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<Presentation>(result, error);
        }
        #endregion


        #region Find
        public async Task<Presentation> Find(int id)
        {
            return await _context.Presentation.FindAsync(id);
        }

        public async Task<Presentation> FirstOrDefault(Expression<Func<Presentation, bool>> predicate)
        {
            return await _context.Presentation.FirstOrDefaultAsync(predicate);
        }

        public async Task<Presentation[]> All()
        {
            return await _context.Presentation.ToArrayAsync();
        }

        public async Task<Presentation[]> FindByUserId(string userId)
        {
            return await (
                from p in _context.Presentation
                where p.UserId == userId
                select p
            ).ToArrayAsync();
        }
        #endregion


        #region Others
        /// <summary>
        /// 保存核心json数据，同时处理历史记录的保存
        /// </summary>
        /// <param name="raw">核心json数据</param>
        /// <param name="presentation">model</param>

        public async Task<IAsyncResult<Presentation>> SaveWithHistory(string raw, Presentation presentation)
        {
            //与上次自动保存相差一个小时或者内容该变量相差超过限定值，则自动创建一个新历史记录。
            //最多保存 maxCount 个历史记录。
            int maxCount = 20;

            if (DateTime.Now - presentation.LastUpdateTime > new TimeSpan(1, 0, 0) ||
                Math.Abs(raw.Length - presentation.Raw.Length) > 10000
            )
            {
                var historyArray = await HistoryManager.Instance.FindByPresentationId(presentation.Id);
                if (historyArray.Length > maxCount)
                {
                    await HistoryManager.Instance.InternalRemove(this._context, historyArray.Last().Id, false);
                }
                var history = new History()
                {
                    PresentationId = presentation.Id,
                    Raw = presentation.Raw
                };

                await HistoryManager.Instance.InternalAdd(this._context, history, false);
            }

            presentation.Raw = raw;
            return await this.Update(presentation, true);//saveChanges
        }
        #endregion
    }
}