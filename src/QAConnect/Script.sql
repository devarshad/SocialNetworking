USE [master]
GO
/****** Object:  Database [QAConnect]    Script Date: 9/7/2017 10:16:49 AM ******/
CREATE DATABASE [QAConnect]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QAConnect', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\QAConnect.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'QAConnect_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.SQLEXPRESS\MSSQL\DATA\QAConnect_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [QAConnect] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QAConnect].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QAConnect] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QAConnect] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QAConnect] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QAConnect] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QAConnect] SET ARITHABORT OFF 
GO
ALTER DATABASE [QAConnect] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QAConnect] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [QAConnect] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QAConnect] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QAConnect] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QAConnect] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QAConnect] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QAConnect] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QAConnect] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QAConnect] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QAConnect] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QAConnect] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QAConnect] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QAConnect] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QAConnect] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QAConnect] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QAConnect] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QAConnect] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QAConnect] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [QAConnect] SET  MULTI_USER 
GO
ALTER DATABASE [QAConnect] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QAConnect] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QAConnect] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QAConnect] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [QAConnect]
GO
/****** Object:  StoredProcedure [dbo].[GetEvents]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetEvents]
	-- Add the parameters for the stored procedure here
	@UserID BIGINT,
	@UserName VARCHAR(50),
	@PageNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @PageUserID AS BIGINT

	SELECT @PageUserID=Id FROM AspNetUsers WHERE UserName=@UserName

	SELECT e.Id,e.Name,e.FullName,e.Icon,eu.StatusID FROM [Event] e 
	JOIN EventUser eu ON e.ID=eu.EventID
	WHERE eu.UserID=@PageUserID AND (eu.StatusID=1 OR eu.StatusID=3) AND (e.Active IS NULL OR e.Active=0)
	ORDER BY e.CreatedOn DESC 
	OFFSET (@PageNumber-1) ROWS FETCH NEXT 25 ROWS ONLY
END

GO
/****** Object:  StoredProcedure [dbo].[GetFriends]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetFriends]
	-- Add the parameters for the stored procedure here
	@UserID BIGINT,
	@UserName VARCHAR(50),
	@PageNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	DECLARE @PageUserID AS BIGINT

	SELECT @PageUserID=Id FROM AspNetUsers WHERE UserName=@UserName

	 SELECT CASE WHEN f.UserID=@PageUserID THEN u1.Id ELSE u2.Id END AS Id,
		 CASE WHEN f.UserID=@PageUserID THEN u1.UserName ELSE u2.UserName END AS UserName,
		 CASE WHEN f.UserID=@PageUserID THEN u1.FullName ELSE u2.FullName END AS Name,
		 CASE WHEN f.UserID=@PageUserID THEN u1.ProfilePicture ELSE u2.ProfilePicture END AS LinkIcon,
		 CASE WHEN f.UserID=@PageUserID THEN u1.PositionInOrganization ELSE u2.PositionInOrganization END AS Designation,
		 CASE WHEN f.UserID=@PageUserID THEN u1.StatusID ELSE u2.StatusID END AS StatusID
	 FROM Friendship f JOIN AspNetUsers u1 ON f.UserID= u1.ID
	 JOIN AspNetUsers u2 ON f.FriendID= u2.ID
	  WHERE (f.UserID=@PageUserID OR f.FriendID=@PageUserID)
	 AND f.[StatusID]=2 ORDER BY f.CreatedOn DESC 
	 OFFSET (@PageNumber-1) ROWS FETCH NEXT 25 ROWS ONLY;
END

GO
/****** Object:  StoredProcedure [dbo].[GetGroups]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetGroups]
	-- Add the parameters for the stored procedure here
	@UserID BIGINT,
	@UserName VARCHAR(50),
	@PageNumber INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @PageUserID AS BIGINT

	SELECT @PageUserID=Id FROM AspNetUsers WHERE UserName=@UserName

	SELECT g.Id,g.Name,g.FullName,g.Icon,gu.StatusID FROM [Group] g 
	JOIN GroupUser gu ON g.ID=gu.GroupID
	WHERE gu.UserID=@PageUserID AND (gu.StatusID=1 OR gu.StatusID=3) AND (g.Active IS NULL OR g.Active=0 )
	ORDER BY g.CreatedOn DESC 
	OFFSET (@PageNumber-1) ROWS FETCH NEXT 25 ROWS ONLY
END

GO
/****** Object:  StoredProcedure [dbo].[GetMusic]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetMusic]
	-- Add the parameters for the stored procedure here
	@UserID BIGINT,
	@PageName VARCHAR(50),
	@PageNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	
	DECLARE @PageUserID AS BIGINT

	SELECT @PageUserID=Id FROM AspNetUsers WHERE UserName=@PageName

	 SELECT p.ID,u.UserName,p.[Text],p.LinkIcon,p.Link
	 FROM Post p JOIN AspNetUsers u ON p.UserID= u.ID
	 WHERE (p.UserID=@PageUserID AND (p.Active IS NULL OR p.Active=1) AND p.TypeID=5)
	 ORDER BY p.CreatedOn DESC 
	 OFFSET (@PageNumber-1) ROWS FETCH NEXT 25 ROWS ONLY;
END

GO
/****** Object:  StoredProcedure [dbo].[GetPhotos]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPhotos]  --[dbo].[GetPhotos] 2,'arshadkhan',1
	-- Add the parameters for the stored procedure here
	@UserID BIGINT,
	@PageName VARCHAR(50),
	@PageNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @PageUserID AS BIGINT

	SELECT @PageUserID=Id FROM AspNetUsers WHERE UserName=@PageName

	 SELECT p.ID,u.UserName,p.[Text],p.LinkIcon,p.Link
	 FROM Post p JOIN AspNetUsers u ON p.UserID= u.ID
	  WHERE (p.UserID=@PageUserID AND (p.Active IS NULL OR p.Active=1) AND p.TypeID=2)
	 ORDER BY p.CreatedOn DESC 
	 OFFSET (@PageNumber-1) ROWS FETCH NEXT 25 ROWS ONLY;
END

GO
/****** Object:  StoredProcedure [dbo].[GetPosts]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPosts] --[dbo].[GetPosts] 2, 2, 1
	-- Add the parameters for the stored procedure here
	@UserID AS BIGINT,
	@PageID AS BIGINT,
	@PageNumber AS INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here

	--Prepare posts by fetching for the user.
	;WITH Posts AS
	(
		SELECT
			pST.[ID] PostId
			,Usr.[UserName] AS PageName
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
			,pST.[PostDescription]
			,CAST(CASE WHEN lIK.UserID=@UserID and pST.ID=lIK.ItemID and lIK.ItemLevel=1 THEN 1 ELSE 0 END AS BIT) AS Liked,
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
		--AND	
		--(			
		--		pST.UserID=@PageID --for page post
 	--		OR
		--		pST.Privacy= CASE  WHEN @UserID =-1 THEN 1 END	--for public post if home page
		--)
		ORDER BY pST.CreatedOn DESC
		OFFSET (@PageNumber-1) ROWS FETCH NEXT 25 ROWS ONLY
	)

	SELECT
		*
	INTO #tempPosts
	FROM Posts

	SELECT
		* FROM #tempPosts

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
/****** Object:  StoredProcedure [dbo].[GetProfile]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetProfile]
	-- Add the parameters for the stored procedure here
	@UserID BIGINT,
	@UserName VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @IsOwner AS BIT =0
	DECLARE @PageUserID AS BIGINT

	SELECT @PageUserID=Id FROM AspNetUsers WHERE UserName=@UserName

	IF(@PageUserID=@UserID)
	BEGIN
		SET @IsOwner=1
	END

    -- Insert statements for procedure here	
	SELECT @IsOwner AS [Owner],ID AS UserID,UserName,FullName,Email,ProfilePicture,
	  CoverPicture,PhoneNumber,[Address]
	 ,BirthDate AS DateOfBirth,PositionInOrganization AS Designation,LivesIn,BloodGroup,Skype,Gender,StatusID
	 ,Rating,Relationship,AboutMe,Portfolio FROM AspNetUsers WHERE ID=@PageUserID

END

GO
/****** Object:  StoredProcedure [dbo].[GetProfileDetail]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetProfileDetail] --[dbo].[GetProfileDetail] 2, 'arshadkhan'
	-- Add the parameters for the stored procedure here	
	@UserID BIGINT,
	@UserName VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	
	 EXEC [dbo].[GetProfile] @UserID,@UserName

	 EXEC [dbo].[GetFriends] @UserID,@UserName,1

	 EXEC [dbo].[GetPhotos] @UserID,@UserName,1

	 EXEC [dbo].[GetMusic] @UserID,@UserName,1
	 
	 EXEC [dbo].[GetVideos] @UserID,@UserName,1

	 EXEC [dbo].[GetGroups] @UserID,@UserName,1

	 EXEC [dbo].[GetEvents] @UserID,@UserName,1
END

GO
/****** Object:  StoredProcedure [dbo].[GetVideos]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetVideos]
	-- Add the parameters for the stored procedure here
	@UserID BIGINT,
	@PageName VARCHAR(50),
	@PageNumber BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @PageUserID AS BIGINT

	SELECT @PageUserID=Id FROM AspNetUsers WHERE UserName=@PageName

	 SELECT p.ID,u.UserName,p.[Text],p.LinkIcon,p.Link
	 FROM Post p JOIN AspNetUsers u ON p.UserID= u.ID
	 WHERE (p.UserID=@PageUserID AND (p.Active IS NULL OR p.Active=1) AND p.TypeID=6)
	 ORDER BY p.CreatedOn DESC 
	 OFFSET (@PageNumber-1) ROWS FETCH NEXT 25 ROWS ONLY;
END

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 9/7/2017 10:16:49 AM ******/
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
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 9/7/2017 10:16:49 AM ******/
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
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 9/7/2017 10:16:49 AM ******/
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
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 9/7/2017 10:16:49 AM ******/
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
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 9/7/2017 10:16:49 AM ******/
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
	[BloodGroup] [varchar](10) NULL,
	[PortFolio] [varchar](100) NULL,
	[LivesIn] [varchar](200) NULL,
	[LastUpdatedOn] [datetime] NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 9/7/2017 10:16:49 AM ******/
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
/****** Object:  Table [dbo].[Event]    Script Date: 9/7/2017 10:16:49 AM ******/
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
	[PrivacyID] [tinyint] NULL,
	[CreatedOn] [datetime] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_Event] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EventUser]    Script Date: 9/7/2017 10:16:49 AM ******/
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
/****** Object:  Table [dbo].[Friendship]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Friendship](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[FriendID] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[StatusID] [tinyint] NOT NULL,
	[PageTypeID] [tinyint] NOT NULL,
	[IsRead] [bit] NULL,
 CONSTRAINT [PK_tblFriendShip] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FriendshipStatus]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FriendshipStatus](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
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
/****** Object:  Table [dbo].[Group]    Script Date: 9/7/2017 10:16:49 AM ******/
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
	[PrivacyID] [tinyint] NULL,
	[CreatedOn] [datetime] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GroupUser]    Script Date: 9/7/2017 10:16:49 AM ******/
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
/****** Object:  Table [dbo].[GroupUserStatus]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupUserStatus](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_GroupUserStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Like]    Script Date: 9/7/2017 10:16:49 AM ******/
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
/****** Object:  Table [dbo].[Page]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Page](
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
	[CreatedBy] [bigint] NULL,
 CONSTRAINT [PK_Page] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PagePrivacy]    Script Date: 9/7/2017 10:16:49 AM ******/
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
/****** Object:  Table [dbo].[PageType]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PageType](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Active] [bigint] NULL,
 CONSTRAINT [PK_PageType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PageUserStatus]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PageUserStatus](
	[ID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_PageUserStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Post]    Script Date: 9/7/2017 10:16:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Post](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Text] [varchar](max) NOT NULL,
	[PostDescription] [varchar](50) NULL,
	[TypeID] [tinyint] NULL,
	[PageID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[Privacy] [int] NULL,
	[Link] [varchar](200) NULL,
	[LinkIcon] [varchar](200) NULL,
	[LinkHeader] [varchar](500) NULL,
	[LinkDescription] [varchar](1000) NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PostType]    Script Date: 9/7/2017 10:16:49 AM ******/
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
/****** Object:  Table [dbo].[UserStatus]    Script Date: 9/7/2017 10:16:49 AM ******/
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
ALTER TABLE [dbo].[Friendship] ADD  CONSTRAINT [DF_Friendship_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Friendship] ADD  CONSTRAINT [DF_Friendship_IsRead]  DEFAULT ((0)) FOR [IsRead]
GO
ALTER TABLE [dbo].[Group] ADD  CONSTRAINT [DF_Group_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Page] ADD  CONSTRAINT [DF_Page_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
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
ALTER TABLE [dbo].[Event]  WITH CHECK ADD  CONSTRAINT [FK_Event_Event] FOREIGN KEY([PrivacyID])
REFERENCES [dbo].[PagePrivacy] ([ID])
GO
ALTER TABLE [dbo].[Event] CHECK CONSTRAINT [FK_Event_Event]
GO
ALTER TABLE [dbo].[EventUser]  WITH CHECK ADD  CONSTRAINT [FK_EventUser_Event] FOREIGN KEY([EventID])
REFERENCES [dbo].[Event] ([ID])
GO
ALTER TABLE [dbo].[EventUser] CHECK CONSTRAINT [FK_EventUser_Event]
GO
ALTER TABLE [dbo].[EventUser]  WITH CHECK ADD  CONSTRAINT [FK_EventUser_PageUserStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[PageUserStatus] ([ID])
GO
ALTER TABLE [dbo].[EventUser] CHECK CONSTRAINT [FK_EventUser_PageUserStatus]
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
ALTER TABLE [dbo].[GroupUser]  WITH CHECK ADD  CONSTRAINT [FK_GroupUser_Group] FOREIGN KEY([GroupID])
REFERENCES [dbo].[Group] ([ID])
GO
ALTER TABLE [dbo].[GroupUser] CHECK CONSTRAINT [FK_GroupUser_Group]
GO
ALTER TABLE [dbo].[GroupUser]  WITH CHECK ADD  CONSTRAINT [FK_GroupUser_PageUserStatus] FOREIGN KEY([StatusID])
REFERENCES [dbo].[PageUserStatus] ([ID])
GO
ALTER TABLE [dbo].[GroupUser] CHECK CONSTRAINT [FK_GroupUser_PageUserStatus]
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
ALTER TABLE [dbo].[Page]  WITH CHECK ADD  CONSTRAINT [FK_Page_Page] FOREIGN KEY([PrivacyID])
REFERENCES [dbo].[PagePrivacy] ([ID])
GO
ALTER TABLE [dbo].[Page] CHECK CONSTRAINT [FK_Page_Page]
GO
ALTER TABLE [dbo].[Page]  WITH CHECK ADD  CONSTRAINT [FK_Page_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Page] CHECK CONSTRAINT [FK_Page_Users]
GO
ALTER TABLE [dbo].[Page]  WITH CHECK ADD  CONSTRAINT [FK_Page_Users_Owner1] FOREIGN KEY([Owner1])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Page] CHECK CONSTRAINT [FK_Page_Users_Owner1]
GO
ALTER TABLE [dbo].[Page]  WITH CHECK ADD  CONSTRAINT [FK_Page_Users_Owner2] FOREIGN KEY([Owner2])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[Page] CHECK CONSTRAINT [FK_Page_Users_Owner2]
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
ALTER DATABASE [QAConnect] SET  READ_WRITE 
GO
