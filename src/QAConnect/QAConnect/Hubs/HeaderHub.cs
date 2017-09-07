using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using C4Connect.Models;
using DataTypes;
using C4Connect.Helpers;

namespace C4Connect.Hubs
{
    public class HeaderHub : BaseHub
    {
        #region Private Data Member

        #endregion

        #region Public Data Members

        #endregion

        #region Protected Data Member

        #endregion

        #region Static Data Member

        #endregion

        #region Constructor

        #endregion

        #region Private Member Functions

        #endregion

        #region Protected Member function

        #endregion

        #region Public Member Function

        /// <summary>
        /// Load header at first page load
        /// </summary>
        public void GetHeader()
        {
            try
            {
                var ret = new
                {
                    Notifications = _hub.GetNotifications(CurrentUser.UserID, 1, 10),

                    Messages = _hub.GetMessages(CurrentUser.UserID, 1, 10),

                    FriendRequests = _hub.GetFriendRequests(CurrentUser.UserID, 1, 10),

                    UserInfo = GetMyInfo()
                };
                Clients.Caller.loadHeader(ret);
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error getting heder for users page", ex);
            }
        }

        /// <summary>
        /// Get users notfications
        /// </summary>
        /// <param name="PageNumber"></param>
        public void GetNotifications(int PageNumber, int PageSize)
        {
            try
            {
                _hub.GetNotifications(CurrentUser.UserID, PageNumber, PageSize);
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error getting users notifications", ex);
            }
        }

        /// <summary>
        /// Get users messages
        /// </summary>
        /// <param name="PageNumber"></param>
        public void GetMessages(int PageNumber, int PageSize)
        {
            try
            {
                _hub.GetMessages(CurrentUser.UserID, PageNumber, PageSize);
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error getting users  messages", ex);
            }
        }

        /// <summary>
        /// Get users friend requests
        /// </summary>
        /// <param name="PageNumber"></param>
        public void GetFriendRequests(int PageNumber, int PageSize)
        {
            try
            {
                _hub.GetFriendRequests(CurrentUser.UserID, PageNumber, PageSize);
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error getting  list  of user's friend request", ex);
            }
        }

        #endregion

        #region Static Member Function

        #endregion
    }
}