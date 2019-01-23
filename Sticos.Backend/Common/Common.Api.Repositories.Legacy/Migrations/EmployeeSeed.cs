namespace Common.Api.Repositories.Legacy.Factories.SeedData
{
    public static class EmployoeeSeed
    {
        public static string UpSQL = @"
IF OBJECT_ID('dbo.HrAnsatt', 'U') IS NULL 
BEGIN
  CREATE TABLE [dbo].[HrAnsatt](
		[Id] [int] IDENTITY(1,1) NOT NULL,
	[Fagforening] [nvarchar](255) NULL,
	[Personnummer] [varbinary](max) NULL,
	[TelefonHjem] [nvarchar](31) NULL,
	[Adresse] [nvarchar](255) NULL,
	[PårørendeFulltnavn] [nvarchar](255) NULL,
	[PårørendeTelefon] [nvarchar](31) NULL,
	[PårørendeEpost] [nvarchar](255) NULL,
	[Enhet] [int] NULL,
	[UserId] [int] NULL,
	[PårørendeRelasjon] [nvarchar](255) NULL,
	[Bilde] [varbinary](max) NULL,
	[Postnummer] [nvarchar](31) NULL,
	[Poststed] [nvarchar](255) NULL,
	[HrSivilstatus] [int] NULL,
	[Tittel] [nvarchar](255) NULL,
	[Beskrivelse] [nvarchar](max) NULL,
	[Kjønn] [int] NOT NULL DEFAULT ((0)),
	[AnsettelseStartDato] [datetime] NOT NULL DEFAULT (CONVERT([datetime],'1753-1-1',(0))),
	[AnsettelseSluttDato] [datetime] NOT NULL DEFAULT (CONVERT([datetime],'9999-12-31',(0))),
	[ErSlettet] [bit] NOT NULL DEFAULT ((0)),
	[Fornavn] [nvarchar](255) NULL,
	[Etternavn] [nvarchar](255) NULL,
	[Fødselsdato] [datetime] NULL,
	[EksternAnsattId] [varchar](50) NULL,
	[Notes] [varbinary](max) NULL,
 CONSTRAINT [PK_HrAnsatt] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
		
	if not exists (select * from [HrAnsatt] where [Fornavn] = 'John') begin INSERT INTO [HrAnsatt]([TelefonHjem],[Enhet],[UserId],Tittel, Fornavn, Etternavn, ErSlettet) VALUES ('99112233',1,10,'Director', 'John', 'Johnsen', 0) end	
	if not exists (select * from [HrAnsatt] where [Fornavn] = 'Peter') begin INSERT INTO [HrAnsatt]([TelefonHjem],[Enhet],[UserId],Tittel, Fornavn, Etternavn, ErSlettet) VALUES ('99887766',1,17,'Accountant', 'Peter', 'Petersen', 0) end	
END
";


    }
}