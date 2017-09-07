using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NReco.VideoConverter;
using System.Threading.Tasks;
namespace C4Connect.Helpers
{
    /// <summary>
    /// It enables to operate on file functions
    /// </summary>
    public static class FileHandler
    {
        #region Private Data Member

        #endregion

        #region Public Data Members

        #endregion

        #region Protected Data Member

        #endregion

        #region Static Data Member
        /// <summary>
        /// object to access ffmpeg functions
        /// </summary>
        public static FFMpegConverter _ffmpeg = null;
        #endregion

        #region Constructor
        /// <summary>
        /// Initialize file handler
        /// </summary>
        static FileHandler()
        {
            _ffmpeg = new FFMpegConverter();
        }

        #endregion

        #region Private Member Functions

        #endregion

        #region Protected Member function

        #endregion

        #region Public Member Function

        #endregion

        #region Static Member Function
        /// <summary>
        /// Get thumbnail of object supplied
        /// </summary>
        /// <param name="source"></param>
        /// <param name="thumbnail"></param>
        /// <returns></returns>
        public static async Task<string> GetThumbnail(string source, string thumbnail)
        {
            try
            {
                _ffmpeg.GetVideoThumbnail(source, thumbnail);
                return thumbnail;
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to incorrect file path.";
                ex.Data.Add("Location : ", "Exception occured while fetching thumbnail.");
                ex.Data.Add("Applpication Tier : ", "1. C4Connect");
                ex.Data.Add("Class : ", "File Handler");
                ex.Data.Add("Method : ", "GetThumbnail");
                ex.Data.Add("Input Parameters : ", string.Format("source: {0},thumbnail: {1}", source, thumbnail));
                ExceptionService.LogError("Error fetching thumbnail.", ex);
                return null;
            }
        }

        public static bool ConvertFile(string source, string thumbnail, string ToFormat)
        {
            //_ffmpeg.ConvertMedia("abc.mp4", "video.mp4", Format.mp4);
            throw new NotImplementedException();
        }

        #endregion
    }
}