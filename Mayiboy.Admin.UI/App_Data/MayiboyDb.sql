USE [MayiboyDb]
GO
/****** Object:  StoredProcedure [dbo].[proc_SystemMenuByNavbarId_select]    Script Date: 2018/4/24 1:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[proc_SystemMenuByNavbarId_select] 
	-- Add the parameters for the stored procedure here
@NavbarId int
AS
BEGIN
	 WITH t AS
	(
		SELECT * FROM dbo.SystemMenu WHERE NavbarId=@NavbarId AND IsValid=1
		UNION ALL
		SELECT a.* FROM dbo.SystemMenu AS a INNER JOIN t AS b ON a.Pid=b.Id
	)

	SELECT * from t
END

GO
/****** Object:  StoredProcedure [dbo].[proc_SystemMenuByUserIdandNavbarId_select]    Script Date: 2018/4/24 1:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[proc_SystemMenuByUserIdandNavbarId_select]
@NavbarId INT,
@UserId INT
AS
BEGIN
	 WITH t AS
	(
		SELECT * FROM dbo.SystemMenu WHERE NavbarId=@NavbarId AND IsValid=1
		UNION ALL
		SELECT a.* FROM dbo.SystemMenu AS a INNER JOIN t AS b ON a.Pid=b.Id
	)

	SELECT * from t

	RETURN @UserId
END

GO
/****** Object:  Table [dbo].[Department]    Script Date: 2018/4/24 1:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Department](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Pid] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[Remark] [varchar](500) NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 2018/4/24 1:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Permissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Code] [varchar](50) NULL,
	[Type] [int] NOT NULL,
	[Remark] [varchar](500) NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SystemAppSettings]    Script Date: 2018/4/24 1:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemAppSettings](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](500) NULL,
	[KeyWord] [varchar](100) NOT NULL,
	[KeyValue] [varchar](1024) NULL,
	[Remark] [varchar](500) NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_SystemAppSettings] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SystemMenu]    Script Date: 2018/4/24 1:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemMenu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Pid] [int] NULL,
	[NavbarId] [int] NULL,
	[Name] [varchar](50) NULL,
	[UrlAddress] [varchar](500) NULL,
	[MenuType] [int] NOT NULL,
	[Icon] [varchar](50) NULL,
	[Sort] [int] NOT NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[Remark] [varchar](500) NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_SystemMenu] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SystemNavbar]    Script Date: 2018/4/24 1:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemNavbar](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Url] [varchar](500) NULL,
	[Remark] [varchar](500) NULL,
	[Sort] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[CreateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[UpdateUserId] [int] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_SystemNavbar] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SystemOperationLog]    Script Date: 2018/4/24 1:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemOperationLog](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Content] [varchar](1024) NULL,
	[Type] [int] NOT NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_SystemOperationLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 2018/4/24 1:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserInfo](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[LoginName] [varchar](200) NOT NULL,
	[Password] [varchar](200) NOT NULL,
	[Email] [varchar](500) NULL,
	[Name] [varchar](50) NULL,
	[HeadimgUrl] [varchar](500) NULL,
	[Sex] [int] NULL,
	[Mobile] [varchar](50) NULL,
	[HomeAddress] [varchar](500) NULL,
	[CreateUserId] [int] NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NOT NULL,
	[Remark] [varchar](500) NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_UserInfo] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 2018/4/24 1:57:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserRole](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Remark] [varchar](500) NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[SystemAppSettings] ON 

INSERT [dbo].[SystemAppSettings] ([Id], [Name], [KeyWord], [KeyValue], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (2, N'系统用户默认密码', N'SystemDefaultPassword', N'123456', NULL, 0, CAST(0x0000A8CC0011664C AS DateTime), NULL, NULL, 1)
INSERT [dbo].[SystemAppSettings] ([Id], [Name], [KeyWord], [KeyValue], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (4, N'系统用户默认头像', N'SystemDefaultHeadimg', N'Content/Images/defaultimg.jpg', NULL, 0, CAST(0x0000A8CC0011664C AS DateTime), NULL, NULL, 1)
SET IDENTITY_INSERT [dbo].[SystemAppSettings] OFF
SET IDENTITY_INSERT [dbo].[SystemMenu] ON 

INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (2, 0, 2, N'系统管理', NULL, 0, N'fa fa-desktop', 1, 0, CAST(0x0000A8C6017DA056 AS DateTime), 0, CAST(0x0000A8C6017DA056 AS DateTime), N'系统菜单', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (3, 0, 2, N'单位组织', NULL, 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C6017DA056 AS DateTime), 0, CAST(0x0000A8C6017DA056 AS DateTime), N'单位组织', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (4, 2, 0, N'通用字典', N'', 0, N'fa fa-book', 5, 0, CAST(0x0000A8C6017DA056 AS DateTime), 0, CAST(0x0000A8C6017DA056 AS DateTime), N'通用字典', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (5, 2, 0, N'系统日志', N'', 0, N'fa fa-navicon', 4, 0, CAST(0x0000A8C6017DA056 AS DateTime), 0, CAST(0x0000A8C6017DA056 AS DateTime), N'系统日志', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (6, 2, 0, N'数据管理', N'', 0, N'fa fa-navicon', 3, 0, CAST(0x0000A8C6017DA056 AS DateTime), 0, CAST(0x0000A8C6017DA056 AS DateTime), N'数据管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (7, 6, 0, N'数据库备份', N'', 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C6017DA056 AS DateTime), 0, CAST(0x0000A8C6017DA056 AS DateTime), N'数据库备份', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (8, 6, 0, N'数据表管理', N'', 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C6017DA056 AS DateTime), 0, CAST(0x0000A8C6017DA056 AS DateTime), N'数据表管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (9, 3, 0, N'部门管理', N'', 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C601838468 AS DateTime), 0, CAST(0x0000A8C601838468 AS DateTime), N'部门管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (10, 0, 3, N'基础管理', N'', 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C601838468 AS DateTime), 0, CAST(0x0000A8C601838468 AS DateTime), N'基础管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (11, 10, 0, N'公众号管理', N'', 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C601838468 AS DateTime), 0, CAST(0x0000A8C601838468 AS DateTime), N'公众号管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (13, 2, 0, N'栏目管理', N'SystemManage/SysNavbar/Index', 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C80014B46E AS DateTime), 0, CAST(0x0000A8C80014B46E AS DateTime), N'栏目管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (14, 2, 0, N'菜单管理', N'SystemManage/SysMenu/Index', 0, N'fa fa-navicon', 2, 0, CAST(0x0000A8C80014B46E AS DateTime), 0, CAST(0x0000A8C80014B46E AS DateTime), N'菜单管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (16, 2, 0, N'角色管理', N'SystemManage/UserRole/Index', 0, N'fa fa-navicon', 2, 0, CAST(0x0000A8C9012FE0D5 AS DateTime), 0, CAST(0x0000A8C9012FE0D5 AS DateTime), N'角色管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (17, 2, 0, N'用户管理', N'SystemManage/UserInfo/Index', 0, N'fa fa-navicon', 2, 0, CAST(0x0000A8C9012FE0D5 AS DateTime), 0, CAST(0x0000A8C9012FE0D5 AS DateTime), N'用户管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (22, 2, 0, N'部门管理', N'SystemManage/Department/Index', 0, N'fa fa-navicon', 2, 0, CAST(0x0000A8C9012FE0D5 AS DateTime), 0, CAST(0x0000A8C9012FE0D5 AS DateTime), N'部门管理', 1)
SET IDENTITY_INSERT [dbo].[SystemMenu] OFF
SET IDENTITY_INSERT [dbo].[SystemNavbar] ON 

INSERT [dbo].[SystemNavbar] ([Id], [Name], [Url], [Remark], [Sort], [CreateTime], [CreateUserId], [UpdateTime], [UpdateUserId], [IsValid]) VALUES (2, N'系统配置', N'', N'主要配置用户基本信息、权限，常量配置', 1, CAST(0x0000A8C50171B30D AS DateTime), NULL, CAST(0x0000A8CA011E8643 AS DateTime), NULL, 1)
INSERT [dbo].[SystemNavbar] ([Id], [Name], [Url], [Remark], [Sort], [CreateTime], [CreateUserId], [UpdateTime], [UpdateUserId], [IsValid]) VALUES (3, N'微信平台', NULL, N'微信相关', 2, CAST(0x0000A8C50171B30D AS DateTime), NULL, CAST(0x0000A8CA003D76D7 AS DateTime), NULL, 1)
INSERT [dbo].[SystemNavbar] ([Id], [Name], [Url], [Remark], [Sort], [CreateTime], [CreateUserId], [UpdateTime], [UpdateUserId], [IsValid]) VALUES (7, N'测试', N'', N'测试', 7, CAST(0x0000A8CB002593FB AS DateTime), 1, CAST(0x0000A8CB0025C100 AS DateTime), 1, 0)
SET IDENTITY_INSERT [dbo].[SystemNavbar] OFF
SET IDENTITY_INSERT [dbo].[UserInfo] ON 

INSERT [dbo].[UserInfo] ([Id], [LoginName], [Password], [Email], [Name], [HeadimgUrl], [Sex], [Mobile], [HomeAddress], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (1, N'admin', N'E10ADC3949BA59ABBE56E057F20F883E', N'system@mayiboy.com', N'蚂蚁男孩', N'Content/Images/headimg.png', 1, N'****', N'*****', 0, CAST(0x0000A8C10021CD26 AS DateTime), 6, CAST(0x0000A8CC00039E53 AS DateTime), N'超级管理员', 1)
SET IDENTITY_INSERT [dbo].[UserInfo] OFF
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级主键Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Pid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Department'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统配置名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings', @level2type=N'COLUMN',@level2name=N'KeyWord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings', @level2type=N'COLUMN',@level2name=N'KeyValue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改用户' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'字典配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemAppSettings'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'Pid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'NavbarId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Url地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'UrlAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'MenuType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'图标' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'Icon'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排序' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'Sort'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统菜单' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemNavbar', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemNavbar', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemNavbar', @level2type=N'COLUMN',@level2name=N'Url'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemNavbar', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemNavbar', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemNavbar', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemNavbar', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemNavbar', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemNavbar', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'导航栏' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemNavbar'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemOperationLog', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作内容说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemOperationLog', @level2type=N'COLUMN',@level2name=N'Content'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型（1:登录；2：退出；3：其他操作）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemOperationLog', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemOperationLog', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemOperationLog', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemOperationLog', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'系统操作日志' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemOperationLog'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户信息主键id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密文手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'Mobile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'家庭地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'HomeAddress'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户信息' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户角色主键Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效;1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRole', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
USE [master]
GO
ALTER DATABASE [MayiboyDb] SET  READ_WRITE 
GO
