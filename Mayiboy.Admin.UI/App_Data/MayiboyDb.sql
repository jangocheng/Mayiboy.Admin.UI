USE [master]
GO
/****** Object:  Database [MayiboyDb]    Script Date: 2018/4/5 6:10:31 ******/
CREATE DATABASE [MayiboyDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MayiboyDb', FILENAME = N'D:\DB\MSSQLDB\MSSQL11.MSSQLSERVER\MSSQL\DATA\MayiboyDb.mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'MayiboyDb_log', FILENAME = N'D:\DB\MSSQLDB\MSSQL11.MSSQLSERVER\MSSQL\DATA\MayiboyDb_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [MayiboyDb] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MayiboyDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MayiboyDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MayiboyDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MayiboyDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MayiboyDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MayiboyDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [MayiboyDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MayiboyDb] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [MayiboyDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MayiboyDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MayiboyDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MayiboyDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MayiboyDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MayiboyDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MayiboyDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MayiboyDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MayiboyDb] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MayiboyDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MayiboyDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MayiboyDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MayiboyDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MayiboyDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MayiboyDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MayiboyDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MayiboyDb] SET RECOVERY FULL 
GO
ALTER DATABASE [MayiboyDb] SET  MULTI_USER 
GO
ALTER DATABASE [MayiboyDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MayiboyDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MayiboyDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MayiboyDb] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'MayiboyDb', N'ON'
GO
USE [MayiboyDb]
GO
/****** Object:  Table [dbo].[Department]    Script Date: 2018/4/5 6:10:31 ******/
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
	[IsValid] [int] NOT NULL,
	[Remark] [varchar](500) NULL,
	[CreateUserId] [int] NOT NULL,
	[CreateTime] [datetime] NOT NULL,
	[UpdateUserId] [int] NULL,
	[UpdateTime] [datetime] NULL,
 CONSTRAINT [PK_Department] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SystemMenu]    Script Date: 2018/4/5 6:10:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SystemMenu](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[MenuAddress] [varchar](500) NULL,
	[MenuType] [int] NOT NULL,
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
/****** Object:  Table [dbo].[UserInfo]    Script Date: 2018/4/5 6:10:31 ******/
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
	[MobileX] [varchar](50) NULL,
	[MobileMask] [varchar](50) NULL,
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
/****** Object:  Table [dbo].[UserRole]    Script Date: 2018/4/5 6:10:31 ******/
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
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'登录名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'LoginName'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'邮箱地址' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'姓名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密文手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'MobileX'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'掩码手机号' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'MobileMask'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'UpdateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'备注' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'Remark'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否有效标识' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'UserInfo', @level2type=N'COLUMN',@level2name=N'IsValid'
GO
USE [master]
GO
ALTER DATABASE [MayiboyDb] SET  READ_WRITE 
GO
