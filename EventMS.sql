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

Create table [Role](
	ID int identity(1,1) NOT NULL PRIMARY KEY,
	Name nvarchar(100) not null,
	Description nvarchar(max) not null
);
GO

CREATE TABLE UserRole (
	UserId int FOREIGN KEY REFERENCES [User](id),
	RoleId int FOREIGN KEY REFERENCES [Role](id)
);
GO

Create table [Group](
	ID int identity(1,1) NOT NULL PRIMARY KEY,
	Name nvarchar(100) not null,
	Description nvarchar(max) not null	
);
GO

CREATE TABLE GroupUser (
	UserId int FOREIGN KEY REFERENCES [User](id),
	GroupId int FOREIGN KEY REFERENCES [Group](id)
);
GO

create table [EventStatus](
	ID int identity(1,1) NOT NULL PRIMARY KEY,
	name nvarchar(100) not null
);
GO

CREATE TABLE Event (
	ID int identity(1,1) NOT NULL PRIMARY KEY,
	Name nvarchar(70) not null,
	Description nvarchar(max) not null,
	CreationDate datetime not null,
	CreationUserID int FOREIGN KEY REFERENCES [User](id),
	RegistrationEndDate datetime not null,
	StartDateTime datetime not null,
	EndDateTime datetime not null,
	Place nvarchar(70) not null,
	isPublic bit not null,
	Capacity int not null,
	Price decimal(14,2) not null,
	StatusID int FOREIGN KEY REFERENCES [EventStatus](id)
);
GO

CREATE TABLE AllowedEventGroup (
	EventId int FOREIGN KEY REFERENCES [Event](id),
	GroupId int FOREIGN KEY REFERENCES [Group](id)
);
GO

Create table comment(
	ID int identity(1,1) NOT NULL PRIMARY KEY,
	EventId int FOREIGN KEY REFERENCES [Event](id),
	CreationUserID int FOREIGN KEY REFERENCES [User](id),
	Text nvarchar(4000) not null,
	CreationDate datetime not null,
);
GO

create table [InvitationResponseType](
	ID int identity(1,1) NOT NULL PRIMARY KEY,
	name nvarchar(4000) not null
);
Go

Create table EventInvitation(
	EventId int FOREIGN KEY REFERENCES [Event](id),
	UserID int FOREIGN KEY REFERENCES [User](id),
	SentDate datetime not null,
	InvitationResponseID int FOREIGN KEY REFERENCES [InvitationResponseType](id),
	TextResponse nvarchar(max) not null,
	ResponseDate datetime not null,
);
GO

create table [EventTicket](
	ID int identity(1,1) NOT NULL PRIMARY KEY,
	EventId int FOREIGN KEY REFERENCES [Event](id),
	OwnerID int FOREIGN KEY REFERENCES [User](id),
	isPaid bit not null,
	PaidDate datetime not null
);
Go
