USE [RightDb]
GO
ALTER TABLE [dbo].[RoleInfos] DROP CONSTRAINT [DF__RoleInfos__IsAdm__34C8D9D1]
GO
/****** Object:  Table [dbo].[UserRoleInfos]    Script Date: 2021/6/10 9:07:21 ******/
DROP TABLE [dbo].[UserRoleInfos]
GO
/****** Object:  Table [dbo].[UserInfos]    Script Date: 2021/6/10 9:07:21 ******/
DROP TABLE [dbo].[UserInfos]
GO
/****** Object:  Table [dbo].[RoleMenuInfos]    Script Date: 2021/6/10 9:07:21 ******/
DROP TABLE [dbo].[RoleMenuInfos]
GO
/****** Object:  Table [dbo].[RoleInfos]    Script Date: 2021/6/10 9:07:21 ******/
DROP TABLE [dbo].[RoleInfos]
GO
/****** Object:  Table [dbo].[MenuInfos]    Script Date: 2021/6/10 9:07:21 ******/
DROP TABLE [dbo].[MenuInfos]
GO
/****** Object:  Table [dbo].[MenuInfos]    Script Date: 2021/6/10 9:07:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MenuInfos](
	[MenuId] [int] IDENTITY(1,1) NOT NULL,
	[MenuName] [nvarchar](50) NULL,
	[ParentId] [int] NULL,
	[FrmName] [nvarchar](50) NULL,
	[MKey] [varchar](50) NULL,
 CONSTRAINT [PK_MenuInfos] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleInfos]    Script Date: 2021/6/10 9:07:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleInfos](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NULL,
	[Remark] [nvarchar](500) NULL,
	[IsAdmin] [int] NOT NULL,
 CONSTRAINT [PK_RoleInfos] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RoleMenuInfos]    Script Date: 2021/6/10 9:07:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RoleMenuInfos](
	[RMId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[MenuIds] [varchar](100) NULL,
 CONSTRAINT [PK_RoleMenuInfos] PRIMARY KEY CLUSTERED 
(
	[RMId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserInfos]    Script Date: 2021/6/10 9:07:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserInfos](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NULL,
	[UserPwd] [varchar](50) NULL,
 CONSTRAINT [PK_UserInfos] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserRoleInfos]    Script Date: 2021/6/10 9:07:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleInfos](
	[URId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[RoleId] [int] NULL,
 CONSTRAINT [PK_UserRoleInfos] PRIMARY KEY CLUSTERED 
(
	[URId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[RoleInfos] ADD  DEFAULT ((0)) FOR [IsAdmin]
GO

