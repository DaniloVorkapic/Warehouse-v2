USE [WarehouseDBVol3]
GO

DECLARE @roleIdAdmin bigint
 DECLARE @roleIdCustomer bigint
 DECLARE @roleIdWarehouseWorker bigint
 DECLARE @claimsIdEdit bigint
 DECLARE @claimsIdEditProductStorage bigint
 DECLARE @claimsIdView bigint
 DECLARE @claimsIdAddProduct bigint
 Declare @claimsIdAddorder bigint



 select @roleIdCustomer = [Id] 
from [WarehouseDBVol3].dbo.Role
where Name = 'Customer'


select @roleIdAdmin = [Id]
from [WarehouseDBVol3].dbo.Role
where Name = 'Administrator'

 select @roleIdWarehouseWorker = [Id]
from [WarehouseDBVol3].dbo.Role
where Name = 'Warehouse worker'





select @claimsIdAddorder =  [Id]
 FROM [WarehouseDBVol3].[dbo].[Claims]
 where Name = 'Add order/order item' 


 select @claimsIdAddProduct =  [Id]
 FROM [WarehouseDBVol3].[dbo].[Claims]
 where Name = 'Add product'

  select @claimsIdEdit =  [Id]
 FROM [WarehouseDBVol3].[dbo].[Claims]
 where Name = 'Edit'

  select @claimsIdEditProductStorage =  [Id]
 FROM [WarehouseDBVol3].[dbo].[Claims]
 where Name = 'Edit Product and storage'

 select @claimsIdView =  [Id]
 FROM [WarehouseDBVol3].[dbo].[Claims]
 where Name = 'View'



INSERT INTO [WarehouseDBVol3].[dbo].[RoleClaims]
           ([RoleId]
           ,[ClaimsId]
           ,[CreateDate]
           ,[ModifyDate])
     select @roleIdAdmin, @claimsIdAddProduct, GETDATE(), NULL union all
	 select @roleIdAdmin, @claimsIdEdit, GETDATE(), NULL union all
	 select @roleIdAdmin , @claimsIdEditProductStorage, GETDATE(), NULL union all
	 select @roleIdCustomer , @claimsIdAddorder, GETDATE(), NULL union all
	 select @roleIdCustomer , @claimsIdView, GETDATE(), NULL union all
	 select @roleIdWarehouseWorker , @claimsIdEditProductStorage, GETDATE(), NULL union all
	 select @roleIdWarehouseWorker , @claimsIdView, GETDATE(), NULL 
GO


