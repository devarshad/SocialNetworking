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
    /// Access all method used for chat opertations
    /// </summary>
    public class PageAccess
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
        public PageAccess()
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
        /// get page id by name
        /// </summary>
        /// <param name="Name"></param>
        /// <param name="PageType"></param>
        /// <returns></returns>
        public long GetPageIDByName(String Name, Enums.PageType PageType)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetUserIDByName(Name);
                case Enums.PageType.Group:
                    return _dr.GetGroupIDByName(Name);
                case Enums.PageType.Event:
                    return _dr.GetEventIDByName(Name);
                default:
                    return -1;
            }
        }


        /// <summary>
        /// Get page detail
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageType"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public dynamic GetPageInfo(long UserID, long PageID, Enums.PageType PageType)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetUserDetailByID(UserID, PageID);
                case Enums.PageType.Group:
                    return _dr.GetGroupDetailByID(UserID, PageID);
                case Enums.PageType.Event:
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
        public dynamic GetLeftHomeInfo(long UserID)
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
        public dynamic GetLeftPageInfo(long PageID, Enums.PageType PageType, int PageNumber, int PageSize)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return new
                    {
                        About = _dr.GetUserTimeline(PageID),
                        Friend = _dr.GetPages(PageID, 1, 10),
                        Event = _dr.GetEvents(PageID, 1, 10),
                        Group = _dr.GetGroups(PageID, 1, 10),
                        Photo = _dr.GetPhotos(PageID, 1, 10),
                        Music = _dr.GetMusics(PageID, 1, 10),
                        Video = _dr.GetVideos(PageID, 1, 10)
                    };
                default:
                    return null;
            }
        }

        public dynamic GetPageTimeline(long PageID, Enums.PageType PageType)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetUserTimeline(PageID);
                default:
                    return null;
            }
        }
        public dynamic GetPageFollowers(long PageID, Enums.PageType PageType, int PageNumber, int PageSize)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetPages(PageID, 1, 10);
                default:
                    return null;
            }
        }
        public dynamic GetEvents(long PageID, Enums.PageType PageType, int PageNumber, int PageSize)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetEvents(PageID, 1, 10);
                default:
                    return null;
            }
        }
        public dynamic GetGroups(long PageID, Enums.PageType PageType, int PageNumber, int PageSize)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetGroups(PageID, 1, 10);
                default:
                    return null;
            }
        }
        public dynamic GetPagePhotos(long PageID, Enums.PageType PageType, int PageNumber, int PageSize)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetPhotos(PageID, 1, 10);
                default:
                    return null;
            }
        }
        public dynamic GetPageMusics(long PageID, Enums.PageType PageType, int PageNumber, int PageSize)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetMusics(PageID, 1, 10);
                default:
                    return null;
            }
        }
        public dynamic GetPageVideos(long PageID, Enums.PageType PageType, int PageNumber, int PageSize)
        {
            switch (PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.GetVideos(PageID, 1, 10);
                default:
                    return null;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FriendRequest"></param>
        /// <returns></returns>
        public BroadcastFriendRequest AddFriendRequest(FriendRequestModel FriendRequest)
        {
            switch (FriendRequest.PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.AddUserFriendRequest(FriendRequest);
                default:
                    return null;
            }
        }

        public BroadcastFriendRequest AcceptFriendRequest(FriendRequestModel FriendRequest)
        {
            switch (FriendRequest.PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.AcceptUserFriendRequest(FriendRequest);
                default:
                    return null;
            }
        }

        public BroadcastFriendRequest RejectUserFriendRequest(FriendRequestModel FriendRequest)
        {
            switch (FriendRequest.PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.RejectUserFriendRequest(FriendRequest);
                default:
                    return null;
            }
        }

        public AboutPageModel SavePageDetail(AboutPageModel AboutPage)
        {
            switch (AboutPage.PageType)
            {
                case Enums.PageType.Profile:
                    return _dr.SavePageDetail(AboutPage);
                default:
                    return null;
            }
        }
        #endregion

        #region Static Member Function

        #endregion
    }
}
