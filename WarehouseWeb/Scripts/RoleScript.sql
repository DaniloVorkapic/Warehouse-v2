USE [WarehouseDBVol3]
GO

INSERT INTO [dbo].[Role]
           ([Name]
           ,[Desctirption]
           ,[CreateDate]
           ,[ModifyDate])
     VALUES
           ('Admin','AdminProba', GETDATE(), NULL),
		   ('cUSTOMER22', 'CUSTOMER PROBA', GETDATE(), NULL)
GO


