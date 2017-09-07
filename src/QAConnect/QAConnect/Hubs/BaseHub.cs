using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Security.Principal;
using log4net;
using BusinessLogic;
using DataTypes;
using C4Connect.Helpers;

namespace C4Connect.Hubs
{
    /// <summary>
    /// Base hub class
    /// </summary>
    public class BaseHub : Hub
    {
        #region Private Data Member

        /// <summary>
        /// Hub access object
        /// </summary>
        protected HubAccess _hub = null;

        #endregion

        #region Public Data Members

        /// <summary>
        /// Current user access for Identity information
        /// </summary>
        public AppUserPrincipal CurrentUser
        {
            get
            {
                return new AppUserPrincipal(Context.User as ClaimsPrincipal);
            }
        }

        /// <summary>
        /// List of all connnected users to hub
        /// </summary>
        public static List<UserDetail> ConnectedUsers = new List<UserDetail>();

        /// <summary>
        /// List of all friends only
        /// </summary>
        public static List<UserDetail> ConnectedFriends = new List<UserDetail>();

        #endregion

        #region Protected Data Member

        #endregion

        #region Static Data Member
        /// <summary>
        /// check to see if user has already been connected
        /// </summary>
        private static string MyConnectionID = "";

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize hub access object
        /// </summary>
        public BaseHub()
        {
            _hub = new HubAccess();
        }
        #endregion

        #region Private Member Functions

        #endregion

        #region Protected Member function

        /// <summary>
        /// Get user information according to chat plugin
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public dynamic GetMyChatInfo(int roomId)
        {
            return new
                    {
                        Id = CurrentUser.UserID,
                        Name = CurrentUser.FullName,
                        Url = CurrentUser.UserName,
                        ProfilePictureUrl = CurrentUser.ProfilePicture,
                        Status = CurrentUser.StatusID,
                        LastActiveOn = CurrentUser.LastStatusOn,
                        RoomId = roomId
                    };
        }

        /// <summary>
        /// Get user information
        /// </summary>
        /// <returns></returns>
        public dynamic GetMyInfo()
        {
            return new
            {
                ID = CurrentUser.UserID,
                Name = CurrentUser.UserName,
                FullName = CurrentUser.FullName,
                Wallpaper = CurrentUser.CoverPicture,
                Picture = CurrentUser.ProfilePicture
            };
        }

        /// <summary>
        /// user list changed
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="conversationId"></param>
        protected void userListChanged(int roomId, long conversationId)
        {
            try
            {
                Clients.Others.userListChanged(new
                {
                    UserList = _hub.GetFollowerListByUserID(CurrentUser.UserID),
                    RoomId = roomId,
                    ConversationId = conversationId
                });
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error getting followers list", ex);
            }
        }

        /// <summary>
        /// Overriden dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #endregion

        #region Public Member Function

        /// <summary>
        /// Hub overriden methood while connecting
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                ///check if already  connected with the same ConnectionID then ignore database update
                if (!MyConnectionID.Equals(Context.ConnectionId))
                {
                    try
                    {
                        CurrentUser.StatusID = 1;
                        _hub.UpdateUserStatus(CurrentUser.UserID, Context.ConnectionId, Enums.BroadCastType.Web, Enums.UserStatus.ONL);

                        IList<string> connectionIDs = new BusinessLogic.UserAccess()
                            .GetFollowerListByConnectID(CurrentUser.UserID, Enums.PageType.Profile, Enums.FriendshipStatus.FA)
                            .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

                        Clients.Clients(connectionIDs).updateUserList(GetMyChatInfo(1));
                        MyConnectionID = Context.ConnectionId;
                    }
                    catch (Exception ex)
                    {
                        ExceptionService.LogError("Error updating user status", ex);
                    }
                }
            }

            return base.OnConnected();
        }

        /// <summary>
        /// Hub overriden methood while connecting
        /// </summary>
        /// <returns></returns>
        public override System.Threading.Tasks.Task OnReconnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                try
                {
                    CurrentUser.StatusID = 1;
                    _hub.UpdateUserStatus(CurrentUser.UserID, Context.ConnectionId, Enums.BroadCastType.Web, Enums.UserStatus.ONL);

                    IList<string> connectionIDs = new BusinessLogic.UserAccess()
                        .GetFollowerListByConnectID(CurrentUser.UserID, Enums.PageType.Profile, Enums.FriendshipStatus.FA)
                        .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

                    Clients.Clients(connectionIDs).updateUserList(GetMyChatInfo(1));
                }
                catch (Exception ex)
                {
                    ExceptionService.LogError("Error updating user status", ex);
                }
            }

            return base.OnReconnected();
        }

        /// <summary>
        /// Hub overriden methood while disconnecting
        /// </summary>
        /// <returns></returns>
        public System.Threading.Tasks.Task OnDisconnected()
        {
            try
            {
                CurrentUser.StatusID = 2;
                _hub.UpdateUserStatus(CurrentUser.UserID, Context.ConnectionId, Enums.BroadCastType.Web, Enums.UserStatus.OFL);
                IList<string> connectionIDs = new BusinessLogic.UserAccess()
                      .GetFollowerListByConnectID(CurrentUser.UserID, Enums.PageType.Profile, Enums.FriendshipStatus.FA)
                      .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

                Clients.Clients(connectionIDs).updateUserList(GetMyChatInfo(1));
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error updating user status", ex);
            }

            return base.OnDisconnected(false);
        }

        #endregion

        #region Static Member Function

        #endregion
    }
}