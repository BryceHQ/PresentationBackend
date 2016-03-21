using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Business.Models;
using Core;

namespace Business.Managers
{
    public class PresentationManager
    {
        private PresentationContext context = new PresentationContext();

        private static PresentationManager instance = new PresentationManager();

        public static PresentationManager Instance
        {
            get
            {
                return instance;
            }
        }

        private PresentationManager()
        {
        }


        #region Add
        public ReturnValue<Presentation> Add(Presentation model)
        {
            return this.Add(model, true);
        }

        public ReturnValue<Presentation> Add(Presentation model, bool saveChanges)
        {
            return this.InternalAdd(this.context, model, saveChanges);
        }

        internal ReturnValue<Presentation> InternalAdd(PresentationContext context, Presentation model, bool saveChanges)
        {
            model.Guid = Guid.NewGuid().ToString();
            model.CreateTime = DateTime.Now;
            model.LastUpdateTime = DateTime.Now;

            var result = context.Presentation.Add(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<Presentation>(result, error);
        } 
        #endregion


        #region Update
        public ReturnValue<Presentation> Update(Presentation model)
        {
            return this.Update(model, true);
        }

        public ReturnValue<Presentation> Update(Presentation model, bool saveChanges)
        {
            return this.InternalUpdate(this.context, model, saveChanges);
        }

        internal ReturnValue<Presentation> InternalUpdate(PresentationContext context, Presentation model, bool saveChanges)
        {
            model.LastUpdateTime = DateTime.Now;
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<Presentation>(model, error);
        } 
        #endregion


        #region Remove
        public ReturnValue<Presentation> Remove(int id)
        {
            return this.Remove(id, true);
        }

        public ReturnValue<Presentation> Remove(int id, bool saveChanges)
        {
            var presentation = context.Presentation.FirstOrDefault(a => a.UserId == id);
            var result = context.Presentation.Remove(presentation);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = context.SafeSaveChanges();
            }
            return new ReturnValue<Presentation>(result, error);
        } 
        #endregion


        #region Get
        public Presentation Get(int id)
        {
            return context.Presentation.FirstOrDefault(a => a.Id == id);
        }

        public Presentation[] Where(Func<Presentation, bool> predicate)
        {
            return context.Presentation.Where(predicate).ToArray();
        }

        public Presentation FirstOrDefault(Func<Presentation, bool> predicate)
        {
            return context.Presentation.FirstOrDefault(predicate);
        }

        public Presentation[] All()
        {
            return context.Presentation.ToArray();
        } 
        #endregion


        #region Others
        /// <summary>
        /// 保存核心json数据，同时处理历史记录的保存
        /// </summary>
        /// <param name="raw">核心json数据</param>
        /// <param name="presentation">model</param>

        public ReturnValue<Presentation> SaveWithHistory(string raw, Presentation presentation)
        {
            //与上次自动保存相差一个小时或者内容该变量相差超过限定值，则自动创建一个新历史记录。
            //最多保存 maxCount 个历史记录。
            int maxCount = 20;
            
            if (DateTime.Now - presentation.LastUpdateTime > new TimeSpan(1, 0, 0) ||
                Math.Abs(raw.Length - presentation.Raw.Length) > 10000
            )
            {
                var historyArray = HistoryManager.Instance.Where(h => h.PresentationId == presentation.Id);
                if (historyArray.Length > maxCount)
                {
                    HistoryManager.Instance.InternalRemove(this.context, historyArray.Last().Id, false);
                }
                var history = new History()
                {
                    PresentationId = presentation.Id,
                    Raw = presentation.Raw
                };

                HistoryManager.Instance.InternalAdd(this.context, history, false);
            }

            presentation.Raw = raw;
            return this.Update(presentation, true);//saveChanges
        }
        #endregion
    }
}