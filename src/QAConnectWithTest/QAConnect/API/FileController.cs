using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using QAConnect.Helpers;
using System.Threading.Tasks;

namespace QAConnect.API
{
    [Authorize]
    public class FileController : BaseController
    {
        /// <summary>
        /// Upload file from post
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> PostFile()
        {
            string errorMessage = "";
            var fileData = System.Web.HttpContext.Current.Request.Files["fileData"];
            if (fileData != null)
            {

                if (fileData.ContentLength / 1000 > 5120)
                {
                    errorMessage = "File size must be less then or equal to 5 MB";
                    return BadRequest(errorMessage);
                }
                var fileType = String.Empty;
                if (fileData.ContentType.StartsWith("image"))
                {
                    fileType = "/Images/";
                }
                else if (fileData.ContentType.StartsWith("audio"))
                {
                    fileType = "/Audio/";
                }
                else if (fileData.ContentType.StartsWith("video"))
                {
                    fileType = "/Video/";
                }

                var fileTime = DateTime.Now.ToString("ddMMyyhhmm");

                string filpath = "~/Profiles/" + CurrentUser.UserID + fileType + fileTime + "-" + fileData.FileName;
                try
                {
                    fileData.SaveAs(System.Web.HttpContext.Current.Server.MapPath(filpath));
                    String iconpath = "~/Profiles/" + CurrentUser.UserID + fileType + "/Icon/" + fileTime + "-" + Path.GetFileNameWithoutExtension(fileData.FileName) + ".jpeg";

                    await FileHandler.GetThumbnail(System.Web.HttpContext.Current.Server.MapPath(filpath), System.Web.HttpContext.Current.Server.MapPath(iconpath));

                    return Ok("/Profiles/" + CurrentUser.UserID + fileType + fileTime + "-" + fileData.FileName + "#" + "/Profiles/" + CurrentUser.UserID + fileType + "/Icon/" + fileTime + "-" + Path.GetFileNameWithoutExtension(fileData.FileName) + ".jpeg");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
                return BadRequest(new Exception().Message);
        }

        /// <summary>
        /// Upload page profile picture
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> PostPicture()
        {
            var fileData = System.Web.HttpContext.Current.Request.Files["fileData"];
            if (fileData != null)
            {
                string errorMessage = "";
                var fileType = String.Empty;
                if (fileData.ContentType.StartsWith("image"))
                {
                    fileType = "/Album/";
                }
                else
                {
                    errorMessage = "File type must be Image";
                    return BadRequest(errorMessage);
                }

                if (fileData.ContentLength / 1000 > 5120)
                {
                    errorMessage = "File size must be less then or equal to 5 MB";
                    return BadRequest(errorMessage);
                }

                var fileTime = DateTime.Now.ToString("ddMMyyhhmm");

                string filpath = "~/Profiles/" + CurrentUser.UserID + fileType + fileTime + "-" + fileData.FileName;
                try
                {
                    fileData.SaveAs(System.Web.HttpContext.Current.Server.MapPath(filpath));
                    String iconpath = "~/Profiles/" + CurrentUser.UserID + fileType + "/Icon/" + fileTime + "-" + Path.GetFileNameWithoutExtension(fileData.FileName) + ".jpeg";

                    await FileHandler.GetThumbnail(System.Web.HttpContext.Current.Server.MapPath(filpath), System.Web.HttpContext.Current.Server.MapPath(iconpath));
                    filpath = filpath.Remove(0, 1);
                    iconpath = iconpath.Remove(0, 1);
                    _user.UpdatePagePofilePicture(CurrentUser.UserID, CurrentUser.UserID, 1, filpath);

                    CurrentUser.ProfilePicture = filpath;

                    return Ok(filpath + "#" + iconpath);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
                return BadRequest(new Exception().Message);
        }

        /// <summary>
        /// Upload page wall paper
        /// </summary>
        /// <returns></returns>
        public async Task<IHttpActionResult> PostWallpaper()
        {
            var fileData = System.Web.HttpContext.Current.Request.Files["fileData"];
            if (fileData != null)
            {
                string errorMessage = "";
                var fileType = String.Empty;
                if (fileData.ContentType.StartsWith("image"))
                {
                    fileType = "/Album/";
                }
                else
                {
                    errorMessage = "File type must be Image";
                    return BadRequest(errorMessage);
                }

                if (fileData.ContentLength / 1000 > 5120)
                {
                    errorMessage = "File size must be less then or equal to 5 MB";
                    return BadRequest(errorMessage);
                }

                var fileTime = DateTime.Now.ToString("ddMMyyhhmm");

                string filpath = "~/Profiles/" + CurrentUser.UserID + fileType + fileTime + "-" + fileData.FileName;
                try
                {
                    fileData.SaveAs(System.Web.HttpContext.Current.Server.MapPath(filpath));
                    String iconpath = "~/Profiles/" + CurrentUser.UserID + fileType + "/Icon/" + fileTime + "-" + Path.GetFileNameWithoutExtension(fileData.FileName) + ".jpeg";

                    await FileHandler.GetThumbnail(System.Web.HttpContext.Current.Server.MapPath(filpath), System.Web.HttpContext.Current.Server.MapPath(iconpath));
                    filpath = filpath.Remove(0, 1);
                    iconpath = iconpath.Remove(0, 1);
                    _user.UpdatePageCoverPicture(CurrentUser.UserID, CurrentUser.UserID, 1, filpath);

                    CurrentUser.CoverPicture = filpath;

                    return Ok(filpath + "#" + iconpath);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            else
                return BadRequest(new Exception().Message);
        }
    }
}
