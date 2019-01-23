namespace Common.Api.Repositories.Legacy.Factories.SeedData
{
    public static class UnitSeed
    {
        public static string UpSQL = @"
IF OBJECT_ID('dbo.Enhet', 'U') IS NULL 
BEGIN
  CREATE TABLE [dbo].[Enhet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Navn] [nvarchar](100) NULL,
	[Orgnr] [nvarchar](255) NULL,
	[Type] [int] NOT NULL,
	[Parent] [int] NULL,
    [ErSlettet] [bit] NULL,
 CONSTRAINT [PK_Enhet] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
	
	DECLARE @parentId [int]
	INSERT INTO [Enhet]([Navn] ,[Orgnr] ,[Type] ) VALUES ('STICOS AS', 971998016, 3)
	SET @parentId = SCOPE_IDENTITY()
	if not exists (select * from [Enhet] where [Navn] = 'Oppslag') begin INSERT INTO [Enhet]([Navn] ,[Type],[Parent]) VALUES ('Oppslag',  4, @parentId) end
	if not exists (select * from [Enhet] where [Navn] = 'Juridisk') begin INSERT INTO [Enhet]([Navn] ,[Type],[Parent]) VALUES ('Juridisk',  4, @parentId) end
	if not exists (select * from [Enhet] where [Navn] = 'Personal') begin INSERT INTO [Enhet]([Navn] ,[Type],[Parent]) VALUES ('Oppslag',  4, @parentId) end
    
    INSERT INTO [Enhet]([Navn] ,[Orgnr] ,[Type] ) VALUES ('STICOS AS Legalunit', 934228391, 3)
    INSERT INTO [Enhet]([Navn] ,[Orgnr] ,[Type] ) VALUES ('STICOS AS MissingOrgnr', null, 3)
END
";


    }
}