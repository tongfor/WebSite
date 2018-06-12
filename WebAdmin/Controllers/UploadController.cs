using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Common;
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

        public async Task<IActionResult> FileManage()
        {
            string webRootPath = _hostingEnv.WebRootPath;
            string contentRootPath = _hostingEnv.ContentRootPath; 

            //根目录路径，相对路径
            String rootPath = "~/" + SiteConfigSettings.DefaultUploadFolder + "/";
            //根目录URL，可以指定绝对路径，比如 http://www.yoursite.com/attached/
            String rootUrl = Request.GetHostUri() + "/" + SiteConfigSettings.DefaultUploadFolder + "/"; 

            //图片扩展名
            String fileTypes = SiteConfigSettings.AllowUploadImageFileExt;

            String currentPath = "";
            String currentUrl = "";
            String currentDirPath = "";
            String moveupDirPath = "";

            //文件保存的实际路径
            String dirPath = webRootPath + "/" + SiteConfigSettings.DefaultUploadFolder + "/";
            String dirName = Request.Query["dir"];

            if (!String.IsNullOrEmpty(dirName))
            {
                if (Array.IndexOf("image,flash,media,file".Split(','), dirName) == -1)
                {
                    return await ShowError("无效目录!");
                }
                dirPath += dirName + "/";
                rootUrl += dirName + "/";
                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }

            //根据path参数，设置各路径和URL
            String path = Request.Query["path"];
            path = String.IsNullOrEmpty(path) ? "" : path;
            if (path == "")
            {
                currentPath = dirPath;
                currentUrl = rootUrl;
                currentDirPath = "";
                moveupDirPath = "";
            }
            else
            {
                currentPath = dirPath + path;
                currentUrl = rootUrl + path;
                currentDirPath = path;
                moveupDirPath = Regex.Replace(currentDirPath, @"(.*?)[^\/]+\/$", "$1");
            }

            //排序形式，name or size or type
            String order = Request.Query["order"];
            order = String.IsNullOrEmpty(order) ? "" : order.ToLower();

            //不允许使用..移动到上一级目录
            if (Regex.IsMatch(path, @"\.\."))
            {
                return await ShowError("Access is not allowed.");
            }
            //最后一个字符不是/
            if (path != "" && !path.EndsWith("/"))
            {
                return await ShowError("Parameter is not valid.");
            }
            //目录不存在或不是目录
            if (!Directory.Exists(currentPath))
            {
                return await ShowError("Directory does not exist.");
            }

            //遍历目录取得文件信息
            string[] dirList = Directory.GetDirectories(currentPath);
            string[] fileList = Directory.GetFiles(currentPath);

            switch (order)
            {
                case "size":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new SizeSorter());
                    break;
                case "type":
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                    break;
                case "name":
                default:
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                    break;
            }

            Hashtable result = new Hashtable
            {
                ["moveup_dir_path"] = moveupDirPath,
                ["current_dir_path"] = currentDirPath,
                ["current_url"] = currentUrl,
                ["total_count"] = dirList.Length + fileList.Length
            };

            await Task.Run(() =>
            {
                List<Hashtable> dirFileList = new List<Hashtable>();
                result["file_list"] = dirFileList;
                foreach (string s in dirList)
                {
                    DirectoryInfo dir = new DirectoryInfo(s);
                    Hashtable hash = new Hashtable
                    {
                        ["is_dir"] = true,
                        ["has_file"] = (dir.GetFileSystemInfos().Length > 0),
                        ["filesize"] = 0,
                        ["is_photo"] = false,
                        ["filetype"] = "",
                        ["filename"] = dir.Name,
                        ["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    dirFileList.Add(hash);
                }
                foreach (string s in fileList)
                {
                    FileInfo file = new FileInfo(s);
                    Hashtable hash = new Hashtable
                    {
                        ["is_dir"] = false,
                        ["has_file"] = false,
                        ["filesize"] = file.Length,
                        ["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0),
                        ["filetype"] = file.Extension.Substring(1),
                        ["filename"] = file.Name,
                        ["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
                    };
                    dirFileList.Add(hash);
                }
            });
           
            return Json(result);
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