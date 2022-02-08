USE master
GO

CREATE DATABASE EventMS;
GO

USE EventMS;
GO

CREATE TABLE [User] (
	ID int identity(1,1) NOT NULL PRIMARY KEY,
	Password nvarchar(30) NOT NULL,
	FirstName nvarchar(70) NOT NULL,
	LastName nvarchar(70) NOT NULL,
	Email nvarchar(140) NOT NULL,
	Bio nvarchar(max) NOT NULL,
	isActive bit not null
);
GO


CREATE TABLE UserRole (
	CustomerId int FOREIGN KEY REFERENCES Customers(CustomerId) ON DELETE CASCADE,
	Password nvarchar(30) NOT NULL,
	FirstName nvarchar(70) NOT NULL,
	LastName nvarchar(70) NOT NULL,
	Email nvarchar(140) NOT NULL,
	Bio nvarchar(max) NOT NULL,
	isActive bit not null
);
GO

INSERT [dbo].[Users] ([UserId], [ContactName], [Address], [Phone], [Password]) VALUES (1, N'User number 1', N'District 5, HCM city', N'0789123123', N'1')
GO