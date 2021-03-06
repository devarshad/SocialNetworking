USE [master]
GO
/****** Object:  Database [C4Connect]    Script Date: 11/16/2016 4:23:28 PM ******/
CREATE DATABASE [C4Connect]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'C4Connect', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\C4Connect.mdf' , SIZE = 3136KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'C4Connect_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\C4Connect_log.ldf' , SIZE = 784KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [C4Connect] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [C4Connect].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [C4Connect] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [C4Connect] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [C4Connect] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [C4Connect] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [C4Connect] SET ARITHABORT OFF 
GO
ALTER DATABASE [C4Connect] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [C4Connect] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [C4Connect] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [C4Connect] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [C4Connect] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [C4Connect] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [C4Connect] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [C4Connect] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [C4Connect] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [C4Connect] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [C4Connect] SET  ENABLE_BROKER 
GO
ALTER DATABASE [C4Connect] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [C4Connect] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [C4Connect] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [C4Connect] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [C4Connect] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [C4Connect] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [C4Connect] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [C4Connect] SET RECOVERY FULL 
GO
ALTER DATABASE [C4Connect] SET  MULTI_USER 
GO
ALTER DATABASE [C4Connect] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [C4Connect] SET DB_CHAINING OFF 
GO
ALTER DATABASE [C4Connect] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [C4Connect] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [C4Connect]
GO
/****** Object:  Schema [arshadkhan]    Script Date: 11/16/2016 4:23:28 PM ******/
CREATE SCHEMA [arshadkhan]
GO
/****** Object:  StoredProcedure [dbo].[GetNotifications]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Mohd. Arshad Khan
-- Create date: 21/11/2015
-- Description:	Get notifications for me
-- =============================================
CREATE PROCEDURE [dbo].[GetNotifications] 
	-- Add the parameters for the stored procedure here
	@UserID BIGINT ,
	@PageSize AS INT,
	@PageNumber AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    --Prepare notifications by fetching for the user.
	;WITH Comments AS
	(
	SELECT
			ROW_NUMBER() OVER(ORDER BY nOTI.ID DESC) AS RowNumber
			,nOTI.[ID]
			,Usr.[ProfilePicture] PostedByAvatar
			,nOTI.[Text]  [Message]
			,nOTI.[ItemID]
			,nOTI.[TypeID]
			,nOTI.[IsRead]

		FROM [dbo].[Notification] AS nOTI
		INNER JOIN [dbo].[AspNetUsers] Usr ON Usr.Id= nOTI.UserID
		WHERE	
		(
			nOTI.[TypeID]=9
			OR
			(
				(	nOTI.Active IS NULL --New post
				OR
					nOTI.Active=1 --Active comment mot deleted
				)
			AND
			(
				--for friends notifications  
				nOTI.UserID IN (
						SELECT 
						CASE WHEN UserID=@UserID THEN FriendID
						ELSE USERID END AS PageID
						FROM [dbo].[Friendship]
						WHERE
							UserID = @UserID OR FriendID = @UserID
						AND
							StatusID=2
					)
			)	
		)
	)
	)
	
	SELECT
	*
	FROM Comments
	WHERE RowNumber BETWEEN (((@PageNumber - 1) * @PageSize) + 1) AND (@PageNumber*@PageSize)
END


GO
/****** Object:  StoredProcedure [dbo].[GetPosts]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Gaurav Singh Jantwal
-- Create date: Novemember 19, 2015
-- Description:	Fetch user posts and comments, as two different tables.
-- =============================================
CREATE PROCEDURE [dbo].[GetPosts] --[GetPosts]  4,3,100,1
	-- Add the parameters for the stored procedure here
	@UserID AS BIGINT,
	@PageID AS BIGINT,
	@PageSize AS INT,
	@PageNumber AS INT
AS
	DECLARE @StatusID AS TINYINT = 0
BEGIN
 SET NOCOUNT ON;
	--Check friendship status only when user is not viewing its own profilee and not in its home page(@userID =null)
	IF @PageID <> @UserID OR @UserID <> -1
	BEGIN
		SELECT @StatusID = StatusID
		FROM [dbo].[Friendship]
		WHERE
			(UserID = @UserID AND FriendID = @PageID)
		OR
		(UserID = @PageID AND FriendID = @UserID)				
	END		
	--Prepare posts by fetching for the user.
	;WITH Posts AS
	(
		SELECT
			ROW_NUMBER() OVER(ORDER BY pST.ID DESC) AS RowNumber
			,pST.[ID] PostId
			,Usr.[UserName]
			,Usr.[FullName] PostedByName
			,Usr.[ProfilePicture] PostedByAvatar
			,pST.[Text]  [Message]
			,pST.[UserID] PostedBy
			,pST.[CreatedOn] PostedDate
			,pST.[Privacy]
			,pST.[Link]
			,pST.[LinkIcon]
			,pST.[LinkHeader]
			,pST.[LinkDescription]
			,pST.[Active]
			,pST.[TypeID] PostType
			,pST.[PostDescription],
			CASE WHEN lIK.UserID=@UserID and pST.ID=lIK.ItemID and lIK.ItemLevel=1 THEN 1 ELSE 0 END AS Liked,
			(SELECT COUNT(ID) from  [Like] where pST.ID=ItemID and ItemLevel=1) TotalLikes

		FROM [dbo].[Post] AS pST
		INNER JOIN [dbo].[AspNetUsers] Usr ON Usr.Id= pST.UserID
		OUTER APPLY
		(
			SELECT TOP 1* from
			[Like]  WHERE pST.ID=ItemID
		 ) lIK
		WHERE	
		(
			(	pST.Active IS NULL --New post
			OR
				pST.Active=1 --Active post mot deleted
				)
			AND
				pST.Privacy!=5 --hidden			
		)	
		AND	
		(			
				pST.UserID=@PageID --for page post
 			OR
				pST.Privacy= CASE  WHEN @UserID =-1 THEN 1 END	--for public post if home page
			OR
				--for friends posts if home page 
				CASE WHEN @UserID =-1 AND pST.UserID IN (
						SELECT 
						CASE WHEN UserID=@PageID THEN FriendID
						ELSE USERID END AS PageID
						FROM [dbo].[Friendship]
						WHERE
							UserID = @PageID OR FriendID = @PageID
						AND
							StatusID=2
					) THEN	1 ELSE 0
					END >0				
 			)
		AND
			(
			ISNULL(@UserID, @PageID)=@PageID  -- Own profile view
					OR
				pST.Privacy = 1 -- Public posts
					OR
				(
					ISNULL(@StatusID, 0) = 2 OR @UserID =-1 -- FriendShip accepted or friend pages filtered above
						AND
					pST.Privacy = 2 -- Posts for friends only
				)
			)
	)
	--Fetch paged posts
	SELECT
		*
	INTO #tempPosts
	FROM Posts
	WHERE RowNumber BETWEEN (((@PageNumber - 1) * @PageSize) + 1) AND (@PageNumber*@PageSize)

	--Fetch all the posts
	SELECT *
	FROM #tempPosts

	--Fetch all the posts comment
	SELECT cMT.[ID] CommentId
      ,cMT.[PostID] PostId
	  ,Usr.[FullName] PostedByName
	  ,Usr.[ProfilePicture] PostedByAvatar
      ,cMT.[Text] [Message]
      ,cMT.[UserID]
      ,cMT.[CreatedOn]
      ,cMT.[Active],
		CASE WHEN lIK.UserID=@UserID and cMT.ID=lIK.ItemID and lIK.ItemLevel=2 THEN 1 ELSE 0 END AS Liked,
		(SELECT COUNT(ItemID) from  [Like] where ID=ItemID and ItemLevel=2) TotalLikes
	FROM [dbo].[Comment] AS cMT
	INNER JOIN [dbo].[AspNetUsers] Usr ON Usr.Id= cMT.UserID
	OUTER APPLY
		(
			SELECT TOP 1* from
			[Like]  WHERE cMT.ID=ItemID
		 ) lIK
	INNER JOIN #tempPosts AS tP ON tP.PostId = cMT.PostID

	DROP TABLE #tempPosts
END

GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [bigint] NOT NULL,
	[RoleId] [bigint] NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[Skype] [varchar](50) NULL,
	[FullName] [nvarchar](max) NULL,
	[BirthDate] [datetime] NULL,
	[Gender] [nvarchar](max) NULL,
	[ProfilePicture] [nvarchar](max) NULL,
	[CoverPicture] [nvarchar](max) NULL,
	[LastStatusOn] [datetime] NULL,
	[StatusID] [tinyint] NULL,
	[CreatedOn] [datetime] NULL,
	[Active] [bit] NULL,
	[Privacy] [smallint] NULL,
	[Rating] [smallint] NULL,
	[NickName] [varchar](50) NULL,
	[Organization] [varchar](100) NULL,
	[OrganizationLocation] [varchar](100) NULL,
	[PositionInOrganization] [varchar](100) NULL,
	[OrganizationJoinDate] [datetime] NULL,
	[Address] [varchar](100) NULL,
	[City] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[PostalCode] [int] NULL,
	[AboutMe] [varchar](300) NULL,
	[Relationship] [varchar](50) NULL,
	[DesireToMeet] [varchar](200) NULL,
	[TimeZone] [varchar](50) NULL,
	[Language] [smallint] NULL,
	[Interest] [varchar](200) NULL,
	[Hobbies] [varchar](200) NULL,
	[Education] [varchar](200) NULL,
	[Religion] [smallint] NULL,
	[FavAnimals] [varchar](200) NULL,
	[FavBooks] [varchar](200) NULL,
	[FavMusics] [varchar](200) NULL,
	[FavMovie] [varchar](200) NULL,
	[favArtist] [varchar](200) NULL,
	[Smoker] [smallint] NULL,
	[Drinker] [smallint] NULL,
	[LastUpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Comment](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[PostID] [bigint] NOT NULL,
	[Text] [varchar](max) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ConnectedUsers]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ConnectedUsers](
	[UserID] [bigint] NOT NULL,
	[ConnectionID] [varchar](128) NULL,
	[ConnectedBy] [tinyint] NULL,
	[StatusID] [tinyint] NULL,
	[ConnectedOn] [datetime] NULL,
 CONSTRAINT [PK_ConnectedUser] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Event]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Event](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[FullName] [varchar](200) NULL,
	[Description] [varchar](1000) NULL,
	[Icon] [varchar](200) NULL,
	[Wallpaper] [varchar](200) NULL,
	[Owner1] [bigint] NULL,
	[Owner2] [bigint] NULL,
	[PrivacyID] [tinyint] NULL,
	[CreatedOn] [datetime] NULL,
	[Active] [bit] NULL,
	[UserID] [bigint] NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EventPost]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventPost](
	[PostID] [bigint] NOT NULL,
	[EventID] [bigint] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EventUser]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventUser](
	[EventID] [bigint] NOT NULL,
	[UserID] [bigint] NULL,
	[CreatedOn] [datetime] NULL,
	[StatusID] [tinyint] NULL,
	[Active] [bit] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Friendship]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Friendship](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[FriendID] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL CONSTRAINT [DF_Friendship_CreatedOn]  DEFAULT (getdate()),
	[StatusID] [tinyint] NOT NULL,
	[PageTypeID] [tinyint] NOT NULL,
	[IsRead] [bit] NULL CONSTRAINT [DF_Friendship_IsRead]  DEFAULT ((0)),
 CONSTRAINT [PK_tblFriendShip] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FriendshipStatus]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FriendshipStatus](
	[ID] [tinyint] NOT NULL,
	[Name] [varchar](100) NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_FriendshipStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Group]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Group](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[FullName] [varchar](200) NULL,
	[Description] [varchar](1000) NULL,
	[Icon] [varchar](200) NULL,
	[Wallpaper] [varchar](200) NULL,
	[Owner1] [bigint] NULL,
	[Owner2] [bigint] NULL,
	[PrivacyID] [tinyint] NULL,
	[CreatedOn] [datetime] NULL,
	[Active] [bit] NULL,
	[UserID] [bigint] NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GroupMessage]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupMessage](
	[GroupID] [bigint] NULL,
	[MessageID] [bigint] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GroupPost]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupPost](
	[PostID] [bigint] NOT NULL,
	[GroupID] [bigint] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[GroupUser]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupUser](
	[GroupID] [bigint] NOT NULL,
	[UserID] [bigint] NULL,
	[Active] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[StatusID] [tinyint] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Like]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Like](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[ItemLevel] [tinyint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_Like] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Message]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Message](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SenderID] [bigint] NOT NULL,
	[RecieverID] [bigint] NOT NULL,
	[Text] [nvarchar](1000) NOT NULL,
	[IsRead] [bit] NULL CONSTRAINT [DF_Message_IsRead]  DEFAULT ((0)),
	[CreatedOn] [datetime] NULL CONSTRAINT [DF_Message_CreatedOn]  DEFAULT (getdate()),
	[PageTypeID] [tinyint] NULL,
	[Active] [bit] NULL CONSTRAINT [DF_Message_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_tblMessages] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Notification]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Notification](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TypeID] [tinyint] NULL,
	[UserID] [bigint] NULL,
	[ItemID] [bigint] NULL,
	[Text] [varchar](500) NULL,
	[CreatedOn] [datetime] NULL CONSTRAINT [DF_Notification_CreatedOn]  DEFAULT (getdate()),
	[IsRead] [bit] NULL CONSTRAINT [DF_Notfication_IsRead]  DEFAULT ((0)),
	[Active] [bit] NULL CONSTRAINT [DF_Notfication_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_Notfication] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[NotificationType]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[NotificationType](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Description] [varchar](500) NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_NotificationType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PagePrivacy]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PagePrivacy](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](20) NULL,
	[Description] [varchar](200) NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_PagePrivacy] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Post]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Post](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [varchar](max) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Privacy] [int] NULL,
	[Link] [varchar](200) NULL,
	[LinkIcon] [varchar](200) NULL,
	[LinkHeader] [varchar](500) NULL,
	[LinkDescription] [varchar](1000) NULL,
	[Active] [bit] NULL,
	[TypeID] [tinyint] NULL,
	[PostDescription] [varchar](50) NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PostType]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PostType](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Active] [bit] NULL CONSTRAINT [DF_PostType_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_PostType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Report]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Report](
	[ID] [bigint] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[ItemLevel] [smallint] NULL,
	[Reason] [smallint] NOT NULL,
	[UserComment] [varchar](200) NULL,
	[Processed] [bit] NULL,
	[AdminComment] [varchar](200) NULL,
	[CreatedOn] [datetime] NULL,
 CONSTRAINT [PK_tblReport] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserStatus]    Script Date: 11/16/2016 4:23:28 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserStatus](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Active] [bit] NULL CONSTRAINT [DF_UserStatus_Active]  DEFAULT ((1)),
 CONSTRAINT [PK_UserStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[Event] ADD  CONSTRAINT [DF_Event_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Group] ADD  CONSTRAINT [DF_Group_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUsers_UserStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[UserStatus] ([ID])
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_AspNetUsers_UserStatus]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Post] FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([ID])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Post]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_Users]
GO
ALTER TABLE [dbo].[ConnectedUsers]  WITH CHECK ADD  CONSTRAINT [FK_ConnectedUser_AspNetUsers] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[ConnectedUsers] CHECK CONSTRAINT [FK_ConnectedUser_AspNetUsers]
GO
ALTER TABLE [dbo].[ConnectedUsers]  WITH CHECK ADD  CONSTRAINT [FK_ConnectedUser_UserStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[UserStatus] ([ID])
GO
ALTER TABLE [dbo].[ConnectedUsers] CHECK CONSTRAINT [FK_ConnectedUser_UserStatus]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Event] FOREIGN KEY([PrivacyID])
REFERENCES [dbo].[PagePrivacy] ([ID])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Event]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Users]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Users_Owner1] FOREIGN KEY([Owner1])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Users_Owner1]
GO
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Users_Owner2] FOREIGN KEY([Owner2])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Users_Owner2]
GO
ALTER TABLE [dbo].[EventPost]  WITH CHECK ADD  CONSTRAINT [FK_EventPost_Event] FOREIGN KEY([EventID])
REFERENCES [dbo].[Event] ([ID])
GO
ALTER TABLE [dbo].[EventPost] CHECK CONSTRAINT [FK_EventPost_Event]
GO
ALTER TABLE [dbo].[EventPost]  WITH CHECK ADD  CONSTRAINT [FK_EventPost_Post] FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([ID])
GO
ALTER TABLE [dbo].[EventPost] CHECK CONSTRAINT [FK_EventPost_Post]
GO
ALTER TABLE [dbo].[EventUser]  WITH CHECK ADD  CONSTRAINT [FK_EventUser_Event] FOREIGN KEY([EventID])
REFERENCES [dbo].[Event] ([ID])
GO
ALTER TABLE [dbo].[EventUser] CHECK CONSTRAINT [FK_EventUser_Event]
GO
ALTER TABLE [dbo].[EventUser]  WITH CHECK ADD  CONSTRAINT [FK_EventUser_FriendshipStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[FriendshipStatus] ([ID])
GO
ALTER TABLE [dbo].[EventUser] CHECK CONSTRAINT [FK_EventUser_FriendshipStatus]
GO
ALTER TABLE [dbo].[EventUser]  WITH CHECK ADD  CONSTRAINT [FK_EventUser_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[EventUser] CHECK CONSTRAINT [FK_EventUser_Users]
GO
ALTER TABLE [dbo].[Friendship]  WITH CHECK ADD  CONSTRAINT [FK_Friendship_FriendshipStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[FriendshipStatus] ([ID])
GO
ALTER TABLE [dbo].[Friendship] CHECK CONSTRAINT [FK_Friendship_FriendshipStatus]
GO
ALTER TABLE [dbo].[Friendship]  WITH CHECK ADD  CONSTRAINT [FK_Friendship_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Friendship] CHECK CONSTRAINT [FK_Friendship_Users]
GO
ALTER TABLE [dbo].[Friendship]  WITH CHECK ADD  CONSTRAINT [FK_Friendship_Users_Friend] FOREIGN KEY([FriendID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Friendship] CHECK CONSTRAINT [FK_Friendship_Users_Friend]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Group_Group] FOREIGN KEY([PrivacyID])
REFERENCES [dbo].[PagePrivacy] ([ID])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_Group]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Group_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_Users]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Group_Users_Owner1] FOREIGN KEY([Owner1])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_Users_Owner1]
GO
ALTER TABLE [dbo].[Group]  WITH CHECK ADD  CONSTRAINT [FK_Group_Users_Owner2] FOREIGN KEY([Owner2])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Group] CHECK CONSTRAINT [FK_Group_Users_Owner2]
GO
ALTER TABLE [dbo].[GroupMessage]  WITH CHECK ADD  CONSTRAINT [FK_GroupMessage_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[GroupMessage] CHECK CONSTRAINT [FK_GroupMessage_Group]
GO
ALTER TABLE [dbo].[GroupMessage]  WITH CHECK ADD  CONSTRAINT [FK_GroupMessage_Message] FOREIGN KEY([MessageID])
REFERENCES [dbo].[Message] ([ID])
GO
ALTER TABLE [dbo].[GroupMessage] CHECK CONSTRAINT [FK_GroupMessage_Message]
GO
ALTER TABLE [dbo].[GroupPost]  WITH CHECK ADD  CONSTRAINT [FK_GroupPost_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[GroupPost] CHECK CONSTRAINT [FK_GroupPost_Group]
GO
ALTER TABLE [dbo].[GroupPost]  WITH CHECK ADD  CONSTRAINT [FK_GroupPost_Post] FOREIGN KEY([PostID])
REFERENCES [dbo].[Post] ([ID])
GO
ALTER TABLE [dbo].[GroupPost] CHECK CONSTRAINT [FK_GroupPost_Post]
GO
ALTER TABLE [dbo].[GroupUser]  WITH CHECK ADD  CONSTRAINT [FK_GroupUser_FriendshipStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[FriendshipStatus] ([ID])
GO
ALTER TABLE [dbo].[GroupUser] CHECK CONSTRAINT [FK_GroupUser_FriendshipStatus]
GO
ALTER TABLE [dbo].[GroupUser]  WITH CHECK ADD  CONSTRAINT [FK_GroupUser_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[GroupUser] CHECK CONSTRAINT [FK_GroupUser_Group]
GO
ALTER TABLE [dbo].[GroupUser]  WITH CHECK ADD  CONSTRAINT [FK_GroupUser_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[GroupUser] CHECK CONSTRAINT [FK_GroupUser_Users]
GO
ALTER TABLE [dbo].[Like]  WITH CHECK ADD  CONSTRAINT [FK_Like_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Like] CHECK CONSTRAINT [FK_Like_Users]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_AspNetUsers_Reciever] FOREIGN KEY([RecieverID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_AspNetUsers_Reciever]
GO
ALTER TABLE [dbo].[Message]  WITH CHECK ADD  CONSTRAINT [FK_Message_AspNetUsers_Sender] FOREIGN KEY([SenderID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Message] CHECK CONSTRAINT [FK_Message_AspNetUsers_Sender]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notfication_NotificationType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[NotificationType] ([ID])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notfication_NotificationType]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [FK_Notification_AspNetUsers] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [FK_Notification_AspNetUsers]
GO
ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_Post_AspNetUsers] FOREIGN KEY([UserID])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Post] CHECK CONSTRAINT [FK_Post_AspNetUsers]
GO
ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_Posts_PostType] FOREIGN KEY([TypeID])
REFERENCES [dbo].[PostType] ([ID])
GO
ALTER TABLE [dbo].[Post] CHECK CONSTRAINT [FK_Posts_PostType]
GO
USE [master]
GO
ALTER DATABASE [C4Connect] SET  READ_WRITE 
GO
