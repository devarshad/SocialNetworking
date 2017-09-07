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
    public class ChatController : BaseController
    {
        // sends a message to a room, conversation or user
        public async Task<IHttpActionResult> SendChatMessage(ChatMessageModel message)
        {
            message.FromID = CurrentUser.UserID;
            message.CreatedOn = DateTime.Now;
            ModelState.Remove("message.FromID");
            ModelState.Remove("message.CreatedOn");
            message.PostedByName = CurrentUser.FullName;
            message.PostedByAvatar = CurrentUser.ProfilePicture;

            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastChatMessage model = _chat.SendChatMessage(message);
                    await _broadcaster.SendChatMessage(model, message.broadCastType);
                    return Ok(model.ChatMessage);
                }
                catch (Exception ex)
                {
                    Logs.Error("Error adding message to page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        // sends a typing signal to a room, conversation or user
        public void sendTypingSignal(int roomId, long conversationId, long ToID)
        {
            try
            {
                //var ret = new
                //{
                //    RoomId = roomId,
                //    ConversationId = conversationId,
                //    ToID = ToID,
                //    UserFrom = GetMyChatInfo(roomId)
                //};
                //var toUser = ConnectedUsers.FirstOrDefault(x => x.UserID == ToID);
                //Clients.Client(toUser.ConnectionId).sendTypingSignal(ret);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error.";
                ex.Data.Add("Location : ", "Exception occured while sending typing signal.");
                ex.Data.Add("Applpication Tier : ", "1. QAConnect");
                ex.Data.Add("Class : ", "Chat Hub");
                ex.Data.Add("Method : ", "sendTypingSignal");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0},Friend ID : {1}, Room ID : {2}, Conversation ID : {3}", CurrentUser.UserID, ToID, roomId, conversationId));

                Logs.Error("Error sending typing signal.", ex);
            }
        }

        // gets the message history from a room, conversation or user
        public dynamic getMessageHistory(int roomId, long conversationId, long otherUserId)
        {
            try
            {
                return _chat.GetMessageHistoryByUserID(CurrentUser.UserID, otherUserId, roomId, conversationId);
            }
            catch (Exception ex)
            {
                Logs.Error("Error fetching the message history", ex);
                return null;
            }
        }

        //// gets the given user info
        public dynamic getUserInfo(long userId)
        {
            try
            {
                return _chat.GetUserInfoByUserID(userId);
            }
            catch (Exception ex)
            {
                Logs.Error("Error fetching the user information", ex);
                return null;
            }
        }

        // gets the user list in a room or conversation
        public dynamic getUserList(int roomId, long conversationId = 1)
        {
            try
            {
                return _chat.GetFollowerListByUserID(CurrentUser.UserID);
            }
            catch (Exception ex)
            {
                Logs.Error("Error fetching the list of followers", ex);
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
    }
}
