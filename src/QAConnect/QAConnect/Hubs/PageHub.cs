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
    public class PageHub : BaseHub
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
        /// Get page detail
        /// </summary>
        /// <param name="PageType"></param>
        /// <param name="PageID"></param>
        public void getPageInfo(int PageType, long PageID)
        {
            try
            {
                Clients.Caller.loadPageInfo(_hub.GetPageInfo(CurrentUser.UserID, PageType, PageID));
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error getting page detail", ex);
            }
        }

        /// <summary>
        /// get left pane detail of user home page
        /// </summary>
        public void GetLeftHome()
        {
            try
            {
                Clients.Caller.loadLeftHome(_hub.GetLeftHome(CurrentUser.UserID));
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error getting left pane home detail", ex);
            }
        }

        /// <summary>
        /// get left pane detail for requested page
        /// </summary>
        /// <param name="PageType"></param>
        /// <param name="PageID"></param>
        public void GetLeftPage(int PageType, long PageID)
        {
            try
            {
                Clients.Caller.loadLeftPage(_hub.GetLeftPageInfo(CurrentUser.UserID, PageType, PageID));
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error getting page left detail", ex);
            }
        }

        #endregion

        #region Static Member Function

        #endregion
    }
}