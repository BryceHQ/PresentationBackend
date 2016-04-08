using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.IO;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Http;
using System.Linq.Expressions;

namespace Web.Core
{
    public class AttachmentManager
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        private static AttachmentManager _instance = new AttachmentManager();

        public static AttachmentManager Instance
        {
            get
            {
                return _instance;
            }
        }

        private AttachmentManager()
        {
        }


        #region Add
        public async Task<IAsyncResult<Attachment>> Add(Attachment model)
        {
            return await this.Add(model, true);
        }

        public async Task<IAsyncResult<Attachment>> Add(Attachment model, bool saveChanges)
        {
            return await this.InternalAdd(this._context, model, saveChanges);
        }

        internal async Task<IAsyncResult<Attachment>> InternalAdd(ApplicationDbContext context, Attachment model, bool saveChanges)
        {
            model.Guid = Guid.NewGuid().ToString();
            model.UploadTime = DateTime.Now;

            var result = context.Attachment.Add(model);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await _context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<Attachment>(result, error);
        }
        #endregion


        #region Update
        public async Task<IAsyncResult<Attachment>> Update(Attachment model)
        {
            return await this.Update(model, true);
        }

        public async Task<IAsyncResult<Attachment>> Update(Attachment model, bool saveChanges)
        {
            return await this.InternalUpdate(this._context, model, saveChanges);
        }

        internal async Task<IAsyncResult<Attachment>> InternalUpdate(ApplicationDbContext context, Attachment model, bool saveChanges)
        {
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await _context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<Attachment>(model, error);
        }
        #endregion


        #region Remove
        public async Task<IAsyncResult<Attachment>> Remove(int id)
        {
            return await this.Remove(id, true);
        }

        public async Task<IAsyncResult<Attachment>> Remove(int id, bool saveChanges)
        {
            var attachment = await _context.Attachment.FindAsync(id);
            var result = _context.Attachment.Remove(attachment);
            ErrorCode error = null;
            if (saveChanges)
            {
                error = await _context.SafeSaveChangesAsync();
            }
            return new DefaultAsyncResult<Attachment>(result, error);
        }
        #endregion


        #region Find
        public async Task<Attachment> Find(int id)
        {
            return await _context.Attachment.FindAsync(id);
        }

        public async Task<Attachment> FirstOrDefault(Expression<Func<Attachment, bool>> predicate)
        {
            return await _context.Attachment.FirstOrDefaultAsync(predicate);
        }

        public async Task<Attachment[]> All()
        {
            return await _context.Attachment.ToArrayAsync();
        }

        public async Task<Attachment[]> FindByUserId(string userId)
        {
            return await (
                from p in _context.Attachment
                where p.UploadUser == userId
                select p
            ).ToArrayAsync();
        }
        #endregion


        #region Others

        public async Task<IAsyncResult<Attachment>> UploadAttachment(string fileName, string userId, Stream stream)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(fileName);
            }
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(userId);
            }

            string folder = (string)GlobalConfiguration.Configuration.Properties.GetOrAdd("uploadFolder", "upload"); ;
            //文件路径
            string saveDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder);

            //生成唯一文件名称
            string guidName = Guid.NewGuid().ToString() + fileName.Substring(fileName.LastIndexOf('.'));

            string secondFolder = AttachmentUtility.GetAttachmentSaveFolder(saveDir);

            string saveName = Path.Combine(saveDir, secondFolder, guidName);

            using (FileStream fs = new FileStream(saveName, FileMode.Create, FileAccess.ReadWrite))
            {
                await stream.CopyToAsync(fs);
            }

            Attachment attachment = new Attachment()
            {
                Name = fileName,
                FileName = guidName,
                FilePath = Path.Combine(folder, secondFolder),
                UploadUser = userId
            };

            return await this.Add(attachment);
        }


        public async Task<IAsyncResult<Attachment>> GenerateIdentityIcon(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(userId);
            }
            var fileName = userId + ".jpg";

            string folder = (string)GlobalConfiguration.Configuration.Properties.GetOrAdd("uploadFolder", "upload"); ;
            //文件路径
            string saveDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder);

            //生成唯一文件名称
            string guidName = Guid.NewGuid().ToString() + fileName.Substring(fileName.LastIndexOf('.'));

            string secondFolder = AttachmentUtility.GetAttachmentSaveFolder(saveDir);

            string saveName = Path.Combine(saveDir, secondFolder, guidName);

            using (FileStream fs = new FileStream(saveName, FileMode.Create, FileAccess.ReadWrite))
            {
                Gravatar.Default.Render(userId, fs);
            }

            Attachment attachment = new Attachment()
            {
                Name = fileName,
                FileName = guidName,
                FilePath = Path.Combine(folder, secondFolder),
                UploadUser = userId
            };

            return await this.Add(attachment);
        }

        #endregion
    }
}