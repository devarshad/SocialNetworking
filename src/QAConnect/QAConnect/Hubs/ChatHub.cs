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
    public class ChatHub : BaseHub
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

        // sends a message to a room, conversation or user
        public void sendMessage(ChatMessageModel message)//int roomId, long conversationId, long otherUserId, string messageText, string clientGuid)
        {
            try
            {
                message.FromID = CurrentUser.UserID;
                message.CreatedOn = DateTime.Now;
                message.PostedByName = CurrentUser.FullName;
                message.PostedByAvatar = CurrentUser.ProfilePicture;
                message.FromName = CurrentUser.UserName;

                BroadcastChatMessage model = _hub.SendChatMessage(message);

                IList<string> connectionIDs = new BusinessLogic.UserAccess().GetFollowerList(model.ChatMessage.FromID, model.ChatMessage.ToID, model.ChatMessage.PageType, Enums.FriendshipStatus.FA)
              .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

                Clients.Clients(connectionIDs).sendMessage(model.ChatMessage);
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error while sending message.", ex);
            }
        }

        // sends a typing signal to a room, conversation or user
        public void sendTypingSignal(int roomId, long conversationId, long ToID)
        {
            try
            {
                var ret = new
                {
                    RoomId = roomId,
                    ConversationId = conversationId,
                    ToID = ToID,
                    UserFrom = GetMyChatInfo(roomId)
                };

                IList<string> connectionIDs = new BusinessLogic.UserAccess().GetFollowerList(CurrentUser.UserID, ToID, Enums.PageType.Profile, Enums.FriendshipStatus.FA)
            .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();


                Clients.Clients(connectionIDs).sendTypingSignal(ret);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error.";
                ex.Data.Add("Location : ", "Exception occured while sending typing signal.");
                ex.Data.Add("Applpication Tier : ", "1. C4Connect");
                ex.Data.Add("Class : ", "Chat Hub");
                ex.Data.Add("Method : ", "sendTypingSignal");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0},Friend ID : {1}, Room ID : {2}, Conversation ID : {3}", CurrentUser.UserID, ToID, roomId, conversationId));

                ExceptionService.LogError("Error sending typing signal.", ex);
            }
        }

        // gets the message history from a room, conversation or user
        public dynamic getMessageHistory(int roomId, long conversationId, long otherUserId)
        {
            try
            {
                return _hub.GetMessageHistoryByUserID(CurrentUser.UserID, otherUserId, roomId, conversationId);
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error fetching the message history", ex);
                return null;
            }
        }

        //// gets the given user info
        public dynamic getUserInfo(long userId)
        {
            try
            {
                return _hub.GetUserInfoByUserID(userId);
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error fetching the user information", ex);
                return null;
            }
        }

        // gets the user list in a room or conversation
        public dynamic getUserList(int roomId, long conversationId = 1)
        {
            try
            {
                return _hub.GetFollowerListByUserID(CurrentUser.UserID);
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error fetching the list of followers", ex);
                return null;
            }
        }

        // gets the rooms list
        public void getRoomsList()
        {

        }

        // enters the given room
        public void enterRoom(int roomId)
        {

        }

        // leaves the given room
        public void leaveRoom(int roomId)
        {

        }


        //public void SendPrivateMessage(string toUserId, string message)
        //{

        //    string fromUserId = Context.ConnectionId;

        //    var toUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == toUserId);
        //    var fromUser = ConnectedUsers.FirstOrDefault(x => x.ConnectionId == fromUserId);

        //    if (toUser != null && fromUser != null)
        //    {
        //        // send to 
        //        Clients.Client(toUserId).sendPrivateMessage(fromUserId, fromUser.UserName, message);

        //        // send to caller user
        //        Clients.Caller.sendPrivateMessage(toUserId, fromUser.UserName, message);
        //    }


        //Clients.Group(CurrentUser.UserID).sendMessage(new
        //{
        //    ToID = otherUserId,
        //    FromID = CurrentUser.IntID,
        //    Message = messageText,
        //    DateTime = mess.CreatedOn,
        //    RoomId = 1
        //});

        //}
        #endregion

        #region Static Member Function

        #endregion
    }
}