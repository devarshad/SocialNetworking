using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using QAConnect.Models;
using DataTypes;
using QAConnect.Helpers;

namespace QAConnect.Hubs
{
    public class UserHub : BaseHub
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
        /// Get curret user information
        /// </summary>
        public void getUserInfo()
        {
            Clients.Caller.loadUserInfo(GetMyInfo());
        }
        
        //public void AddGroup(Group groupdata)
        //{
        //    groupdata.CreatedOn = DateTime.Now;
        //    groupdata.UserID = CurrentUser.UserID;
        //    groupdata.Active = true;
        //    groupdata.Icon = "/Profiles/Common/ProfilePic/user.png";
        //    groupdata.Wallpaper = "/Profiles/Common/ProfileCoverPic/Hydrangeas.jpg";

        //    db.Groups.Add(groupdata);
        //    db.SaveChanges();

        //    var ret = new
        //    {
        //        ID = groupdata.ID,
        //        Name = groupdata.Name,
        //        FullName = groupdata.FullName,
        //        Icon = groupdata.Icon,
        //        Privacy = groupdata.PagePrivacy,
        //        Wallpaper = groupdata.Wallpaper,
        //        Description = groupdata.Description
        //    };

        //    Clients.Caller.addGroup(ret);
        //}

        //public void AddEvent(Post post)
        //{
        //    post.UserID = CurrentUser.UserID;
        //    post.CreatedOn = DateTime.Now;

        //    db.Posts.Add(post);

        //    Notification notification = new Notification();
        //    notification.UserID = CurrentUser.UserID;
        //    notification.TypeID = 1;
        //    notification.Text = CurrentUser.FullName + " " + post.PostDescription;
        //    notification.CreatedOn = DateTime.Now;
        //    notification.Active = true;
        //    db.Notifications.Add(notification);

        //    db.SaveChanges();
        //    var ret = new
        //    {
        //        Message = post.Text,
        //        PostedBy = post.UserID,
        //        PostedByName = CurrentUser.FullName,
        //        PostedByAvatar = CurrentUser.ProfilePicture,
        //        PostedDate = post.CreatedOn,
        //        PostId = post.ID,
        //        Privacy = post.Privacy,
        //        Link = post.Link,
        //        LinkIcon = post.LinkIcon,
        //        LinkHeader = post.LinkHeader,
        //        LinkDescription = post.LinkDescription,
        //        PostDescription = post.PostDescription,
        //        PostType = post.TypeID
        //    };

        //    Clients.Caller.addPost(ret);
        //    Clients.Others.newPost(
        //        ret,
        //       new
        //       {
        //           ID = notification.ID,
        //           Message = notification.Text,
        //           PostedByAvatar = CurrentUser.ProfilePicture,
        //           IsRead = notification.IsRead
        //       }
        //    );
        //}

        //public void AddFriendMessage(string toUserID, string messageText)
        //{
        //    Message mess = new Message();
        //    mess.SenderID = CurrentUser.UserID;
        //    mess.Text = messageText;
        //    mess.RecieverID = toUserID;
        //    mess.IsRead = false;
        //    mess.Active = true;
        //    mess.CreatedOn = DateTime.Now;

        //    db.Messages.Add(mess);

        //    db.SaveChanges();
        //    Clients.Others.newMessages(
        //       new
        //       {
        //           ID = mess.ID,
        //           Message = mess.Text,
        //           PostedByAvatar = CurrentUser.ProfilePicture,
        //           IsRead = false
        //       }
        //    );
        //}

        //public void AddFriendRequest(string FriendID)
        //{
        //    Friendship fs = new Friendship
        //    {
        //        UserID = CurrentUser.UserID,
        //        FriendID = FriendID.Trim(),
        //        CreatedOn = DateTime.Now,
        //        StatusID = 1
        //    };

        //    db.Friendships.Add(fs);

        //    Notification notification = new Notification();
        //    notification.UserID = CurrentUser.UserID;
        //    notification.TypeID = 2;
        //    notification.Text = CurrentUser.FullName + " sent the friend request to you";
        //    notification.CreatedOn = DateTime.Now;
        //    notification.Active = true;

        //    db.Notifications.Add(notification);

        //    db.SaveChanges();

        //    Clients.Others.addNotification(
        //      new
        //      {
        //          ID = notification.ID,
        //          Message = notification.Text,
        //          PostedByAvatar = CurrentUser.ProfilePicture,
        //          IsRead = notification.IsRead
        //      }
        //   );
        //}

        //public void AddFriend(string FriendID)
        //{
        //    var model = db.Friendships.FirstOrDefault(x => x.UserID == FriendID && x.StatusID == 1 && x.FriendID == CurrentUser.UserID);
        //    model.CreatedOn = DateTime.Now;
        //    model.StatusID = 2;

        //    Notification notification = new Notification();
        //    notification.UserID = CurrentUser.UserID;
        //    notification.TypeID = 2;
        //    notification.Text = CurrentUser.FullName + " is now friend with " + model.User.FullName;
        //    notification.CreatedOn = DateTime.Now;
        //    notification.Active = true;

        //    db.Notifications.Add(notification);

        //    db.SaveChanges();

        //    Clients.Others.addNotification(
        //      new
        //      {
        //          ID = notification.ID,
        //          Message = notification.Text,
        //          PostedByAvatar = CurrentUser.ProfilePicture,
        //          IsRead = notification.IsRead
        //      }
        //   );
        //}

        //public void CancelFriendRequest(String FriendID)
        //{
        //    var model = db.Friendships.FirstOrDefault(x => x.FriendID == FriendID && x.StatusID == 1 && x.UserID == CurrentUser.UserID);
        //    model.CreatedOn = DateTime.Now;
        //    model.StatusID = 3;

        //    db.SaveChanges();
        //}

        //public void RejectFriend(string FriendID)
        //{
        //    var model = db.Friendships.FirstOrDefault(x => x.UserID == FriendID && x.StatusID == 1 && x.FriendID == CurrentUser.UserID);
        //    model.CreatedOn = DateTime.Now;
        //    model.StatusID = 3;

        //    db.SaveChanges();
        //}

        #endregion

        #region Static Member Function

        #endregion
    }
}