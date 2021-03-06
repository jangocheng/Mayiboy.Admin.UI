USE [master]
GO
/****** Object:  Database [MayiboyDb]    Script Date: 2018/7/4 23:06:26 ******/
CREATE DATABASE [MayiboyDb]

USE [MayiboyDb]
GO
/****** Object:  StoredProcedure [dbo].[proc_PermissionByUserId_select]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*
==========================================================================
Author:	闲僧
Create date: 2018-4-3
Description:根据用户Id查询用户权限
==========================================================================
*/

CREATE PROCEDURE [dbo].[proc_PermissionByUserId_select]
@UserId int
AS
BEGIN
	--1、查询用户所属权限

	--2、查询用户所属角色权限
	WITH userrolep AS (
	SELECT p.* FROM UserRoleJoin urj 
		LEFT JOIN dbo.RolePermissionsJoin rpj ON urj.RoleId=rpj.RoleId 
		LEFT JOIN dbo.Permissions p ON rpj.PermissionsId=p.Id
	WHERE urj.IsValid=1 AND rpj.IsValid=1 AND p.IsValid=1 and urj.UserId=@UserId
	)

	--3、查询用户拒绝权限[待开发延伸]
	
	SELECT DISTINCT * FROM userrolep
END

GO
/****** Object:  StoredProcedure [dbo].[proc_SystemDepartmentById_select]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
==========================================================================
Author:	闲僧
Create date: 2018-4-3
Description:根据部门Id查询下面的所有部门，包含自己
==========================================================================
*/
CREATE PROCEDURE [dbo].[proc_SystemDepartmentById_select]
@Id int
AS
BEGIN

WITH t AS
	(
		SELECT * FROM dbo.Department WITH(NOLOCK) WHERE Pid=@Id AND IsValid=1
		UNION ALL
		SELECT * FROM dbo.Department WITH(NOLOCK) WHERE Id=@Id AND IsValid=1
		UNION ALL
		SELECT a.* FROM dbo.Department AS a WITH(NOLOCK) INNER JOIN t AS b ON a.Pid=b.Id AND a.IsValid=1
	)

	SELECT DISTINCT * from t ORDER BY t.Id
END

GO
/****** Object:  StoredProcedure [dbo].[proc_SystemMenuById_select]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
==========================================================================
Author:	闲僧
Create date: 2018-4-3
Description:根据菜单Id查询子菜单包含自己
==========================================================================
*/
CREATE PROCEDURE [dbo].[proc_SystemMenuById_select]
@Id int
AS
BEGIN
 WITH t AS
	(
		SELECT * FROM dbo.SystemMenu WITH(NOLOCK) WHERE Id=@Id AND IsValid=1
		UNION ALL
		SELECT a.* FROM dbo.SystemMenu AS a WITH(NOLOCK) INNER JOIN t AS b ON a.Pid=b.Id AND a.IsValid=1
	)

	SELECT DISTINCT * from t ORDER BY t.Sort
END

GO
/****** Object:  StoredProcedure [dbo].[proc_SystemMenuByNavbarId_select]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
==========================================================================
Author:	闲僧
Create date: 2018-4-3
Description:查询栏目所有菜单
==========================================================================
*/
CREATE PROCEDURE [dbo].[proc_SystemMenuByNavbarId_select] 
	-- Add the parameters for the stored procedure here
@NavbarId int
AS
BEGIN
	 WITH t AS
	(
		SELECT * FROM dbo.SystemMenu WITH(NOLOCK) WHERE NavbarId=@NavbarId AND IsValid=1
		UNION ALL
		SELECT a.* FROM dbo.SystemMenu AS a WITH(NOLOCK) INNER JOIN t AS b ON a.Pid=b.Id AND a.IsValid=1
	)

	SELECT DISTINCT * from t ORDER BY t.Sort
END

GO
/****** Object:  StoredProcedure [dbo].[proc_SystemMenuByUserIdandNavbarId_select]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
==========================================================================
Author:	闲僧
Create date: 2018-4-3
Description:查询用户 栏目所有系统菜单
==========================================================================
*/
CREATE PROCEDURE [dbo].[proc_SystemMenuByUserIdandNavbarId_select]
	@NavbarId INT,
	@UserId INT
AS
BEGIN

		WITH sysmenu AS (
				SELECT sm.* FROM UserRoleJoin urj 
					LEFT JOIN dbo.UserRole ur ON urj.RoleId=ur.Id
					LEFT JOIN dbo.RolePermissionsJoin rpj ON ur.Id=rpj.RoleId
					LEFT JOIN dbo.Permissions p ON rpj.PermissionsId=p.Id 
					LEFT JOIN dbo.SystemMenu sm ON p.MenuId=sm.Id
				WHERE urj.IsValid=1 
					AND ur.IsValid=1 
					AND rpj.IsValid=1 
					AND p.IsValid=1 
					AND sm.IsValid=1 
					AND p.IsValid=1 
					AND urj.UserId=@UserId AND sm.NavbarId=@NavbarId
		), tb AS(
			SELECT DISTINCT * FROM sysmenu
		), t AS
		(
			select * from tb WITH(nolock) where Id IN (SELECT Id FROM tb)
			union all
			select a.* from SystemMenu AS a WITH(nolock) inner join t AS b on a.Id=b.Pid WHERE a.Pid =0
		)
 
		SELECT DISTINCT * from t
END

GO
/****** Object:  StoredProcedure [dbo].[proc_SystemNavbarByUserId_select]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
==========================================================================
Author:	闲僧
Create date: 2018-4-3
Description:查询用户所有的栏目
==========================================================================
*/
CREATE PROCEDURE [dbo].[proc_SystemNavbarByUserId_select]
@UserId	 int
AS
BEGIN

	WITH sysnavbar AS
	 (
		SELECT sn.* FROM dbo.SystemNavbar sn 
			LEFT JOIN dbo.SystemMenu sm ON sn.Id=sm.NavbarId
			LEFT JOIN dbo.Permissions p ON sm.Id=p.MenuId
			LEFT JOIN dbo.RolePermissionsJoin rpj ON p.Id=rpj.PermissionsId
			LEFT JOIN dbo.UserRole ur ON rpj.RoleId=ur.Id
			LEFT JOIN dbo.UserRoleJoin urj ON ur.Id=urj.RoleId
			LEFT JOIN dbo.UserInfo ui ON urj.UserId=ui.Id
		WHERE ui.IsValid=1 
			AND urj.IsValid=1 
			AND ur.IsValid=1 
			AND rpj.IsValid=1 
			AND p.IsValid=1 
			AND  sm.IsValid=1 
			AND sn.IsValid=1 
			AND ui.Id=@UserId
		)


		SELECT DISTINCT * FROM sysnavbar
END

GO
/****** Object:  Table [dbo].[AppIdAuth]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AppIdAuth](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AppId] [varchar](50) NOT NULL,
	[AuthToken] [varchar](50) NOT NULL,
	[EncryptionType] [int] NOT NULL,
	[SecretKey] [varchar](50) NULL,
	[PrivateKey] [varchar](2048) NULL,
	[PublicKey] [varchar](2048) NULL,
	[Status] [int] NOT NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[Remark] [varchar](500) NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_AppIdAuth] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AppProject]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AppProject](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProjectName] [varchar](100) NOT NULL,
	[ApplicationId] [varchar](50) NOT NULL,
	[Remark] [varchar](500) NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_AppProject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[City]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[City](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Pid] [int] NOT NULL,
	[CityName] [varchar](50) NOT NULL,
	[ZipCode] [varchar](50) NULL,
	[Remark] [varchar](500) NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_City] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Department]    Script Date: 2018/7/4 23:06:26 ******/
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
/****** Object:  Table [dbo].[Permissions]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Permissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MenuId] [int] NULL,
	[Name] [varchar](50) NULL,
	[Action] [varchar](500) NULL,
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
/****** Object:  Table [dbo].[RolePermissionsJoin]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RolePermissionsJoin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NavbarId] [int] NOT NULL,
	[MenuId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[PermissionsId] [int] NOT NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_RolePermissionsJoin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SystemAppSettings]    Script Date: 2018/7/4 23:06:26 ******/
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
/****** Object:  Table [dbo].[SystemMenu]    Script Date: 2018/7/4 23:06:26 ******/
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
	[AddressAuth] [int] NULL,
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
/****** Object:  Table [dbo].[SystemNavbar]    Script Date: 2018/7/4 23:06:26 ******/
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
/****** Object:  Table [dbo].[SystemOperationLog]    Script Date: 2018/7/4 23:06:26 ******/
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
/****** Object:  Table [dbo].[UserAppIdAuth]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserAppIdAuth](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[AppId] [varchar](50) NOT NULL,
	[Remark] [varchar](500) NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_UserAppIdAuth] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserDepartmentJoin]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserDepartmentJoin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[DepartmentId] [int] NOT NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_UserDepartmentJoin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserInfo]    Script Date: 2018/7/4 23:06:26 ******/
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
	[DepartmentId] [int] NULL,
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
/****** Object:  Table [dbo].[UserRole]    Script Date: 2018/7/4 23:06:26 ******/
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
/****** Object:  Table [dbo].[UserRoleJoin]    Script Date: 2018/7/4 23:06:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleJoin](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
	[IsValid] [int] NOT NULL,
 CONSTRAINT [PK_UserRoleJoin] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[AppIdAuth] ON 

INSERT [dbo].[AppIdAuth] ([Id], [AppId], [AuthToken], [EncryptionType], [SecretKey], [PrivateKey], [PublicKey], [Status], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (1, N'000001', N'2efd0f58295249b0846be6f45b8afaa3', 1, N'7edbc2e9cb6748c8b070933e0dc9c792', N'PFJTQUtleVZhbHVlPjxNb2R1bHVzPjM3SCtBRzAyeHExT05VRTRsbS9BbytsL1QyZStMeFcrbWQ2TS9GeElYbjdTd28xbVQ3amM5dHdPTk9HWHgrNG5iVXE2bjVoSHNyditMNW5LRUNRYTdQaW13S2YwOWxDRjdHcy8zYjd3TzJ6UU5ud1JBdHR6RFphYUxRTGpxS1gwVFNiaUVCQ3ZWckl5MUliUGJCWmtSM243WjFRWXhxVkFnYmZYSDhtNXc3RT08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjxQPjhXSnBoNitMVVl4M0NPN3N6WnBNUklZQUxiQW11eHk1ZE1SK1hkaUVyYVdCQ0VBcWw0OE9wdUY1NzNWY0Zsb2Z5WkFRcFozbXcyRWhhVXFIL1BpQjd3PT08L1A+PFE+N1QxamlYU0xZdUJlRng1UUpwajlZRm5yWFFuam1rV2lzcnlBUy9tbDJib2ZGTGpTZ21ENUpQM255WTlmQ01Tb1dDNjcvKzVTSTZveHdaVUd1N00wWHc9PTwvUT48RFA+cmFlTEhCNWlNQ3pKaGhlOUxMMFFtVXQ5M1dDWXZJd1JyN3VjM1k3akJMbklkbE1UVVMyTjhPak5mRVFHRjk2R2Q4KzQ2aTBLMHd5UnFOb0JzekJrNlE9PTwvRFA+PERRPjNxcDZyUDIxQWFYQ3lnSUtpbCtYbVFKc2xSTitDME1HbEttMTVSazRuK1UvM013bjBIVkNBb1ZKTXZZYzVYYm01blZld0xTblpaQU5TVHNkYWJpbUV3PT08L0RRPjxJbnZlcnNlUT5qV3lQQ2hYaDdCN1c4ZUMyY3p3aFdJVVAvS0lFajBmU2FtL3V6S0g1Mnh5ZjBoWFRiUytDSm9LQ0dybGdIRlZ4YS9GWmsxWlF4SmpwN2NpVSsvN1pyZz09PC9JbnZlcnNlUT48RD5UWGV1cHljcldSS2Jpd1hUT3dYb2lOdE1JNHVoNlZ4MmJZTU5BRGhWTjZSa0NNdy9La3l5K1Nxc0lOUWZaRFd0b2NQSGJZMm5CdGJqNkVneUdmVG0vWlE5NUEzT3VDR3U3c296OWtzL3M5dXg4OEl3aEZDcFowazZSenJCUEJRTEdRSGRuWnVtRlFUZTBWdERYcFhEazU1VEhwNWtQcElvaUw1NDNkd3lLYUU9PC9EPjwvUlNBS2V5VmFsdWU+', N'PFJTQUtleVZhbHVlPjxNb2R1bHVzPjM3SCtBRzAyeHExT05VRTRsbS9BbytsL1QyZStMeFcrbWQ2TS9GeElYbjdTd28xbVQ3amM5dHdPTk9HWHgrNG5iVXE2bjVoSHNyditMNW5LRUNRYTdQaW13S2YwOWxDRjdHcy8zYjd3TzJ6UU5ud1JBdHR6RFphYUxRTGpxS1gwVFNiaUVCQ3ZWckl5MUliUGJCWmtSM243WjFRWXhxVkFnYmZYSDhtNXc3RT08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjwvUlNBS2V5VmFsdWU+', 1, 8, CAST(0x0000A8EF005B4562 AS DateTime), 1, CAST(0x0000A8FB000BB5F3 AS DateTime), NULL, 1)
INSERT [dbo].[AppIdAuth] ([Id], [AppId], [AuthToken], [EncryptionType], [SecretKey], [PrivateKey], [PublicKey], [Status], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (4, N'000002', N'77599f9bb85445289dda2186804342b6', 3, N'dda363ea125143bda15a20aa7049ad83', N'PFJTQUtleVZhbHVlPjxNb2R1bHVzPm82QjRCSGcvT0IxUWl6OW9UK0VBaWZFYlAzR2NMQ0VrVktTcmpscE9oRmVzb2NueDRROGJZZFY0Q2U2a1JIQmxURVhuUDdnS0d3TXl4N250MUZVMFRqZnN1T21TbTFIcVcvTXp3ZjRDSTl4V3VwMWlGVzM0elpvM0QzMFMrZ1VsV1plM2Q2VkNsQ2dhS2hORnQ5WG9pWGpoYUdRdEJsMFBLY29WN0p0UzZPMD08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjxQPjVLcUxFYk8rVFJrK0Y4cExkMTRybGxTS0pYdHRTcjZDQjJYWGhSVWxHN1VPTkEwakJ5YUVxNi9EaVlpS1VEWTJEcU4yNGFwQWdSWGsvL1RkRzh4WWlRPT08L1A+PFE+dHkrazVxZ21DbzhSeGsrVHcwd0hTTllFWFJSN0g4V2dQTnVZM1lMNG9zcXJpWUlkZDlCb3h1a1ZpOEluLzU1OTNQTGxmK1NaOU5uQStWcUtMT3VzUlE9PTwvUT48RFA+UkR1dnErU2EwSjdMcklXczV6MnVlL2xXRDhFNDU2alpYQ1BEbis2aldCVTYyOFZmd201NEFGTStZZmZnYmZiL1FuYjhybklETllHM0I3bWxrUG4xSVE9PTwvRFA+PERRPlBaMFZ6c3NCdUkxMGdnb2ovZzBsYWhjcDhwQy85T2pJdzQ5czlCeG9Pb0VHZjBLM1hLUkppWlcrUEhxNmIxekVvcnREKytkTTdURU1taEljNVkwRExRPT08L0RRPjxJbnZlcnNlUT5ieFJOTStBcGpJWEpJeTNuYzZKWGtSQ2ZjV3k5Y0RYUGJoc3ZJMjl5REl4a2RjY3RCTS9ZQm5kcWMzdnFoWTlhNFJZMlhucmNTSVFOdStTVW5HOVRDZz09PC9JbnZlcnNlUT48RD5TWmtXSXJ5TGd3djhmSVdxdjdNdThOellLck5KVWJnZWhVd0VObVY4UjU2LzhZM0tGRGNkVStZV3RqRDNCaW1nQnF5eWxVQ01teFhYTTJyNEJMZkdiR2NQRnpXWUxMbG9GOU10NzRkNStOV0hsdGNCam1tdGdwdUpmV1VGdWlxMHVMQUJ3cnlnN24xWml6cVNQQ3ZRcEErSG9BRlZiOTNubVB6QkxwcTNQTUU9PC9EPjwvUlNBS2V5VmFsdWU+', N'PFJTQUtleVZhbHVlPjxNb2R1bHVzPm82QjRCSGcvT0IxUWl6OW9UK0VBaWZFYlAzR2NMQ0VrVktTcmpscE9oRmVzb2NueDRROGJZZFY0Q2U2a1JIQmxURVhuUDdnS0d3TXl4N250MUZVMFRqZnN1T21TbTFIcVcvTXp3ZjRDSTl4V3VwMWlGVzM0elpvM0QzMFMrZ1VsV1plM2Q2VkNsQ2dhS2hORnQ5WG9pWGpoYUdRdEJsMFBLY29WN0p0UzZPMD08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjwvUlNBS2V5VmFsdWU+', 1, 1, CAST(0x0000A8EF006CE483 AS DateTime), 1, CAST(0x0000A908000384BB AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[AppIdAuth] OFF
SET IDENTITY_INSERT [dbo].[AppProject] ON 

INSERT [dbo].[AppProject] ([Id], [ProjectName], [ApplicationId], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (1, N'通知项目', N'00001', N'统用消息通知', 1, CAST(0x0000A9130176675A AS DateTime), 1, CAST(0x0000A913017B49AF AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[AppProject] OFF
SET IDENTITY_INSERT [dbo].[Department] ON 

INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (1, 0, N'技术部', N'技术部', 1, CAST(0x0000A8D200B15035 AS DateTime), 1, CAST(0x0000A8D200B15035 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (2, 1, N'官网', N'处理公司官网业务', 1, CAST(0x0000A8D200B15035 AS DateTime), 1, CAST(0x0000A90D01546388 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (3, 1, N'物流', N'物流系统开发', 1, CAST(0x0000A8D200B15035 AS DateTime), 1, CAST(0x0000A90D0154E599 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (4, 0, N'销售部', N'销售部', 1, CAST(0x0000A8D200B15035 AS DateTime), 1, CAST(0x0000A8D200B15035 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (5, 0, N'客服部', N'客服部', 1, CAST(0x0000A8D200B15035 AS DateTime), 1, CAST(0x0000A8D200B15035 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (6, 0, N'财务部', N'财务部', 1, CAST(0x0000A8D200B15035 AS DateTime), 1, CAST(0x0000A8D200B15035 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (7, 0, N'人事部', N'人事部', 1, CAST(0x0000A8D200B15035 AS DateTime), 1, CAST(0x0000A8D2012BE272 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (8, 0, N'法务部', N'法务部', 1, CAST(0x0000A8D200B15035 AS DateTime), 1, CAST(0x0000A8D2012BDEBC AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (13, 1, N'产品部', N'', 1, CAST(0x0000A90D01545AEB AS DateTime), 1, CAST(0x0000A90D015477D4 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (14, 1, N'测试部', N'', 1, CAST(0x0000A90D0154706F AS DateTime), 1, CAST(0x0000A90D0154706F AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (15, 0, N'市场部', N'', 1, CAST(0x0000A90D015490A1 AS DateTime), 1, CAST(0x0000A90D015490A1 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (16, 1, N'运维部', N'', 1, CAST(0x0000A90D0154AA68 AS DateTime), 1, CAST(0x0000A90D0156D08B AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (17, 16, N'网络运维', N'', 1, CAST(0x0000A90D0154F9A4 AS DateTime), 1, CAST(0x0000A90D0154F9A4 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (18, 16, N'应用运维', N'', 1, CAST(0x0000A90D0155044A AS DateTime), 1, CAST(0x0000A90D0155044A AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (19, 1, N'BI部门', N'', 1, CAST(0x0000A90D01552893 AS DateTime), 1, CAST(0x0000A90D01552893 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (20, 1, N'移动部', N'', 1, CAST(0x0000A90D0155A7E7 AS DateTime), 1, CAST(0x0000A90D0155A7E7 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (21, 7, N'招聘组', N'', 1, CAST(0x0000A90D01563109 AS DateTime), 1, CAST(0x0000A90D01563109 AS DateTime), 1)
INSERT [dbo].[Department] ([Id], [Pid], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (22, 7, N'公司文化', N'', 1, CAST(0x0000A90D01564C16 AS DateTime), 1, CAST(0x0000A90D01564C16 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Department] OFF
SET IDENTITY_INSERT [dbo].[Permissions] ON 

INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (5, 13, N'保存栏目', N'SysNavbar/Save', N'P1805025216', 0, N'编辑栏目', 1, CAST(0x0000A8D401242302 AS DateTime), 1, CAST(0x0000A8D80074E772 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (6, 13, N'删除栏目', N'SysNavbar/Del', N'P1805025243', 0, N'删除栏目删除栏目删除栏目删除栏目', 1, CAST(0x0000A8D401244225 AS DateTime), 1, CAST(0x0000A8D7011C3028 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (19, 13, N'查询栏目', N'SysNavbar/Query', N'P1805060347', 0, N'查询栏目', 1, CAST(0x0000A8D800747C48 AS DateTime), 1, CAST(0x0000A8D800747C48 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (20, 14, N'查询菜单', N'SysMenu/Query', N'P1805060505', 0, NULL, 1, CAST(0x0000A8D80074C43C AS DateTime), 1, CAST(0x0000A8D80074C43C AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (21, 14, N'保存菜单', N'SysMenu/Save', N'P1805060614', 0, NULL, 1, CAST(0x0000A8D8007513A4 AS DateTime), 1, CAST(0x0000A8D8007513A4 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (22, 14, N'删除系统菜单', N'SysMenu/Del', N'P1805060637', 0, NULL, 1, CAST(0x0000A8D800752E8B AS DateTime), 1, CAST(0x0000A8D800752E8B AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (23, 14, N'保存菜单权限', N'SysMenu/SaveMenuPermissions', N'P1805060727', 0, NULL, 1, CAST(0x0000A8D8007573DB AS DateTime), 1, CAST(0x0000A8D8007573DB AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (24, 14, N'删除菜单权限', N'SysMenu/DelPermissions', N'P1805060745', 0, NULL, 1, CAST(0x0000A8D800759764 AS DateTime), 1, CAST(0x0000A8D800759764 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (25, 16, N'查询角色', N'UserRole/Query', N'P1805060854', 0, NULL, 1, CAST(0x0000A8D80075D148 AS DateTime), 1, CAST(0x0000A8D80075E024 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (26, 16, N'保存用户角色', N'UserRole/Save', N'P1805060937', 0, NULL, 1, CAST(0x0000A8D80076011F AS DateTime), 1, CAST(0x0000A8D80076011F AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (27, 16, N'删除用户角色', N'UserRole/Del', N'P1805061007', 0, NULL, 1, CAST(0x0000A8D80076252B AS DateTime), 1, CAST(0x0000A8D80076252B AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (28, 16, N'保存角色权限', N'UserRole/SaveRolePermissions', N'P1805061028', 0, NULL, 1, CAST(0x0000A8D800763C74 AS DateTime), 1, CAST(0x0000A8D800763C74 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (29, 17, N'页面权限', N'UserInfo/Index', N'P1805061120', 0, NULL, 1, CAST(0x0000A8D800767B99 AS DateTime), 1, CAST(0x0000A8D800767B99 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (30, 17, N'用户信息查询', N'UserInfo/Query', N'P1805061136', 0, NULL, 1, CAST(0x0000A8D800768C40 AS DateTime), 1, CAST(0x0000A8D800768C40 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (31, 17, N'保存用户信息查询', N'UserInfo/Save', N'P1805061201', 0, NULL, 1, CAST(0x0000A8D80076AD7B AS DateTime), 1, CAST(0x0000A8D80076AD7B AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (32, 17, N'保存用户角色', N'UserInfo/SaveUserRole', N'P1805061223', 0, NULL, 1, CAST(0x0000A8D80076C364 AS DateTime), 1, CAST(0x0000A8D80076C364 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (33, 17, N'删除用户', N'UserInfo/Del', N'P1805061253', 0, NULL, 1, CAST(0x0000A8D80076E6A2 AS DateTime), 1, CAST(0x0000A8D80076E6A2 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (34, 22, N'部门页面显示', N'Department/Index', N'P1805061323', 0, NULL, 1, CAST(0x0000A8D800770B1F AS DateTime), 1, CAST(0x0000A8D800770B1F AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (35, 22, N'查询部门', N'Department/Query', N'P1805061352', 0, NULL, 1, CAST(0x0000A8D800772B97 AS DateTime), 1, CAST(0x0000A8D80077705C AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (36, 22, N'保存部门', N'Department/Save', N'P1805061416', 0, NULL, 1, CAST(0x0000A8D80077486A AS DateTime), 1, CAST(0x0000A8D80077486A AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (37, 22, N'删除部门', N'Department/Del', N'P1805061505', 0, NULL, 1, CAST(0x0000A8D800778D6F AS DateTime), 1, CAST(0x0000A8D800778D6F AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (38, 11, N'查询微信公众号', N'WeiXin/Index', N'P1805061204', 0, NULL, 1, CAST(0x0000A8D80087397A AS DateTime), 1, CAST(0x0000A8D80087397A AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (39, 9, N'部门', NULL, N'P1805100608', 0, NULL, 1, CAST(0x0000A8DC00A67D50 AS DateTime), 1, CAST(0x0000A8DC00A67D50 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (40, 5, N'系统操作日志页面', N'SysLog/Index', N'P1805133556', 0, N'系统操作日志页面', 1, CAST(0x0000A8DF01220742 AS DateTime), 1, CAST(0x0000A8DF012A2E32 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (41, 5, N'系统操作日志查询', N'SysLog/Query', N'P1805130622', 0, N'系统操作日志查询', 1, CAST(0x0000A8DF012A666E AS DateTime), 1, CAST(0x0000A8DF012A666E AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (42, 4, N'通用字典页面', N'SysDict/Index', N'P1805130856', 0, NULL, 1, CAST(0x0000A8DF012AFFA8 AS DateTime), 1, CAST(0x0000A8DF012B18FE AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (43, 4, N'通用字典页面查询', N'SysDict/Query', N'P1805130914', 0, NULL, 1, CAST(0x0000A8DF012B34BF AS DateTime), 1, CAST(0x0000A8DF012B34BF AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (44, 4, N'系统字典删除', N'SysDict/Del', N'P1805130939', 0, NULL, 1, CAST(0x0000A8DF012B4A0E AS DateTime), 1, CAST(0x0000A8DF012B4A0E AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (45, 4, N'系统字典保存', N'SysDict/Save', N'P1805130956', 0, N'系统字典保存', 1, CAST(0x0000A8DF012B605A AS DateTime), 1, CAST(0x0000A8DF012B605A AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (46, 17, N'重置密码', N'UserInfo/ResetPassword', N'P1805164219', 0, N'重置用户默认密码', 1, CAST(0x0000A8E2008F8079 AS DateTime), 1, CAST(0x0000A8E2008F8079 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (47, 1023, N'应用授权页面', N'AppIdAuth/Index', N'P1805271154', 0, N'应用授权页面', 1, CAST(0x0000A8ED016DD5F8 AS DateTime), 1, CAST(0x0000A8ED016DD5F8 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (48, 1023, N'应用授权保存', N'AppIdAuth/Save', N'P1805271226', 0, N'应用授权保存', 1, CAST(0x0000A8ED016E0216 AS DateTime), 1, CAST(0x0000A8ED016E0216 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (49, 1023, N'删除应用授权', N'AppIdAuth/Del', N'P1805271303', 0, N'删除应用授权', 1, CAST(0x0000A8ED016E1F77 AS DateTime), 1, CAST(0x0000A8ED016E2512 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (50, 1023, N'更新应用授权状态', N'AppIdAuth/UpdateStatus', N'P1805271345', 0, N'应用授权状态', 1, CAST(0x0000A8ED016E6D46 AS DateTime), 1, CAST(0x0000A8ED016E6D46 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (51, 1023, N'配置秘钥', N'AppIdAuth/SaveSecretKey', N'P1806090108', 0, N'配置秘钥', 1, CAST(0x0000A8FA0108281E AS DateTime), 1, CAST(0x0000A8FA0108281E AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (52, 1024, N'保存登录授权', N'SystemManage/UserAppIdAuth/Save', N'P1806271307', 0, N'添加、编辑登录授权', 1, CAST(0x0000A90C017EB775 AS DateTime), 1, CAST(0x0000A90C017EB775 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (53, 1024, N'删除登录授权', N'SystemManage/UserAppIdAuth/Del', N'P1806271338', 0, N'删除登录授权', 1, CAST(0x0000A90C017ECEAF AS DateTime), 1, CAST(0x0000A90D0000DBF7 AS DateTime), 1)
INSERT [dbo].[Permissions] ([Id], [MenuId], [Name], [Action], [Code], [Type], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (54, 1025, N'项目页面', N'SystemManage/AppProject/Index', N'P1807042832', 0, N'项目页面', 1, CAST(0x0000A913017269F3 AS DateTime), 1, CAST(0x0000A913017269F3 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[Permissions] OFF
SET IDENTITY_INSERT [dbo].[RolePermissionsJoin] ON 

INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (6, 2, 13, 2, 6, 1, CAST(0x0000A8D701421799 AS DateTime), 1, CAST(0x0000A8D701421799 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (30, 2, 9, 2, 39, 1, CAST(0x0000A8DC00EF1FEC AS DateTime), 1, CAST(0x0000A8DC00EF1FEC AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (31, 2, 13, 2, 5, 1, CAST(0x0000A8DC00EF2BF2 AS DateTime), 1, CAST(0x0000A8DC00EF2BF2 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (32, 2, 13, 2, 19, 1, CAST(0x0000A8DC00EF2BF3 AS DateTime), 1, CAST(0x0000A8DC00EF2BF3 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (33, 2, 14, 2, 20, 1, CAST(0x0000A8DC00EF3012 AS DateTime), 1, CAST(0x0000A8DC00EF3012 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (34, 2, 14, 2, 21, 1, CAST(0x0000A8DC00EF3012 AS DateTime), 1, CAST(0x0000A8DC00EF3012 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (35, 2, 14, 2, 22, 1, CAST(0x0000A8DC00EF3013 AS DateTime), 1, CAST(0x0000A8DC00EF3013 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (36, 2, 14, 2, 23, 1, CAST(0x0000A8DC00EF3013 AS DateTime), 1, CAST(0x0000A8DC00EF3013 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (37, 2, 14, 2, 24, 1, CAST(0x0000A8DC00EF3013 AS DateTime), 1, CAST(0x0000A8DC00EF3013 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (38, 2, 16, 2, 25, 1, CAST(0x0000A8DC00EF32D2 AS DateTime), 1, CAST(0x0000A8DC00EF32D2 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (39, 2, 16, 2, 26, 1, CAST(0x0000A8DC00EF32D3 AS DateTime), 1, CAST(0x0000A8DC00EF32D3 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (40, 2, 16, 2, 27, 1, CAST(0x0000A8DC00EF32D3 AS DateTime), 1, CAST(0x0000A8DC00EF32D3 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (41, 2, 16, 2, 28, 1, CAST(0x0000A8DC00EF32D3 AS DateTime), 1, CAST(0x0000A8DC00EF32D3 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (42, 2, 17, 2, 29, 1, CAST(0x0000A8DC00EF3590 AS DateTime), 1, CAST(0x0000A8DC00EF3590 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (43, 2, 17, 2, 30, 1, CAST(0x0000A8DC00EF3590 AS DateTime), 1, CAST(0x0000A8DC00EF3590 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (44, 2, 17, 2, 31, 1, CAST(0x0000A8DC00EF3591 AS DateTime), 1, CAST(0x0000A8DC00EF3591 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (45, 2, 17, 2, 32, 1, CAST(0x0000A8DC00EF3591 AS DateTime), 1, CAST(0x0000A8DC00EF3591 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (46, 2, 17, 2, 33, 1, CAST(0x0000A8DC00EF3591 AS DateTime), 1, CAST(0x0000A8DC00EF3591 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (47, 2, 22, 2, 34, 1, CAST(0x0000A8DC00EF3897 AS DateTime), 1, CAST(0x0000A8DC00EF3897 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (48, 2, 22, 2, 35, 1, CAST(0x0000A8DC00EF3897 AS DateTime), 1, CAST(0x0000A8DC00EF3897 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (49, 2, 22, 2, 36, 1, CAST(0x0000A8DC00EF3897 AS DateTime), 1, CAST(0x0000A8DC00EF3897 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (50, 2, 22, 2, 37, 1, CAST(0x0000A8DC00EF3897 AS DateTime), 1, CAST(0x0000A8DC00EF3897 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (51, 2, 5, 1002, 40, 1, CAST(0x0000A8DF01222966 AS DateTime), 1, CAST(0x0000A8DF01222966 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (52, 2, 5, 2, 40, 1, CAST(0x0000A8DF012A81AA AS DateTime), 1, CAST(0x0000A8DF012A81AA AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (53, 2, 5, 2, 41, 1, CAST(0x0000A8DF012A81AC AS DateTime), 1, CAST(0x0000A8DF012A81AC AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (54, 2, 4, 2, 42, 1, CAST(0x0000A8DF012C0E33 AS DateTime), 1, CAST(0x0000A8DF012C0E33 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (55, 2, 4, 2, 43, 1, CAST(0x0000A8DF012C0E34 AS DateTime), 1, CAST(0x0000A8DF012C0E34 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (56, 2, 4, 2, 44, 1, CAST(0x0000A8DF012C0E34 AS DateTime), 1, CAST(0x0000A8DF012C0E34 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (57, 2, 4, 2, 45, 1, CAST(0x0000A8DF012C0E34 AS DateTime), 1, CAST(0x0000A8DF012C0E34 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (59, 2, 17, 2, 46, 1, CAST(0x0000A8E2008F8EEB AS DateTime), 1, CAST(0x0000A8E2008F8EEB AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (60, 2, 5, 1002, 41, 1, CAST(0x0000A8E20091449E AS DateTime), 1, CAST(0x0000A8E20091449E AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (61, 2, 1023, 2, 47, 1, CAST(0x0000A8ED016E78FA AS DateTime), 1, CAST(0x0000A8ED016E78FA AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (62, 2, 1023, 2, 48, 1, CAST(0x0000A8ED016E78FB AS DateTime), 1, CAST(0x0000A8ED016E78FB AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (63, 2, 1023, 2, 49, 1, CAST(0x0000A8ED016E78FB AS DateTime), 1, CAST(0x0000A8ED016E78FB AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (64, 2, 1023, 2, 50, 1, CAST(0x0000A8ED016E78FC AS DateTime), 1, CAST(0x0000A8ED016E78FC AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (65, 3, 11, 2, 38, 1, CAST(0x0000A8EF00A51827 AS DateTime), 1, CAST(0x0000A8EF00A51827 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (66, 2, 1023, 2, 51, 1, CAST(0x0000A8FA01083201 AS DateTime), 1, CAST(0x0000A8FA01083201 AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (67, 2, 1024, 2, 52, 1, CAST(0x0000A90C017EEE3D AS DateTime), 1, CAST(0x0000A90C017EEE3D AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (68, 2, 1024, 2, 53, 1, CAST(0x0000A90C017EEE3E AS DateTime), 1, CAST(0x0000A90C017EEE3E AS DateTime), 1)
INSERT [dbo].[RolePermissionsJoin] ([Id], [NavbarId], [MenuId], [RoleId], [PermissionsId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (69, 2, 1025, 2, 54, 1, CAST(0x0000A91301727E0D AS DateTime), 1, CAST(0x0000A91301727E0D AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[RolePermissionsJoin] OFF
SET IDENTITY_INSERT [dbo].[SystemAppSettings] ON 

INSERT [dbo].[SystemAppSettings] ([Id], [Name], [KeyWord], [KeyValue], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (2, N'默认密码', N'SystemDefaultPassword', N'123456', N'添加用户默认密码', 0, CAST(0x0000A8CC0011664C AS DateTime), 1, CAST(0x0000A8E000EA4B50 AS DateTime), 1)
INSERT [dbo].[SystemAppSettings] ([Id], [Name], [KeyWord], [KeyValue], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (4, N'默认头像', N'SystemDefaultHeadimg', N'Content/Images/defaultimg.jpg', N'添加系统用户默认头像', 0, CAST(0x0000A8CC0011664C AS DateTime), 1, CAST(0x0000A8E000EA674D AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[SystemAppSettings] OFF
SET IDENTITY_INSERT [dbo].[SystemMenu] ON 

INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (2, 0, 2, N'系统管理', NULL, NULL, 0, N'fa fa-desktop', 1, 0, CAST(0x0000A8C6017DA056 AS DateTime), 0, CAST(0x0000A8C6017DA056 AS DateTime), N'系统菜单', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (3, 0, 2, N'单位组织', NULL, NULL, 1, N'fa fa-navicon', 1, 0, CAST(0x0000A8C6017DA056 AS DateTime), 1, CAST(0x0000A8EF006860AB AS DateTime), N'单位组织', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (4, 2, 2, N'通用字典', N'SystemManage/SysDict/Index', NULL, 0, N'fa fa-navicon', 5, 0, CAST(0x0000A8C6017DA056 AS DateTime), 1, CAST(0x0000A8D2016D6E5C AS DateTime), N'通用字典', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (5, 2, 2, N'系统操作日志', N'SystemManage/SysLog/Index', NULL, 0, N'fa fa-navicon', 4, 0, CAST(0x0000A8C6017DA056 AS DateTime), 1, CAST(0x0000A8DF0129FD72 AS DateTime), N'系统操作日志', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (6, 2, 2, N'数据管理', NULL, NULL, 0, N'fa fa-navicon', 3, 0, CAST(0x0000A8C6017DA056 AS DateTime), 1, CAST(0x0000A8D500ED878F AS DateTime), N'数据管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (7, 6, 2, N'数据库备份', NULL, NULL, 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C6017DA056 AS DateTime), 1, CAST(0x0000A8D500ED8BC3 AS DateTime), N'数据库备份', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (8, 6, 2, N'数据表管理', N'', NULL, 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C6017DA056 AS DateTime), 0, CAST(0x0000A8C6017DA056 AS DateTime), N'数据表管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (10, 0, 3, N'基础管理', N'', NULL, 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C601838468 AS DateTime), 0, CAST(0x0000A8C601838468 AS DateTime), N'基础管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (11, 10, 3, N'公众号管理', N'Weixin/Index', NULL, 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C601838468 AS DateTime), 1, CAST(0x0000A8D80087BFB0 AS DateTime), N'公众号管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (13, 2, 2, N'栏目管理', N'SystemManage/SysNavbar/Index', 1, 0, N'fa fa-navicon', 1, 0, CAST(0x0000A8C80014B46E AS DateTime), 1, CAST(0x0000A908017D9A8F AS DateTime), N'栏目管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (14, 2, 2, N'菜单管理', N'SystemManage/SysMenu/Index', NULL, 0, N'fa fa-navicon', 2, 0, CAST(0x0000A8C80014B46E AS DateTime), 0, CAST(0x0000A8C80014B46E AS DateTime), N'菜单管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (16, 2, 2, N'角色管理', N'SystemManage/UserRole/Index', 1, 0, N'fa fa-navicon', 2, 0, CAST(0x0000A8C9012FE0D5 AS DateTime), 1, CAST(0x0000A90801794738 AS DateTime), N'角色管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (17, 2, 2, N'用户管理', N'SystemManage/UserInfo/Index', NULL, 0, N'fa fa-navicon', 2, 0, CAST(0x0000A8C9012FE0D5 AS DateTime), 0, CAST(0x0000A8C9012FE0D5 AS DateTime), N'用户管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (22, 2, 2, N'部门管理', N'SystemManage/Department/Index', NULL, 0, N'fa fa-navicon', 2, 0, CAST(0x0000A8C9012FE0D5 AS DateTime), 0, CAST(0x0000A8C9012FE0D5 AS DateTime), N'部门管理', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (1023, 2, 2, N'应用授权管理', N'SystemManage/AppIdAuth/Index', NULL, 0, N'fa fa-navicon', 5, 1, CAST(0x0000A8ED016D3CEC AS DateTime), 1, CAST(0x0000A8ED016D9239 AS DateTime), N'主要是编辑后台接口调用着授权Token', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (1024, 2, 2, N'登录授权', N'SystemManage/UserAppIdAuth/Index', 0, 0, N'fa fa-navicon', 5, 1, CAST(0x0000A90C017E7A51 AS DateTime), 1, CAST(0x0000A90C01837791 AS DateTime), N'统一登录授权AppId', 1)
INSERT [dbo].[SystemMenu] ([Id], [Pid], [NavbarId], [Name], [UrlAddress], [AddressAuth], [MenuType], [Icon], [Sort], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (1025, 2, 2, N'项目管理', N'SystemManage/AppProject/Index', 0, 0, N'fa fa-navicon', 6, 1, CAST(0x0000A91301720859 AS DateTime), 1, CAST(0x0000A91301724FF4 AS DateTime), N'添加项目', 1)
SET IDENTITY_INSERT [dbo].[SystemMenu] OFF
SET IDENTITY_INSERT [dbo].[SystemNavbar] ON 

INSERT [dbo].[SystemNavbar] ([Id], [Name], [Url], [Remark], [Sort], [CreateTime], [CreateUserId], [UpdateTime], [UpdateUserId], [IsValid]) VALUES (2, N'系统配置', N'', N'主要配置用户基本信息、权限，常量配置', 1, CAST(0x0000A8C50171B30D AS DateTime), NULL, CAST(0x0000A8E001040AC8 AS DateTime), 1, 1)
INSERT [dbo].[SystemNavbar] ([Id], [Name], [Url], [Remark], [Sort], [CreateTime], [CreateUserId], [UpdateTime], [UpdateUserId], [IsValid]) VALUES (3, N'微信平台', NULL, N'微信相关', 2, CAST(0x0000A8C50171B30D AS DateTime), NULL, CAST(0x0000A8CA003D76D7 AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[SystemNavbar] OFF
SET IDENTITY_INSERT [dbo].[UserAppIdAuth] ON 

INSERT [dbo].[UserAppIdAuth] ([Id], [UserId], [AppId], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (1, 1, N'0001', N'授权登录', 1, CAST(0x0000A8EF006CE483 AS DateTime), 1, CAST(0x0000A90E0125A130 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[UserAppIdAuth] OFF
SET IDENTITY_INSERT [dbo].[UserDepartmentJoin] ON 

INSERT [dbo].[UserDepartmentJoin] ([Id], [UserId], [DepartmentId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (8, 8, 1, 1, CAST(0x0000A8E3001BF34B AS DateTime), 1, CAST(0x0000A8E3001BF34B AS DateTime), 1)
INSERT [dbo].[UserDepartmentJoin] ([Id], [UserId], [DepartmentId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (9, 1, 0, 1, CAST(0x0000A8EF013727B9 AS DateTime), 1, CAST(0x0000A8EF013727B9 AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[UserDepartmentJoin] OFF
SET IDENTITY_INSERT [dbo].[UserInfo] ON 

INSERT [dbo].[UserInfo] ([Id], [LoginName], [Password], [Email], [Name], [HeadimgUrl], [Sex], [Mobile], [HomeAddress], [DepartmentId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (1, N'admin', N'E10ADC3949BA59ABBE56E057F20F883E', N'system@mayiboy.com', N'蚂蚁男孩', N'Content/Images/headimg.png', 1, N'***********', NULL, 2, 0, CAST(0x0000A8C10021CD26 AS DateTime), 1, CAST(0x0000A91000FCE475 AS DateTime), N'超级管理员', 1)
INSERT [dbo].[UserInfo] ([Id], [LoginName], [Password], [Email], [Name], [HeadimgUrl], [Sex], [Mobile], [HomeAddress], [DepartmentId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [Remark], [IsValid]) VALUES (8, N'simon', N'E10ADC3949BA59ABBE56E057F20F883E', N'caimeng2009@126.com', N'闲僧', N'Content/Images/defaultimg.jpg', 0, N'098765432123', NULL, 17, 1, CAST(0x0000A8D500A67AB6 AS DateTime), 1, CAST(0x0000A90D0155C9C3 AS DateTime), NULL, 1)
SET IDENTITY_INSERT [dbo].[UserInfo] OFF
SET IDENTITY_INSERT [dbo].[UserRole] ON 

INSERT [dbo].[UserRole] ([Id], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (2, N'管理员', N'管理员', 1, CAST(0x0000A8CF00576B3D AS DateTime), 1, CAST(0x0000A8CF00576B3E AS DateTime), 1)
INSERT [dbo].[UserRole] ([Id], [Name], [Remark], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (1002, N'技术部', N'管理后台配置', 1, CAST(0x0000A8D401239D52 AS DateTime), 1, CAST(0x0000A8E00104284A AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[UserRole] OFF
SET IDENTITY_INSERT [dbo].[UserRoleJoin] ON 

INSERT [dbo].[UserRoleJoin] ([Id], [UserId], [RoleId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (3, 1, 2, 1, CAST(0x0000A8D500E0683D AS DateTime), 1, CAST(0x0000A8D500E0683D AS DateTime), 1)
INSERT [dbo].[UserRoleJoin] ([Id], [UserId], [RoleId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (4, 1, 1002, 1, CAST(0x0000A8D500E09002 AS DateTime), 1, CAST(0x0000A8D500E09002 AS DateTime), 1)
INSERT [dbo].[UserRoleJoin] ([Id], [UserId], [RoleId], [CreateUserId], [CreateTime], [UpdateUserId], [UpdateTime], [IsValid]) VALUES (5, 8, 1002, 1, CAST(0x0000A8D80073BF5E AS DateTime), 1, CAST(0x0000A8D80073BF5E AS DateTime), 1)
SET IDENTITY_INSERT [dbo].[UserRoleJoin] OFF
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'AppId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'授权Token' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'AuthToken'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'接口数据加密类型（0：不加密；1：对称加密（DES）；2：对称加密（AES）；3：非对称加密' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'EncryptionType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'对称加密-秘钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'SecretKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'非对称加密-私钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'PrivateKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'非对称加密-公钥' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'PublicKey'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否启用状态（0：未启用；1：已启用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用授权' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppIdAuth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppProject', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'项目名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppProject', @level2type=N'COLUMN',@level2name=N'ProjectName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'AppId' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppProject', @level2type=N'COLUMN',@level2name=N'ApplicationId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'描述' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppProject', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppProject', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppProject', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppProject', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppProject', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AppProject', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'父级Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'City', @level2type=N'COLUMN',@level2name=N'Pid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'城市名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'City', @level2type=N'COLUMN',@level2name=N'CityName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮政编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'City', @level2type=N'COLUMN',@level2name=N'ZipCode'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'City', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'City', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'City', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'City', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'City', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'MenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'操作地址（控制器/操作）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'Action'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限代码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注说明' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新用户id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Permissions', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermissionsJoin', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'栏目id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermissionsJoin', @level2type=N'COLUMN',@level2name=N'NavbarId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermissionsJoin', @level2type=N'COLUMN',@level2name=N'MenuId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermissionsJoin', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'权限Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermissionsJoin', @level2type=N'COLUMN',@level2name=N'PermissionsId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermissionsJoin', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermissionsJoin', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermissionsJoin', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermissionsJoin', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效（0：无效；1：有效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RolePermissionsJoin', @level2type=N'COLUMN',@level2name=N'IsValid'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'地址是否鉴权（1：鉴权）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'AddressAuth'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'菜单类型(0：内部；1：外部)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'SystemMenu', @level2type=N'COLUMN',@level2name=N'MenuType'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAppIdAuth', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAppIdAuth', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'应用Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAppIdAuth', @level2type=N'COLUMN',@level2name=N'AppId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAppIdAuth', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAppIdAuth', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAppIdAuth', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAppIdAuth', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效标识（1：有效；0：无效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserAppIdAuth', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDepartmentJoin', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDepartmentJoin', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'部门Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDepartmentJoin', @level2type=N'COLUMN',@level2name=N'DepartmentId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDepartmentJoin', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDepartmentJoin', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDepartmentJoin', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDepartmentJoin', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效标识（1：有效；0：无效）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDepartmentJoin', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户部门关联' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserDepartmentJoin'
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleJoin', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleJoin', @level2type=N'COLUMN',@level2name=N'UserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'角色Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleJoin', @level2type=N'COLUMN',@level2name=N'RoleId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleJoin', @level2type=N'COLUMN',@level2name=N'CreateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleJoin', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改用户Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleJoin', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleJoin', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效(1:有效；0：无效)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserRoleJoin', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
USE [master]
GO
ALTER DATABASE [MayiboyDb] SET  READ_WRITE 
GO
