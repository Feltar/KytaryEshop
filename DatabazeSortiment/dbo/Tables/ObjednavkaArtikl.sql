CREATE TABLE [dbo].[ObjednavkaArtikl] (
    [IdObjednavkaArtikl] INT IDENTITY (1, 1) NOT NULL,
    [IdArtikl]           INT NOT NULL,
    [IdObjednavka]       INT NOT NULL,
    [PocetKusu]          INT NOT NULL,
    [CenaKus]            INT NOT NULL,
    PRIMARY KEY CLUSTERED ([IdObjednavkaArtikl] ASC),
    FOREIGN KEY ([IdObjednavka]) REFERENCES [dbo].[Objednavka] ([IdObjednavka])
);

