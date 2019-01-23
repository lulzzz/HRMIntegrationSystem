namespace Common.Api.Repositories.Legacy.Factories.SeedData
{
    public static class UserSeed
    {
        public static string UpSQL = @"
IF OBJECT_ID('dbo.Bruker', 'U') IS NULL 
BEGIN
CREATE TABLE [dbo].[Bruker](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Epost] [nvarchar](100) NULL,
	[Kunde] [int] NULL,
	[EnhetId] [int] NOT NULL,
	[SistInnlogget] [datetime] NULL,
	[ErAktiv] [bit] NULL,
	[Mobiltelefon] [nvarchar](100) NULL,
	[Jobbtelefon] [nvarchar](100) NULL,
	[AntallGangerInnlogget] [int] NULL,
	[PassordMåEndres] [bit] NULL,
	[Fornavn] [nvarchar](255) NULL,
	[Etternavn] [nvarchar](255) NULL,
	[ErSlettet] [bit] NOT NULL DEFAULT ((0)),
	[GodtattBrukervilkårTidspunkt] [datetime] NULL,
	[UserId] [int] NULL,
	[ErSA] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Bruker] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
		
	if not exists (select * from [Bruker] where [UserId] = 10) begin INSERT INTO [dbo].[Bruker] ([Epost], [Kunde], [EnhetId], [ErAktiv], [Mobiltelefon], [Jobbtelefon], [ErSlettet], [UserId]) VALUES ('john.johnsen@sticos.no', 1, 1, 1, '99112233','22334455',0,10) end	
if not exists (select * from [Bruker] where [UserId] = 17) begin INSERT INTO [dbo].[Bruker] ([Epost], [Kunde], [EnhetId], [ErAktiv], [Mobiltelefon], [Jobbtelefon], [ErSlettet], [UserId]) VALUES ('per.persen@sticos.no', 1, 1, 1, '99112233','55443322',0,17) end	
END
";


    }
}