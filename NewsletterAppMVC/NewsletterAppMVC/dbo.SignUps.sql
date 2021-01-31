CREATE TABLE [dbo].[SignUps] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]    VARCHAR (50)  NULL,
    [LastName]     VARCHAR (50)  NULL,
    [EmailAddress] VARCHAR (100) NULL, 
    [SocialSecurityNumber] VARCHAR(15) NULL
);

