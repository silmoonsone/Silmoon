using Microsoft.AspNetCore.Mvc;
using Silmoon.AspNetCore.Extensions;
using Silmoon.AspNetCore.Services.Interfaces;
using Silmoon.AspNetCore.Test.Models;
using Silmoon.AspNetCore.Test.Services;
using Silmoon.Drawing;
using Silmoon.Extension;
using Silmoon.Runtime.Cache;
using Silmoon.Runtime.Collections;
using Silmoon.Secure;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Silmoon.AspNetCore.Test.Controllers
{
    public class WebApiController : Controller
    {
        Core Core { get; set; }
        ISilmoonAuthService SilmoonUserService { get; set; }
        public WebApiController(Core core, ISilmoonAuthService silmoonUserService)
        {
            Core = core;
            SilmoonUserService = silmoonUserService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateSession(string Username, string Password, string SigninType, string AppId, string Url, string CallbackUrl)
        {
            if (Username.IsNullOrEmpty() || Password.IsNullOrEmpty()) return this.JsonStateFlag(false, "用户名或密码为空");
            var user = Core.GetUser(Username);
            if (user is null) return this.JsonStateFlag(false, "用户名不存在或者密码错误");
            //user.Password = EncryptHelper.RsaDecrypt(user.Password);
            if (Username == user.Username && Password == user.Password)
            {
                await SilmoonUserService.SignIn(user);
                if (Url.IsNullOrEmpty()) Url = "../Account/Summary";
                return this.JsonStateFlag(true, Data: Url);
            }
            else
            {
                return this.JsonStateFlag(false, "用户名不存在或者密码错误");
            }
        }
        public async Task<IActionResult> ClearSession()
        {
            var result = await SilmoonUserService.SignOut();
            return this.JsonStateFlag(result);
        }


        public IActionResult UploadTempImage(string UserId, string fileName)
        {
            if (fileName.IsNullOrEmpty()) fileName = HashHelper.RandomChars(32);
            var files = ObjectCache<string, NameObjectCollection<byte[]>>.Get(UserId + ":temp_images");

            var image = new Bitmap(Request.Form.Files[0].OpenReadStream());
            var imageFormat = image.RawFormat;

            ImageHelper.FixiPhoneOrientation(image);
            var image2 = ImageHelper.ResizeWidth(image, 800, true, true);
            image.Dispose();
            var image3 = ImageHelper.Compress(image2, CompositingQuality.HighSpeed);
            image2.Dispose();
            var data = image3.GetBytes(imageFormat);
            image3.Dispose();

            if (files.Matched)
                files.Value[fileName] = data;
            else
                ObjectCache<string, NameObjectCollection<byte[]>>.Set(UserId + ":temp_images", new NameObjectCollection<byte[]>() { { fileName, data } });

            return this.JsonStateFlag(true);
        }
        public IActionResult GetTempImageNames(string UserId)
        {
            var files = ObjectCache<string, NameObjectCollection<byte[]>>.Get(UserId + ":temp_images");
            if (files.Matched)
            {
                return this.JsonStateFlag(true, Data: files.Value.GetAllKeys());
            }
            else
                return this.JsonStateFlag(true, Data: 0);
        }
        public IActionResult DeleteTempImage(string UserId, string fileName)
        {
            var files = ObjectCache<string, NameObjectCollection<byte[]>>.Get(UserId + ":temp_images");
            if (files.Matched)
            {
                files.Value.Remove(fileName);
                return this.JsonStateFlag(true);
            }
            else
                return this.JsonStateFlag(false);
        }
        //[OutputCache(Duration = 3600)]
        public IActionResult ShowTempImage(string UserId, string fileName)
        {
            var files = ObjectCache<string, NameObjectCollection<byte[]>>.Get(UserId + ":temp_images");
            if (files.Matched)
                return File(files.Value.Get(fileName) ?? new byte[0], "image/jpeg");
            else
                return new EmptyResult();
        }




        public IActionResult UploadFile(string UserId, string fileName)
        {
            var files = ObjectCache<string, NameObjectCollection<byte[]>>.Get(UserId + ":temp_files");

            var data = Request.Form.Files[0].OpenReadStream().ToBytes();

            if (files.Matched)
                files.Value.Set(fileName.IsNullOrEmpty() ? Request.Form.Files[0].FileName : fileName, data);
            else
                ObjectCache<string, NameObjectCollection<byte[]>>.Set(UserId + ":temp_files", new NameObjectCollection<byte[]>() { { fileName.IsNullOrEmpty() ? Request.Form.Files[0].FileName : fileName, data } });

            return this.JsonStateFlag(true);
        }
        public IActionResult GetTempFileNames(string UserId)
        {
            var files = ObjectCache<string, NameObjectCollection<byte[]>>.Get(UserId + ":temp_files");
            if (files.Matched)
            {
                return this.JsonStateFlag(true, Data: files.Value.GetAllKeys());
            }
            else
                return this.JsonStateFlag(true, Data: 0);
        }
        public IActionResult DeleteTempFile(string UserId, string fileName)
        {
            var files = ObjectCache<string, NameObjectCollection<byte[]>>.Get(UserId + ":temp_files");
            if (files.Matched)
            {
                files.Value.Remove(fileName);
                return this.JsonStateFlag(true);
            }
            else
                return this.JsonStateFlag(false);
        }
        public IActionResult ShowTempFile(string UserId, string fileName)
        {
            var files = ObjectCache<string, NameObjectCollection<byte[]>>.Get(UserId + ":temp_files");
            new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider().TryGetContentType(fileName, out var contentType);

            if (files.Matched)
                return File(files.Value.Get(fileName) ?? new byte[0], contentType);
            else
                return new EmptyResult();
        }
    }
}
