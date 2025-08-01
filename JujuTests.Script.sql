USE [master]
GO
/****** Object:  Database [JujuTest]    Script Date: 31/07/2025 3:15:28 p. m. ******/
CREATE DATABASE [JujuTest]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'JujuTest', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\JujuTest.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'JujuTest_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\JujuTest_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [JujuTest] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [JujuTest].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [JujuTest] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [JujuTest] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [JujuTest] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [JujuTest] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [JujuTest] SET ARITHABORT OFF 
GO
ALTER DATABASE [JujuTest] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [JujuTest] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [JujuTest] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [JujuTest] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [JujuTest] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [JujuTest] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [JujuTest] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [JujuTest] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [JujuTest] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [JujuTest] SET  ENABLE_BROKER 
GO
ALTER DATABASE [JujuTest] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [JujuTest] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [JujuTest] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [JujuTest] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [JujuTest] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [JujuTest] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [JujuTest] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [JujuTest] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [JujuTest] SET  MULTI_USER 
GO
ALTER DATABASE [JujuTest] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [JujuTest] SET DB_CHAINING OFF 
GO
ALTER DATABASE [JujuTest] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [JujuTest] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [JujuTest] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [JujuTest] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [JujuTest] SET QUERY_STORE = ON
GO
ALTER DATABASE [JujuTest] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [JujuTest]
GO
/****** Object:  User [admin]    Script Date: 31/07/2025 3:15:28 p. m. ******/
CREATE USER [admin] WITHOUT LOGIN WITH DEFAULT_SCHEMA=[dbo]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 31/07/2025 3:15:28 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 31/07/2025 3:15:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 31/07/2025 3:15:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Logs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](4000) NULL,
	[MessageTemplate] [nvarchar](4000) NULL,
	[Level] [nvarchar](50) NULL,
	[TimeStamp] [datetime] NULL,
	[Exception] [nvarchar](4000) NULL,
	[Properties] [nvarchar](4000) NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Post]    Script Date: 31/07/2025 3:15:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Post](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](500) NOT NULL,
	[Body] [nvarchar](500) NOT NULL,
	[Type] [int] NOT NULL,
	[Category] [nvarchar](500) NOT NULL,
	[CustomerId] [int] NOT NULL,
 CONSTRAINT [PK_Post] PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Token]    Script Date: 31/07/2025 3:15:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Token](
	[IdToken] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Token] [varchar](max) NOT NULL,
	[FechaCreacion] [datetime2](7) NOT NULL,
	[FechaVencimiento] [datetime2](7) NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_Token] PRIMARY KEY CLUSTERED 
(
	[IdToken] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 31/07/2025 3:15:29 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[LastName] [varchar](255) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[Rol] [varchar](255) NOT NULL,
	[Phone] [varchar](20) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250731021830_InitialMigracion', N'8.0.18')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20250731021859_InitialUser', N'8.0.18')
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([CustomerId], [Name]) VALUES (1, N'Pedro Navaja')
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Logs] ON 

INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (1, N'Ya existe un cliente con el nombre ''Pedro Navaja''', N'Customer name is required', N'Validation', CAST(N'2025-07-31T04:09:26.253' AS DateTime), N'InvalidOperationException', N'{ "CustomerName": "Pedro Navaja" }')
INSERT [dbo].[Logs] ([Id], [Message], [MessageTemplate], [Level], [TimeStamp], [Exception], [Properties]) VALUES (2, N'Ya existe un cliente con el nombre ''Pedro Navaja''', N'Customer name is required', N'Validation', CAST(N'2025-07-31T14:10:39.683' AS DateTime), N'InvalidOperationException', N'{ "CustomerName": "Pedro Navaja" }')
SET IDENTITY_INSERT [dbo].[Logs] OFF
GO
SET IDENTITY_INSERT [dbo].[Post] ON 

INSERT [dbo].[Post] ([PostId], [Title], [Body], [Type], [Category], [CustomerId]) VALUES (1, N'Example', N'ejemplo de post', 1, N'Farándula', 1)
SET IDENTITY_INSERT [dbo].[Post] OFF
GO
SET IDENTITY_INSERT [dbo].[Token] ON 

INSERT [dbo].[Token] ([IdToken], [IdUsuario], [Token], [FechaCreacion], [FechaVencimiento], [Estado]) VALUES (2, 2, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhbmFmcmFua0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJBbmEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zdXJuYW1lIjoiRnJhbmsiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJ1c2VyIiwiZXhwIjoxNzUzOTMyOTQxfQ.ztROv155sHWLbiZAsePY5dZe8PPnqMeDwzOWSgu3jGM', CAST(N'2025-07-30T21:35:41.7698738' AS DateTime2), CAST(N'2025-07-30T22:35:41.7544742' AS DateTime2), 0)
INSERT [dbo].[Token] ([IdToken], [IdUsuario], [Token], [FechaCreacion], [FechaVencimiento], [Estado]) VALUES (3, 1, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhbGVqb3BlcnR1ekBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJBbGVqbyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3N1cm5hbWUiOiJQZXJ0dXoiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsImV4cCI6MTc1MzkzNTU1OH0.wQWw6nHMr_T66sDstW_IJGr5ux2VdRbbeALijlv6Gnc', CAST(N'2025-07-30T22:19:18.8513725' AS DateTime2), CAST(N'2025-07-30T23:19:18.8195183' AS DateTime2), 0)
INSERT [dbo].[Token] ([IdToken], [IdUsuario], [Token], [FechaCreacion], [FechaVencimiento], [Estado]) VALUES (4, 2, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhbmFmcmFua0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJBbmEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zdXJuYW1lIjoiRnJhbmsiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJ1c2VyIiwiZXhwIjoxNzUzOTM4NTQxfQ.SFybkTXjout28dCsE5-G1yLCGCRUznsFmminzHHBIEc', CAST(N'2025-07-30T23:09:01.4573894' AS DateTime2), CAST(N'2025-07-31T00:09:01.4403129' AS DateTime2), 0)
INSERT [dbo].[Token] ([IdToken], [IdUsuario], [Token], [FechaCreacion], [FechaVencimiento], [Estado]) VALUES (5, 2, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhbmFmcmFua0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJBbmEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zdXJuYW1lIjoiRnJhbmsiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJ1c2VyIiwiZXhwIjoxNzUzOTc0NjE1fQ.TeqSl9eW7X0Jbxt0L4ETUrAQemWE-k8m_U2lxqW0hEA', CAST(N'2025-07-31T09:10:15.8472264' AS DateTime2), CAST(N'2025-07-31T10:10:15.8049436' AS DateTime2), 0)
INSERT [dbo].[Token] ([IdToken], [IdUsuario], [Token], [FechaCreacion], [FechaVencimiento], [Estado]) VALUES (6, 2, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjIiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhbmFmcmFua0BnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJBbmEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9zdXJuYW1lIjoiRnJhbmsiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJ1c2VyIiwiZXhwIjoxNzUzOTkyNTY3fQ.77JICSTaKpDnMPyk3PA5ymrtIMASOvbFgyeJp4rIWPY', CAST(N'2025-07-31T14:09:27.6522633' AS DateTime2), CAST(N'2025-07-31T15:09:27.5786650' AS DateTime2), 1)
INSERT [dbo].[Token] ([IdToken], [IdUsuario], [Token], [FechaCreacion], [FechaVencimiento], [Estado]) VALUES (7, 1, N'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhbGVqb3BlcnR1ekBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9naXZlbm5hbWUiOiJBbGVqbyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3N1cm5hbWUiOiJQZXJ0dXoiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsImV4cCI6MTc1Mzk5NDE4MX0.L-aoz4_iyWS3hetHDhazDwlpZWP2sikGJHCYDulqtjs', CAST(N'2025-07-31T14:36:21.3779084' AS DateTime2), CAST(N'2025-07-31T15:36:21.3132389' AS DateTime2), 1)
SET IDENTITY_INSERT [dbo].[Token] OFF
GO
SET IDENTITY_INSERT [dbo].[User] ON 

INSERT [dbo].[User] ([Id], [Name], [LastName], [Email], [Password], [Rol], [Phone]) VALUES (1, N'Alejo', N'Pertuz', N'alejopertuz@gmail.com', N'123', N'admin', N'+573218899857')
INSERT [dbo].[User] ([Id], [Name], [LastName], [Email], [Password], [Rol], [Phone]) VALUES (2, N'Ana', N'Frank', N'anafrank@gmail.com', N'321', N'user', N'+578899966455')
SET IDENTITY_INSERT [dbo].[User] OFF
GO
/****** Object:  Index [IX_Post_CustomerId]    Script Date: 31/07/2025 3:15:29 p. m. ******/
CREATE NONCLUSTERED INDEX [IX_Post_CustomerId] ON [dbo].[Post]
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Post]  WITH CHECK ADD  CONSTRAINT [FK_Post_Customer_CustomerId] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Post] CHECK CONSTRAINT [FK_Post_Customer_CustomerId]
GO
USE [master]
GO
ALTER DATABASE [JujuTest] SET  READ_WRITE 
GO
