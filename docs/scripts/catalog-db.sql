/* Remove all entries in the tables */

DELETE FROM [dbo].[Products]
DELETE FROM [dbo].[ProductCategories]

/* Reseed the indexes of the tables */

DBCC CHECKIDENT ('[Products]', RESEED, 0);
GO

DBCC CHECKIDENT ('[ProductCategories]', RESEED, 0);
GO