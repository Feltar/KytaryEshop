CREATE TABLE [dbo].[PolozkaKosiku] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [IdUzivatel] NVARCHAR (450) NOT NULL,
    [IdArtikl]   INT            NOT NULL,
    [PocetKusu]  INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([IdArtikl]) REFERENCES [dbo].[Artikl] ([IdArtikl]),
    UNIQUE NONCLUSTERED ([IdArtikl] ASC, [IdUzivatel] ASC)
);

