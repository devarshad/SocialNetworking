using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository;
using DataTypes;
using System.Collections;

namespace BusinessLogic
{
    /// <summary>
    /// Access all method used for hub opertations
    /// </summary>
    public class HubAccess
    {
        #region Private Data Member

        /// <summary>
        /// Data access object
        /// </summary>
        private DataAccess _dr = null;

        #endregion

        #region Protected Data Member

        #endregion

        #region Public Data Members

        #endregion

        #region Static Data Member

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize data access object
        /// </summary>
        public HubAccess()
        {
            _dr = new DataAccess();
        }
        #endregion

        #region Private Member Functions

        #endregion

        #region Protected Member Functions

        #endregion

        #region Public Member Function

        #region Chat functions

        /// <summary>
        /// Update user entry in connected user list
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="ConnecttionID"></param>
        /// <param name="BroadCastType"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public bool UpdateUserStatus(long UserID, string ConnecttionID, Enums.BroadCastType BroadCastType, Enums.UserStatus Status)
        {
            return _dr.UpdateUserStatus(UserID, ConnecttionID, BroadCastType, Status);
        }

        /// <summary>
        /// Get the list of all followers according to user id
        /// </summary>
        /// <param name="UserID">userc id of caller</param>
        /// <returns>collection of users following the caller</returns>
        public dynamic GetFollowerListByUserID(long UserID)
        {
            return _dr.GetFollowerListByUserID(UserID);
        }

        /// <summary>
        /// Get user info by user id
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public dynamic GetUserInfoByUserID(long UserID)
        {
            return _dr.GetUserInfoByUserID(UserID);
        }

        /// <summary>
        /// Gets the message history from a room, conversation and user
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="FriendID"></param>
        /// <param name="roomId"></param>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        public dynamic GetMessageHistoryByUserID(long UserID, long FriendID, int roomId, long conversationId)
        {
            return _dr.GetMessageHistoryByUserID(UserID, FriendID, roomId, conversationId);
        }

        /// <summary>
        /// Send message to user
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="FriendID"></param>
        /// <param name="roomId"></param>
        /// <param name="conversationId"></param>
        /// <param name="clientGuid"></param>
        public BroadcastChatMessage SendChatMessage(ChatMessageModel message)
        {
            return _dr.SendChatMessage(message);
        }

        #endregion

        #region Wall Headeder Functions

        /// <summary>
        /// Get users notfications
        /// </summary>
        /// <param name="PageNumber"></param>
        public dynamic GetNotifications(long UserID, int PageNumber, int PageSize)
        {
            return _dr.GetNotifications(UserID, PageNumber, PageSize);
        }

        /// <summary>
        /// Get users messages
        /// </summary>
        /// <param name="PageNumber"></param>
        public dynamic GetMessages(long UserID, int PageNumber, int PageSize)
        {
            return _dr.GetMessages(UserID, PageNumber, PageSize);
        }

        /// <summary>
        /// Get users friend requests
        /// </summary>
        /// <param name="PageNumber"></param>
        public dynamic GetFriendRequests(long UserID, int PageNumber, int PageSize)
        {
            return _dr.GetFriendRequests(UserID, PageNumber, PageSize);
        }
        #endregion

        #region Wall Post Functions

        /// <summary>
        /// Get posts of a page
        /// </summary>
        /// <param name="PageNumber"></param>
        public dynamic GetPosts(long UserID, int PageType, long PageID, int PageNumber, int PageSize)
        {
            switch (PageType)
            {
                case 1:
                    return _dr.GetPosts(UserID, PageID, PageNumber, PageSize);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Add post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public dynamic AddPost(PostModel post)
        {
            return _dr.AddPost(post);
        }

        /// <summary>
        /// add comment to post
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public dynamic AddComment(CommentModel comment)
        {
            return _dr.AddComment(comment);
        }

        /// <summary>
        /// Adding like of item level
        /// </summary>
        /// <param name="likeModel"></param>
        /// <returns></returns>
        public dynamic AddLike(LikeModel likeModel)
        {
            return _dr.Like(likeModel);
        }

        /// <summary>
        /// Deleting like for an item
        /// </summary>
        /// <param name="likeModel"></param>
        /// <returns></returns>
        public dynamic DeleteLike(LikeModel likeModel)
        {
            return _dr.UnLike(likeModel);
        }


        #endregion

        #region Page Info

        /// <summary>
        /// Get page detail
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageType"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public dynamic GetPageInfo(long UserID, int PageType, long PageID)
        {
            switch (PageType)
            {
                case 1:
                    return _dr.GetUserDetailByID(UserID, PageID);
                case 2:
                    return _dr.GetGroupDetailByID(UserID, PageID);
                case 3:
                    return _dr.GetEventDetailByID(UserID, PageID);
                default:
                    return null;
            }

        }

        /// <summary>
        /// get left pane detail of users home page
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public dynamic GetLeftHome(long UserID)
        {
            return new
            {
                Groups = _dr.GetGroups(UserID, 1, 10),
                Events = _dr.GetEvents(UserID, 1, 10)
            };
        }

        /// <summary>
        /// Get left pane detail for requested page
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageType"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public dynamic GetLeftPageInfo(long UserID, int PageType, long PageID)
        {
            switch (PageType)
            {
                case 1:
                    return new
                    {
                        About = _dr.GetUserTimeline(UserID),
                        Friend = _dr.GetPages(UserID, 1, 10),
                        Event = _dr.GetEvents(UserID, 1, 10),
                        Group = _dr.GetGroups(UserID, 1, 10),
                        Photo = _dr.GetPhotos(UserID, 1, 10),
                        Music = _dr.GetMusics(UserID, 1, 10),
                        Video = _dr.GetVideos(UserID, 1, 10)
                    };
                default:
                    return null;
            }

        }

        #endregion

        #endregion

        #region Static Member Function

        #endregion

    }
}