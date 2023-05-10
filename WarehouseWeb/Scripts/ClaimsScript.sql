/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP (1000) [Id]
      ,[RoleId]
      ,[CustomerId]
      ,[Description]
      ,[CreateDate]
      ,[ModifyDate]
  FROM [WarehouseDBVol3].[dbo].[CustomerRole]