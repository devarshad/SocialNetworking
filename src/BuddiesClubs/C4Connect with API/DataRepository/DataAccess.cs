using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTypes;
using System.Data.SqlClient;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.EntityClient;
using Newtonsoft.Json;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("C4Connect")]
namespace DataRepository
{
    /// <summary>
    /// Repository class to access database object and enable database intraction functions
    /// </summary>
    public class DataAccess
    {
        #region Private Data Member

        /// <summary>
        /// main database entity object to access database member and function
        /// </summary>
        private DbEntities _db = null;

        #endregion

        #region Protected Data Member

        #endregion

        #region Public Data Members

        #endregion

        #region Static Data Member

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize data repository object
        /// </summary>
        public DataAccess()
        {
            _db = new DbEntities();
        }

        #endregion

        #region Private Member Functions

        #endregion

        #region Protected Member Functions

        #endregion

        #region Public Member Function

        #region Broadcasting Functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public List<FollwersList> GetFollowerListByConnectID(long PageID, Enums.FriendshipStatus status)
        {
            try
            {
                return _db.Friendships.Where(x => (x.UserID == PageID || x.FriendID == PageID) && x.StatusID == (byte)status)
                    .Select(x => new FollwersList
                    {
                        UserID = x.AspNetUser.Id == PageID ? x.AspNetUser1.Id : x.AspNetUser.Id,
                        ConnectionID = x.AspNetUser.Id == PageID ? x.AspNetUser1.ConnectedUser.ConnectionID : x.AspNetUser.ConnectedUser.ConnectionID,
                        ConnectedBy = x.AspNetUser.Id == PageID ? x.AspNetUser1.ConnectedUser.ConnectedBy : x.AspNetUser.ConnectedUser.ConnectedBy,
                        StatusID = x.AspNetUser.Id == PageID ? x.AspNetUser1.ConnectedUser.StatusID : x.AspNetUser.ConnectedUser.StatusID,
                    }).ToList();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error.";
                ex.Data.Add("Location : ", "Exception occured while fetching followers list.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetFollowerListByConnectID");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", PageID));
                throw ex;
            }
            throw new NotImplementedException();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public List<FollwersList> GetMyFollowerList(long UserID, long PageID, Enums.FriendshipStatus status)
        {
            try
            {
                return _db.Friendships.Where(x => ((x.UserID == PageID && x.FriendID == UserID) || (x.UserID == UserID && x.FriendID == PageID)) && x.StatusID == (byte)status)
                    .Select(x => new FollwersList
                    {
                        UserID = x.AspNetUser.Id == UserID ? x.AspNetUser1.Id : x.AspNetUser.Id,
                        ConnectionID = x.AspNetUser.Id == UserID ? x.AspNetUser1.ConnectedUser.ConnectionID : x.AspNetUser.ConnectedUser.ConnectionID,
                        ConnectedBy = x.AspNetUser.Id == UserID ? x.AspNetUser1.ConnectedUser.ConnectedBy : x.AspNetUser.ConnectedUser.ConnectedBy,
                        StatusID = x.AspNetUser.Id == UserID ? x.AspNetUser1.ConnectedUser.StatusID : x.AspNetUser.ConnectedUser.StatusID,
                    }).ToList();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error.";
                ex.Data.Add("Location : ", "Exception occured while fetching followers list.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetFollowerListByUserID");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", UserID));
                throw ex;
            }
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public IList<FollwersList> GetGroupFollowerList(long UserID, long PageID)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public IList<FollwersList> GetEventFollowerList(long UserID, long PageID)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Chat functions
        /// <summary>
        /// Update user status with user id and status id
        /// </summary>
        /// <param name="UserID">long user id</param>
        /// <param name="StatusID">User status</param>
        /// <returns></returns>
        public bool UpdateUserStatus(long UserID, string ConnecttionID, Enums.BroadCastType BroadCastType, Enums.UserStatus Status)
        {
            Boolean _returnStatus = false;
            ConnectedUser _model = null;
            try
            {
                _model = _db.ConnectedUsers.Find(UserID);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to configuration at web.config level in connection string.";
                //ex.Data.Add("Connection String : ", _db.Database.Connection.ConnectionString);
                ex.Data.Add("Location : ", "Exception occured while accessing database C4Connect.");
                throw ex;
            }

            if (_model == null)
            {
                _model = new ConnectedUser();
                _model.UserID = UserID;
                _model.ConnectionID = ConnecttionID;
                _model.ConnectedBy = (byte)BroadCastType;
                _model.StatusID = (byte)Status;
                _model.ConnectedOn = DateTime.UtcNow;
                _db.ConnectedUsers.Add(_model);
            }
            else
            {
                _model.ConnectionID = ConnecttionID;
                _model.ConnectedBy = (byte)BroadCastType;
                _model.StatusID = (byte)Status;
                _model.ConnectedOn = DateTime.UtcNow;

            }

            try
            {
                _db.SaveChanges();
                _returnStatus = true;
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to bad enum type to suser status id in database UserStatus table ID.";
                ex.Data.Add("Location : ", "Exception occured while updating user status.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "UpdateUserStatus");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Status: {1}", UserID, Status.ToString()));
                throw ex;
            }
            return _returnStatus;
        }
        /// <summary>
        /// Get the list of all followers according to user id
        /// </summary>
        /// <param name="UserID">userc id of caller</param>
        /// <returns>collection of users following the caller</returns>
        public dynamic GetFollowerListByUserID(long UserID)
        {
            try
            {
                var ret = _db.Friendships.Where(x => (x.UserID == UserID || x.FriendID == UserID) && x.StatusID == (byte)Enums.FriendshipStatus.FA)
                    .Select(x => new
                    {
                        Id = x.AspNetUser.Id == UserID ? x.AspNetUser1.Id : x.AspNetUser.Id,
                        Name = x.AspNetUser.Id == UserID ? x.AspNetUser1.FullName : x.AspNetUser.FullName,
                        Url = x.AspNetUser.Id == UserID ? "/" + x.AspNetUser1.UserName : "/" + x.AspNetUser.UserName,
                        ProfilePictureUrl = x.AspNetUser.Id == UserID ? x.AspNetUser1.ProfilePicture : x.AspNetUser.ProfilePicture,
                        Status = x.AspNetUser.Id == UserID ? x.AspNetUser1.ConnectedUser.StatusID : x.AspNetUser.ConnectedUser.StatusID,
                        LastActiveOn = x.AspNetUser.Id == UserID ? x.AspNetUser1.LastStatusOn : x.AspNetUser.LastStatusOn,
                        RoomId = 1
                    })
                    .ToList();
                return ret;
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error.";
                ex.Data.Add("Location : ", "Exception occured while fetching followers list.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetFollowerListByUserID");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", UserID));
                throw ex;
            }
        }

        /// <summary>
        /// Get user info by user id
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public dynamic GetUserInfoByUserID(long UserID)
        {
            try
            {
                return _db.AspNetUsers.Where(x => x.Id == UserID)
               .Select(x => new
               {
                   Id = x.Id,
                   Name = x.FullName,
                   Url = "/" + x.UserName,
                   ProfilePictureUrl = x.ProfilePicture,
                   Status = x.ConnectedUser.StatusID,
                   LastActiveOn = x.LastStatusOn,
                   RoomId = 1
               }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error.";
                ex.Data.Add("Location : ", "Exception occured while fetching user information.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetUserInfoByUserID");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", UserID));
                throw ex;
            }
        }

        /// <summary>
        /// Get message history by user id
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="FriendID"></param>
        /// <param name="roomId"></param>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        public dynamic GetMessageHistoryByUserID(long UserID, long FriendID, int roomId, long conversationId)
        {
            try
            {
                return _db.Messages.Where(x => (x.RecieverID == UserID || x.AspNetUser.Id == FriendID)
                    && (x.AspNetUser1.Id == FriendID || x.SenderID == UserID))
                .Select(x => new
                {
                    FromID = x.AspNetUser1.Id,
                    FromName = x.AspNetUser1.UserName,
                    PostedByName = x.AspNetUser1.FullName,
                    PostedByAvatar = x.AspNetUser1.ProfilePicture,
                    ToID = x.AspNetUser.Id,
                    ToName = x.AspNetUser.UserName,
                    UserToPictureUrl = x.AspNetUser.ProfilePicture,
                    UserToName = x.AspNetUser.FullName,
                    Message = x.Text,
                    CreatedOn = x.CreatedOn,
                    RoomId = roomId
                })
                .ToList();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error.";
                ex.Data.Add("Location : ", "Exception occured while fetching user information.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetMessageHistoryByUserID");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0},Friend ID : {1}, Room ID : {2}, Conversation ID : {3}", UserID, FriendID, roomId, conversationId));
                throw ex;
            }
        }

        /// <summary>
        /// Send message to user
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="FriendID"></param>
        /// <param name="messageText"></param>
        /// <param name="roomId"></param>
        /// <param name="conversationId"></param>
        /// <param name="clientGuid"></param>
        public BroadcastChatMessage SendChatMessage(ChatMessageModel message)
        {
            Message _message = _db.Messages.Add(new Message
            {
                SenderID = message.FromID,
                Text = message.Message,
                RecieverID = message.ToID,
                PageTypeID = (byte)message.PageType,
                IsRead = false,
                Active = true,
                CreatedOn = message.CreatedOn
            });

            //Notification notification = AddNotificationAsync(new NotificationModel
            //{
            //    UserID = message.FromID,
            //    ItemID = _message.ID,
            //    TypeID = 8,
            //    Message = message.PostedByName + " added a new message : " + message.Message,
            //});

            try
            {
                _db.SaveChanges();

                return new BroadcastChatMessage
                {
                    ChatMessage = message,
                    //Notification = new NotificationModel
                    //{
                    //    ID = notification.ID,
                    //    Message = notification.Text,
                    //    PostedByAvatar = message.PostedByAvatar,
                    //    ItemID = notification.ItemID ?? 0,
                    //    TypeID = notification.TypeID ?? 1,
                    //    IsRead = notification.IsRead
                    //}
                };
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error.";
                ex.Data.Add("Location : ", "Exception occured while sending message to user.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "SendMessageToUser");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0},To ID : {1}, Message :{2}, Room ID : {3}, Conversation ID : {4}", message.FromID, message.ToID, message.RoomID, message.ConversationID));
                throw ex;
            }
        }
        #endregion

        #region Wall Header Functions
        /// <summary>
        /// get users notification read or unread
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public dynamic GetNotifications(long UserID, int PageNumber, int PageSize)
        {
            try
            {
                List<NotificationModel> _main = new List<NotificationModel>();

                var dataset = ExecuteStoredProcedure("GetNotifications", new SqlParameter[] 
                {        new SqlParameter("@UserID", UserID), 
                         new SqlParameter("@PageSize", PageSize),
                         new SqlParameter("@PageNumber", PageNumber)});

                if (dataset.Tables.Count > 0)
                {
                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        _main = JsonConvert.DeserializeObject<List<NotificationModel>>(JsonConvert.SerializeObject(dataset.Tables[0]));
                    }
                }
                return _main;

                //return _db.Notifications.Where(noti =>
                //    (
                //    noti.UserID == noti.AspNetUser.Friendships.FirstOrDefault(f => (f.FriendID == UserID) && f.StatusID == (byte)Enums.FriendshipStatus.FA).UserID
                //    ||
                //     noti.UserID == noti.AspNetUser.Friendships.FirstOrDefault(f => (f.UserID == UserID) && f.StatusID == (byte)Enums.FriendshipStatus.FA).FriendID
                //    )
                //    && (noti.Active == true || noti.Active == null))
                //    .OrderByDescending(d => d.CreatedOn)
                //    .Skip(PageSize * (PageNumber - 1))
                //    .Select(x => new
                //    {
                //        ID = x.ID,
                //        Message = x.Text,
                //        PostedByAvatar = x.AspNetUser.ProfilePicture,
                //        ItemID = x.ItemID ?? 0,
                //        TypeID = x.TypeID ?? 1,
                //        IsRead = x.IsRead ?? false
                //    }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching user's notifications.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetNotifications");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page Number: {1}, Page Size: {3}", UserID, PageNumber, PageSize));
                throw ex;
            }
        }

        /// <summary>
        /// get users messages read or unread
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public dynamic GetMessages(long UserID, int PageNumber, int PageSize)
        {
            try
            {
                return _db.Messages.Where(mesg => mesg.RecieverID == UserID && (mesg.Active == true || mesg.Active == null))
                .OrderByDescending(d => d.CreatedOn)
                .Skip(PageSize * (PageNumber - 1))
                    .Select(x => new
                    {
                        ID = x.ID,
                        Message = x.Text,
                        PostedByAvatar = x.AspNetUser1.ProfilePicture,
                        PostedByName = x.AspNetUser1.FullName,
                        CreatedOn = x.CreatedOn,
                        IsRead = x.IsRead ?? false
                    }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching user's messages.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetMessages");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page Number: {1}, Page Size: {3}", UserID, PageNumber, PageSize));
                throw ex;
            }
        }

        /// <summary>
        /// get users friend requests read or unread
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public dynamic GetFriendRequests(long UserID, int PageNumber, int PageSize)
        {
            try
            {
                return _db.Friendships.Where(freq => freq.FriendID == UserID && freq.StatusID == 1)
                .OrderByDescending(d => d.CreatedOn)
                .Skip(PageSize * (PageNumber - 1))
                    .Select(x => new
                    {
                        ID = x.UserID,
                        UserID = x.AspNetUser.Id,
                        PostedByName = x.AspNetUser.FullName,
                        UserInfo = x.AspNetUser.Gender,
                        PostedByAvatar = x.AspNetUser.ProfilePicture,
                        IsRead = x.IsRead ?? false,
                        PageType = x.PageTypeID,
                    }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching user's friend requests .");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetFriendRequests");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page Number: {1}, Page Size: {3}", UserID, PageNumber, PageSize));
                throw ex;
            }
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
            try
            {
                foreach (long ID in IDs)
                {
                    var model = _db.Notifications.Find(ID);
                    if (model != null)
                    {
                        model.IsRead = true;
                    }
                }
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while upadting read notifications.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "ReadNotifications");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, IDs: {1}", UserID, IDs.ToString()));
                throw ex;
            }
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
            try
            {
                foreach (long ID in IDs)
                {
                    var model = _db.Friendships.Find(ID);
                    if (model != null)
                    {
                        model.IsRead = true;
                    }
                }
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while upadting read friendship requests.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "ReadFriendRequests");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, IDs: {1}", UserID, IDs.ToString()));
                throw ex;
            }
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
            try
            {
                foreach (long ID in IDs)
                {
                    var model = _db.Messages.Find(ID);
                    if (model != null)
                    {
                        model.IsRead = true;
                    }
                }
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while upadting read messages.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "ReadMessages");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, IDs: {1}", UserID, IDs.ToString()));
                throw ex;
            }
        }

        public dynamic GetSearchNewPages(long UserID, String Key, int PageNumber, int PageSize)
        {
            try
            {
                return _db.AspNetUsers.Where(x => ((x.UserName.Contains(Key) || x.FullName.Contains(Key) || x.Email.Contains(Key) || x.Address.Contains(Key)) || 1 == 1))
                .OrderByDescending(d => d.LastUpdatedOn)
                .Skip(PageSize * (PageNumber - 1))
                    .Select(x => new
                    {
                        Id = x.Id,
                        Name = "/" + x.UserName,
                        FullName = x.FullName,
                        Avtar = x.ProfilePicture,
                        Detail = x.Gender
                    }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching search's pages.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetSearchPages");
                ex.Data.Add("Input Parameters : ", string.Format("Key: {0}, Page Number: {1}, Page Size: {3}", Key, PageNumber, PageSize));
                throw ex;
            }
        }

        #endregion

        #region Wall Post Functions
        public DataSet ExecuteStoredProcedure(string storedProcedureName, IEnumerable<SqlParameter> parameters)
        {
            var connectionString = _db.Database.Connection.ConnectionString;
            var ds = new DataSet();

            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = storedProcedureName;
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }

            return ds;
        }

        /// <summary>
        /// get users notification read or unread
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public dynamic GetPosts(long UserID, long PageID, int PageNumber, int PageSize)
        {
            try
            {
                List<PostModel> _main = new List<PostModel>();

                var dataset = ExecuteStoredProcedure("GetPosts", new SqlParameter[] 
                {        new SqlParameter("@UserID", UserID), 
                         new SqlParameter("@PageID", PageID),
                         new SqlParameter("@PageSize", PageSize),
                         new SqlParameter("@PageNumber", PageNumber)});

                if (dataset.Tables.Count > 0)
                {
                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        _main = JsonConvert.DeserializeObject<List<PostModel>>(JsonConvert.SerializeObject(dataset.Tables[0]));
                        var _temp = JsonConvert.DeserializeObject<List<CommentModel>>(JsonConvert.SerializeObject(dataset.Tables[1]));

                        _main.ForEach(x => x.PostComments = _temp.Where(c => c.PostId == x.PostId).ToList());
                    }
                }
                return _main;
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching pages's posts.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetPosts");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page ID {1}, Page Number: {2}, Page Size: {4}", UserID, PageID, PageNumber, PageSize));
                throw ex;
            }
        }

        /// <summary>
        /// Add a new post
        /// </summary>
        /// <param name="post"></param>
        public BroadcastPost AddPost(PostModel postModel)
        {
            try
            {
                Post post = AddPostAsync(postModel);

                _db.SaveChanges();

                Notification notification = AddNotificationAsync(new NotificationModel
                {
                    UserID = post.UserID,
                    ItemID = post.ID,
                    TypeID = 1,
                    Message = postModel.PostedByName + " " + post.PostDescription,
                });

                return new BroadcastPost
                 {
                     Post = new PostModel
                     {
                         Message = post.Text,
                         PostedBy = post.UserID,
                         PostedByName = postModel.PostedByName,
                         PostedByAvatar = postModel.PostedByAvatar,
                         PostedDate = post.CreatedOn,
                         PostId = post.ID,
                         Privacy = post.Privacy,
                         Link = post.Link,
                         LinkIcon = post.LinkIcon,
                         LinkHeader = post.LinkHeader,
                         LinkDescription = post.LinkDescription,
                         PostDescription = post.PostDescription,
                         PostType = post.TypeID,
                         Active = post.Active
                     },
                     Notification = new NotificationModel
                     {
                         ID = notification.ID,
                         Message = notification.Text,
                         PostedByAvatar = postModel.PostedByAvatar,
                         ItemID = notification.ItemID ?? 0,
                         TypeID = notification.TypeID ?? 1,
                         IsRead = notification.IsRead
                     }
                 };
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while adding post to database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "AddPost");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", postModel.PostedBy));
                throw ex;
            }

        }

        /// <summary>
        /// Edit post for a page
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public dynamic EditPost(PostModel postModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Share post to another page
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public dynamic SharePost(PostModel postModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Report post as an abusive
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public dynamic ReportPost(PostModel postModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Hide post due to some reason
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public bool HidePost(PostModel postModel)
        {
            bool _status = false;
            Post post = null;
            try
            {
                post = _db.Posts.FirstOrDefault(po => po.ID == postModel.PostId && po.UserID == postModel.PostedBy);
                if (post != null)
                {
                    post.Privacy = 5;
                    _db.SaveChanges();
                    _status = true;
                    return _status;
                }
                return _status;
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while hiding post to database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "HidePost");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", post.UserID));
                throw ex;
            }
        }

        /// <summary>
        /// Delete an existing post
        /// </summary>
        /// <param name="postModel"></param>
        /// <returns></returns>
        public BroadcastPost DeletePost(PostModel postModel)
        {
            Post post = null;
            try
            {
                post = _db.Posts.FirstOrDefault(po => po.ID == postModel.PostId && po.UserID == postModel.PostedBy);
                if (post == null)
                {
                    return null;
                }
                else
                {
                    post.Active = false;
                    _db.SaveChanges();
                    return new BroadcastPost
                    {
                        Post = new PostModel
                        {
                            Message = post.Text,
                            PostedBy = post.UserID,
                            PostedByName = postModel.PostedByName,
                            PostedByAvatar = postModel.PostedByAvatar,
                            PostedDate = post.CreatedOn,
                            PostId = post.ID,
                            Privacy = post.Privacy,
                            Link = post.Link,
                            LinkIcon = post.LinkIcon,
                            LinkHeader = post.LinkHeader,
                            LinkDescription = post.LinkDescription,
                            PostDescription = post.PostDescription,
                            PostType = post.TypeID,
                            Active = post.Active
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while deleting post to database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "DeletePost");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", post.UserID));
                throw ex;
            }
        }

        /// <summary>
        /// adding comment ot post
        /// </summary>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        public BroadcastComment AddComment(CommentModel commentModel)
        {
            try
            {
                Comment _comment = new Comment
                 {
                     PostID = commentModel.PostId,
                     Text = commentModel.Message,
                     UserID = commentModel.UserID,
                     CreatedOn = DateTime.Now,
                     Active = true

                 };
                _db.Comments.Add(_comment);

                _db.SaveChanges();

                Notification notification = AddNotificationAsync(new NotificationModel
                {
                    UserID = commentModel.UserID,
                    ItemID = _comment.ID,
                    TypeID = 2,
                    Message = commentModel.PostedByName + " commented on post on your wall-" + commentModel.Message
                });

                return new BroadcastComment
                {
                    Comment = new CommentModel
                    {
                        UserID = _comment.UserID,
                        PostedByName = commentModel.PostedByName,
                        PostedByAvatar = commentModel.PostedByAvatar,
                        CreatedOn = _comment.CreatedOn,
                        CommentId = _comment.ID,
                        Message = _comment.Text,
                        PostId = _comment.PostID
                    },
                    Notification = new NotificationModel
                    {
                        ID = notification.ID,
                        Message = notification.Text,
                        PostedByAvatar = commentModel.PostedByAvatar,
                        ItemID = notification.ItemID ?? 0,
                        TypeID = notification.TypeID,
                        IsRead = notification.IsRead
                    }
                };
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while adding comment to database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "AddComment");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", commentModel.UserID));
                throw ex;
            }

        }

        /// <summary>
        /// Edit comment of post
        /// </summary>
        /// <param name="commentModel"></param>
        /// <returns></returns>
        public dynamic EditComment(CommentModel commentModel)
        {
            try
            {
                Comment _commment = _db.Comments.FirstOrDefault(com => com.ID == commentModel.CommentId && com.UserID == commentModel.UserID);

                if (_commment != null)
                {
                    _commment.Text = commentModel.Message;
                    _commment.CreatedOn = DateTime.Now;
                }

                Notification notification = AddNotificationAsync(new NotificationModel
                {
                    UserID = commentModel.UserID,
                    TypeID = 1,
                    Message = commentModel.PostedByName + " upadetd comment of post posted on your wall-" + commentModel.Message
                });
                _db.SaveChanges();

                return new
                {
                    comment = new
                    {
                        CommentedBy = _commment.UserID,
                        CommentedByName = commentModel.PostedByName,
                        CommentedByAvatar = commentModel.PostedByAvatar,
                        CommentedDate = _commment.CreatedOn,
                        CommentId = _commment.ID,
                        Message = _commment.Text,
                        PostId = _commment.PostID
                    },
                    postID = _commment.PostID,
                    notification = new
                    {
                        ID = notification.ID,
                        Message = notification.Text,
                        PostedByAvatar = commentModel.PostedByAvatar,
                        IsRead = notification.IsRead
                    }
                };
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while editing comment to database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "EditComment");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", commentModel.UserID));
                throw ex;
            }
        }

        /// <summary>
        /// Deleting comment for a post
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CommentID"></param>
        /// <returns></returns>
        public dynamic DeleteComment(long UserID, long CommentID)
        {
            bool _status = false;
            try
            {
                Comment _comment = _db.Comments.FirstOrDefault(com => com.ID == CommentID && com.UserID == UserID);
                if (_comment != null)
                {
                    _db.Comments.Remove(_comment);
                    _db.SaveChanges();
                    _status = true;
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while deleting comment from database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "DeleteComment");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Comment ID: {1}", UserID, CommentID));
                throw ex;
            }
            return _status;
        }

        /// <summary>
        /// Adding like of item level
        /// </summary>
        /// <param name="likeModel"></param>
        /// <returns></returns>
        public BroadcastLike Like(LikeModel likeModel)
        {
            try
            {
                Like _like = _db.Likes.Add(new Like
                {
                    ItemID = likeModel.ItemID,
                    ItemLevel = likeModel.ItemLevel,
                    UserID = likeModel.UserID,
                    CreatedOn = DateTime.UtcNow
                });

                Notification notification = AddNotificationAsync(new NotificationModel
                {
                    UserID = likeModel.UserID,
                    ItemID = _like.ItemID,
                    TypeID = likeModel.ItemLevel == 1 ? (byte)3 : (byte)4,
                    Message = likeModel.PostedByName + " like it"
                });

                _db.SaveChanges();

                return new BroadcastLike
                {
                    Like = new LikeModel
                    {
                        LikeID = _like.ID,
                        UserID = _like.UserID,
                        ItemID = _like.ItemID,
                        ItemLevel = _like.ItemLevel
                    },
                    TotalLikes = _db.Likes.Where(x => x.ItemID == likeModel.ItemID && x.ItemLevel == likeModel.ItemLevel).Count(),
                    Notification = new NotificationModel
                    {
                        ID = notification.ID,
                        Message = notification.Text,
                        PostedByAvatar = likeModel.PostedByAvatar,
                        ItemID = notification.ItemID ?? 0,
                        TypeID = notification.TypeID,
                        IsRead = notification.IsRead
                    }
                };
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while adding like to database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "AddLike");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", likeModel.UserID));
                throw ex;
            }
        }

        /// <summary>
        /// Deleting like for an item
        /// </summary>
        /// <param name="likeModel"></param>
        /// <returns></returns>
        public BroadcastLike UnLike(LikeModel likeModel)
        {
            try
            {
                Like _like = _db.Likes.FirstOrDefault(com => com.ItemID == likeModel.ItemID && com.UserID == likeModel.UserID);
                if (_like != null)
                {
                    _db.Likes.Remove(_like);
                }
                _db.SaveChanges();

                return new BroadcastLike
                {
                    Like = new LikeModel
                    {
                        LikeID = _like.ID,
                        UserID = _like.UserID,
                        ItemID = _like.ItemID,
                        ItemLevel = _like.ItemLevel
                    },
                    TotalLikes = _db.Likes.Where(x => x.ItemID == likeModel.ItemID && x.ItemLevel == likeModel.ItemLevel).Count()
                };
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while deleting like from database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "DeleteLike");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Item ID: {1}", likeModel.UserID, likeModel.ItemID));
                throw ex;
            }
        }

        #region Entity Async Functions
        /// <summary>
        /// adding post model to entity context
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public Post AddPostAsync(PostModel post)
        {
            try
            {
                var ret = new Post
                {
                    UserID = post.PostedBy,
                    CreatedOn = post.PostedDate,
                    Text = post.Message,
                    PostDescription = post.PostDescription,
                    Privacy = post.Privacy,
                    Link = post.Link,
                    LinkIcon = post.LinkIcon,
                    LinkHeader = post.LinkHeader,
                    LinkDescription = post.LinkDescription,
                    Active = post.Active,
                    TypeID = post.PostType
                };
                _db.Posts.Add(ret);
                return ret;
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incoreect model values.";
                ex.Data.Add("Location : ", "Exception occured while adding post to entity context.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "AddPostAsync");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", post.PostedBy));
                throw ex;
            }
        }

        /// <summary>
        /// adding notification model to entity context
        /// </summary>
        /// <param name="notification"></param>
        /// <returns></returns>
        public Notification AddNotificationAsync(NotificationModel notification)
        {
            try
            {
                var model = new Notification
                {
                    UserID = notification.UserID,
                    Text = notification.Message,
                    ItemID = notification.ItemID,
                    TypeID = notification.TypeID,
                    CreatedOn = notification.CreatedOn,
                    Active = true
                };

                _db.Notifications.Add(model);
                _db.SaveChanges();
                return model;
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incoreect model values.";
                ex.Data.Add("Location : ", "Exception occured while adding notification to entity context.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "AddNotificationAsync");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", notification.UserID));
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Page Info

        /// <summary>
        /// Get user detail
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public dynamic GetUserTimeline(long UserID)
        {
            try
            {
                return _db.AspNetUsers.Where(x => x.Id == UserID).Select(x => new
                {
                    NickName = x.NickName,
                    Gender = x.Gender,
                    Age = DateTime.UtcNow.Year - x.BirthDate.Value.Year,
                    Religion = x.Religion,
                    AboutMe = x.AboutMe,
                    Rating = x.Rating,
                    PhoneNumber = x.PhoneNumber,
                    Skype = x.Skype,
                    Email = x.Email,
                    Organization = x.Organization,
                    OrganizationLocation = x.OrganizationLocation,
                    PositionInOrganization = x.PositionInOrganization,
                    OrganizationJoinDate = x.OrganizationJoinDate,
                    Address = x.Address,
                    City = x.City,
                    Country = x.Country,
                    PostalCode = x.PostalCode,
                    Relationship = x.Relationship,
                    Interest = x.Interest,
                    Hobbies = x.Hobbies
                }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error.";
                ex.Data.Add("Location : ", "Exception occured while fetching user detail.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetUserDetailByUserID");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", UserID));
                throw ex;
            }
        }

        /// <summary>
        /// Get detail of page profile
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public dynamic GetUserDetailByID(long UserID, long PageID)
        {
            try
            {
                var friend = _db.Friendships.FirstOrDefault(x => (x.AspNetUser.Id == PageID && x.AspNetUser1.Id == UserID) || (x.AspNetUser.Id == UserID && x.AspNetUser1.Id == PageID));
                var Requester = friend == null ? 0 : friend.UserID;
                var Reciever = friend == null ? 0 : friend.FriendID;
                var PageRelationStatus = friend == null ? 0 : friend.StatusID;

                return _db.AspNetUsers.Where(x => x.Id == PageID)
                      .Select(x => new
                      {
                          ID = x.Id,
                          Name = x.UserName,
                          FullName = x.FullName,
                          Wallpaper = x.CoverPicture,
                          Picture = x.ProfilePicture,
                          PageType = (byte)Enums.PageType.Profile,
                          Requester,
                          Reciever,
                          PageRelationStatus,
                      }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to Invalid user id, page id or database connection.";
                ex.Data.Add("Location : ", "Exception occured while gettibg page detail.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetUserDetailByID");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page ID: {1}, Page Type: {2}", UserID, PageID, (byte)Enums.PageType.Profile));
                throw ex;
            }
        }

        /// <summary>
        /// Get group detail
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public dynamic GetGroupDetailByID(long UserID, long PageID)
        {
            try
            {
                var group = _db.GroupUsers.FirstOrDefault(x => (x.UserID == UserID && x.GroupID == PageID));
                var Requester = group == null ? 0 : group.UserID;
                var PageRelationStatus = group == null ? 0 : group.StatusID;

                return _db.Groups.Where(x => x.ID == PageID)
                    .Select(x => new
                    {
                        ID = x.ID,
                        Name = x.Name,
                        FullName = x.FullName,
                        Wallpaper = x.Wallpaper,
                        Picture = x.Icon,
                        PageType = (byte)Enums.PageType.Profile,
                        Requester,
                        PageRelationStatus,
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to Invalid user id , group id or database connection.";
                ex.Data.Add("Location : ", "Exception occured while getting group detail.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetGroupDetailByID");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page ID: {1}, Page Type: {2}", UserID, PageID, (byte)Enums.PageType.Group));
                throw ex;
            }
        }

        /// <summary>
        /// get event detail
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <returns></returns>
        public dynamic GetEventDetailByID(long UserID, long PageID)
        {
            try
            {
                var _event = _db.EventUsers.FirstOrDefault(x => (x.UserID == UserID && x.EventID == PageID));
                var Requester = _event == null ? 0 : _event.UserID;
                var PageRelationStatus = _event == null ? 0 : _event.StatusID;

                return _db.Events.Where(x => x.ID == PageID)
                    .Select(x => new
                    {
                        ID = x.ID,
                        Name = x.Name,
                        FullName = x.FullName,
                        Wallpaper = x.Wallpaper,
                        Picture = x.Icon,
                        PageType = (byte)Enums.PageType.Profile,
                        Requester,
                        PageRelationStatus,
                    }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to Invalid user id , group id or database connection.";
                ex.Data.Add("Location : ", "Exception occured while getting group detail.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetGroupDetailByID");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page ID: {1}, Page Type: {2}", UserID, PageID, (byte)Enums.PageType.Event));
                throw ex;
            }
        }

        /// <summary>
        /// Get pages of users
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public dynamic GetPages(long UserID, int PageNumber, int PageSize)
        {
            try
            {
                return _db.Friendships.Where(x => (x.UserID == UserID || x.FriendID == UserID) && x.StatusID == 2)
                       .OrderByDescending(d => d.CreatedOn)
                .Skip(PageSize * (PageNumber - 1))
                    .Select(x => new
                {
                    Id = x.UserID == UserID ? x.AspNetUser1.Id : x.AspNetUser.Id,
                    Name = x.UserID == UserID ? x.AspNetUser1.UserName : x.AspNetUser.UserName,
                    FullName = x.UserID == UserID ? x.AspNetUser1.FullName : x.AspNetUser.FullName,
                    Avtar = x.UserID == UserID ? x.AspNetUser1.ProfilePicture : x.AspNetUser.ProfilePicture,
                    Detail = x.UserID == UserID ? x.AspNetUser1.StatusID : x.AspNetUser.StatusID,
                }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching user's pages.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetPages");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page Number: {1}, Page Size: {3}", UserID, PageNumber, PageSize));
                throw ex;
            }
        }

        /// <summary>
        /// Get users groups
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public dynamic GetGroups(long UserID, int PageNumber, int PageSize)
        {
            try
            {
                return _db.Groups.Where(gro => gro.UserID == UserID && (gro.Active == true || gro.Active == null))
                .OrderByDescending(d => d.CreatedOn)
                .Skip(PageSize * (PageNumber - 1))
                    .Select(x => new
                    {
                        ID = x.ID,
                        Name = x.Name,
                        FullName = x.FullName,
                        Icon = x.Icon
                    }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching user's groups.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetGroups");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page Number: {1}, Page Size: {3}", UserID, PageNumber, PageSize));
                throw ex;
            }
        }

        /// <summary>
        /// Get users events
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public dynamic GetEvents(long UserID, int PageNumber, int PageSize)
        {
            try
            {
                return _db.Events.Where(eve => eve.UserID == UserID && (eve.Active == true || eve.Active == null))
                .OrderByDescending(d => d.CreatedOn)
                .Skip(PageSize * (PageNumber - 1))
                    .Select(x => new
                    {
                        ID = x.ID,
                        Name = x.Name,
                        FullName = x.FullName,
                        Icon = x.Icon
                    }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching user's events.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetEvents");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page Number: {1}, Page Size: {3}", UserID, PageNumber, PageSize));
                throw ex;
            }
        }

        public dynamic GetSearchPages(long UserID, String Key, int PageNumber, int PageSize)
        {
            try
            {
                return _db.AspNetUsers.Where(x => x.UserName.Contains(Key) || x.FullName.Contains(Key) || x.Email.Contains(Key) || x.Address.Contains(Key))
                .OrderByDescending(d => d.LastUpdatedOn)
                .Skip(PageSize * (PageNumber - 1))
                    .Select(x => new
                    {
                        Url = "/" + x.UserName,
                        Fullname = x.FullName,
                        Username = x.UserName,
                        Gender = x.Gender,
                        Image = x.ProfilePicture
                    }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching search's pages.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetSearchPages");
                ex.Data.Add("Input Parameters : ", string.Format("Key: {0}, Page Number: {1}, Page Size: {3}", Key, PageNumber, PageSize));
                throw ex;
            }
        }

        /// <summary>
        /// Get photos
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public dynamic GetPhotos(long UserID, int PageNumber, int PageSize)
        {
            try
            {
                return _db.Posts.Where(x => x.UserID == UserID && (x.Active == true || x.Active == null) && x.TypeID == 2)
               .OrderByDescending(d => d.CreatedOn)
                  .Skip(PageSize * (PageNumber - 1))
                   .Select(x => new
                   {
                       ID = x.ID,
                       Name = x.AspNetUser.UserName,
                       FullName = "",
                       Avtar = x.Link,
                       AvtarIcon = x.LinkIcon,
                       Detail = x.Text
                   }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching user's photos.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetPhotos");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page Number: {1}, Page Size: {3}", UserID, PageNumber, PageSize));
                throw ex;
            }
        }

        /// <summary>
        /// Get users music
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public dynamic GetMusics(long UserID, int PageNumber, int PageSize)
        {
            try
            {
                return _db.Posts.Where(x => x.UserID == UserID && (x.Active == true || x.Active == null) && x.TypeID == 5)
                .OrderByDescending(d => d.CreatedOn)
                   .Skip(PageSize * (PageNumber - 1))
                    .Select(x => new
                    {
                        ID = x.ID,
                        Name = x.AspNetUser.UserName,
                        FullName = "",
                        Avtar = x.Link,
                        AvtarIcon = x.LinkIcon,
                        Detail = x.Text
                    }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching user's musics.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetMusics");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page Number: {1}, Page Size: {3}", UserID, PageNumber, PageSize));
                throw ex;
            }
        }

        /// <summary>
        /// get users videos
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public dynamic GetVideos(long UserID, int PageNumber, int PageSize)
        {
            try
            {
                return _db.Posts.Where(x => x.UserID == UserID && (x.Active == true || x.Active == null) && x.TypeID == 6)
               .OrderByDescending(d => d.CreatedOn)
                  .Skip(PageSize * (PageNumber - 1))
                   .Select(x => new
                   {
                       ID = x.ID,
                       Name = x.AspNetUser.UserName,
                       FullName = "",
                       Avtar = x.Link,
                       AvtarIcon = x.LinkIcon,
                       Detail = x.Text
                   }).Take(PageSize);
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching user's videos.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetVideos");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, Page Number: {1}, Page Size: {3}", UserID, PageNumber, PageSize));
                throw ex;
            }
        }

        /// <summary>
        /// update event cover picture
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <returns></returns>
        public bool UpdateEventCoverPicture(long UserID, long PageID, string Url)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// update group cover picture
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <returns></returns>
        public bool UpdateGroupCoverPicture(long UserID, long PageID, string Url)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// update user cover picture
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <returns></returns>
        public bool UpdateUserCoverPicture(long UserID, long PageID, string Url)
        {
            try
            {
                var user = _db.AspNetUsers.Find(PageID);
                if (user != null)
                {
                    user.CoverPicture = Url;
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while upadting user's cover picture.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "UpdateUserCoverPicture");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, PageID: {1}, Url: {3}", UserID, PageID, Url));
                throw ex;
            }

        }

        /// <summary>
        /// update event profile picture
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <returns></returns>
        public bool UpdateEventPofilePicture(long UserID, long PageID, string Url)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  update group profile picture
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <returns></returns>
        public bool UpdateGroupPofilePicture(long UserID, long PageID, string Url)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///  update user profile picture
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="PageID"></param>
        /// <param name="PageType"></param>
        /// <returns></returns>
        public bool UpdateUserPofilePicture(long UserID, long PageID, string Url)
        {
            try
            {
                var user = _db.AspNetUsers.Find(PageID);
                if (user != null)
                {
                    user.ProfilePicture = Url;
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while updating user's profile picture.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "UpdateUserPofilePicture");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}, PageID: {1}, Url: {3}", UserID, PageID, Url));
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public long GetUserIDByName(String UserName)
        {
            try
            {
                AspNetUser _user = _db.AspNetUsers.FirstOrDefault(x => x.UserName == UserName && (x.Active == true || x.Active == null));
                if (_user != null)
                {
                    return _user.Id;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching user's ID.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetUserIDByName");
                ex.Data.Add("Input Parameters : ", string.Format("UserName: {0}", UserName));
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public long GetGroupIDByName(String GroupName)
        {
            try
            {
                Group _group = _db.Groups.FirstOrDefault(x => x.Name == GroupName && (x.Active == true || x.Active == null));
                if (_group != null)
                {
                    return _group.ID;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching group's ID.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetGroupIDByName");
                ex.Data.Add("Input Parameters : ", string.Format("GroupName: {0}", GroupName));
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public long GetEventIDByName(String EventName)
        {
            try
            {
                Event _event = _db.Events.FirstOrDefault(x => x.Name == EventName && (x.Active == true || x.Active == null));
                if (_event != null)
                {
                    return _event.ID;
                }
                else
                {
                    return -1;
                }
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to invalid data supplied or database connection.";
                ex.Data.Add("Location : ", "Exception occured while fetching event's ID.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "GetEventIDByName");
                ex.Data.Add("Input Parameters : ", string.Format("GroupName: {0}", EventName));
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="FriendRequest"></param>
        /// <returns></returns>
        public BroadcastFriendRequest AddUserFriendRequest(FriendRequestModel FriendRequest)
        {
            Friendship _friendship = _db.Friendships.FirstOrDefault(x => (x.FriendID == FriendRequest.PageID && x.UserID == FriendRequest.UserID) || (x.FriendID == FriendRequest.UserID && x.UserID == FriendRequest.PageID));

            if (_friendship == null)
            {
                _friendship = new Friendship();
            }

            _friendship.UserID = FriendRequest.UserID;
            _friendship.FriendID = FriendRequest.PageID;
            _friendship.CreatedOn = FriendRequest.CreatedOn;
            _friendship.StatusID = (byte)FriendRequest.PageRelationStatus;
            _friendship.PageTypeID = (byte)FriendRequest.PageType;
            _friendship.IsRead = false;

            if (_friendship.AspNetUser == null)
                _db.Friendships.Add(_friendship);

            try
            {
                _db.SaveChanges();

                return new BroadcastFriendRequest
                {
                    FriendRequest = new FriendRequestModel
                    {
                        ID = _friendship.ID,
                        UserID = FriendRequest.UserID,
                        PostedByName = FriendRequest.PostedByName,
                        PostedByAvatar = FriendRequest.PostedByAvatar,
                        UserInfo = FriendRequest.UserInfo,
                        PageID = _friendship.FriendID,
                        PageType = FriendRequest.PageType,
                        PageRelationStatus = (Enums.FriendshipStatus)_friendship.StatusID,
                        IsRead = _friendship.IsRead,
                        CreatedOn = FriendRequest.CreatedOn,
                    }
                };
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while adding friendship request to database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "AddFriend");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", FriendRequest.UserID));
                throw ex;
            }
        }

        public BroadcastFriendRequest AcceptUserFriendRequest(FriendRequestModel FriendRequest)
        {
            Friendship _friendship = _db.Friendships.FirstOrDefault(x => x.FriendID == FriendRequest.PageID && x.StatusID == (byte)Enums.FriendshipStatus.FS && x.UserID == FriendRequest.UserID);

            if (_friendship == null)
            {
                return null;
            }

            _friendship.CreatedOn = DateTime.UtcNow;
            _friendship.StatusID = (byte)Enums.FriendshipStatus.FA;

            Notification notification = AddNotificationAsync(new NotificationModel
            {
                UserID = _friendship.FriendID,
                ItemID = _friendship.ID,
                TypeID = 4,
                Message = FriendRequest.PostedByName + " is now friend with " + _friendship.AspNetUser.FullName,
            });

            try
            {
                _db.SaveChanges();

                return new BroadcastFriendRequest
                {
                    FriendRequest = new FriendRequestModel
                    {
                        ID = _friendship.ID,
                        UserID = FriendRequest.UserID,
                        PostedByName = FriendRequest.PostedByName,
                        PostedByAvatar = FriendRequest.PostedByAvatar,
                        UserInfo = FriendRequest.UserInfo,
                        PageID = _friendship.FriendID,
                        PageType = FriendRequest.PageType,
                        PageRelationStatus = (Enums.FriendshipStatus)_friendship.StatusID,
                        IsRead = _friendship.IsRead,
                        CreatedOn = FriendRequest.CreatedOn,
                    },
                    Notification = new NotificationModel
                   {
                       ID = notification.ID,
                       Message = notification.Text,
                       PostedByAvatar = FriendRequest.PostedByAvatar,
                       ItemID = notification.ItemID ?? 0,
                       TypeID = notification.TypeID ?? 1,
                       IsRead = notification.IsRead
                   }
                };
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while accepting friendship request to database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "AcceptUserFriendRequest");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", FriendRequest.UserID));
                throw ex;
            }
        }

        public BroadcastFriendRequest RejectUserFriendRequest(FriendRequestModel FriendRequest)
        {
            Friendship _friendship = _db.Friendships.FirstOrDefault(x => ((x.FriendID == FriendRequest.PageID && x.UserID == FriendRequest.UserID) || (x.FriendID == FriendRequest.UserID && x.UserID == FriendRequest.PageID)) && x.StatusID == (byte)FriendRequest.PageRelationStatus);

            if (_friendship == null)
            {
                return null;
            }

            _friendship.CreatedOn = DateTime.UtcNow;
            _friendship.StatusID = (byte)Enums.FriendshipStatus.FR;

            try
            {
                _db.SaveChanges();

                return new BroadcastFriendRequest
                {
                    FriendRequest = new FriendRequestModel
                    {
                        ID = _friendship.ID,
                        UserID = _friendship.UserID,
                        PostedByName = FriendRequest.PostedByName,
                        PostedByAvatar = FriendRequest.PostedByAvatar,
                        UserInfo = FriendRequest.UserInfo,
                        PageID = _friendship.FriendID,
                        PageType = FriendRequest.PageType,
                        PageRelationStatus = Enums.FriendshipStatus.FR,
                        IsRead = _friendship.IsRead,
                        CreatedOn = FriendRequest.CreatedOn,
                    }
                };
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while accepting friendship request to database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "RejectUserFriendRequest");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", FriendRequest.UserID));
                throw ex;
            }
        }

        public AboutPageModel SavePageDetail(AboutPageModel AboutPage)
        {
            AspNetUser _model = _db.AspNetUsers.FirstOrDefault(x => (x.Id == AboutPage.Id && (x.Active == null || x.Active == true)));

            if (_model == null)
            {
                return null;
            }

            _model.NickName = AboutPage.NickName;
            _model.Religion = AboutPage.Religion;
            _model.Language = AboutPage.Language;
            _model.AboutMe = AboutPage.AboutMe;
            _model.Privacy = AboutPage.Privacy;
            _model.PhoneNumber = AboutPage.PhoneNumber;
            _model.Skype = AboutPage.Skype;
            _model.Education = AboutPage.Education;
            _model.Organization = AboutPage.Organization;
            _model.OrganizationJoinDate = AboutPage.OrganizationJoinDate;
            _model.OrganizationLocation = AboutPage.OrganizationLocation;
            _model.PositionInOrganization = AboutPage.PositionInOrganization;
            _model.Address = AboutPage.Address;
            _model.City = AboutPage.City;
            _model.Country = AboutPage.Country;
            _model.PostalCode = AboutPage.PostalCode;
            _model.TimeZone = AboutPage.TimeZone;
            _model.Relationship = AboutPage.Relationship;
            _model.Interest = AboutPage.Interest;
            _model.Hobbies = AboutPage.Hobbies;
            _model.FavAnimals = AboutPage.FavAnimals;
            _model.favArtist = AboutPage.favArtist;
            _model.FavBooks = AboutPage.FavBooks;
            _model.FavMusics = AboutPage.FavMusics;
            _model.Smoker = AboutPage.Smoker;
            _model.Drinker = AboutPage.Drinker;

            try
            {
                _db.SaveChanges();
                return AboutPage;
            }
            catch (Exception ex)
            {
                ex.HelpLink = "It can be due to database connection error or incorect model values.";
                ex.Data.Add("Location : ", "Exception occured while adding friendship request to database.");
                ex.Data.Add("Applpication Tier : ", "3. Data Repository");
                ex.Data.Add("Class : ", "Data Access");
                ex.Data.Add("Method : ", "AddFriend");
                ex.Data.Add("Input Parameters : ", string.Format("User ID: {0}", AboutPage.Id));
                throw ex;
            }
        }
        #endregion

        #endregion

        #region Static Member Function

        #endregion
    }
}
