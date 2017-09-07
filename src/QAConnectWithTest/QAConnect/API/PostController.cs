using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DataTypes;
using BusinessLogic;
using QAConnect.Helpers;
using System.Net.Http.Formatting;
using QAConnect.Hubs;
using System.Threading.Tasks;

namespace QAConnect.API
{
    [Authorize]
    public class PostController : BaseController
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


        public PostController()
        {

        }

        #endregion

        #region Private Member Functions

        #endregion

        #region Protected Member function

        #endregion

        #region Public Member Function

        /// <summary>
        /// add post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> AddPost(PostModel post)
        {
            post.PostedBy = CurrentUser.UserID;
            post.PostedDate = DateTime.Now;
            ModelState.Remove("post.PostedBy");
            ModelState.Remove("post.PostedDate");
            post.PostedByName = CurrentUser.FullName;
            post.PostedByAvatar = CurrentUser.ProfilePicture;

            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastPost model = _post.AddPost(post);
                    await _broadcaster.AddPost(model, post.broadCastType);
                    return Ok(model.Post);
                }
                catch (Exception ex)
                {
                    Logs.Error("Error adding post to page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        /// <summary>
        /// edit post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> EditPost(PostModel post)
        {
            post.PostedBy = CurrentUser.UserID;
            post.PostedDate = DateTime.Now;
            ModelState.Remove("post.PostedBy");
            ModelState.Remove("post.PostedDate");
            post.PostedByName = CurrentUser.FullName;
            post.PostedByAvatar = CurrentUser.ProfilePicture;

            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastPost model = _post.AddPost(post);
                    await _broadcaster.AddPost(model, post.broadCastType);
                    return Ok(model.Post);
                }
                catch (Exception ex)
                {
                    Logs.Error("Error adding post to page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        /// <summary>
        /// hide post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> HidePost(PostModel post)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    return Ok(_post.HidePost(post));
                }
                catch (Exception ex)
                {
                    Logs.Error("Error hiding post to page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        /// <summary>
        /// add post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> DeletePost(PostModel post)
        {
            if (post.PostedBy != CurrentUser.UserID)
            {
                ModelState.AddModelError("Unauthorized", "You are not authorized to do that");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastPost model = _post.DeletePost(post);
                    await _broadcaster.DeletePost(model, post.broadCastType);
                    return Ok(model.Post);
                }
                catch (Exception ex)
                {
                    Logs.Error("Error adding post to page", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }



        /// <summary>
        /// get list of posts of a page
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageID"></param>
        public async Task<IHttpActionResult> GetPosts(int PageNumber, int PageSize, int PageType, long PageID, bool? IsHome)
        {
            try
            {
                var _model = _post.GetPosts(!IsHome.Value ? CurrentUser.UserID : -1, PageType, PageID, PageNumber, PageSize);
                return Ok(_model);
            }
            catch (Exception ex)
            {
                Logs.Error("Error getting page posts list", ex);
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///add new comment to post
        /// </summary>
        /// <param name="comment"></param>
        public async Task<IHttpActionResult> AddComment(CommentModel comment)
        {
            comment.UserID = CurrentUser.UserID;
            comment.CreatedOn = DateTime.Now;
            ModelState.Remove("post.PostedBy");
            ModelState.Remove("post.PostedDate");
            comment.PostedByName = CurrentUser.FullName;
            comment.PostedByAvatar = CurrentUser.ProfilePicture;

            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastComment model = _post.AddComment(comment);
                    await _broadcaster.AddComment(model, comment.broadCastType);
                    return Ok(model.Comment);
                }
                catch (Exception ex)
                {
                    Logs.Error("Error adding comment to post", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        /// <summary>
        /// Adding like of item level
        /// </summary>
        /// <param name="likeModel"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> Like(LikeModel like)
        {
            like.UserID = CurrentUser.UserID;
            like.CreatedOn = DateTime.Now;
            ModelState.Remove("post.PostedBy");
            ModelState.Remove("post.PostedDate");
            like.PostedByName = CurrentUser.FullName;
            like.PostedByAvatar = CurrentUser.ProfilePicture;

            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastLike model = _post.Like(like);
                    await _broadcaster.Like(model, like.broadCastType);
                    return Ok(model.TotalLikes);
                }
                catch (Exception ex)
                {
                    Logs.Error("Error adding like to item", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }

        /// <summary>
        /// Deleting like for an item
        /// </summary>
        /// <param name="likeModel"></param>
        /// <returns></returns>
        public async Task<IHttpActionResult> UnLike(LikeModel like)
        {
            like.UserID = CurrentUser.UserID;
            like.CreatedOn = DateTime.Now;
            ModelState.Remove("post.PostedBy");
            ModelState.Remove("post.PostedDate");
            like.PostedByName = CurrentUser.FullName;
            like.PostedByAvatar = CurrentUser.ProfilePicture;

            if (ModelState.IsValid)
            {
                try
                {
                    BroadcastLike model = _post.UnLike(like);
                    await _broadcaster.UnLike(model, like.broadCastType);
                    return Ok(model.TotalLikes);
                }
                catch (Exception ex)
                {
                    Logs.Error("Error removing like to item", ex);
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState.JsonValidation());
            }
        }


        #endregion

        #region Static Member Function

        #endregion
    }
}
