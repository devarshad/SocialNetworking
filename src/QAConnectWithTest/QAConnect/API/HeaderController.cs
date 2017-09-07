using QAConnect.Helpers;
using DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace QAConnect.API
{
    [Authorize]
    public class HeaderController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageType"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> GetHeader()
        {
            try
            {
                var _model = new
                {
                    Notifications = _user.GetNotifications(CurrentUser.UserID, 1, 10),

                    Messages = _user.GetMessages(CurrentUser.UserID, 1, 10),

                    FriendRequests = _user.GetFriendRequests(CurrentUser.UserID, 1, 10),

                    UserInfo = GetMyInfo()
                };
                return Ok(_model);
            }
            catch (Exception ex)
            {
                Logs.Error("Error getting heder for users page", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get users notfications
        /// </summary>
        /// <param name="PageNumber"></param>
        public async Task<IHttpActionResult> GetNotifications(int PageNumber, int PageSize)
        {
            try
            {
                return Ok(_user.GetNotifications(CurrentUser.UserID, PageNumber, PageSize));
            }
            catch (Exception ex)
            {
                Logs.Error("Error getting users notifications", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get users messages
        /// </summary>
        /// <param name="PageNumber"></param>
        public async Task<IHttpActionResult> GetMessages(int PageNumber, int PageSize)
        {
            try
            {
                return Ok(_user.GetMessages(CurrentUser.UserID, PageNumber, PageSize));
            }
            catch (Exception ex)
            {
                Logs.Error("Error getting users  messages", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get users friend requests
        /// </summary>
        /// <param name="PageNumber"></param>
        public async Task<IHttpActionResult> GetFriendRequests(int PageNumber, int PageSize)
        {
            try
            {
                return Ok(_user.GetFriendRequests(CurrentUser.UserID, PageNumber, PageSize));
            }
            catch (Exception ex)
            {
                Logs.Error("Error getting  list  of user's friend request", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get search's pages
        /// </summary>
        /// <param name="PageNumber"></param>
        public async Task<IHttpActionResult> GetSearchPages(String Key, int PageNumber, int PageSize)
        {
            try
            {
                return Ok(_user.GetSearchPages(CurrentUser.UserID, Key, PageNumber, PageSize));
            }
            catch (Exception ex)
            {
                Logs.Error("Error getting  list  of search's pages", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> ReadHeader(ReadHeaderModel model)
        {
            try
            {
                switch (model.type)
                {
                    case 1:
                        _user.ReadNotifications(model.IDs, CurrentUser.UserID);
                        break;
                    case 2:
                        _user.ReadMessages(model.IDs, CurrentUser.UserID);
                        break;
                    case 3:
                        _user.ReadFriendRequests(model.IDs, CurrentUser.UserID);
                        break;
                }
                return Ok("");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public async Task<IHttpActionResult> GetLeftHome()
        {
            try
            {
                return Ok(_user.GetLeftHome(CurrentUser.UserID));
            }
            catch (Exception ex)
            {
                Logs.Error("Error getting left pane home detail", ex);
                return BadRequest(ex.Message);
            }
        }
    }
}
