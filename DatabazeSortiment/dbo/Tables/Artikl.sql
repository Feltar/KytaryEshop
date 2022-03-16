CREATE TABLE [dbo].[Artikl] (
    [IdArtikl]     INT          IDENTITY (1, 1) NOT NULL,
    [CenaKus]      INT          NOT NULL,
    [Znacka]       VARCHAR (50) NOT NULL,
    [NazevProdukt] VARCHAR (50) NOT NULL,
    [KusuNaSklade] INT          NOT NULL,
    [TypArtiklu]   INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([IdArtikl] ASC),
    CHECK ([KusuNaSklade]>=(0)),
    CHECK ([TypArtiklu]=(2) OR [TypArtiklu]=(1) OR [TypArtiklu]=(0))
);

