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
    public class PostAccess
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
        public PostAccess()
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
        /// Get posts of a page
        /// </summary>
        /// <param name="PageNumber"></param>
        public dynamic GetPosts(long UserID, int PageType, long PageID, int PageNumber, int PageSize)
        {
            switch (PageType)
            {
                case 1:
                    return _dr.GetPosts(UserID, PageID, PageNumber, PageSize);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Add post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public BroadcastPost AddPost(PostModel post)
        {
            return _dr.AddPost(post);
        }


        /// <summary>
        /// Add post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public BroadcastPost EditPost(PostModel post)
        {
            return _dr.EditPost(post);
        }

        /// <summary>
        /// Add post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public BroadcastPost SharePost(PostModel post)
        {
            return _dr.SharePost(post);
        }


        /// <summary>
        /// Add post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public BroadcastPost ReportPost(PostModel post)
        {
            return _dr.ReportPost(post);
        }

        /// <summary>
        /// Add post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public bool HidePost(PostModel post)
        {
            return _dr.HidePost(post);
        }

        /// <summary>
        /// Add post to page
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public BroadcastPost DeletePost(PostModel post)
        {
            return _dr.DeletePost(post);
        }

        /// <summary>
        /// add comment to post
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public dynamic AddComment(CommentModel comment)
        {
            return _dr.AddComment(comment);
        }

        /// <summary>
        /// Adding like of item level
        /// </summary>
        /// <param name="likeModel"></param>
        /// <returns></returns>
        public BroadcastLike Like(LikeModel likeModel)
        {
            return _dr.Like(likeModel);
        }

        /// <summary>
        /// Deleting like for an item
        /// </summary>
        /// <param name="likeModel"></param>
        /// <returns></returns>
        public BroadcastLike UnLike(LikeModel likeModel)
        {
            return _dr.UnLike(likeModel);
        }

        #endregion

        #region Static Member Function

        #endregion
    }
}
