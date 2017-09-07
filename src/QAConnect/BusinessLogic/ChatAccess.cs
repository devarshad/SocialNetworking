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
    public class ChatAccess
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
        public ChatAccess()
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
        /// Update user status with user id and status id
        /// </summary>
        /// <param name="UserID">long user id</param>
        /// <param name="StatusID">User status</param>
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

        #region Static Member Function

        #endregion
    }
}
