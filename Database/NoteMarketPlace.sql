USE [master]
GO
/****** Object:  Database [NoteMarketPlace]    Script Date: 19-04-2021 1.10.08 PM ******/
CREATE DATABASE [NoteMarketPlace]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'NoteMarketPlace', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\NoteMarketPlace.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'NoteMarketPlace_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\NoteMarketPlace_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [NoteMarketPlace] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [NoteMarketPlace].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ARITHABORT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [NoteMarketPlace] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [NoteMarketPlace] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET  DISABLE_BROKER 
GO
ALTER DATABASE [NoteMarketPlace] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [NoteMarketPlace] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET RECOVERY FULL 
GO
ALTER DATABASE [NoteMarketPlace] SET  MULTI_USER 
GO
ALTER DATABASE [NoteMarketPlace] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [NoteMarketPlace] SET DB_CHAINING OFF 
GO
ALTER DATABASE [NoteMarketPlace] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [NoteMarketPlace] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [NoteMarketPlace] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'NoteMarketPlace', N'ON'
GO
ALTER DATABASE [NoteMarketPlace] SET QUERY_STORE = OFF
GO
USE [NoteMarketPlace]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[CountryCode] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Downloads]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Downloads](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[Seller] [int] NOT NULL,
	[Downloader] [int] NOT NULL,
	[IsSellerHasAllowedDownload] [bit] NOT NULL,
	[AttachmentPath] [varchar](max) NULL,
	[IsAttachementDownloaded] [bit] NOT NULL,
	[AttacmentDownloadedDate] [datetime] NULL,
	[IsPaid] [bit] NOT NULL,
	[PurchasedPrice] [decimal](18, 0) NULL,
	[NoteTitle] [varchar](100) NOT NULL,
	[NoteCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_Downloads] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteCategories]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteCategories](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteCategories] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NoteTypes]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NoteTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_NoteTypes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReferenceData]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReferenceData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](100) NOT NULL,
	[DataValue] [varchar](100) NOT NULL,
	[RefCategory] [varchar](100) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_ReferenceData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotes]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SellerID] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[ActionedBy] [int] NULL,
	[AdminRemarks] [varchar](max) NULL,
	[PubilshedDate] [datetime] NULL,
	[Title] [varchar](100) NOT NULL,
	[Category] [int] NOT NULL,
	[DisplayPicture] [varchar](500) NULL,
	[NoteType] [int] NULL,
	[NumberofPages] [int] NULL,
	[Description] [varchar](max) NOT NULL,
	[UniversityName] [varchar](200) NULL,
	[Country] [int] NULL,
	[Course] [varchar](100) NULL,
	[CourseCode] [varchar](100) NULL,
	[Professor] [varchar](100) NULL,
	[IsPaid] [bit] NOT NULL,
	[SellingPrice] [decimal](18, 0) NULL,
	[NotesPreview] [nvarchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotes] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesAttachements]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesAttachements](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[FileName] [varchar](100) NOT NULL,
	[FilePath] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ISActive] [bit] NOT NULL,
	[AttachementSize] [bigint] NULL,
 CONSTRAINT [PK_SellerNotesAttachements] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReportedIssues]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReportedIssues](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReportedByID] [int] NOT NULL,
	[AgainstDownloadID] [int] NOT NULL,
	[Remarks] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
 CONSTRAINT [PK_SellerNotesReportedIssues] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SellerNotesReviews]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SellerNotesReviews](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[NoteID] [int] NOT NULL,
	[ReviewedBy] [int] NOT NULL,
	[AgainstDownloadsID] [int] NOT NULL,
	[Ratings] [decimal](18, 0) NOT NULL,
	[Comments] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[Isactive] [bit] NOT NULL,
 CONSTRAINT [PK_SellerNotesReviews] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SystemConfigurations]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SystemConfigurations](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Key] [varchar](max) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SystemConfigurations] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserProfile]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProfile](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[DOB] [datetime] NULL,
	[Gender] [int] NULL,
	[SecondaryEmailAddress] [varchar](100) NULL,
	[PhoneNumberCountryCode] [varchar](5) NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[ProfilePicture] [varchar](500) NULL,
	[AddressLine1] [varchar](100) NULL,
	[AddressLine2] [varchar](100) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[ZipCode] [varchar](50) NULL,
	[Country] [varchar](50) NULL,
	[University] [varchar](100) NULL,
	[College] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserProfile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoles]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](max) NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 19-04-2021 1.10.08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [int] NOT NULL,
	[FirstName] [varchar](50) NOT NULL,
	[LastName] [varchar](50) NOT NULL,
	[EmailID] [varchar](100) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[IsEmailVerified] [bit] NOT NULL,
	[CreatedDate] [datetime] NULL,
	[CreatedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 

INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1004, N'India', N'91', CAST(N'2021-03-24T09:37:26.273' AS DateTime), 1120, CAST(N'2021-03-24T09:37:26.273' AS DateTime), 1120, 1)
INSERT [dbo].[Countries] ([ID], [Name], [CountryCode], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1005, N'USA', N'1', CAST(N'2021-03-24T16:48:55.053' AS DateTime), 1120, CAST(N'2021-03-24T16:48:55.053' AS DateTime), 1120, 1)
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Downloads] ON 

INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2056, 1036, 1122, 1122, 1, N'/Members/1122/1036/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-24T09:59:14.623' AS DateTime), 0, NULL, N'Basic Computers', N'IT', CAST(N'2021-03-24T09:59:14.627' AS DateTime), 1122, CAST(N'2021-03-24T09:59:14.627' AS DateTime), 1122)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2057, 1039, 1123, 1122, 1, N'/Members/1123/1039/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-09T10:21:33.000' AS DateTime), 1, CAST(56 AS Decimal(18, 0)), N'BCA Basics', N'BCA', CAST(N'2021-03-24T10:22:22.480' AS DateTime), 1122, CAST(N'2021-03-24T10:22:22.480' AS DateTime), 1123)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2058, 1036, 1122, 1123, 1, N'/Members/1122/1036/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-11T10:22:59.000' AS DateTime), 0, NULL, N'Basic Computers', N'IT', CAST(N'2021-03-24T10:22:59.550' AS DateTime), 1123, CAST(N'2021-03-24T10:22:59.550' AS DateTime), 1123)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2059, 1039, 1123, 1122, 1, N'/Members/1123/1039/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T11:50:35.627' AS DateTime), 1, CAST(56 AS Decimal(18, 0)), N'BCA Basics', N'BCA', CAST(N'2021-03-25T11:49:40.630' AS DateTime), 1122, CAST(N'2021-03-25T11:50:35.627' AS DateTime), 1123)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2060, 1039, 1123, 1122, 1, N'/Members/1123/1039/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T11:56:06.860' AS DateTime), 1, CAST(56 AS Decimal(18, 0)), N'BCA Basics', N'BCA', CAST(N'2021-03-25T11:53:18.523' AS DateTime), 1122, CAST(N'2021-03-25T11:56:06.860' AS DateTime), 1123)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2061, 1039, 1123, 1122, 1, N'/Members/1123/1039/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T12:03:23.977' AS DateTime), 1, CAST(56 AS Decimal(18, 0)), N'BCA Basics', N'BCA', CAST(N'2021-03-25T12:02:35.100' AS DateTime), 1122, CAST(N'2021-03-25T12:03:23.977' AS DateTime), 1123)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2062, 1040, 1122, 1123, 1, N'/Members/1122/1040/Attachements/Attachement1_25032021.pdf;', 1, CAST(N'2021-03-25T14:19:24.680' AS DateTime), 1, CAST(500 AS Decimal(18, 0)), N'MCA Basic Notes', N'MCA', CAST(N'2021-03-25T14:18:56.590' AS DateTime), 1123, CAST(N'2021-03-25T14:19:24.680' AS DateTime), 1122)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2063, 1037, 1123, 1122, 1, N'/Members/1123/1037/Attachements/Attachement1_25032021.pdf;', 1, CAST(N'2021-03-25T14:23:20.453' AS DateTime), 1, CAST(150 AS Decimal(18, 0)), N'BCA Concepts', N'BCA', CAST(N'2021-03-25T14:22:42.923' AS DateTime), 1122, CAST(N'2021-03-25T14:23:20.453' AS DateTime), 1123)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2064, 1039, 1123, 1123, 1, N'/Members/1123/1039/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-26T16:05:45.690' AS DateTime), 1, CAST(56 AS Decimal(18, 0)), N'BCA Basics', N'BCA', CAST(N'2021-03-26T15:54:59.007' AS DateTime), 1123, CAST(N'2021-03-26T16:05:45.690' AS DateTime), 1123)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2065, 1039, 1123, 1123, 1, N'/Members/1123/1039/Attachements/Attachement1_24032021.pdf;', 1, CAST(N'2021-03-25T18:25:02.837' AS DateTime), 1, CAST(56 AS Decimal(18, 0)), N'BCA Basics', N'BCA', CAST(N'2021-03-25T14:25:55.460' AS DateTime), 1123, CAST(N'2021-03-25T18:25:02.837' AS DateTime), 1123)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2066, 1040, 1122, 1123, 1, N'/Members/1122/1040/Attachements/Attachement1_25032021.pdf;', 0, NULL, 1, CAST(500 AS Decimal(18, 0)), N'MCA Basic Notes', N'MCA', CAST(N'2021-04-15T19:08:06.057' AS DateTime), 1123, CAST(N'2021-04-15T19:08:06.057' AS DateTime), 1122)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2067, 1040, 1122, 1123, 1, N'/Members/1122/1040/Attachements/Attachement1_25032021.pdf;', 1, CAST(N'2021-03-26T16:05:48.420' AS DateTime), 1, CAST(500 AS Decimal(18, 0)), N'MCA Basic Notes', N'MCA', CAST(N'2021-03-26T15:53:50.293' AS DateTime), 1123, CAST(N'2021-03-26T16:05:48.420' AS DateTime), 1122)
INSERT [dbo].[Downloads] ([ID], [NoteID], [Seller], [Downloader], [IsSellerHasAllowedDownload], [AttachmentPath], [IsAttachementDownloaded], [AttacmentDownloadedDate], [IsPaid], [PurchasedPrice], [NoteTitle], [NoteCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (2068, 1037, 1123, 1122, 1, N'/Members/1123/1037/Attachements/Attachement1_25032021.pdf;', 0, NULL, 1, CAST(150 AS Decimal(18, 0)), N'BCA Concepts', N'BCA', CAST(N'2021-03-26T15:54:44.377' AS DateTime), 1122, CAST(N'2021-03-26T15:54:44.377' AS DateTime), 1123)
SET IDENTITY_INSERT [dbo].[Downloads] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteCategories] ON 

INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1003, N'BCA', N'BCA Category.', CAST(N'2021-03-24T09:43:16.067' AS DateTime), 1120, CAST(N'2021-03-24T09:43:16.067' AS DateTime), 1120, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1004, N'IT', N'IT category.', CAST(N'2021-03-24T09:43:31.507' AS DateTime), 1120, CAST(N'2021-03-24T09:43:31.507' AS DateTime), 1120, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1005, N'MCA', N'MCA Category.', CAST(N'2021-03-24T16:47:25.083' AS DateTime), 1120, CAST(N'2021-03-24T16:47:25.083' AS DateTime), 1120, 1)
INSERT [dbo].[NoteCategories] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1006, N'CS', N'Computer Science Category', CAST(N'2021-03-30T09:35:59.297' AS DateTime), 1120, CAST(N'2021-03-30T09:35:59.297' AS DateTime), 1120, 1)
SET IDENTITY_INSERT [dbo].[NoteCategories] OFF
GO
SET IDENTITY_INSERT [dbo].[NoteTypes] ON 

INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1003, N'Book', N'Books ', CAST(N'2021-03-24T09:43:54.607' AS DateTime), 1120, CAST(N'2021-03-24T09:43:54.607' AS DateTime), 1120, 1)
INSERT [dbo].[NoteTypes] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1004, N'Handwritten', N'Handwritten Notes.', CAST(N'2021-03-24T16:48:19.883' AS DateTime), 1120, CAST(N'2021-03-24T16:48:19.883' AS DateTime), 1120, 1)
SET IDENTITY_INSERT [dbo].[NoteTypes] OFF
GO
SET IDENTITY_INSERT [dbo].[ReferenceData] ON 

INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1, N'Draft', N'Draft', N'Notes Status', CAST(N'2021-02-18T22:08:31.490' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (3, N'Submitted For Review', N'Submitted For Review', N'Notes Status', CAST(N'2021-02-18T22:09:34.830' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (4, N'In Review', N'In Review', N'Notes Status', CAST(N'2021-02-18T22:10:07.980' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (5, N'Published', N'Approved', N'Notes Status', CAST(N'2021-02-18T22:11:20.670' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (6, N'Rejected', N'Rejected', N'Notes Status', CAST(N'2021-02-18T22:11:50.697' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (7, N'Removed', N'Removed', N'Notes Status', CAST(N'2021-02-18T22:12:14.793' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (8, N'Male', N'M', N'Gender', CAST(N'2021-02-18T00:00:00.000' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (9, N'Female', N'Fe', N'Gender', CAST(N'2021-02-18T00:00:00.000' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (10, N'Unknown', N'U', N'Gender', CAST(N'2021-02-18T00:00:00.000' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[ReferenceData] ([ID], [Value], [DataValue], [RefCategory], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (11, N'Paid', N'P', N'Selling Mode', CAST(N'2021-02-18T00:00:00.000' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[ReferenceData] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotes] ON 

INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PubilshedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1036, 1122, 5, 1121, NULL, CAST(N'2021-03-24T09:52:15.293' AS DateTime), N'Basic Computers', 1004, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 1003, 200, N'Bachelor of Computer Applications (BCA) beginer level Notes', N'IIT', 1004, N'Information technology', N'SB2201', N'Professor Name', 0, NULL, NULL, CAST(N'2021-03-24T09:48:07.910' AS DateTime), 1122, CAST(N'2021-03-24T09:52:15.293' AS DateTime), 1121, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PubilshedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1037, 1123, 5, 1120, NULL, CAST(N'2021-03-25T14:21:52.753' AS DateTime), N'BCA Concepts', 1003, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 1003, 456, N'Bachelor of Computer Applications (BCA) Intermediate Level Notes', N'IIT', 1004, N'Information technology', N'SB2212', N'Professor Name', 1, CAST(199 AS Decimal(18, 0)), N'/Members/1123/1037/Preview_25032021.pdf', CAST(N'2021-03-24T10:12:25.267' AS DateTime), 1123, CAST(N'2021-03-25T14:21:52.753' AS DateTime), 1120, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PubilshedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1038, 1123, 6, 1121, N'rejected by admin.', NULL, N'MCA Basic Notes', 1003, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 1003, 4561, N'Bachelor of Computer Applications (BCA) Advance Concepts Notes', N'IIT', 1004, N'Information technology', N'SB1213', N'Professor Name', 1, CAST(129 AS Decimal(18, 0)), N'/Members/1123/1038/Preview_24032021.pdf', CAST(N'2021-03-24T10:14:51.227' AS DateTime), 1123, CAST(N'2021-03-24T10:17:21.677' AS DateTime), 1121, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PubilshedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1039, 1123, 5, 1121, NULL, CAST(N'2021-03-24T10:20:09.100' AS DateTime), N'BCA Basics', 1003, N'/Members/1123/1039/DP_24032021.jpeg', 1003, 455, N'Bachelor of Computer Applications (BCA) Basic Introduction Notes', N'IIT', 1004, N'Information technology', N'SB1313', N'Professor Name', 1, CAST(59 AS Decimal(18, 0)), N'/Members/1123/1039/Preview_24032021.pdf', CAST(N'2021-03-24T10:19:26.253' AS DateTime), 1123, CAST(N'2021-03-24T10:20:09.100' AS DateTime), 1121, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PubilshedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1040, 1122, 5, 1120, NULL, CAST(N'2021-03-25T14:17:56.773' AS DateTime), N'MCA Basic Notes', 1005, N'/Members/1122/1040/DP_25032021.png', 1004, 456, N'Master of Computer Applications (MCA) Basic Notes ', N'MIT', 1005, N'Information technology', N'SB3144', N'Professor Name', 1, CAST(499 AS Decimal(18, 0)), N'/Members/1122/1040/Preview_25032021.pdf', CAST(N'2021-03-25T14:15:43.903' AS DateTime), 1122, CAST(N'2021-03-25T14:17:56.773' AS DateTime), 1120, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PubilshedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1041, 1123, 5, 1120, NULL, CAST(N'2021-04-19T11:10:19.673' AS DateTime), N'Data Structures', 1003, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 1004, 456, N'Data Structure Notes includes all topics of Data Structure such as Array, Pointer, Structure, Linked List, Stack, Queue, Graph, Searching, Sorting, Programs, etc.', N'IIT', 1004, N'Information technology', N'SB0023', N'Professor Name', 0, NULL, NULL, CAST(N'2021-03-26T17:27:34.293' AS DateTime), 1123, CAST(N'2021-04-19T11:10:19.673' AS DateTime), 1120, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PubilshedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1042, 1123, 1, NULL, NULL, NULL, N'BCA Basics', 1003, N'/Members/1123/1042/DP_26032021.jpeg', 1003, 200, N'C++  Notes include details about C++ a general-purpose programming language that was developed as an enhancement of the C language to include object-oriented paradigm. It is an imperative and a compiled language.', N'IIT', 1004, N'Information technology', N'SB1101', N'Professor Name', 0, NULL, NULL, CAST(N'2021-03-26T17:29:13.760' AS DateTime), 1123, NULL, NULL, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PubilshedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1043, 1130, 5, 1120, NULL, CAST(N'2021-04-16T17:35:54.923' AS DateTime), N'Basic Computers', 1006, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 1004, 299, N'Computer', NULL, 1004, NULL, NULL, NULL, 0, NULL, NULL, CAST(N'2021-04-16T17:30:10.840' AS DateTime), 1130, CAST(N'2021-04-16T17:35:54.923' AS DateTime), 1120, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PubilshedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1047, 1122, 5, 1120, NULL, CAST(N'2021-04-19T13:06:01.990' AS DateTime), N'Basic Computers', 1006, N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', 1003, NULL, N'basic', NULL, 1004, NULL, NULL, NULL, 0, NULL, NULL, CAST(N'2021-04-18T20:25:43.617' AS DateTime), 1122, CAST(N'2021-04-19T13:06:01.990' AS DateTime), 1120, 1)
INSERT [dbo].[SellerNotes] ([ID], [SellerID], [Status], [ActionedBy], [AdminRemarks], [PubilshedDate], [Title], [Category], [DisplayPicture], [NoteType], [NumberofPages], [Description], [UniversityName], [Country], [Course], [CourseCode], [Professor], [IsPaid], [SellingPrice], [NotesPreview], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1048, 1151, 1, NULL, NULL, NULL, N'Master Asp.Net MVC 5', 1004, N'/Members/1151/1048/DP_19042021.jpg', 1004, NULL, N'The ASP.NET MVC 5 Framework is the latest evolution of Microsoft''s ASP.NET web platform. It provides a high-productivity programming model that promotes cleaner code architecture, test-driven development, and powerful extensibility, combined with all the benefits of ASP.NET.', NULL, 1005, NULL, NULL, NULL, 1, CAST(999 AS Decimal(18, 0)), N'/Members/1151/1048/Preview_19042021.pdf', CAST(N'2021-04-19T12:16:23.423' AS DateTime), 1151, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[SellerNotes] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesAttachements] ON 

INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [ISActive], [AttachementSize]) VALUES (1036, 1036, N'Attachement1_24032021.pdf;', N'/Members/1122/1036/Attachements/Attachement1_24032021.pdf;', CAST(N'2021-03-24T09:48:07.947' AS DateTime), 1122, CAST(N'2021-03-24T09:49:56.043' AS DateTime), 1122, 1, 71)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [ISActive], [AttachementSize]) VALUES (1037, 1037, N'Attachement1_25032021.pdf;', N'/Members/1123/1037/Attachements/Attachement1_25032021.pdf;', CAST(N'2021-03-24T10:12:25.340' AS DateTime), 1123, CAST(N'2021-03-25T14:21:36.070' AS DateTime), 1123, 1, 339)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [ISActive], [AttachementSize]) VALUES (1038, 1038, N'Attachement1_24032021.pdf;', N'/Members/1123/1038/Attachements/Attachement1_24032021.pdf;', CAST(N'2021-03-24T10:14:51.267' AS DateTime), 1123, CAST(N'2021-03-24T10:16:09.030' AS DateTime), 1123, 1, 388)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [ISActive], [AttachementSize]) VALUES (1039, 1039, N'Attachement1_24032021.pdf;', N'/Members/1123/1039/Attachements/Attachement1_24032021.pdf;', CAST(N'2021-03-24T10:19:26.363' AS DateTime), 1123, CAST(N'2021-03-24T10:19:48.807' AS DateTime), 1123, 1, 388)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [ISActive], [AttachementSize]) VALUES (1040, 1040, N'Attachement1_25032021.pdf;', N'/Members/1122/1040/Attachements/Attachement1_25032021.pdf;', CAST(N'2021-03-25T14:15:45.527' AS DateTime), 1122, CAST(N'2021-03-25T14:17:38.293' AS DateTime), 1122, 1, 388)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [ISActive], [AttachementSize]) VALUES (1041, 1041, N'Attachement1_26032021.pdf;', N'/Members/1123/1041/Attachements/Attachement1_26032021.pdf;', CAST(N'2021-03-26T17:27:34.540' AS DateTime), 1123, CAST(N'2021-03-26T17:27:57.213' AS DateTime), 1123, 1, 71)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [ISActive], [AttachementSize]) VALUES (1042, 1042, N'Attachement1_26032021.pdf;', N'/Members/1123/1042/Attachements/Attachement1_26032021.pdf;', CAST(N'2021-03-26T17:29:13.790' AS DateTime), 1123, NULL, NULL, 1, 71)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [ISActive], [AttachementSize]) VALUES (1043, 1043, N'Attachement1_16042021.pdf;', N'/Members/1130/1043/Attachements/Attachement1_16042021.pdf;', CAST(N'2021-04-16T17:30:10.883' AS DateTime), 1130, CAST(N'2021-04-16T17:31:20.067' AS DateTime), 1130, 1, 16)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [ISActive], [AttachementSize]) VALUES (1047, 1047, N'Attachement1_18042021.pdf;', N'/Members/1122/1047/Attachements/Attachement1_18042021.pdf;', CAST(N'2021-04-18T20:25:43.963' AS DateTime), 1122, CAST(N'2021-04-18T20:39:38.320' AS DateTime), 1122, 1, 16)
INSERT [dbo].[SellerNotesAttachements] ([ID], [NoteID], [FileName], [FilePath], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [ISActive], [AttachementSize]) VALUES (1048, 1048, N'Attachement1_19042021.pdf;', N'/Members/1151/1048/Attachements/Attachement1_19042021.pdf;', CAST(N'2021-04-19T12:16:23.477' AS DateTime), 1151, NULL, NULL, 1, 317)
SET IDENTITY_INSERT [dbo].[SellerNotesAttachements] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesReportedIssues] ON 

INSERT [dbo].[SellerNotesReportedIssues] ([ID], [NoteID], [ReportedByID], [AgainstDownloadID], [Remarks], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy]) VALUES (14, 1039, 1123, 2064, N'Inappropriate Notes', CAST(N'2021-03-26T17:28:34.933' AS DateTime), 1123, CAST(N'2021-03-26T17:28:34.937' AS DateTime), 1123)
SET IDENTITY_INSERT [dbo].[SellerNotesReportedIssues] OFF
GO
SET IDENTITY_INSERT [dbo].[SellerNotesReviews] ON 

INSERT [dbo].[SellerNotesReviews] ([ID], [NoteID], [ReviewedBy], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [Isactive]) VALUES (34, 1039, 1122, 2057, CAST(5 AS Decimal(18, 0)), N'Thank You! Great Notes for studies ', CAST(N'2021-03-24T10:26:07.357' AS DateTime), 1122, CAST(N'2021-03-24T10:26:07.357' AS DateTime), 1122, 1)
INSERT [dbo].[SellerNotesReviews] ([ID], [NoteID], [ReviewedBy], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [Isactive]) VALUES (36, 1036, 1123, 2058, CAST(3 AS Decimal(18, 0)), N'Notes are awesome some minor improvements needed .', CAST(N'2021-03-24T17:01:57.710' AS DateTime), 1123, CAST(N'2021-03-24T17:01:57.710' AS DateTime), 1123, 1)
INSERT [dbo].[SellerNotesReviews] ([ID], [NoteID], [ReviewedBy], [AgainstDownloadsID], [Ratings], [Comments], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [Isactive]) VALUES (37, 1037, 1122, 2068, CAST(5 AS Decimal(18, 0)), N'Great!! Thank You.', CAST(N'2021-04-06T18:14:11.443' AS DateTime), 1122, CAST(N'2021-04-06T18:14:11.443' AS DateTime), 1122, 1)
SET IDENTITY_INSERT [dbo].[SellerNotesReviews] OFF
GO
SET IDENTITY_INSERT [dbo].[SystemConfigurations] ON 

INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (29, N'SupportEmailAddress', N'superadmin@gmail.com', CAST(N'2021-03-24T09:40:14.577' AS DateTime), 1120, CAST(N'2021-04-15T15:44:14.013' AS DateTime), 1124, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (30, N'EmailPassword', N'Pass#4321', CAST(N'2021-03-24T09:40:14.590' AS DateTime), 1120, CAST(N'2021-04-15T15:44:14.027' AS DateTime), 1124, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (31, N'SupportPhoneNumber', N'0123456789', CAST(N'2021-03-24T09:40:14.590' AS DateTime), 1120, CAST(N'2021-04-15T15:44:14.033' AS DateTime), 1124, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (32, N'EmailAddress', N'superadmin@gmail.com', CAST(N'2021-03-24T09:40:14.593' AS DateTime), 1120, CAST(N'2021-04-15T15:44:14.040' AS DateTime), 1124, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (33, N'FacebookURL', N'https://www.facebook.com/', CAST(N'2021-03-24T09:40:14.593' AS DateTime), 1120, CAST(N'2021-04-15T15:44:14.047' AS DateTime), 1124, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (34, N'LinkedinURL', N'https://www.linkedin.com/', CAST(N'2021-03-24T09:40:14.597' AS DateTime), 1120, CAST(N'2021-04-15T15:44:14.053' AS DateTime), 1124, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (35, N'TwitterURL', N'https://twitter.com/', CAST(N'2021-03-24T09:40:14.600' AS DateTime), 1120, CAST(N'2021-04-15T15:44:14.060' AS DateTime), 1124, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (36, N'DefaultImageForNotes', N'/SystemConfigurations/DefaultImages/DefaultNoteImage.jpg', CAST(N'2021-03-24T09:40:14.603' AS DateTime), 1120, CAST(N'2021-04-15T15:44:14.070' AS DateTime), 1124, 1)
INSERT [dbo].[SystemConfigurations] ([ID], [Key], [Value], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (37, N'DefaultProfilePicture', N'/SystemConfigurations/DefaultImages/DefaultUserImage.jpg', CAST(N'2021-03-24T09:40:14.613' AS DateTime), 1120, CAST(N'2021-04-15T15:44:14.080' AS DateTime), 1124, 1)
SET IDENTITY_INSERT [dbo].[SystemConfigurations] OFF
GO
SET IDENTITY_INSERT [dbo].[UserProfile] ON 

INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1019, 1120, NULL, NULL, N'superadmin@gmail.com', N'91', N'0123456789', N'/SystemConfigurations/DefaultImages/DefaultUserImage.jpg', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-03-24T09:41:02.380' AS DateTime), 1120, CAST(N'2021-04-19T13:06:40.653' AS DateTime), 1120, 0)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1020, 1121, NULL, NULL, NULL, N'91', N'0123456789', N'/Members/1121/DP_15042021.jpg', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-03-24T09:42:11.747' AS DateTime), 1120, CAST(N'2021-04-15T15:25:00.053' AS DateTime), 1121, 1)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1021, 1122, CAST(N'1996-06-15T00:00:00.000' AS DateTime), 8, NULL, N'91', N'0123456789', N'/SystemConfigurations/DefaultImages/DefaultUserImage.jpg', N'Address Line 1 Text', N'Address Line 2 Text', N'Bombay', N'Maharashtra', N'381100', N'India', N'IIT', N'Indian Institute of Technology Bombay', CAST(N'2021-03-24T09:46:58.477' AS DateTime), 1122, CAST(N'2021-04-19T11:06:55.673' AS DateTime), 1122, 0)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1022, 1123, CAST(N'2021-03-17T00:00:00.000' AS DateTime), 8, NULL, N'91', N'0123456789', N'/Members/1123/DP_15042021.jpg', N'Address Line 1 Text', N'Address Line 2 Text', N'Gandhinagar', N'Gujarat ', N'382355', N'india', N'IIT', N'Indian Institute of Technology Gandhinagar', CAST(N'2021-03-24T10:11:18.080' AS DateTime), 1123, CAST(N'2021-04-15T15:39:30.313' AS DateTime), 1123, 0)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1023, 1125, NULL, NULL, NULL, N'91', N'0123456789', N'/Members/1125/DP_15042021.jpg', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-03-24T16:54:37.207' AS DateTime), 1120, CAST(N'2021-04-15T15:49:28.243' AS DateTime), 1125, 1)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1024, 1126, NULL, NULL, NULL, N'91', N'0123456789', N'/Members/1126/DP_15042021.jpg', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-04-05T10:04:23.687' AS DateTime), 1120, CAST(N'2021-04-15T15:56:02.777' AS DateTime), 1126, 1)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1025, 1127, NULL, 8, NULL, N'91', N'0123456789', N'/Members/1127/DP_15042021.jpg', N'Ad1', N'Ad2', N'india', N'india', N'111111', N'india', NULL, NULL, CAST(N'2021-04-14T21:06:37.610' AS DateTime), 1127, CAST(N'2021-04-15T15:58:22.460' AS DateTime), 1127, 0)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1026, 1124, NULL, NULL, NULL, N'91', N'0123456789', N'/Members/1124/DP_15042021.jpg', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, CAST(N'2021-04-15T15:42:06.467' AS DateTime), 1124, CAST(N'2021-04-15T15:42:06.467' AS DateTime), 1124, 0)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1027, 1130, CAST(N'2021-04-21T00:00:00.000' AS DateTime), 8, NULL, N'91', N'0123456789', N'/SystemConfigurations/DefaultImages/DefaultUserImage.jpg', N'Address Line 1 Text', N'Address Line 2 Text', N'Gandhinagar', N'Gujarat', N'382355', N'India', NULL, NULL, CAST(N'2021-04-16T17:24:51.053' AS DateTime), 1130, CAST(N'2021-04-16T17:26:53.370' AS DateTime), 1130, 0)
INSERT [dbo].[UserProfile] ([ID], [UserID], [DOB], [Gender], [SecondaryEmailAddress], [PhoneNumberCountryCode], [PhoneNumber], [ProfilePicture], [AddressLine1], [AddressLine2], [City], [State], [ZipCode], [Country], [University], [College], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1028, 1151, NULL, 8, NULL, N'91', N'0123456789', N'/SystemConfigurations/DefaultImages/DefaultUserImage.jpg', N'Address Line 1 Text', N'Address Line 2 Text', N'Gandhinagar', N'Gujarat', N'382012', N'India', NULL, NULL, CAST(N'2021-04-19T12:00:26.673' AS DateTime), 1151, CAST(N'2021-04-19T12:03:56.873' AS DateTime), 1151, 0)
SET IDENTITY_INSERT [dbo].[UserProfile] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON 

INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedBy], [ModifiedDate], [IsActive]) VALUES (1, N'Member', N'This is simple Member', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedBy], [ModifiedDate], [IsActive]) VALUES (2, N'Admin', N'This is Admin User', NULL, NULL, NULL, NULL, 1)
INSERT [dbo].[UserRoles] ([ID], [Name], [Description], [CreatedDate], [CreatedBy], [ModifiedBy], [ModifiedDate], [IsActive]) VALUES (3, N'SuperAdmin', N'This is Super Admin User', NULL, NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1120, 3, N'Super', N'Admin', N'superadmin@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-03-24T00:00:00.000' AS DateTime), NULL, CAST(N'2021-03-24T16:49:14.390' AS DateTime), 1120, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1121, 2, N'normal', N'admin', N'normaladmin@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-03-24T09:42:05.260' AS DateTime), 1120, CAST(N'2021-03-24T16:50:21.970' AS DateTime), 1121, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1122, 1, N'user', N'one', N'userone@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-03-24T09:45:09.857' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1123, 1, N'user', N'two', N'usertwo@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-03-24T10:04:01.657' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1124, 3, N'NewSuper', N'Admin', N'superadmin2@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-03-24T16:29:16.337' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1125, 2, N'normal', N'admintwo', N'normaladmintwo@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-03-24T16:54:31.360' AS DateTime), 1120, CAST(N'2021-03-24T16:54:31.360' AS DateTime), 1120, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1126, 2, N'Normal', N'AdminThree', N'normaladminthree@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-04-05T10:04:18.513' AS DateTime), 1120, CAST(N'2021-04-05T10:04:18.513' AS DateTime), 1120, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1127, 1, N'super', N'legand', N'superlegand@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-04-14T20:57:16.177' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1129, 1, N'Rahul', N'Sharma', N'rahulsharma91282@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-04-16T12:50:43.120' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1130, 1, N'demo', N'user', N'demouser27272@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-04-16T17:21:03.373' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1131, 1, N'User', N'Three', N'userthree4517@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-04-17T08:51:00.490' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1135, 1, N'fourth', N'User', N'fourthuser266@gmail.com', N'26c75c3c778d49228c3329e27c946a9b', 1, CAST(N'2021-04-17T12:44:26.063' AS DateTime), NULL, NULL, NULL, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1151, 1, N'Fifth', N'user', N'fifthuser2727@gmail.com', N'bd48673993638cd851b60e839be7313e', 1, CAST(N'2021-04-19T11:41:12.277' AS DateTime), NULL, CAST(N'2021-04-19T12:04:35.803' AS DateTime), 1151, 1)
INSERT [dbo].[Users] ([ID], [RoleID], [FirstName], [LastName], [EmailID], [Password], [IsEmailVerified], [CreatedDate], [CreatedBy], [ModifiedDate], [ModifiedBy], [IsActive]) VALUES (1152, 1, N'Dummy', N'User', N'dummyuser3993@gmail.com', N'01117edd74db716199ff1caac2359751', 1, CAST(N'2021-04-19T12:37:29.250' AS DateTime), NULL, NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  Index [IX_ReferenceData]    Script Date: 19-04-2021 1.10.08 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_ReferenceData] ON [dbo].[ReferenceData]
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Users]    Script Date: 19-04-2021 1.10.08 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Users] ON [dbo].[Users]
(
	[EmailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[ReferenceData] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[SellerNotes] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[SellerNotes] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_RoleID]  DEFAULT ((1)) FOR [RoleID]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (lower(CONVERT([varchar](32),hashbytes('MD5','Admin@123'),(2)))) FOR [Password]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsEmailVerified]  DEFAULT ((0)) FOR [IsEmailVerified]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Countries]  WITH CHECK ADD  CONSTRAINT [FK_Countries_Users_createdby] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Countries] CHECK CONSTRAINT [FK_Countries_Users_createdby]
GO
ALTER TABLE [dbo].[Countries]  WITH CHECK ADD  CONSTRAINT [FK_Countries_Users_modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Countries] CHECK CONSTRAINT [FK_Countries_Users_modifiedby]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_SellerNotes1] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_SellerNotes1]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users_createdby] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users_createdby]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users_downloader] FOREIGN KEY([Downloader])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users_downloader]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users_modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users_modifiedby]
GO
ALTER TABLE [dbo].[Downloads]  WITH CHECK ADD  CONSTRAINT [FK_Downloads_Users_seller] FOREIGN KEY([Seller])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Downloads] CHECK CONSTRAINT [FK_Downloads_Users_seller]
GO
ALTER TABLE [dbo].[NoteCategories]  WITH CHECK ADD  CONSTRAINT [FK_NoteCategories_Users_createdby] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteCategories] CHECK CONSTRAINT [FK_NoteCategories_Users_createdby]
GO
ALTER TABLE [dbo].[NoteCategories]  WITH CHECK ADD  CONSTRAINT [FK_NoteCategories_Users_modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteCategories] CHECK CONSTRAINT [FK_NoteCategories_Users_modifiedby]
GO
ALTER TABLE [dbo].[NoteTypes]  WITH CHECK ADD  CONSTRAINT [FK_NoteTypes_Users_createdby] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteTypes] CHECK CONSTRAINT [FK_NoteTypes_Users_createdby]
GO
ALTER TABLE [dbo].[NoteTypes]  WITH CHECK ADD  CONSTRAINT [FK_NoteTypes_Users_modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[NoteTypes] CHECK CONSTRAINT [FK_NoteTypes_Users_modifiedby]
GO
ALTER TABLE [dbo].[ReferenceData]  WITH CHECK ADD  CONSTRAINT [FK_ReferenceData_Users_createdby] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ReferenceData] CHECK CONSTRAINT [FK_ReferenceData_Users_createdby]
GO
ALTER TABLE [dbo].[ReferenceData]  WITH CHECK ADD  CONSTRAINT [FK_ReferenceData_Users_modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[ReferenceData] CHECK CONSTRAINT [FK_ReferenceData_Users_modifiedby]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Countries] FOREIGN KEY([Country])
REFERENCES [dbo].[Countries] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Countries]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_NoteCategories] FOREIGN KEY([Category])
REFERENCES [dbo].[NoteCategories] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_NoteCategories]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_NoteTypes] FOREIGN KEY([NoteType])
REFERENCES [dbo].[NoteTypes] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_NoteTypes]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_ReferenceData] FOREIGN KEY([Status])
REFERENCES [dbo].[ReferenceData] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_ReferenceData]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Users] FOREIGN KEY([SellerID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Users]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Users_createdby] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Users_createdby]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Users1] FOREIGN KEY([ActionedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Users1]
GO
ALTER TABLE [dbo].[SellerNotes]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotes_Users3] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotes] CHECK CONSTRAINT [FK_SellerNotes_Users3]
GO
ALTER TABLE [dbo].[SellerNotesAttachements]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesAttachements_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SellerNotesAttachements] CHECK CONSTRAINT [FK_SellerNotesAttachements_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesAttachements]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesAttachements_Users_createdby] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesAttachements] CHECK CONSTRAINT [FK_SellerNotesAttachements_Users_createdby]
GO
ALTER TABLE [dbo].[SellerNotesAttachements]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesAttachements_Users_modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesAttachements] CHECK CONSTRAINT [FK_SellerNotesAttachements_Users_modifiedby]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_Downloads] FOREIGN KEY([AgainstDownloadID])
REFERENCES [dbo].[Downloads] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_Downloads]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_Users_createdby] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_Users_createdby]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_Users_modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_Users_modifiedby]
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReportedIssues_Users_reportedby] FOREIGN KEY([ReportedByID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReportedIssues] CHECK CONSTRAINT [FK_SellerNotesReportedIssues_Users_reportedby]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_Downloads] FOREIGN KEY([AgainstDownloadsID])
REFERENCES [dbo].[Downloads] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_Downloads]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_SellerNotes] FOREIGN KEY([NoteID])
REFERENCES [dbo].[SellerNotes] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_SellerNotes]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_User_reviewedby] FOREIGN KEY([ReviewedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_User_reviewedby]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_Users_createdby] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_Users_createdby]
GO
ALTER TABLE [dbo].[SellerNotesReviews]  WITH CHECK ADD  CONSTRAINT [FK_SellerNotesReviews_Users_modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SellerNotesReviews] CHECK CONSTRAINT [FK_SellerNotesReviews_Users_modifiedby]
GO
ALTER TABLE [dbo].[SystemConfigurations]  WITH CHECK ADD  CONSTRAINT [FK_SystemConfigurations_Users_createdby] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SystemConfigurations] CHECK CONSTRAINT [FK_SystemConfigurations_Users_createdby]
GO
ALTER TABLE [dbo].[SystemConfigurations]  WITH CHECK ADD  CONSTRAINT [FK_SystemConfigurations_Users_modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[SystemConfigurations] CHECK CONSTRAINT [FK_SystemConfigurations_Users_modifiedby]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_Users] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_Users]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_Users_Modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_Users_Modifiedby]
GO
ALTER TABLE [dbo].[UserProfile]  WITH CHECK ADD  CONSTRAINT [FK_UserProfile_Users1] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserProfile] CHECK CONSTRAINT [FK_UserProfile_Users1]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK_UserRoles_Users_modifiedby] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK_UserRoles_Users_modifiedby]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users_CreatedBy] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users_CreatedBy]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Users_ModifiedBy] FOREIGN KEY([ModifiedBy])
REFERENCES [dbo].[Users] ([ID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Users_ModifiedBy]
GO
USE [master]
GO
ALTER DATABASE [NoteMarketPlace] SET  READ_WRITE 
GO
