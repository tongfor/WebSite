using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebAdmin.Controllers
{
    public class UploadController : BaseController
    {
        private IHostingEnvironment _hostingEnv;

        public UploadController(IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IOptions<SiteConfig> options, IHostingEnvironment env) : base(operateLogService, adminBugService, adminMenuService, options)
        {
            this._hostingEnv = env;
        }

        [HttpPost]
        public async Task<IActionResult> FileSave(List<IFormFile> files)
        {
            string webRootPath = _hostingEnv.WebRootPath;

            string contentRootPath = _hostingEnv.ContentRootPath;            

            //文件保存目录路径
            String savePath = "~/" + SiteConfigSettings.DefaultUploadFolder + "/";

            //文件保存目录URL
            String saveUrl = Request.GetHostUri() + "/" + SiteConfigSettings.DefaultUploadFolder + "/";

            //文件保存的实际路径
            String dirPath = webRootPath + "/" + SiteConfigSettings.DefaultUploadFolder + "/";

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable
            {
                { "image", SiteConfigSettings.AllowUploadImageFileExt },
                { "flash", SiteConfigSettings.AllowUploadFlashFileExt },
                { "media", SiteConfigSettings.AllowUploadMediaFileExt },
                { "file", SiteConfigSettings.AllowUploadFileExt },
                { "businessplan", SiteConfigSettings.AllowBusinessPlanUploadFileExt }
            };

            //最大文件大小
            long maxSize = long.Parse(SiteConfigSettings.DefaultUploadFileMaxSize);

            var imgFile = Request == null || Request.Form == null ? null : Request.Form.Files["imgFile"];
            if (imgFile == null)
            {
                return await ShowError("请选择文件。");
            }
            
            if (!Directory.Exists(dirPath))
            {
                return await ShowError("上传目录不存在。");
            }

            String dirName = Request.Query["dir"];
            if (String.IsNullOrEmpty(dirName))
            {
                dirName = "image";
            }
            if (!extTable.ContainsKey(dirName))
            {
                return await ShowError("目录名不正确。");
            }
            if (dirName == "businessplan")
            {
                maxSize = long.Parse(SiteConfigSettings.DefaultBusinessPlanUploadFileMaxSize);
            }

            if (imgFile != null)
            {
                String fileName = imgFile.FileName;
                String fileExt = Path.GetExtension(fileName).ToLower();

                if (imgFile.Length <0 || imgFile.Length > maxSize)
                {
                    return await ShowError("上传文件大小超过限制。");
                }

                if (String.IsNullOrEmpty(fileExt) || Array.IndexOf(((String)extTable[dirName]).Split(','), fileExt.Substring(1).ToLower()) == -1)
                {
                    return await ShowError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
                }

                try
                {
                    //创建文件夹
                    dirPath += dirName + "/";
                    saveUrl += dirName + "/";
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }
                    String ymd = DateTime.Now.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
                    dirPath += ymd + "/";
                    saveUrl += ymd + "/";
                    if (!Directory.Exists(dirPath))
                    {
                        Directory.CreateDirectory(dirPath);
                    }

                    String newFileName = DateTime.Now.ToString("yyyyMMddHHmmss_ffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
                    String filePath = dirPath + newFileName;

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imgFile.CopyToAsync(stream);
                    }

                    String fileUrl = saveUrl + newFileName;

                    Hashtable hash = new Hashtable
                    {
                        ["error"] = 0,
                        ["url"] = fileUrl
                    };

                    return Json(hash);
                }
                catch(Exception ex)
                {
                    return await ShowError(ex.Message);
                }
            }
            return await ShowError("非法访问！");
        }

        private async Task<IActionResult> ShowError(string message)
        {
            Hashtable hash = new Hashtable();
            await Task.Run(() =>
            {
                hash = new Hashtable
                {
                    ["error"] = 1,
                    ["message"] = message
                };                
            });
            return Json(hash);
        }
    }
}