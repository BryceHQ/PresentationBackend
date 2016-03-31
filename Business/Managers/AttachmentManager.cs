using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Business.Models;
using Core;
using System.IO;
using Business.Utility;

namespace Business.Managers
{
    public class AttachmentManager
    {
        private PresentationContext context = new PresentationContext();

        private static AttachmentManager instance = new AttachmentManager();

        public static AttachmentManager Instance
        {
            get
            {
                return instance;
            }
        }

        private AttachmentManager()
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

        public string UploadAttachment(string fileName, string description, Stream stream)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException(fileName);
            }
            string folder = "upload";
            //文件路径
            string saveDir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, folder);

            //生成唯一文件名称
            string guidName = Guid.NewGuid().ToString() + fileName.Substring(fileName.LastIndexOf('.'));

            string secondFolder = AttachmentUtility.GetAttachmentSaveFolder(saveDir);

            string saveName = System.IO.Path.Combine(saveDir, secondFolder, guidName);

            using (FileStream fs = new FileStream(saveName, FileMode.Create, FileAccess.ReadWrite))
            {
                stream.CopyTo(fs);
            }
            //Attachment attachment = new Attachment()
            //{
            //    Id = 0,
            //    Name = fileName,
            //    FileName = guidName,
            //    FilePath = folderName,
            //    Description = description,
            //    UploadUser = SecurityContext.Current.User.AccountId
            //};

            //int id = this.AddAttachment(attachment);
            return string.Format("/presentation/{0}/{1}/{2}", folder, secondFolder, guidName).Replace('\\', '/');
        }
        #endregion
    }
}