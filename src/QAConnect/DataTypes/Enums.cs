using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTypes
{
    public static class Enums
    {
        /// <summary>
        /// Enum to hold user status
        /// </summary>
        public enum UserStatus : byte
        {
            /// <summary>
            /// Online
            /// </summary>        
            ONL = 1,
            /// <summary>
            /// Offline
            /// </summary>
            OFL,
            /// <summary>
            /// Busy
            /// </summary>
            BSY,
            /// <summary>
            /// DO not disturb
            /// </summary>
            DND
        }

        /// <summary>
        /// Friend ship status
        /// </summary>
        public enum FriendshipStatus : byte
        {
            /// <summary>
            /// Friend Request send
            /// </summary>
            FS = 1,
            /// <summary>
            /// Friend Added
            /// </summary>
            FA,
            /// <summary>
            /// Friend Rejected
            /// </summary>
            FR,
            /// <summary>
            /// Friend Request Cancel
            /// </summary>
            FC
        }
        /// <summary>
        /// Defines the type of a alert/message.
        /// </summary>
        public enum MessageType : byte
        {
            /// <summary>
            /// Indicates error occured while in the execution in the program.
            /// In red color.
            /// </summary>
            Error,
            /// <summary>
            /// Indicates information is displayed to the user.
            /// In blue color.
            /// </summary>
            Information,
            /// <summary>
            /// Indicates a task completion success.
            /// In green color.
            /// </summary>
            Success,
            /// <summary>
            /// Indicates a system or program warning.
            /// In orange color.
            /// </summary>
            Warning
        }

        /// <summary>
        /// Define the type of Broadcasting messages to different clients
        /// </summary>
        public enum BroadCastType : byte
        {
            /// <summary>
            /// Web based client like SignalR
            /// </summary>
            Web = 1,

            /// <summary>
            /// App based client like mobile devices which supports notification services
            /// </summary>
            App
        }

        /// <summary>
        /// Page type
        /// </summary>
        public enum PageType : byte
        {
            /// <summary>
            /// User profile detail
            /// </summary>
            Profile = 1,

            /// <summary>
            /// Group detail
            /// </summary>
            Group,

            /// <summary>
            /// Event detail
            /// </summary>
            Event
        }

    }
}