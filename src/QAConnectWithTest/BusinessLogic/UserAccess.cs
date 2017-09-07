using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataRepository;
using DataTypes;
namespace BusinessLogic
{
    /// <summary>
    /// Access all method used for user opertations
    /// </summary>
    public class UserAccess
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
        public UserAccess()
        {
            _dr = new DataAccess();
        }
        #endregion

        #region Private Member Functions

        #endregion

        #region Protected Member Functions

        #endregion

        #region Public Member Function

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

        /// <summary>
        /// Set Read Notifications
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public void ReadNotifications(long[] IDs, long UserID)
        {
            _dr.ReadNotifications(IDs, UserID);
        }

        /// <summary>
        /// Set read friend Requests
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public void ReadFriendRequests(long[] IDs, long UserID)
        {
            _dr.ReadFriendRequests(IDs, UserID);
        }

        /// <summary>
        /// Set read messages
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public void ReadMessages(long[] IDs, long UserID)
        {
            _dr.ReadMessages(IDs, UserID);
        }

        public dynamic GetSearchPages(long UserID, string Key, int PageNumber, int PageSize)
        {
            return _dr.GetSearchPages(UserID, Key, PageNumber, PageSize);
        }

        public dynamic GetSearchNewPages(long UserID, string Key, int PageNumber, int PageSize)
        {
            return _dr.GetSearchNewPages(UserID, Key, PageNumber, PageSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<FollwersList> GetFollowerList(long UserID, long PageID, Enums.PageType PageType, Enums.FriendshipStatus status)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetMyFollowerList(UserID, PageID, status);
                //case 2:
                //    return _dr.GetGroupFollowerList(UserID, PageID);
                //case 3:
                //    return _dr.GetEventFollowerList(UserID, PageID);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public List<FollwersList> GetFollowerListByConnectID(long PageID, Enums.PageType PageType, Enums.FriendshipStatus status)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetFollowerListByConnectID(PageID, status);
                //case 2:
                //    return _dr.GetGroupFollowerList(UserID, PageID);
                //case 3:
                //    return _dr.GetEventFollowerList(UserID, PageID);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Update page profile picture
        /// </summary>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public bool UpdatePagePofilePicture(long UserID, long PageID, int PageType, string Url)
        {
            switch (PageType)
            {
                case 1:
                    return _dr.UpdateUserPofilePicture(UserID, PageID, Url);
                case 2:
                    return _dr.UpdateGroupPofilePicture(UserID, PageID, Url);
                case 3:
                    return _dr.UpdateEventPofilePicture(UserID, PageID, Url);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Update page cover picture
        /// </summary>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <param name="Url"></param>
        /// <returns></returns>
        public bool UpdatePageCoverPicture(long UserID, long PageID, int PageType, string Url)
        {
            switch (PageType)
            {
                case 1:
                    return _dr.UpdateUserCoverPicture(UserID, PageID, Url);
                case 2:
                    return _dr.UpdateGroupCoverPicture(UserID, PageID, Url);
                case 3:
                    return _dr.UpdateEventCoverPicture(UserID, PageID, Url);
                default:
                    return false;
            }
        }


        #endregion

        #region Static Member Function

        #endregion

        public dynamic GetLeftHome(long UserID)
        {
            return new
            {
                Groups = _dr.GetGroups(UserID, 1, 10),
                Events = _dr.GetEvents(UserID, 1, 10)
            };
        }
    }
}
