using C4Connect.Helpers;
using DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace C4Connect.API
{
    [Authorize]
    public class PageController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="PageType"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetPageIDByName(String name, Enums.PageType PageType)
        {
            if (String.IsNullOrEmpty(name) || String.IsNullOrWhiteSpace(name))
            {
                ModelState.AddModelError("Name", "Name of page is required.");
            }
            if (!Enum.IsDefined(typeof(Enums.PageType), PageType))
            {
                ModelState.AddModelError("PageType", "PageType of page is not valid.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    long ID = _page.GetPageIDByName(name, PageType);
                    if (ID != -1)
                    {
                        return Ok(ID);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error finding ID of page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        /// <summary>
        /// Get curret user information
        /// </summary>
        public async Task<IHttpActionResult> GetUserInfo()
        {
            try
            {
                return Ok(GetMyInfo());
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error fetching detail of current user.", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///  Get page detail
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetPageInfo(long PageID, Enums.PageType PageType)
        {
            if (!Enum.IsDefined(typeof(Enums.PageType), PageType))
            {
                ModelState.AddModelError("PageType", "PageType of page is not valid.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(_page.GetPageInfo(CurrentUser.UserID, PageID, PageType));
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error fetching detail of page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        /// <summary>
        /// get left pane detail of users home page
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetLeftHomeInfo(long UserID)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(_page.GetLeftHomeInfo(UserID));
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error fetching detail of home left pane", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        /// <summary>
        /// Get left pane detail for requested page
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageType"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetLeftPageInfo(long PageID, Enums.PageType PageType)
        {
            if (!Enum.IsDefined(typeof(Enums.PageType), PageType))
            {
                ModelState.AddModelError("PageType", "PageType of page is not valid.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var ret = new
                    {
                        About = _page.GetPageTimeline(PageID, PageType),
                        Friend = _page.GetPageFollowers(PageID, PageType, 1, 10),
                        Event = _page.GetEvents(PageID, PageType, 1, 10),
                        Group = _page.GetGroups(PageID, PageType, 1, 10),
                        Photo = _page.GetPagePhotos(PageID, PageType, 1, 10),
                        Music = _page.GetPageMusics(PageID, PageType, 1, 10),
                        Video = _page.GetPageVideos(PageID, PageType, 1, 10)
                    };
                    return Ok(ret);
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error fetching detail of left pane of page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        /// <summary>
        /// Get left pane detail for requested page
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageType"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetPageTimeline(long PageID, Enums.PageType PageType)
        { 
            if (!Enum.IsDefined(typeof(Enums.PageType), PageType))
            {
                ModelState.AddModelError("PageType", "PageType of page is not valid.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(_page.GetPageTimeline(PageID, PageType));
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error fetching detail of left pane of page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="FriendRequest"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> AddFriendRequest(FriendRequestModel FriendRequest)
        {
            FriendRequest.UserID = CurrentUser.UserID;
            FriendRequest.CreatedOn = DateTime.Now;
            ModelState.Remove("FriendRequest.UserID");
            ModelState.Remove("FriendRequest.CreatedOn");
            FriendRequest.PostedByName = CurrentUser.FullName;
            FriendRequest.PostedByAvatar = CurrentUser.ProfilePicture;
            FriendRequest.UserInfo = CurrentUser.Gender;
            FriendRequest.PageRelationStatus = Enums.FriendshipStatus.FS;
            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastFriendRequest model = _page.AddFriendRequest(FriendRequest);
                    await _broadcaster.AddFriendRequest(model, FriendRequest.broadCastType);
                    return Ok((byte)model.FriendRequest.PageRelationStatus);
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error adding friend request for page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        public async Task<IHttpActionResult> AcceptFriendRequest(FriendRequestModel FriendRequest)
        {
            FriendRequest.PageID = CurrentUser.UserID;
            FriendRequest.CreatedOn = DateTime.Now;
            ModelState.Remove("FriendRequest.PageID");
            ModelState.Remove("FriendRequest.CreatedOn");
            FriendRequest.PostedByName = CurrentUser.FullName;
            FriendRequest.PostedByAvatar = CurrentUser.ProfilePicture;
            FriendRequest.PageRelationStatus = Enums.FriendshipStatus.FA;
            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastFriendRequest model = _page.AcceptFriendRequest(FriendRequest);
                    await _broadcaster.AcceptFriendRequest(model, FriendRequest.broadCastType);
                    return Ok(model.FriendRequest);
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error accepting friend request to page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        public async Task<IHttpActionResult> RejectUserFriendRequest(FriendRequestModel FriendRequest)
        {
            FriendRequest.PageID = CurrentUser.UserID;
            FriendRequest.CreatedOn = DateTime.Now;
            ModelState.Remove("FriendRequest.PageID");
            ModelState.Remove("FriendRequest.CreatedOn");
            FriendRequest.PostedByName = CurrentUser.FullName;
            FriendRequest.PostedByAvatar = CurrentUser.ProfilePicture;
            FriendRequest.PageRelationStatus = Enums.FriendshipStatus.FS;
            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastFriendRequest model = _page.RejectUserFriendRequest(FriendRequest);
                    await _broadcaster.CancelFriendRequest(model, FriendRequest.broadCastType);
                    return Ok(model.FriendRequest);
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error rejecting friend request to page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        public async Task<IHttpActionResult> CancelFriendRequest(FriendRequestModel FriendRequest)
        {
            FriendRequest.UserID = CurrentUser.UserID;
            FriendRequest.CreatedOn = DateTime.Now;
            ModelState.Remove("FriendRequest.PageID");
            ModelState.Remove("FriendRequest.CreatedOn");
            FriendRequest.PostedByName = CurrentUser.FullName;
            FriendRequest.PostedByAvatar = CurrentUser.ProfilePicture;
            FriendRequest.PageRelationStatus = Enums.FriendshipStatus.FS;
            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastFriendRequest model = _page.RejectUserFriendRequest(FriendRequest);
                    await _broadcaster.CancelFriendRequest(model, FriendRequest.broadCastType);
                    return Ok(model.FriendRequest);
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error cancel friend request to page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        public async Task<IHttpActionResult> RemoveFriend(FriendRequestModel FriendRequest)
        {
            if (FriendRequest.PageID == CurrentUser.UserID)
            {
                long _temp = FriendRequest.PageID;
                FriendRequest.PageID = CurrentUser.UserID;
                FriendRequest.UserID = _temp;
            }
            else
            {
                FriendRequest.UserID = CurrentUser.UserID;
            }
            FriendRequest.CreatedOn = DateTime.Now;
            ModelState.Remove("FriendRequest.PageID");
            ModelState.Remove("FriendRequest.CreatedOn");
            FriendRequest.PostedByName = CurrentUser.FullName;
            FriendRequest.PostedByAvatar = CurrentUser.ProfilePicture;
            FriendRequest.PageRelationStatus = Enums.FriendshipStatus.FA;
            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastFriendRequest model = _page.RejectUserFriendRequest(FriendRequest);
                    await _broadcaster.CancelFriendRequest(model, FriendRequest.broadCastType);
                    return Ok(model.FriendRequest);
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error remove friend to page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FriendRequest"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> SavePageDetail(AboutPageModel AboutPage)
        {
            if (AboutPage.Id != CurrentUser.UserID)
            {
                ModelState.AddModelError("Unauthorized", "You are not authorized to do that");
            }

            AboutPage.Id = CurrentUser.UserID;

            AboutPage.LastUpdatedOn = DateTime.Now;
            ModelState.Remove("FriendRequest.UserID");
            ModelState.Remove("FriendRequest.CreatedOn");
            if (ModelState.IsValid)
            {
                try
                {
                    AboutPageModel _result = _page.SavePageDetail(AboutPage);
                    if (_result == null)
                        return NotFound();
                    else
                        return Ok(_result);
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error saving page detail", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }
    }
}
