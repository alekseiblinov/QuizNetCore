---------------------------------------------------------------------------------
-- Purpose: SQL script for creating a database of the Quiz project and filling it with initial data.
-- DBMS version: Microsoft SQL Server 2012.
---------------------------------------------------------------------------------
USE [master]
GO

/****** Object:  Database [quiz]    Script Date: 09.11.2024 17:03:02 ******/
CREATE DATABASE [quiz]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'quiz', FILENAME = N'd:\!DataBases\MSSQL14.SQL2017\MSSQL\DATA\quiz.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'quiz_log', FILENAME = N'd:\!DataBases\MSSQL14.SQL2017\MSSQL\DATA\quiz_log.ldf' , SIZE = 28608KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [quiz].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [quiz] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [quiz] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [quiz] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [quiz] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [quiz] SET ARITHABORT OFF 
GO
ALTER DATABASE [quiz] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [quiz] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [quiz] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [quiz] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [quiz] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [quiz] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [quiz] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [quiz] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [quiz] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [quiz] SET  DISABLE_BROKER 
GO
ALTER DATABASE [quiz] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [quiz] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [quiz] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [quiz] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [quiz] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [quiz] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [quiz] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [quiz] SET RECOVERY FULL 
GO
ALTER DATABASE [quiz] SET  MULTI_USER 
GO
ALTER DATABASE [quiz] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [quiz] SET DB_CHAINING OFF 
GO
ALTER DATABASE [quiz] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [quiz] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'quiz', N'ON'
GO
USE [quiz]
GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[Email] [nvarchar](256) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogRecords]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogRecords](
	[Id] [uniqueidentifier] NOT NULL,
	[Message] [nvarchar](1024) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Question]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Question](
	[Id] [uniqueidentifier] NOT NULL,
	[TopicId] [uniqueidentifier] NOT NULL,
	[QuestionText] [nvarchar](500) NOT NULL,
	[Option01] [nvarchar](255) NOT NULL,
	[Option02] [nvarchar](255) NULL,
	[Option03] [nvarchar](255) NULL,
	[Option04] [nvarchar](255) NULL,
	[Answer] [nvarchar](255) NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Question] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SecurityTokens]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SecurityTokens](
	[Id] [uniqueidentifier] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[UserName] [nvarchar](256) NOT NULL,
	[IpAddress] [varchar](39) NULL,
	[MethodName] [nvarchar](1024) NOT NULL,
	[MethodParametersHash] [nvarchar](max) NOT NULL,
	[IsUsed] [bit] NOT NULL,
 CONSTRAINT [PK_AspNetSecurityTokens] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Topics]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Topics](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[OrderNumber] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
 CONSTRAINT [PK_Quiz] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserQuestionProgress]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserQuestionProgress](
	[Id] [uniqueidentifier] NOT NULL,
	[QuizId] [uniqueidentifier] NOT NULL,
	[UserId] [nvarchar](450) NOT NULL,
	[QuestionId] [uniqueidentifier] NOT NULL,
	[LastAnswered] [datetime] NOT NULL,
	[IsCorrect] [bit] NOT NULL,
	[SelectedAnswerText] [nvarchar](255) NULL,
	[Repetitions] [int] NOT NULL,
	[RepetitionInterval] [int] NOT NULL,
	[NextDue] [datetime] NOT NULL,
 CONSTRAINT [PK_UserQuestionProgress] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'21F1AECF-B4BA-4E78-925A-EC0B3EA524D8', N'Все', N'ВСЕ', NULL)
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'3A5FC2E8-93CA-4BFC-B57E-85B7FEC80B68', N'Студенты', N'СТУДЕНТЫ', NULL)
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'E27D3A8B-50A8-44B0-B418-BC29089244B0', N'Администраторы', N'АДМИНИСТРАТОРЫ', NULL)
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5c3345cb-cc3f-4bf1-92c5-7e84e20a10ef', N'21F1AECF-B4BA-4E78-925A-EC0B3EA524D8')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'5c3345cb-cc3f-4bf1-92c5-7e84e20a10ef', N'E27D3A8B-50A8-44B0-B418-BC29089244B0')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'708e2e9c-e1c5-48e4-8951-26c97c571a29', N'21F1AECF-B4BA-4E78-925A-EC0B3EA524D8')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'708e2e9c-e1c5-48e4-8951-26c97c571a29', N'3A5FC2E8-93CA-4BFC-B57E-85B7FEC80B68')
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [CreatedAt]) VALUES (N'5c3345cb-cc3f-4bf1-92c5-7e84e20a10ef', N'Администратор', N'АДМИНИСТРАТОР', N'admin@mail.dom.ain', N'ADMIN@MAIL.DOM.AIN', 0, N'AQAAAAEAACcQAAAAEOyvSGKtgS1Vo7lojOyP2aMqtBzzS2Q1hAbjMguDg4MxkwvA9gGr3fOZQDiHsXMsHw==', N'IKIJN4HC47AAKINSGQU43IHXSLINQJUF', N'f13d008c-9010-48eb-be20-29db87e08d85', NULL, 0, 0, NULL, 1, 0, CAST(N'2024-03-23T23:08:50.747' AS DateTime))
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [CreatedAt]) VALUES (N'708e2e9c-e1c5-48e4-8951-26c97c571a29', N'Студент', N'СТУДЕНТ', N'student@mail.dom.ain', N'STUDENT@MAIL.DOM.AIN', 0, N'AQAAAAEAACcQAAAAEJMyLOP063EUQK6xEnWTAkiZYUElzyU2yFrvhD1B2Cy7IWuT8mNg9IjjFqE3GI5ZNg==', N'HK7RU67RSAWCE2WCXGMXBTJX2Q7ZNHR5', N'29710829-9639-4572-b48b-b7408086b64b', NULL, 0, 0, NULL, 1, 0, CAST(N'2024-10-30T20:09:31.183' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'e3d6c722-c14d-498b-b4ef-063a80df5644', N'261fa5ce-5da9-488c-97b1-57e89017621f', N'Which SQL statement is used to retrieve data from a database?', N'a) SELECT', N'b) GET', N'c) FETCH', N'd) EXTRACT', N'a) SELECT', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'66e31dd3-72ae-4d71-aee7-0ac5bcea09f0', N'261fa5ce-5da9-488c-97b1-57e89017621f', N'Which of the following is used to eliminate duplicate rows in a SQL query result?', N'a) DISTINCT', N'b) UNIQUE', N'c) REMOVE', N'd) DELETE', N'a) DISTINCT', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'd488b243-bbc8-4b63-b28e-0dfadf4aa495', N'ed98f1ca-1ac1-415a-8ca4-c5cbb0ceecd8', N'What does CSS stand for?', N'a) Cascading Style Sheets', N'b) Computer Style Sheets', N'c) Creative Style Sheets', N'd) Colorful Style Sheets', N'a) Cascading Style Sheets', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'5b18b501-c4f5-49f1-8381-10c2e1e9c27c', N'dc0227a8-8e2a-48b5-8dc2-ef03c37bfe83', N'What does HTML stand for?', N'a) HyperText Markup Language', N'b) HyperText Markdown Language', N'c) Hyperlink and Text Markup Language', N'd) Hyperlink and Text Markdown Language', N'a) HyperText Markup Language', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'dca56061-6b2c-4f7a-aa5e-1e7184683639', N'5910d0a0-6245-4196-aced-2c6024e198c6', N'What is the output of 3 + 2 * 2?', N'a) 7', N'b) 10', N'c) 9', N'd) 8', N'a) 7', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'6e7b48e9-4efb-42de-a354-2a1736c59baf', N'ed98f1ca-1ac1-415a-8ca4-c5cbb0ceecd8', N'Which of the following is the correct syntax to apply a background color in CSS?', N'a) background-color: blue', N'b) bg-color: blue', N'c) color-background: blue', N'd) color-bg: blue', N'a) background-color: blue', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'ef065fbd-3b84-4150-a777-4061befc855b', N'ed98f1ca-1ac1-415a-8ca4-c5cbb0ceecd8', N'What is the correct way to make text bold using CSS?', N'a) font-weight: bold', N'b) font-style: bold', N'c) text-weight: bold', N'd) font-size: bold', N'a) font-weight: bold', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'f749496c-171a-4043-a8e6-41727772429d', N'5910d0a0-6245-4196-aced-2c6024e198c6', N'Which of the following is a mutable data type in Python?', N'a) String', N'b) Tuple', N'c) List', N'd) Integer', N'c) List', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'6f24423e-7d8c-4adb-b114-50282be15d0b', N'dc0227a8-8e2a-48b5-8dc2-ef03c37bfe83', N'Which tag is used to create a paragraph in HTML?', N'a) <p>', N'b) <para>', N'c) <paragraph>', N'd) <pg>', N'a) <p>', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'225e6823-8db4-45e9-ae20-5086217f8879', N'ed98f1ca-1ac1-415a-8ca4-c5cbb0ceecd8', N'Which of the following selectors is used to target an element with a specific ID in CSS?', N'a) .myElement', N'b) #myElement', N'c) *myElement', N'd) &myElement', N'b) #myElement', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'7f18debb-5f9e-42a6-b73c-6f5cfbc1ab0a', N'261fa5ce-5da9-488c-97b1-57e89017621f', N'What is the correct SQL statement to create a new table named Students?', N'a) CREATE Students', N'b) CREATE TABLE Students', N'c) NEW TABLE Students', N'd) ADD TABLE Students', N'b) CREATE TABLE Students', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'965cfeb6-b733-4f85-93b0-8bd22cd4d088', N'261fa5ce-5da9-488c-97b1-57e89017621f', N'Which clause is used to filter the results returned by a SELECT query?', N'a) WHERE', N'b) FILTER', N'c) LIMIT', N'd) GROUP', N'a) WHERE', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'32ed2e7a-9799-4a2f-b625-98b3be8daedd', N'ed98f1ca-1ac1-415a-8ca4-c5cbb0ceecd8', N'How do you add an external CSS file to an HTML document?', N'a) <link rel="stylesheet" type="text/css" href="styles.css">', N'b) <style src="styles.css"></style>', N'c) <stylesheet link="styles.css"></stylesheet>', N'd) <link src="styles.css" type="text/css">', N'a) <link rel="stylesheet" type="text/css" href="styles.css">', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'd17a631c-2d3a-4daf-958a-ac1cf0c82009', N'5910d0a0-6245-4196-aced-2c6024e198c6', N'Which of the following is used to start a loop in Python?', N'a) repeat', N'b) for', N'c) loop', N'd) iterate', N'b) for', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'3eec6c35-a1e2-4ce9-9d36-afeda11e48dc', N'5910d0a0-6245-4196-aced-2c6024e198c6', N'What does the len() function do?', N'a) Returns the number of characters in a string', N'b) Returns the length of a list', N'c) Returns the number of elements in a list', N'd) All of the above', N'd) All of the above', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'abe73709-b1db-4412-9c65-c97591770c6b', N'dc0227a8-8e2a-48b5-8dc2-ef03c37bfe83', N'Which of the following is the correct syntax to create a hyperlink in HTML?', N'a) <a url="http://example.com">Link</a>', N'b) <a href="http://example.com">Link</a>', N'c) <a link="http://example.com">Link</a>', N'd) <link href="http://example.com">Link</link>', N'b) <a href="http://example.com">Link</a>', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'a0105c36-93e0-421f-8858-dfb1cb0c7c5c', N'dc0227a8-8e2a-48b5-8dc2-ef03c37bfe83', N'What is the correct way to add an image in HTML?', N'a) <img src="image.jpg" alt="Image description">', N'b) <image src="image.jpg" description="Image description">', N'c) <img href="image.jpg" text="Image description">', N'd) <image href="image.jpg" alt="Image description">', N'a) <img src="image.jpg" alt="Image description">', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'7eeaf316-cf1c-43ea-bec6-e8f30c72747e', N'dc0227a8-8e2a-48b5-8dc2-ef03c37bfe83', N'Which HTML tag is used to define an unordered list?', N'a) <ul>', N'b) <ol>', N'c) <li>', N'd) <list>', N'a) <ul>', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'2bab579a-222a-4eda-901c-f831fe3b3f43', N'5910d0a0-6245-4196-aced-2c6024e198c6', N'What is the correct way to create a function in Python?', N'a) def functionName:', N'b) function functionName():', N'c) def functionName():', N'd) create function functionName()', N'c) def functionName():', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Question] ([Id], [TopicId], [QuestionText], [Option01], [Option02], [Option03], [Option04], [Answer], [CreatedAt]) VALUES (N'a674f43c-9778-41c9-acac-f9788af13604', N'261fa5ce-5da9-488c-97b1-57e89017621f', N'Which SQL keyword is used to sort the result-set?', N'a) SORT', N'b) ORDER', N'c) ARRANGE', N'd) ORGANIZE', N'b) ORDER', CAST(N'2024-10-04T13:38:21.310' AS DateTime))
GO
INSERT [dbo].[Topics] ([Id], [Name], [OrderNumber], [CreatedAt]) VALUES (N'5910d0a0-6245-4196-aced-2c6024e198c6', N'Python', 1, CAST(N'2024-10-03T12:46:27.110' AS DateTime))
GO
INSERT [dbo].[Topics] ([Id], [Name], [OrderNumber], [CreatedAt]) VALUES (N'261fa5ce-5da9-488c-97b1-57e89017621f', N'MySQL', 3, CAST(N'2024-10-03T12:46:27.110' AS DateTime))
GO
INSERT [dbo].[Topics] ([Id], [Name], [OrderNumber], [CreatedAt]) VALUES (N'ed98f1ca-1ac1-415a-8ca4-c5cbb0ceecd8', N'CSS', 4, CAST(N'2024-10-03T12:46:27.110' AS DateTime))
GO
INSERT [dbo].[Topics] ([Id], [Name], [OrderNumber], [CreatedAt]) VALUES (N'dc0227a8-8e2a-48b5-8dc2-ef03c37bfe83', N'HTML', 2, CAST(N'2024-10-03T12:46:27.110' AS DateTime))
GO
/****** Object:  Index [IX_Question]    Script Date: 09.11.2024 17:03:02 ******/
ALTER TABLE [dbo].[Question] ADD  CONSTRAINT [IX_Question] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
GO
/****** Object:  Index [UQ_Topics]    Script Date: 09.11.2024 17:03:02 ******/
ALTER TABLE [dbo].[Topics] ADD  CONSTRAINT [UQ_Topics] UNIQUE NONCLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 75) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  CONSTRAINT [DF_AspNetUsers_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[LogRecords] ADD  CONSTRAINT [DF_Log_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[LogRecords] ADD  CONSTRAINT [DF_Log_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Question] ADD  CONSTRAINT [DF_Question_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Question] ADD  CONSTRAINT [DF_Question_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[SecurityTokens] ADD  CONSTRAINT [DF_AspNetSecurityTokens_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[SecurityTokens] ADD  CONSTRAINT [DF_AspNetSecurityTokens_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Topics] ADD  CONSTRAINT [DF_Quiz_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[Topics] ADD  CONSTRAINT [DF_Topics_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[UserQuestionProgress] ADD  CONSTRAINT [DF_UserQuestionProgress_Id]  DEFAULT (newid()) FOR [Id]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserTokens]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserTokens] CHECK CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[Question]  WITH CHECK ADD  CONSTRAINT [FK_Question_Quiz] FOREIGN KEY([TopicId])
REFERENCES [dbo].[Topics] ([Id])
GO
ALTER TABLE [dbo].[Question] CHECK CONSTRAINT [FK_Question_Quiz]
GO
ALTER TABLE [dbo].[UserQuestionProgress]  WITH CHECK ADD  CONSTRAINT [FK_UserQuestionProgress_Question] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Question] ([Id])
GO
ALTER TABLE [dbo].[UserQuestionProgress] CHECK CONSTRAINT [FK_UserQuestionProgress_Question]
GO
/****** Object:  StoredProcedure [dbo].[quiz_DatebaseClean]    Script Date: 09.11.2024 17:03:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =========================================================================================
-- Author:		Alexey M. Blinov
-- Create date: 02.11.2024
-- Description:	Удаление уставревших данных из БД для ускорения быстродействия запросов
-- =========================================================================================
CREATE PROCEDURE [dbo].[quiz_DatebaseClean]
AS
BEGIN
	SET NOCOUNT ON;
	-- База должна быть почищена без сбоев, поэтому у этой проедуры самый высокий приоритет в случае возникновения deadlock.
	SET DEADLOCK_PRIORITY HIGH;
	-- Так как при работе с таблицами не происходит отката транзакций ни в одном запросе, можно применить самый низкий уровень изоляции.
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	-- Дата, записи лога ранее которой удаляются.
	declare @logRecordThresholdDatetime as datetime = dateadd(day, -30, getdate())
	-- Дата, Токены безопасности ранее которой удаляются.
	declare @securityTokensThresholdDatetime as datetime = dateadd(hour, -24, getdate())
	
	-- Удаление устаревших записей лога.
	delete
	from [dbo].[LogRecords]
	where [CreatedAt] <= @logRecordThresholdDatetime

	-- Удаление устаревших Токенов безопасности.
	delete
	from [dbo].[SecurityTokens]
	where [CreatedAt] <= @securityTokensThresholdDatetime

	--Сжатие БД.
	DBCC SHRINKDATABASE(N'quiz')
	-- Сжатие основного mdf-файла данных 
	DBCC SHRINKFILE (1, 0, TRUNCATEONLY)
	-- Сжатие файла лога.
	--ALTER DATABASE quiz SET RECOVERY SIMPLE	-- У базы данных и так Simple recovery model 
	DBCC SHRINKFILE(2, 26)
	--ALTER DATABASE quiz SET RECOVERY FULL

	-- Перестроение всех индексов во всех таблицах quiz.
	DECLARE @TableName varchar(255) 

	DECLARE TableCursor CURSOR FOR 
	SELECT [table_name]
	FROM [information_schema].[tables] with (nolock)
	WHERE	[table_type] = 'base table' 
			--and [table_name] not like 'yaf_%'
	OPEN TableCursor 
		FETCH NEXT FROM TableCursor INTO @TableName 
		WHILE @@FETCH_STATUS = 0 
		BEGIN 
			DBCC DBREINDEX(@TableName,' ',75) 
			FETCH NEXT FROM TableCursor INTO @TableName 
		END 
	CLOSE TableCursor 
	DEALLOCATE TableCursor 

	-- Обновление статистики для всех таблиц.
	EXEC sp_updatestats;
END
GO
USE [master]
GO
ALTER DATABASE [quiz] SET  READ_WRITE 
GO
