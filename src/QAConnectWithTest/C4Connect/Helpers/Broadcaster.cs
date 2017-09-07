using DataTypes;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using BusinessLogic;
using C4Connect.Hubs;

namespace C4Connect.Helpers
{
    public class Broadcaster
    {
        #region Private Data Member

        // Singleton instance
        private readonly static IHubContext _headerBroadCaster = GlobalHost.ConnectionManager.GetHubContext<HeaderHub>();
        private readonly static IHubContext _userBroadCaster = GlobalHost.ConnectionManager.GetHubContext<UserHub>();
        private readonly static IHubContext _chatBroadCaster = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
        private readonly static IHubContext _postBroadCaster = GlobalHost.ConnectionManager.GetHubContext<PostHub>();

        protected UserAccess _user = null;
        protected ChatAccess _chat = null;
        protected PostAccess _post = null;
        #endregion

        #region Public Data Members

        #endregion

        #region Protected Data Member
        IHubContext _context;
        #endregion

        #region Static Data Member

        #endregion

        #region Constructor

        public Broadcaster()
        {
            _user = new UserAccess();
            _chat = new ChatAccess();
            _post = new PostAccess();
        }
        public Broadcaster(IHubContext context)
        {
            _context = context;
        }
        #endregion

        #region Private Member Functions

        #endregion

        #region Protected Member function

        #endregion

        #region Public Member Function

        /// <summary>
        /// Select a broad casting way to new post to a page
        /// </summary>
        /// <param name="post"></param>
        public async Task AddPost(BroadcastPost model, Enums.BroadCastType BroadCastType)
        {
            try
            {
                switch (BroadCastType)
                {
                    case Enums.BroadCastType.Web:
                        await BroadcastAddPostToWeb(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error broadcasting post to page", ex);
            }
        }
        /// <summary>
        /// Select a broad casting way to delete post to a page
        /// </summary>
        /// <param name="post"></param>
        public async Task DeletePost(BroadcastPost model, Enums.BroadCastType BroadCastType)
        {
            try
            {
                switch (BroadCastType)
                {
                    case Enums.BroadCastType.Web:
                        await BroadcastDeletePostToWeb(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error broadcasting post to page", ex);
            }
        }
        /// <summary>
        /// Select a broad casting way to new comment to a post
        /// </summary>
        /// <param name="post"></param>
        public async Task AddComment(BroadcastComment model, Enums.BroadCastType BroadCastType)
        {
            try
            {
                switch (BroadCastType)
                {
                    case Enums.BroadCastType.Web:
                        await BroadcastAddCommentToWeb(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error broadcasting comment to post", ex);
            }
        }

        /// <summary>
        /// Select a broad casting way to new comment to a post
        /// </summary>
        /// <param name="post"></param>
        public async Task Like(BroadcastLike model, Enums.BroadCastType BroadCastType)
        {
            try
            {
                switch (BroadCastType)
                {
                    case Enums.BroadCastType.Web:
                        await BroadcastLikeToWeb(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error broadcasting comment to post", ex);
            }
        }

        /// <summary>
        /// Select a broad casting way to new comment to a post
        /// </summary>
        /// <param name="post"></param>
        public async Task UnLike(BroadcastLike model, Enums.BroadCastType BroadCastType)
        {
            try
            {
                switch (BroadCastType)
                {
                    case Enums.BroadCastType.Web:
                        await BroadcastUnLikeToWeb(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error broadcasting comment to post", ex);
            }
        }

        /// <summary>
        /// Select a broad casting way to new comment to a post
        /// </summary>
        /// <param name="post"></param>
        public async Task SendChatMessage(BroadcastChatMessage model, Enums.BroadCastType BroadCastType)
        {
            try
            {
                switch (BroadCastType)
                {
                    case Enums.BroadCastType.Web:
                        await BroadcastChatMessageToWeb(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error broadcasting comment to post", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="broadCastType"></param>
        /// <returns></returns>
        public async Task AddFriendRequest(BroadcastFriendRequest model, Enums.BroadCastType broadCastType)
        {
            try
            {
                switch (broadCastType)
                {
                    case Enums.BroadCastType.Web:
                        await BroadcastAddFriendRequestToWeb(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error broadcasting comment to post", ex);
            }
        }

        public async Task AcceptFriendRequest(BroadcastFriendRequest model, Enums.BroadCastType broadCastType)
        {
            try
            {
                switch (broadCastType)
                {
                    case Enums.BroadCastType.Web:
                        await BroadcastAcceptFriendRequestToWeb(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error broadcasting comment to post", ex);
            }
        }

        public async Task CancelFriendRequest(BroadcastFriendRequest model, Enums.BroadCastType broadCastType)
        {
            try
            {
                switch (broadCastType)
                {
                    case Enums.BroadCastType.Web:
                        await BroadcastCancelFriendRequestToWeb(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error broadcasting cancel to page", ex);
            }
        }

        public async Task RejectUserFriendRequest(BroadcastFriendRequest model, Enums.BroadCastType broadCastType)
        {
            try
            {
                switch (broadCastType)
                {
                    case Enums.BroadCastType.Web:
                        await BroadcastRejectUserFriendRequestToWeb(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionService.LogError("Error broadcasting cancel to page", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task BroadcastAddFriendRequestToWeb(BroadcastFriendRequest model)
        {
            IList<string> connectionIDs = _user.GetFollowerList(model.FriendRequest.UserID, model.FriendRequest.PageID, model.FriendRequest.PageType, model.FriendRequest.PageRelationStatus)
                 .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

            await _headerBroadCaster.Clients.Clients(connectionIDs)
                            .newRequests(model.FriendRequest);
        }

        private async Task BroadcastAcceptFriendRequestToWeb(BroadcastFriendRequest model)
        {
            var users = _user.GetFollowerList(model.FriendRequest.PageID, model.FriendRequest.UserID, model.FriendRequest.PageType, model.FriendRequest.PageRelationStatus);
            //  .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

            IList<string> requesterID = users.Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web && x.UserID == model.FriendRequest.UserID).Select(x => x.ConnectionID).ToList();

            IList<string> connectionIDs = users.Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).SkipWhile(x => x.UserID == model.FriendRequest.UserID).Select(x => x.ConnectionID).ToList();

            await _headerBroadCaster.Clients.Clients(connectionIDs)
                            .newNotifications(model.Notification);
            await _chatBroadCaster.Clients.Clients(requesterID)
                            .updateUserList(_user.GetUserInfoByUserID(model.FriendRequest.PageID));
            await _headerBroadCaster.Clients.Clients(requesterID)
                           .removeRequests(model.FriendRequest);
        }

        private async Task BroadcastCancelFriendRequestToWeb(BroadcastFriendRequest model)
        {
            IList<string> connectionIDs = _user.GetFollowerList(model.FriendRequest.PageID, model.FriendRequest.UserID, model.FriendRequest.PageType, model.FriendRequest.PageRelationStatus)
                 .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

            await _headerBroadCaster.Clients.Clients(connectionIDs)
                            .removeRequests(model.FriendRequest);
            await _chatBroadCaster.Clients.Clients(connectionIDs)
                            .updateUserList(_user.GetUserInfoByUserID(model.FriendRequest.PageID));
        }


        private async Task BroadcastRejectUserFriendRequestToWeb(BroadcastFriendRequest model)
        {
            IList<string> connectionIDs = _user.GetFollowerList(model.FriendRequest.PageID, model.FriendRequest.UserID, model.FriendRequest.PageType, model.FriendRequest.PageRelationStatus)
                 .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

            await _headerBroadCaster.Clients.Clients(connectionIDs)
                            .removeRequests(model.FriendRequest);
        }


        /// <summary>
        /// broadcast post to page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task BroadcastAddPostToWeb(BroadcastPost model)
        {
            IList<string> connectionIDs = _user.GetFollowerListByConnectID(model.Post.PostedBy, Enums.PageType.Profile, Enums.FriendshipStatus.FA)
                .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

            await _postBroadCaster.Clients.Clients(connectionIDs)
                            .addPost(model.Post, model.Notification);
        }

        /// <summary>
        /// broadcast post to page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task BroadcastDeletePostToWeb(BroadcastPost model)
        {
            IList<string> connectionIDs = _user.GetFollowerListByConnectID(model.Post.PostedBy, Enums.PageType.Profile, Enums.FriendshipStatus.FA)
                .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

            await _postBroadCaster.Clients.Clients(connectionIDs)
                            .deletePost(model.Post);
        }


        /// <summary>
        /// broadcast comment to post
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task BroadcastAddCommentToWeb(BroadcastComment model)
        {
            IList<string> connectionIDs = _user.GetFollowerListByConnectID(model.Comment.UserID, Enums.PageType.Profile, Enums.FriendshipStatus.FA)
                .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

            await _postBroadCaster.Clients.Clients(connectionIDs)
                            .addComment(model.Comment, model.Notification);
        }

        /// <summary>
        /// broadcast like to item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task BroadcastLikeToWeb(BroadcastLike model)
        {
            IList<string> connectionIDs = _user.GetFollowerListByConnectID(model.Like.UserID, Enums.PageType.Profile, Enums.FriendshipStatus.FA)
                .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

            await _postBroadCaster.Clients.Clients(connectionIDs)
                            .Like(model.Like, model.TotalLikes, model.Notification);
        }

        /// <summary>
        /// broadcast like to item
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task BroadcastUnLikeToWeb(BroadcastLike model)
        {
            IList<string> connectionIDs = _user.GetFollowerListByConnectID(model.Like.UserID, Enums.PageType.Profile, Enums.FriendshipStatus.FA)
                .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

            await _postBroadCaster.Clients.Clients(connectionIDs)
                            .UnLike(model.Like, model.TotalLikes);
        }

        /// <summary>
        /// broadcast message to page
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task BroadcastChatMessageToWeb(BroadcastChatMessage model)
        {
            IList<string> connectionIDs = _user.GetFollowerList(model.ChatMessage.FromID, model.ChatMessage.ToID, model.ChatMessage.PageType, Enums.FriendshipStatus.FA)
                .Where(x => x.ConnectedBy == (byte)Enums.BroadCastType.Web).Select(x => x.ConnectionID).ToList();

            await _chatBroadCaster.Clients.Clients(connectionIDs)
                            .sendMessage(model.ChatMessage);
        }

        #endregion

        #region Static Member Function

        #endregion

    }
}