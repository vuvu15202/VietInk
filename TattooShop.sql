USE [master]
GO
CREATE DATABASE [tattooshop]
GO
USE [tattooshop]
GO
--DROP DATABASE PRN231_uni;

--CREATE TABLE Students (
--	Id int primary key IDENTITY(1,1) not null ,
--    StudentID varchar(8) unique not null,
--    StudentName varchar(255) not null,
--	Photo VARCHAR(MAX) null
--);

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users]  (
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[FirstName] [nvarchar](255) NOT NULL,
	[LastName] [nvarchar](255) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[PhoneNumber] [nvarchar](255)  NULL,
	[Address] [NVARCHAR](MAX) NULL,
	[Role] [int] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[OrderDate] [datetime] NULL,
	[RequiredDate] [datetime] NULL,
	[ShippedDate] [datetime] NULL,
	[Freight] [money] NULL,
	[ShipAddress] [nvarchar](max) NULL,
	[ShipCity] [nvarchar](255) NULL,
	[ShipRegion] [nvarchar](255) NULL,
	[ShipPostalCode] [nvarchar](10) NULL,
	[ShipCountry] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](255)  NULL,
	[Note] [nvarchar](255)  NULL,
	[Discount] [float] NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](255) NOT NULL,
	[CategoryName] [nvarchar](255) NULL,
	[QuantityPerUnit] [nvarchar](255) NULL,
	[UnitPrice] [money] NULL,
	[UnitsInStock] [int] NULL,	
	[Image] [NVARCHAR](MAX) NULL,
	[Discontinued] [bit] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


SET IDENTITY_INSERT [dbo].[Products] ON 
INSERT [dbo].[Products] ([ProductID], [ProductName], [CategoryName], [QuantityPerUnit], [UnitPrice], [UnitsInStock], [Image], [Discontinued]) 
VALUES (1, N'Xăm cá chép',N'to', N'1 hình lớn', 50000, 100, N'djfhjdf', 0)
SET IDENTITY_INSERT [dbo].[Products] OFF
GO



SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order Details](
	[OrderID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[Quantity] [int] NOT NULL,
 CONSTRAINT [PK_Order_Details] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
GO




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Requirements](
	[RequirementID] [int] IDENTITY(1,1) NOT NULL,
	[CustomerName] [nvarchar](255) NOT NULL,
	[Email] [nvarchar](255) NULL,
	[PhoneNumber] [nvarchar](255) NOT NULL,
	[Note] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Requirements] PRIMARY KEY CLUSTERED 
(
	[RequirementID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




SET IDENTITY_INSERT [dbo].[Users] ON 
INSERT [dbo].[Users] ([UserId], [UserName], [FirstName], [LastName], [Password], [Email], [PhoneNumber], [Address], [Role]) VALUES (1, N'vuvu15202',N'Vu', N'Vu Truong', N'123', N'vuvthe163299@fpt.edu.vn', N'0329053888', N'HN', 1)
INSERT [dbo].[Users] ([UserId], [UserName], [FirstName], [LastName], [Password], [Email], [PhoneNumber], [Address], [Role]) VALUES (2, N'anhqd', N'Anh',N'Do Quang',  N'123', N'anhqd', N'0329053888', N'HN', 1)
INSERT [dbo].[Users] ([UserId], [UserName], [FirstName], [LastName], [Password], [Email], [PhoneNumber], [Address], [Role]) VALUES (3, N'thangdt', N'Thang',N'Dinh Toan', N'123', N'thanhdt', N'0329053888', N'HN', 1)
INSERT [dbo].[Users] ([UserId], [UserName], [FirstName], [LastName], [Password], [Email], [PhoneNumber], [Address], [Role]) VALUES (4, N'vuvu15202',N'Vu', N'Vu Truong', N'123', N'vuvthe163299@fpt.edu.vn', N'0329053888', N'HN', 1)
INSERT [dbo].[Users] ([UserId], [UserName], [FirstName], [LastName], [Password], [Email], [PhoneNumber], [Address], [Role]) VALUES (5, N'anhqd', N'Anh',N'Do Quang',  N'123', N'anhqd', N'0329053888', N'HN', 1)
INSERT [dbo].[Users] ([UserId], [UserName], [FirstName], [LastName], [Password], [Email], [PhoneNumber], [Address], [Role]) VALUES (6, N'thangdt', N'Thang',N'Dinh Toan', N'123', N'thanhdt', N'0329053888', N'HN', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO


SET IDENTITY_INSERT [dbo].[Orders] ON 
INSERT [dbo].[Orders] ([OrderID], [UserId], [OrderDate], [RequiredDate], [ShippedDate], [Freight], [ShipAddress], [ShipCity], [ShipRegion], [ShipPostalCode], [ShipCountry], [PhoneNumber], [Note], [Discount]) 
VALUES (1, 1, CAST(N'1997-12-22T00:00:00.000' AS DateTime), CAST(N'1997-12-22T00:00:00.000' AS DateTime), CAST(N'1997-12-22T00:00:00.000' AS DateTime), 100000, N'Tony house', N'hanoi', N'ThachThat', 10000, N'VN', N'0329053888', N'note', 0)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO




----------------------------------- Default -----------------------------------

--GO
--ALTER TABLE [dbo].[Order Details] ADD  CONSTRAINT [DF_Order_Details_UnitPrice]  DEFAULT ((0)) FOR [UnitPrice]
--GO
--ALTER TABLE [dbo].[Order Details] ADD  CONSTRAINT [DF_Order_Details_Quantity]  DEFAULT ((1)) FOR [Quantity]
--GO
--ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Order_Details_Discount]  DEFAULT ((0)) FOR [Discount]
--GO
--ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_Freight]  DEFAULT ((0)) FOR [Freight]
--GO
--ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_UnitPrice]  DEFAULT ((0)) FOR [UnitPrice]
--GO
--ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_UnitsInStock]  DEFAULT ((0)) FOR [UnitsInStock]
--GO
--ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_Discontinued]  DEFAULT ((0)) FOR [Discontinued]
--GO

----------------------------------- FOREIGN KEY -----------------------------------


ALTER TABLE [dbo].[Order Details]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Details_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[Order Details] CHECK CONSTRAINT [FK_Order_Details_Orders]
GO
ALTER TABLE [dbo].[Order Details]  WITH NOCHECK ADD  CONSTRAINT [FK_Order_Details_Products] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
GO
ALTER TABLE [dbo].[Order Details] CHECK CONSTRAINT [FK_Order_Details_Products]
GO
ALTER TABLE [dbo].[Orders]  WITH NOCHECK ADD  CONSTRAINT [FK_Orders_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Users]
GO


----------------------------------- CHECK -----------------------------------

GO
ALTER TABLE [dbo].[Orders]  WITH NOCHECK ADD  CONSTRAINT [CK_Discount] CHECK  (([Discount]>=(0) AND [Discount]<=(1)))
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [CK_Discount]
GO
ALTER TABLE [dbo].[Order Details]  WITH NOCHECK ADD  CONSTRAINT [CK_Quantity] CHECK  (([Quantity]>(0)))
GO
ALTER TABLE [dbo].[Order Details] CHECK CONSTRAINT [CK_Quantity]
GO
ALTER TABLE [dbo].[Order Details]  WITH NOCHECK ADD  CONSTRAINT [CK_UnitPrice] CHECK  (([UnitPrice]>=(0)))
GO
ALTER TABLE [dbo].[Order Details] CHECK CONSTRAINT [CK_UnitPrice]
GO
ALTER TABLE [dbo].[Products]  WITH NOCHECK ADD  CONSTRAINT [CK_Products_UnitPrice] CHECK  (([UnitPrice]>=(0)))
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [CK_Products_UnitPrice]
GO
ALTER TABLE [dbo].[Products]  WITH NOCHECK ADD  CONSTRAINT [CK_UnitsInStock] CHECK  (([UnitsInStock]>=(0)))
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [CK_UnitsInStock]
GO



--select * from [dbo].[Students]

--select * from [dbo].[User]
--select * from [UserRole]
--select * from [Role]

--update [UserRole] set UserId=6, RoleId=2 where UserRoleId=1007

--ALTER TABLE Students
--DROP COLUMN StudentID;

--ALTER TABLE Students
--ADD Photo VARCHAR(MAX) null;

--ALTER TABLE Students
--ALTER COLUMN StudentID varchar(8) not null;

